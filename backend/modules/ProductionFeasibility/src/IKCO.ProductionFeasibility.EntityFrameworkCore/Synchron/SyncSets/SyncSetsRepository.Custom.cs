using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial class SyncSetsRepository
{
    public async Task<int?> FindShortagePartListIdAsync(string partNo)
    {
        var parameters = new SpParameters().VarChar("@PartNo", partNo, 20);
        var rows = await QueryRowsAsync<PartInShortageListRow>("dbo.SIMP_SelectParInShortagePartList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
        return rows.FirstOrDefault(r => r.ShortagePartListId > 0)?.ShortagePartListId;
    }

    public Task<GlobalResultDto?> InsertPartsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetPart> parts)
        => ExecuteTableAsync("dbo.S_InsertCriticalPartsSet_Parts_Web", parts.Select(p => new Dictionary<string, object?>
        {
            ["CriticalPartsSetId"] = criticalPartsSetId, ["ShortagePartListId"] = p.ShortagePartListId, ["PartNo"] = p.PartNo,
        }));

    public Task<GlobalResultDto?> InsertColorsAsync(int criticalPartsSetId, IReadOnlyList<int> colorIds)
        => ExecuteTableAsync("dbo.S_InsertCriticalPartsSet_Colors_Web", colorIds.Select(c => new Dictionary<string, object?>
        {
            ["CriticalPartsSetId"] = criticalPartsSetId, ["CarColorId"] = c,
        }));

    public Task<GlobalResultDto?> InsertSaloonsAsync(int criticalPartsSetId, IReadOnlyList<int> saloonIds)
        => ExecuteTableAsync("dbo.S_InsertCriticalPartsSet_Saloons_Web", saloonIds.Select(s => new Dictionary<string, object?>
        {
            ["CriticalPartsSetId"] = criticalPartsSetId, ["SaloonId"] = s,
        }));

    public async Task<List<GlobalResultDto?>> InsertProductsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetProduct> products)
    {
        var results = new List<GlobalResultDto?>(products.Count);
        foreach (var p in products)
            results.Add(await AddProductAsync(criticalPartsSetId, p.CarGroupId, p.CarModelId, p.CarClassId, p.CarSubClassId, p.CarTrimId,
                p.CarToodooziId, p.CarOptionId, p.CarColorId, p.SaloonId));
        return results;
    }

    /// <summary>رویه‌های <c>S_InsertCriticalPartsSet_*_Web</c> (پارامتر جدولی <c>@Table</c>، خروجی Result/Message)</summary>
    private async Task<GlobalResultDto?> ExecuteTableAsync(string procedure, IEnumerable<Dictionary<string, object?>> rows)
    {
        var table = await CreateTableParameterAsync(procedure, "@Table", rows.Cast<IReadOnlyDictionary<string, object?>>().ToList());
        return await QueryFirstOrDefaultAsync<GlobalResultDto>(procedure, new SpParameters().Table("@Table", table), LegacyCallStyle.AdoNet,
            PfCommandTimeouts.Command);
    }
}
