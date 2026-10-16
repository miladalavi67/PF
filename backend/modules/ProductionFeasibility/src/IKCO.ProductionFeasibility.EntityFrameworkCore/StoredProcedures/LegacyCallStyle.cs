namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// نحوه فراخوانی رویه در سامانه قدیم؛ رفتار تراکنشی همان حفظ می‌شود.
/// </summary>
public enum LegacyCallStyle
{
    /// <summary>
    /// فراخوانی از طریق FunctionImport در EF6: هر فراخوانی در تراکنش مستقل خودش اجرا می‌شد
    /// (EnsureTransactionsForFunctionsAndCommands). اگر unit of work تراکنشی فعال باشد به آن ملحق می‌شود.
    /// </summary>
    EfFunctionImport,

    /// <summary>
    /// فراخوانی ADO.NET با <c>SqlHelper</c> بدون تراکنش (autocommit)؛ اگر unit of work تراکنشی فعال باشد به آن ملحق می‌شود.
    /// </summary>
    AdoNet
}

/// <summary>timeout های سامانه قدیم (ثانیه).</summary>
public static class LegacyCommandTimeouts
{
    /// <summary>timeout پیش‌فرض ADO.NET/EF6 وقتی تنظیم نشده بود</summary>
    public const int Default = 30;

    /// <summary><c>SqlHelper.SelectRows</c> و <c>Database.CommandTimeout = 3600</c> در گزارش‌ها</summary>
    public const int SelectRows = 3600;

    /// <summary><c>SqlHelper.ExecuteReader&lt;T&gt;</c></summary>
    public const int ExecuteReader = 720;

    /// <summary><c>SqlHelper.ExecuteNonQuery</c> با CommandTimeout = 0 (نامحدود)</summary>
    public const int Unlimited = 0;

    /// <summary><c>DAL_Report.GetWINRptEngineSerial</c></summary>
    public const int EngineSerialReport = 360;
}
