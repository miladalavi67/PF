using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial class SyncSetsRepository : PfDapperRepository<PfDbContext>, ISyncSetsRepository, ITransientDependency
{
    public SyncSetsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<CriticalPartSetRow>> GetByIdAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryRowsAsync<CriticalPartSetRow>("dbo.S_CriticalPartSetSelectById", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetPartRow>> GetPartsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetPartRow>("dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetPartWithShortageRow>> GetPartsWithShortageListAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetPartWithShortageRow>("dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetColorRow>> GetColorsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetColorRow>("dbo.S_CriticalPartSetColorSelectByCriticalPartSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetSaloonRow>> GetSaloonsAsync(int criticalPartSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartSetId", criticalPartSetId);
        return await QueryRowsAsync<SetSaloonRow>("dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetProductRow>> GetProductsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetProductRow>("dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SetProductDetailRow>> GetProductDetailsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetProductDetailRow>("dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<int?> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        var scalar = await ExecuteScalarAsync("dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
        return scalar is null or DBNull ? null : Convert.ToInt32(scalar);
    }

    public async Task<List<SetSaloonItemRow>> GetSetSaloonsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryRowsAsync<SetSaloonItemRow>("dbo.S_GetCriticalPartsSet_Saloons", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BomPartInfoRow>> GetPartInfoAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<BomPartInfoRow>("dbo.SIMP_PartGetInfoByPartNo_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SimilarColorRow>> GetSimilarColorsAsync(string code)
    {
        var parameters = new SpParameters()
            .Char("@Code", code, 5);
        return await QueryRowsAsync<SimilarColorRow>("dbo.S_SelectColorsSimilarTo5DigitCode", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<SaloonByCarModelRow>> GetSaloonsByCarModelsAsync(string carModelIdSTR, byte plantId)
    {
        var parameters = new SpParameters()
            .VarChar("@CarModelIdSTR", carModelIdSTR, 2000)
            .TinyInt("@PlantId", plantId);
        return await QueryRowsAsync<SaloonByCarModelRow>("dbo.S_SelectSaloonsByCarModels", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<GlobalResultDto?> CreateHeaderAsync(int setId, int deptId, string productsColorCode, int productsColorId, int productsTrimId, int productsToodooziId, int productsOptionId, short stock, string? note, int userId)
    {
        var parameters = new SpParameters()
            .Int("@SetId", setId)
            .Int("@DeptId", deptId)
            .Char("@ProductsColorCode", productsColorCode, 5)
            .Int("@ProductsColorId", productsColorId)
            .Int("@ProductsTrimId", productsTrimId)
            .Int("@ProductsToodooziId", productsToodooziId)
            .Int("@ProductsOptionId", productsOptionId)
            .SmallInt("@Stock", stock)
            .VarChar("@Note", note, 100)
            .Int("@UserId", userId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsSetInsert_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> UpdateHeaderAsync(int id, int deptId, short stock, string? note, byte status, int userId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@DeptId", deptId)
            .SmallInt("@Stock", stock)
            .NVarChar("@Note", note, 100)
            .TinyInt("@Status", status)
            .Int("@UserId", userId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsSetUpdate_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> AddProductAsync(int criticalPartsSetId, int carGroupId, int carModelId, int carClassId, int carSubClassId, int carTrimId, int carToodooziId, int carOptionId, int carColorId, int saloonId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId)
            .Int("@CarGroupId", carGroupId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId)
            .Int("@CarSubClassId", carSubClassId)
            .Int("@CarTrimId", carTrimId)
            .Int("@CarToodooziId", carToodooziId)
            .Int("@CarOptionId", carOptionId)
            .Int("@CarColorId", carColorId)
            .Int("@SaloonId", saloonId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsSetProductInsert_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, short saloonId, int indicatorCarId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartsSetId", criticalPartsSetId)
            .SmallInt("@SaloonId", saloonId)
            .Int("@IndicatorCarId", indicatorCarId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsSetSaloonUpdate_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task DeleteAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        await ExecuteAsync("dbo.S_CriticalPartsSetDeleteLogically", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
