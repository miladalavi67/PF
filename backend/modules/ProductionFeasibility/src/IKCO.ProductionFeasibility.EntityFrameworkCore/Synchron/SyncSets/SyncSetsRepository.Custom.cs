using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial class SyncSetsRepository
{
    private async Task<bool> ProcedureExistsAsync(string name)
    {
        var ids = await QuerySqlAsync<int?>("SELECT OBJECT_ID(@Name, N'P')", new { Name = name }, PfCommandTimeouts.Lookup);
        return ids.FirstOrDefault().HasValue;
    }

    public async Task<int?> FindShortagePartListIdAsync(string partNo)
    {
        var ids = await QuerySqlAsync<int?>(
            "SELECT TOP 1 CAST(Id AS int) FROM dbo.T_ShortagePartList WITH (NOLOCK) WHERE PartNo = @PartNo AND [Status] = 1 ORDER BY Id",
            new { PartNo = partNo }, PfCommandTimeouts.Lookup);
        return ids.FirstOrDefault();
    }

    public async Task InsertPartsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetPart> parts)
    {
        const string procedure = "dbo.S_InsertCriticalPartsSet_Parts_Web";
        if (await ProcedureExistsAsync(procedure))
        {
            var rows = parts.Select(p => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["CriticalPartsSetId"] = criticalPartsSetId, ["ShortagePartListId"] = p.ShortagePartListId, ["PartNo"] = p.PartNo,
            }).ToList();
            var table = await CreateTableParameterAsync(procedure, "@Table", rows);
            await ExecuteAsync(procedure, new SpParameters().Table("@Table", table), LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
            return;
        }

        foreach (var p in parts)
            await ExecuteSqlAsync(
                "INSERT INTO dbo.T_CriticalPartsSet_Parts (CriticalPartsSetId, ShortagePartListId, PartNo) VALUES (@SetId, @ShortagePartListId, @PartNo)",
                new { SetId = criticalPartsSetId, p.ShortagePartListId, p.PartNo });
    }

    public async Task InsertColorsAsync(int criticalPartsSetId, IReadOnlyList<int> colorIds)
    {
        const string procedure = "dbo.S_InsertCriticalPartsSet_Colors_Web";
        if (await ProcedureExistsAsync(procedure))
        {
            var rows = colorIds.Select(c => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["CriticalPartsSetId"] = criticalPartsSetId, ["CarColorId"] = c,
            }).ToList();
            var table = await CreateTableParameterAsync(procedure, "@Table", rows);
            await ExecuteAsync(procedure, new SpParameters().Table("@Table", table), LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
            return;
        }

        foreach (var c in colorIds)
            await ExecuteSqlAsync("INSERT INTO dbo.T_CriticalPartsSet_Colors (CriticalPartsSetId, CarColorId) VALUES (@SetId, @ColorId)",
                new { SetId = criticalPartsSetId, ColorId = c });
    }

    public async Task InsertSaloonsAsync(int criticalPartsSetId, IReadOnlyList<int> saloonIds)
    {
        const string procedure = "dbo.S_InsertCriticalPartsSet_Saloons_Web";
        if (await ProcedureExistsAsync(procedure))
        {
            var rows = saloonIds.Select(s => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["CriticalPartsSetId"] = criticalPartsSetId, ["SaloonId"] = s,
            }).ToList();
            var table = await CreateTableParameterAsync(procedure, "@Table", rows);
            await ExecuteAsync(procedure, new SpParameters().Table("@Table", table), LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
            return;
        }

        foreach (var s in saloonIds)
            await ExecuteSqlAsync("INSERT INTO dbo.T_CriticalPartsSet_Saloons (CriticalPartsSetId, SaloonId) VALUES (@SetId, @SaloonId)",
                new { SetId = criticalPartsSetId, SaloonId = s });
    }

    public async Task InsertProductsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetProduct> products)
    {
        const string procedure = "dbo.S_InsertCriticalPartsSet_Products";
        var rows = products.Select(p => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["CriticalPartsSetId"] = criticalPartsSetId, ["CarGroupId"] = p.CarGroupId, ["CarModelId"] = p.CarModelId, ["CarClassId"] = p.CarClassId,
            ["CarSubClassId"] = p.CarSubClassId, ["CarTrimId"] = p.CarTrimId, ["CarToodooziId"] = p.CarToodooziId, ["CarOptionId"] = p.CarOptionId,
            ["CarColorId"] = p.CarColorId, ["SaloonId"] = p.SaloonId,
        }).ToList();
        var table = await CreateTableParameterAsync(procedure, "@dt", rows);
        await ExecuteAsync(procedure, new SpParameters().Table("@dt", table), LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }
}
