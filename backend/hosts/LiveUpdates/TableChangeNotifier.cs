using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.LiveUpdates;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IKCO.ProductionFeasibility.Host.LiveUpdates;

/// <summary>تنظیمات <c>ProductionFeasibility:LiveUpdates</c></summary>
public sealed class LiveUpdateOptions
{
    public const string SectionName = "ProductionFeasibility:LiveUpdates";

    public bool Enabled { get; set; } = true;

    /// <summary><c>Auto</c> (Service Broker و در صورت نبود آن پایش دوره‌ای)، <c>ServiceBroker</c> یا <c>Polling</c></summary>
    public string Mode { get; set; } = "Auto";

    /// <summary>فاصله پایش دوره‌ای (ثانیه) وقتی Service Broker در دسترس نیست</summary>
    public int PollingSeconds { get; set; } = 10;

    /// <summary>تجمیع تغییرات پشت‌سرهم یک کانال در یک پیام (میلی‌ثانیه)</summary>
    public int DebounceMilliseconds { get; set; } = 1500;
}

/// <summary>
/// پایش تغییر جدول‌های گزارش‌های ردیابی و اعلام آن به کلاینت‌های عضو کانال در <see cref="PfLiveHub"/> (PFReview بند ۵).
/// مانند فرم‌های ویندوزی از Query Notification (SqlDependency روی Service Broker) استفاده می‌شود؛ اگر Service Broker روی پایگاه داده
/// فعال نباشد یا پرس‌وجویی برای اعلان معتبر نباشد، همان پرس‌وجو با checksum به صورت دوره‌ای پایش می‌شود.
/// </summary>
public sealed class TableChangeNotifier : BackgroundService
{
    private readonly IHubContext<PfLiveHub> _hub;
    private readonly ILogger<TableChangeNotifier> _logger;
    private readonly LiveUpdateOptions _options;
    private readonly string? _connectionString;
    private readonly ConcurrentDictionary<string, byte> _pending = new();
    private readonly ConcurrentDictionary<(string Channel, string Query), byte> _polled = new();
    private CancellationToken _stopping;

    public TableChangeNotifier(IHubContext<PfLiveHub> hub, IConfiguration configuration, IOptions<LiveUpdateOptions> options,
        ILogger<TableChangeNotifier> logger)
    {
        _hub = hub;
        _logger = logger;
        _options = options.Value;
        _connectionString = configuration.GetConnectionString("Default");
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _stopping = stoppingToken;
        if (!_options.Enabled || string.IsNullOrWhiteSpace(_connectionString))
        {
            _logger.LogInformation("به‌روزرسانی زنده گزارش‌ها غیرفعال است (تنظیمات یا رشته اتصال).");
            return;
        }

        var broker = false;
        if (!string.Equals(_options.Mode, "Polling", StringComparison.OrdinalIgnoreCase))
        {
            try
            {
                SqlDependency.Start(_connectionString);
                broker = true;
            }
            catch (Exception e)
            {
                _logger.LogWarning(e, "Service Broker در دسترس نیست؛ تغییرات گزارش‌ها به صورت دوره‌ای پایش می‌شود.");
            }
        }

        foreach (var channel in PfLiveChannels.All)
        foreach (var query in channel.Queries)
        {
            if (broker)
                await RegisterAsync(channel.Name, query);
            else
                _polled.TryAdd((channel.Name, query), 0);
        }

        try
        {
            await PollAsync(stoppingToken);
        }
        finally
        {
            if (broker)
                SqlDependency.Stop(_connectionString);
        }
    }

    /// <summary>ثبت اعلان یک پرس‌وجو؛ پس از هر اعلان دوباره ثبت می‌شود (اعلان‌های SqlDependency یک‌بار مصرف‌اند)</summary>
    private async Task RegisterAsync(string channel, string query)
    {
        if (_stopping.IsCancellationRequested)
            return;
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(_stopping);
            await using var command = new SqlCommand(query, connection);
            var dependency = new SqlDependency(command);
            dependency.OnChange += (_, e) =>
            {
                if (e.Type == SqlNotificationType.Subscribe)
                {
                    _logger.LogWarning("پرس‌وجوی کانال {Channel} برای Query Notification معتبر نیست ({Info})؛ پایش دوره‌ای جایگزین شد.", channel, e.Info);
                    _polled.TryAdd((channel, query), 0);
                    return;
                }

                Notify(channel);
                _ = RegisterAsync(channel, query);
            };
            await using var reader = await command.ExecuteReaderAsync(_stopping);
            while (await reader.ReadAsync(_stopping))
            {
            }
        }
        catch (OperationCanceledException) when (_stopping.IsCancellationRequested)
        {
        }
        catch (Exception e)
        {
            _logger.LogWarning(e, "ثبت اعلان تغییر کانال {Channel} ناموفق بود؛ پایش دوره‌ای جایگزین شد.", channel);
            _polled.TryAdd((channel, query), 0);
        }
    }

    private async Task PollAsync(CancellationToken stoppingToken)
    {
        var last = new Dictionary<(string, string), (long?, long)>();
        var interval = TimeSpan.FromSeconds(Math.Max(2, _options.PollingSeconds));
        while (!stoppingToken.IsCancellationRequested)
        {
            foreach (var key in _polled.Keys.ToList())
            {
                try
                {
                    await using var connection = new SqlConnection(_connectionString);
                    await connection.OpenAsync(stoppingToken);
                    await using var command = new SqlCommand(
                        $"SELECT CHECKSUM_AGG(BINARY_CHECKSUM(*)), COUNT_BIG(*) FROM ({key.Query}) AS q", connection);
                    await using var reader = await command.ExecuteReaderAsync(stoppingToken);
                    if (!await reader.ReadAsync(stoppingToken))
                        continue;
                    var current = (reader.IsDBNull(0) ? (long?)null : Convert.ToInt64(reader.GetValue(0)), reader.GetInt64(1));
                    if (last.TryGetValue(key, out var previous) && previous != current)
                        Notify(key.Channel);
                    last[key] = current;
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "پایش تغییرات کانال {Channel} ناموفق بود.", key.Channel);
                }
            }

            try
            {
                await Task.Delay(interval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }

    private void Notify(string channel)
    {
        if (!_pending.TryAdd(channel, 0))
            return;
        _ = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(Math.Max(0, _options.DebounceMilliseconds), _stopping);
                _pending.TryRemove(channel, out _);
                await _hub.Clients.Group(channel).SendAsync(PfLiveHub.ChangedMethod, channel, DateTime.Now.ToString("O"), _stopping);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                _pending.TryRemove(channel, out _);
                _logger.LogWarning(e, "ارسال اعلان تغییر کانال {Channel} ناموفق بود.", channel);
            }
        });
    }
}
