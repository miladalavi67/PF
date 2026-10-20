using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.StoredProcedures;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsRepository
{
    public async Task<List<GlobalResultDto?>> SaveProductsAsync(int criticalPartsId, int carId, int trimShopId, short plantId, int isNew,
        IReadOnlyList<MechPartProduct> products, int userId)
    {
        var results = new List<GlobalResultDto?>(products.Count);
        foreach (var p in products)
        {
            var parameters = new SpParameters()
                .Int("@CarId", carId)
                .Int("@CriticalPartsId", criticalPartsId)
                .Int("@CarGroupId", p.CarGroupId)
                .Int("@CarModelId", p.CarModelId)
                .Int("@CarClassId", p.CarClassId)
                .Int("@SubClassId", p.SubClassId)
                .Int("@ColorId", p.ColorId)
                .Int("@TrimId", p.TrimId)
                .Int("@OptionId", p.OptionId)
                .Int("@UserId", userId)
                .Int("@IsNew", isNew)
                .Int("@TrimShopId", trimShopId)
                .SmallInt("@PlantId", plantId)
                .TinyInt("@Quantity", p.Quantity);
            results.Add(await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsPruductUpdate_Web", parameters, LegacyCallStyle.AdoNet,
                PfCommandTimeouts.Command));
        }

        return results;
    }

    public async Task<List<SubClassContrastRow>> CompareSubClassesAsync(IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, int saloonId, int type)
    {
        const string procedure = "dbo.S_GetContrastOfSubClasses";
        var table = await CreateTableParameterAsync(procedure, "@Tbl", rows);
        var parameters = new SpParameters()
            .Table("@Tbl", table)
            .Int("@SaloonId", saloonId)
            .Int("@Type", type);
        return await QueryRowsAsync<SubClassContrastRow>(procedure, parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }
}
