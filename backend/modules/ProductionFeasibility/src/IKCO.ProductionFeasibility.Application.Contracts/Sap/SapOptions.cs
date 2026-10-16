namespace IKCO.ProductionFeasibility.Sap;

/// <summary>
/// تنظیمات سرویس SAP برای «استفاده‌شده در» (ZMAT_WHERE_USED). نشانی، نام کاربری و رمز باید با
/// <c>dotnet user-secrets</c> یا متغیر محیطی تنظیم شوند؛ در بک‌اند قبلی نام کاربری و رمز در کد ثابت شده بود.
/// </summary>
public class SapOptions
{
    public const string SectionName = "ProductionFeasibility:Sap";

    /// <summary>نشانی سرویس SOAP «زوج ماده - محصول»</summary>
    public string WhereUsedUrl { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public int TimeoutSeconds { get; set; } = 30;
}
