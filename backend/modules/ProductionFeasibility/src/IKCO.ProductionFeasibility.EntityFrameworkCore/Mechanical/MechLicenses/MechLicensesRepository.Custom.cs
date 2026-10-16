using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Repositories;
using IKCO.ProductionFeasibility.StoredProcedures;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

public partial class MechLicensesRepository
{
    public Task UpdateMechanicalProductionPossibilityAsync(int id, byte productionPossibility, short? possibleProductionNo, string? toDate,
        string? toTime, string? letterNo, string? note, byte productionFallId)
        => ExecuteAsync("dbo.S_CriticalPartsProdPosUpdate", new SpParameters()
                .Int("@Id", id)
                .TinyInt("@ProductionPossibility", productionPossibility)
                .SmallInt("@PossibleProductionNo", possibleProductionNo ?? 0)   // رویه 0 و -1 را null می‌کند
                .Char("@ProductionPossibilityToDate", toDate ?? string.Empty, 10)
                .Char("@ProductionPossibilityToTime", toTime ?? string.Empty, 8)
                .VarChar("@ProductionStateLetterNo", letterNo ?? string.Empty, 30)
                .VarChar("@ProductionStateNote", note ?? string.Empty, 100)
                .TinyInt("@ProductionFallId", productionFallId),
            LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);

    public Task UpdateSetProductionPossibilityAsync(int id, byte productionPossibility, short? possibleProductionNo, byte productionFallId,
        string? toDate, string? toTime, string? letterNo, string? note)
        => ExecuteAsync("dbo.S_CriticalPartsSetUpdateProductionPossibility", new SpParameters()
                .Int("@Id", id)
                .TinyInt("@ProductionPossibilityId", productionPossibility)
                .SmallInt("@PossibleProductionNo", possibleProductionNo)
                .TinyInt("@ProductionFallId", productionFallId)
                .Char("@ProductionPossibilityToDate", toDate, 10)
                .Char("@ProductionPossibilityToTime", toTime, 8)
                .VarChar("@ProductionPossibilityLetterNo", letterNo, 50)
                .VarChar("@ProductionPossibilityNote", note, 100),
            LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);

    public Task InfluenceTrackingAsync(int criticalPartTypeId, int criticalPartId, int productionPossibility, int? possibleProductionNo,
        string? toDate, string? toTime, int personId, int controlFlag)
        => ExecuteAsync("dbo.S_InfluenceShortageLicenseTracking", new SpParameters()
                .Int("@CriticalPartTypeId", criticalPartTypeId)
                .Int("@CriticalPartId", criticalPartId)
                .Int("@ProductionPossibilityId", productionPossibility)
                .Int("@PossibleProductionNo", possibleProductionNo)
                .VarChar("@ProductionPossibilityToDate", toDate, 50)
                .VarChar("@ProductionPossibilityToTime", toTime, 50)
                .Int("@UserId", personId)
                .Int("@ControlFlag", controlFlag),
            LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);

    public async Task<LicenseSummary> GetSummaryAsync(int criticalPartTypeId, int criticalPartId)
    {
        var rows = await QuerySqlAsync<SummaryRow>(
            @"SELECT dbo.U_GetValidRemainingOfAllShouldPreviousShortageLicenses(@Type, @Id) AS ValidRemainingOfAll,
                     dbo.U_GetValidRemainingOfLastLimitedShortageLicense(@Type, @Id) AS ValidRemainingOfLastLimited,
                     CASE WHEN @Type = 0 THEN dbo.U_UsedLicenseOfCriticalPossibleProductionNo(@Id)
                          ELSE dbo.U_UsedLicenseOfCriticalSetPossibleProductionNo(@Id) END AS UsedLicense",
            new { Type = criticalPartTypeId, Id = criticalPartId }, PfCommandTimeouts.Lookup);
        var row = rows.FirstOrDefault();
        return new LicenseSummary(row?.UsedLicense, row?.ValidRemainingOfAll, row?.ValidRemainingOfLastLimited);
    }

    private sealed class SummaryRow
    {
        public int? UsedLicense { get; set; }
        public int? ValidRemainingOfAll { get; set; }
        public int? ValidRemainingOfLastLimited { get; set; }
    }
}
