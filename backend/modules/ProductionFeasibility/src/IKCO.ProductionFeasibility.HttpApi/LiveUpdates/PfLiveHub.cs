using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace IKCO.ProductionFeasibility.LiveUpdates;

/// <summary>
/// هاب SignalR به‌روزرسانی زنده گزارش‌ها (جایگزین Service Broker فرم‌های ویندوزی و <c>BodyCriticalHub</c> بک‌اند قبلی).
/// صفحه گزارش با <see cref="Subscribe"/> عضو کانال می‌شود و با پیام <see cref="ChangedMethod"/> داده را دوباره می‌خواند؛
/// خود داده از همان endpoint گزارش (با مجوز همان گزارش) خوانده می‌شود و از هاب عبور نمی‌کند.
/// </summary>
[Authorize]
public class PfLiveHub : Hub
{
    /// <summary>مسیر هاب</summary>
    public const string Route = "/signalr-hubs/production-feasibility";

    /// <summary>نام متد سمت کلاینت برای اعلام تغییر: <c>(channel: string, changedAt: string)</c></summary>
    public const string ChangedMethod = "changed";

    public Task Subscribe(string channel) => Groups.AddToGroupAsync(Context.ConnectionId, Validate(channel));

    public Task Unsubscribe(string channel) => Groups.RemoveFromGroupAsync(Context.ConnectionId, Validate(channel));

    private static string Validate(string channel)
    {
        var known = PfLiveChannels.All.FirstOrDefault(c => string.Equals(c.Name, channel, StringComparison.OrdinalIgnoreCase));
        return known?.Name ?? throw new HubException($"کانال «{channel}» تعریف نشده است.");
    }
}
