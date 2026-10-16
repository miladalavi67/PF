using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.StoredProcedures;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

public partial class BodyCriticalsRepository
{
    public async Task<int> CreateProductsAsync(int criticalPartsId, int carId, IReadOnlyList<(int CarClassId, int UsageFactor)> items, int userId)
    {
        foreach (var (carClassId, usageFactor) in items)
        {
            var parameters = new SpParameters()
                .Int("@CriticalPartsId", criticalPartsId)
                .Int("@CarClassId", carClassId)
                .Int("@UsageFactor", usageFactor)
                .Int("@UserId", userId)
                .Int("@CarId", carId);
            await ExecuteAsync("PF.S_CreateCritocalPartsProduct", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
        }

        return items.Count;
    }

    public async Task<GlobalResultDto?> RegisterLicenseAsync(int criticalPartId, int criticalPartTypeId, int productionPossibilityId,
        int? possibleProductionNo, string? toDate, string? toTime, string? note, int userId)
    {
        // سازنده اصلی بحران: کاربر اولین ردیف ردیابی مجوز (مانند DA_Critical.RegisterShortageLicense سامانه ویندوزی)
        var creators = await QuerySqlAsync<int?>(
            "SELECT TOP (1) UserId FROM PF.T_ShortageLicenseTracking WITH (NOLOCK) WHERE CriticalPartId = @CriticalPartId ORDER BY Id",
            new { CriticalPartId = criticalPartId });
        var creatorUser = creators.FirstOrDefault() ?? userId;

        var parameters = new SpParameters()
            .Int("@CriticalPartId", criticalPartId)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@ProductionPossibilityId", productionPossibilityId)
            .Int("@PossibleProductionNo", possibleProductionNo)
            .VarChar("@ProductionPossibilityToDate", toDate, PfConsts.ShamsiDateLength)
            .VarChar("@ProductionPossibilityToTime", toTime, PfConsts.TimeLength)
            .NVarChar("@Note", note, 250)
            .Int("@UserId", userId)
            .Int("@CreatorUser", creatorUser);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("PF.RegisterShortageLicense_V1", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<TabularResult> BatchCreateAsync(int biwSiteId, IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, int userId)
    {
        const string procedure = "PF.S_CreateBatchCriticalPartProduct";
        var table = await CreateTableParameterAsync(procedure, "@Table", rows);
        var parameters = new SpParameters()
            .Int("@BIWSiteId", biwSiteId)
            .Table("@Table", table)
            .Int("@UserId", userId);
        return await QueryTableAsync(procedure, parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }
}
