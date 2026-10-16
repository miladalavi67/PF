using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses.Repositories;

/// <summary>خلاصه مجوز خوانده‌شده از توابع <c>U_*</c></summary>
public sealed record LicenseSummary(int? UsedLicense, int? ValidRemainingOfAll, int? ValidRemainingOfLastLimited);

public partial interface IMechLicensesRepository
{
    /// <summary><c>S_CriticalPartsProdPosUpdate</c> (مکانیکی)</summary>
    Task UpdateMechanicalProductionPossibilityAsync(int id, byte productionPossibility, short? possibleProductionNo, string? toDate, string? toTime,
        string? letterNo, string? note, byte productionFallId);

    /// <summary><c>S_CriticalPartsSetUpdateProductionPossibility</c> (سنکرون)</summary>
    Task UpdateSetProductionPossibilityAsync(int id, byte productionPossibility, short? possibleProductionNo, byte productionFallId, string? toDate,
        string? toTime, string? letterNo, string? note);

    /// <summary><c>S_InfluenceShortageLicenseTracking</c>؛ <paramref name="controlFlag"/> = 3 ویرایش آخرین مجوز، سایر مقادیر ثبت جدید</summary>
    Task InfluenceTrackingAsync(int criticalPartTypeId, int criticalPartId, int productionPossibility, int? possibleProductionNo, string? toDate,
        string? toTime, int personId, int controlFlag);

    Task<LicenseSummary> GetSummaryAsync(int criticalPartTypeId, int criticalPartId);
}
