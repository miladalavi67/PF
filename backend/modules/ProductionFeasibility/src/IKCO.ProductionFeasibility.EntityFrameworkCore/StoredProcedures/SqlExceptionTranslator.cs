using System;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Exceptions;
using Microsoft.Data.SqlClient;

namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// تبدیل خطاهای SQL Server به خطای استاندارد IKCO. سامانه قدیم پیام خطای رویه (RAISERROR/THROW) را عیناً به کاربر
/// نشان می‌داد (<c>RaiseError</c> / <c>Error = e.InnerException.Message</c>)؛ همان پیام به عنوان پیام خطای کاربرپسند
/// برگردانده می‌شود و شماره خط/نام رویه فقط در داده خطا قرار می‌گیرد. سایر خطاها بدون جزئیات فنی بازگردانده می‌شوند.
/// </summary>
public static class SqlExceptionTranslator
{
    private static readonly int[] ConnectionErrors = { -1, 2, 53, 64, 121, 233, 258, 1231, 4060, 10053, 10054, 10060, 10061, 11001, 18456, 40613 };

    public static Exception Translate(SqlException exception, string procedure)
    {
        var number = exception.Number;

        if (number == -2)
            return new BaseException(PfErrorCodes.DatabaseCommandTimeout, "زمان اجرای درخواست در پایگاه داده به پایان رسید.")
                .WithData("Procedure", procedure);

        // خطاهای شبکه/اتصال (severity 20 به بالا یا شماره‌های شناخته‌شده)
        if (Array.IndexOf(ConnectionErrors, number) >= 0 || exception.Class >= 20)
            return new BaseException(PfErrorCodes.SQLServerNetworkNotResponding, "ارتباط با پایگاه داده برقرار نشد.")
                .WithData("Procedure", procedure);

        if (number == 2627 || number == 2601)
            return new BaseException(PfErrorCodes.DuplicateRecord, "اطلاعات وارد شده تکراری است.")
                .WithData("Procedure", procedure);

        if (number == 547)
            return new BaseException(PfErrorCodes.ReferenceConstraintViolation, "به دلیل وابستگی اطلاعات، عملیات امکان‌پذیر نیست.")
                .WithData("Procedure", procedure);

        // RAISERROR با متن (50000) یا THROW (>= 50000) یا خطای کاربری با severity 11 تا 16
        if (number >= 50000 || exception.Class is >= 11 and <= 16)
            return new BaseException(PfErrorCodes.StoredProcedureError, exception.Message)
                .WithData("Procedure", exception.Procedure ?? procedure)
                .WithData("LineNumber", exception.LineNumber);

        return exception;
    }

    /// <summary>اجرای عملیات EF Core خارج از مخازن Dapper با همان ترجمه خطای SQL</summary>
    public static async Task<T> RunAsync<T>(string operation, Func<Task<T>> action)
    {
        try
        {
            return await action();
        }
        catch (Exception ex) when (Find(ex) is { } sql && !ReferenceEquals(Translate(sql, operation), sql))
        {
            throw Translate(sql, operation);
        }
    }

    private static SqlException? Find(Exception? ex)
    {
        for (; ex != null; ex = ex.InnerException)
            if (ex is SqlException sql)
                return sql;
        return null;
    }
}
