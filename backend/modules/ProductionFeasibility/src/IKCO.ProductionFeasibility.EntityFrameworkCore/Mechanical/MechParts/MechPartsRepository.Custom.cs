using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using IKCO.ProductionFeasibility.StoredProcedures;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsRepository
{
    public async Task<int> SaveProductsAsync(int criticalPartsId, int carId, int trimShopId, short plantId, int isNew,
        IReadOnlyList<MechPartProduct> products, int userId)
    {
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
            await ExecuteAsync("dbo.S_CriticalPartsPruductUpdate_H_V1", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
        }

        return products.Count;
    }

    public async Task<TabularResult> CompareSubClassesAsync(IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, int saloonId, int type)
    {
        const string procedure = "dbo.S_GetContrastOfSubClasses";
        var table = await CreateTableParameterAsync(procedure, "@Tbl", rows);
        var parameters = new SpParameters()
            .Table("@Tbl", table)
            .Int("@SaloonId", saloonId)
            .Int("@Type", type);
        return await QueryTableAsync(procedure, parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }
}
