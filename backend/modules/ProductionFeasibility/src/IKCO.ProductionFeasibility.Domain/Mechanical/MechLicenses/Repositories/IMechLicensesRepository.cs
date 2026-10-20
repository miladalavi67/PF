using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses.Repositories;

/// <summary>شرایط تولید و مجوزهای تولید با کسری — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechLicensesRepository
{
    /// <summary>اطلاعات ردیابی مجوزهای یک بحران (frmCriticalShortageLicenseTracking) — <c>dbo.S_ShortageLicenseSelectAllTrackingInfo</c></summary>
    Task<List<ShortageLicenseTrackingRow>> GetTrackingInfoAsync(int criticalPartTypeId, int criticalPartId);

    /// <summary>تاریخچه مجوزهای یک بحران (frmLicenseLogs) — <c>dbo.S_ShortageLicenseTrackingSelectAll</c></summary>
    Task<List<ShortageLicenseTrackingRow>> GetTrackingListAsync(int criticalPartTypeId, int criticalPartId);

    /// <summary>گزارش پارامترهای تولید با کسری تعریف‌شده (frmShortageLicenseTracking) — <c>dbo.S_ShortageLicenseTrackingSearch</c></summary>
    Task<List<LicenseTrackingSearchRow>> SearchTrackingAsync(string fromDate, string toDate, string? partNo, string? setCode);

    /// <summary>بحران‌های دارای مجوز تولید با کسری (frmCriticalShortageLicenseTracking) — <c>dbo.S_ShortagelicenseTrackingSearchCriticals</c></summary>
    Task<List<LicenseTrackingCriticalRow>> SearchTrackingCriticalsAsync(string fromDate, string toDate, string? partNo, string? setCode);

    /// <summary>بدنه‌های مصرف‌کننده مجوز تولید (frmBodiesRelatedToUsedLicense) — <c>dbo.S_SelectBodiesRelatedToUsedLicense</c></summary>
    Task<List<UsedLicenseBodyRow>> GetBodiesRelatedToUsedLicenseAsync(int criticalPartTypeId, int criticalPartId);

}
