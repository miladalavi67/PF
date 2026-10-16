using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Exceptions;
using Microsoft.Data.SqlClient;
using Volo.Abp.Domain.Repositories.Dapper;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// پایه مخازن Dapper برای رویه‌های ذخیره‌شده سامانه قدیم. اتصال و تراکنش از unit of work ابپ گرفته می‌شود
/// (<c>GetDbConnectionAsync</c> / <c>GetDbTransactionAsync</c>) تا در تراکنش‌های صریح (مثل تعویض سریال) مشارکت کند.
/// </summary>
public abstract class PfDapperRepository<TDbContext> : DapperRepository<TDbContext>
    where TDbContext : IEfCoreDbContext
{
    private readonly IDbContextProvider<TDbContext> _dbContextProvider;

    protected PfDapperRepository(IDbContextProvider<TDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
        _dbContextProvider = dbContextProvider;
    }

    /// <summary>DbContext همان unit of work برای بخش‌هایی که سامانه قدیم با EF6 LINQ انجام می‌داد</summary>
    protected Task<TDbContext> GetDbContextAsync() => _dbContextProvider.GetDbContextAsync();

    /// <summary>اجرای بخش EF Core با همان ترجمه خطای SQL رویه‌ها</summary>
    protected async Task<TResult> RunEfAsync<TResult>(string operation, Func<TDbContext, Task<TResult>> action)
    {
        try
        {
            return await action(await GetDbContextAsync());
        }
        catch (Exception ex) when (FindSqlException(ex) is { } sql)
        {
            var translated = SqlExceptionTranslator.Translate(sql, operation);
            if (ReferenceEquals(translated, sql))
                throw;
            throw translated;
        }
    }

    private static SqlException? FindSqlException(Exception? ex)
    {
        for (; ex != null; ex = ex.InnerException)
            if (ex is SqlException sql)
                return sql;
        return null;
    }

    /// <summary>ردیف‌های اولین result set با نگاشت نام ستون به خصوصیت</summary>
    protected Task<List<T>> QueryAsync<T>(string procedure, SpParameters parameters, LegacyCallStyle style, int commandTimeout)
        => RunAsync(procedure, style, async (connection, transaction) =>
            (await connection.QueryAsync<T>(procedure, parameters.Dapper, transaction, commandTimeout, CommandType.StoredProcedure)).ToList());

    /// <summary>اولین ردیف (معادل <c>.FirstOrDefault()</c> روی نتیجه FunctionImport)</summary>
    protected async Task<T?> QueryFirstOrDefaultAsync<T>(string procedure, SpParameters parameters, LegacyCallStyle style, int commandTimeout)
        where T : class
        => (await QueryAsync<T>(procedure, parameters, style, commandTimeout)).FirstOrDefault();

    /// <summary>مقدار ستون اول ردیف اول (خروجی اسکالر FunctionImport یا <c>dt.Rows[0][0]</c>)</summary>
    protected Task<object?> ExecuteScalarAsync(string procedure, SpParameters parameters, LegacyCallStyle style, int commandTimeout)
        => RunAsync(procedure, style, (connection, transaction) =>
            connection.ExecuteScalarAsync(procedure, parameters.Dapper, transaction, commandTimeout, CommandType.StoredProcedure));

    /// <summary>اجرای بدون خروجی؛ تعداد ردیف‌های متاثر</summary>
    protected Task<int> ExecuteAsync(string procedure, SpParameters parameters, LegacyCallStyle style, int commandTimeout)
        => RunAsync(procedure, style, (connection, transaction) =>
            connection.ExecuteAsync(procedure, parameters.Dapper, transaction, commandTimeout, CommandType.StoredProcedure));

    /// <summary>
    /// معادل <c>SqlHelper.SelectRows</c>: اولین result set به صورت جدول پویا. نام ستون‌های تکراری مانند
    /// <c>DataTable</c> با پسوند عددی یکتا می‌شوند.
    /// </summary>
    protected Task<TabularResult> QueryTableAsync(string procedure, SpParameters parameters, LegacyCallStyle style, int commandTimeout)
        => RunAsync(procedure, style, async (connection, transaction) =>
        {
            await using var reader = await connection.ExecuteReaderAsync(
                procedure, parameters.Dapper, transaction, commandTimeout, CommandType.StoredProcedure);
            return await ReadTableAsync(reader);
        });

    /// <summary>پرس‌وجوی متنی پارامتری (فقط برای فهرست‌های مرجع روی جدول/نمای پایه که رویه‌ای ندارند)؛ هرگز رشته‌چسبانی نشود.</summary>
    protected Task<List<T>> QuerySqlAsync<T>(string sql, object? parameters = null, int commandTimeout = LegacyCommandTimeouts.Default)
        => RunAsync(sql, LegacyCallStyle.AdoNet, async (connection, transaction) =>
            (await connection.QueryAsync<T>(sql, parameters, transaction, commandTimeout, CommandType.Text)).ToList());

    /// <summary>اجرای دستور متنی پارامتری بدون خروجی (درج‌های ساده‌ای که رویه ذخیره‌شده ندارند)</summary>
    protected Task<int> ExecuteSqlAsync(string sql, object? parameters = null, int commandTimeout = PfCommandTimeouts.Command)
        => RunAsync(sql, LegacyCallStyle.AdoNet, (connection, transaction) =>
            connection.ExecuteAsync(sql, parameters, transaction, commandTimeout, CommandType.Text));

    /// <summary>پرس‌وجوی متنی پارامتری با نتیجه جدولی پویا</summary>
    protected Task<TabularResult> QueryTableSqlAsync(string sql, object? parameters = null, int commandTimeout = LegacyCommandTimeouts.Default)
        => RunAsync(sql, LegacyCallStyle.AdoNet, async (connection, transaction) =>
        {
            await using var reader = await connection.ExecuteReaderAsync(sql, parameters, transaction, commandTimeout, CommandType.Text);
            return await ReadTableAsync(reader);
        });

    protected async Task<TResult> RunAsync<TResult>(string procedure, LegacyCallStyle style, Func<DbConnection, DbTransaction?, Task<TResult>> action)
    {
        var connection = (DbConnection)await GetDbConnectionAsync();
        var ambient = (DbTransaction?)await GetDbTransactionAsync();
        try
        {
            if (ambient != null || style == LegacyCallStyle.AdoNet)
                return await action(connection, ambient);

            // EF6 FunctionImport: هر فراخوانی در تراکنش مستقل
            var wasClosed = connection.State != ConnectionState.Open;
            if (wasClosed)
                await connection.OpenAsync();
            try
            {
                await using var transaction = await connection.BeginTransactionAsync();
                var result = await action(connection, transaction);
                await transaction.CommitAsync();
                return result;
            }
            finally
            {
                if (wasClosed)
                    await connection.CloseAsync();
            }
        }
        catch (Exception exception) when (FindSqlException(exception) is { } ex)
        {
            var translated = SqlExceptionTranslator.Translate(ex, procedure);
            if (ReferenceEquals(translated, ex))
                throw;
            throw await ResolveNumericMessageAsync(translated, ex, connection);
        }
    }

    /// <summary>
    /// رویه‌های قدیمی خطا را به صورت شماره پیام (<c>RAISERROR('50149', 16, -1)</c>) برمی‌گردانند و سامانه ویندوزی متن فارسی
    /// آن را از <c>dbo.B_SqlMessages</c> می‌خواند؛ همان رفتار اینجا حفظ می‌شود (در صورت نبودن، پیام اصلی می‌ماند).
    /// </summary>
    private static async Task<Exception> ResolveNumericMessageAsync(Exception translated, SqlException original, DbConnection connection)
    {
        if (translated is not BaseException { ErrorCode: PfErrorCodes.StoredProcedureError } baseException
            || !int.TryParse(original.Message.Trim(), out var code))
            return translated;

        if (!SqlMessageCache.TryGetValue(code, out var text))
        {
            try
            {
                text = await connection.QueryFirstOrDefaultAsync<string>(
                    "SELECT TOP (1) DescF FROM dbo.B_SqlMessages WITH (NOLOCK) WHERE [error] = @Code", new { Code = code });
                SqlMessageCache[code] = text;
            }
            catch (Exception)
            {
                return translated;
            }
        }

        return string.IsNullOrWhiteSpace(text)
            ? translated
            : new BaseException(PfErrorCodes.StoredProcedureError, text.Trim()).WithData("MessageCode", code);
    }

    private static readonly ConcurrentDictionary<int, string?> SqlMessageCache = new();

    /// <summary>
    /// پارامتر جدولی (table-valued) یک رویه؛ نوع جدولی از <c>sys.parameters</c> کشف می‌شود و ستون‌ها بر اساس نام پر می‌شوند
    /// (ترتیب ستون‌ها در سامانه قدیم DataTable بر اساس ترتیب خصوصیت DTO بود).
    /// </summary>
    protected async Task<SqlMapper.ICustomQueryParameter> CreateTableParameterAsync(
        string procedure, string parameterName, IEnumerable<IReadOnlyDictionary<string, object?>> rows)
    {
        var connection = (DbConnection)await GetDbConnectionAsync();
        var transaction = (DbTransaction?)await GetDbTransactionAsync();
        return await TableValuedParameterFactory.CreateForProcedureAsync(connection, transaction, procedure, parameterName, rows);
    }

    private static async Task<TabularResult> ReadTableAsync(DbDataReader reader)
    {
        var result = new TabularResult();
        var names = new string[reader.FieldCount];
        var used = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < reader.FieldCount; i++)
        {
            var name = string.IsNullOrEmpty(reader.GetName(i)) ? "Column" + (i + 1) : reader.GetName(i);
            var unique = name;
            var suffix = 1;
            while (!used.Add(unique))
                unique = name + suffix++;
            names[i] = unique;
            result.Columns.Add(unique);
        }

        while (await reader.ReadAsync())
        {
            var row = new Dictionary<string, object?>(reader.FieldCount);
            for (var i = 0; i < reader.FieldCount; i++)
                row[names[i]] = await reader.IsDBNullAsync(i) ? null : reader.GetValue(i);
            result.Rows.Add(row);
        }

        return result;
    }
}
