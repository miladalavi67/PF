using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
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

    public async Task<TabularResult> GetByIdAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryTableAsync("dbo.S_CriticalPartSetSelectById", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetPartsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetPartsWithShortageListAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetColorsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetColorSelectByCriticalPartSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetSaloonsAsync(int criticalPartSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartSetId", criticalPartSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetProductsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetProductDetailsAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsSetId", criticalPartsSetId);
        return await QueryTableAsync("dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<int> CreateHeaderAsync(int setId, int deptId, string productsColorCode, int productsColorId, int productsTrimId, int productsToodooziId, int productsOptionId, short stock, string? note, int userId)
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
        parameters.Output("@Id", System.Data.DbType.Int32);
        await ExecuteAsync("dbo.S_CriticalPartsSetInsert", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
        return parameters.Get<int>("@Id");
    }

    public async Task UpdateHeaderAsync(int id, int deptId, short stock, string? note, byte status, int userId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@DeptId", deptId)
            .SmallInt("@Stock", stock)
            .NVarChar("@Note", note, 100)
            .TinyInt("@Status", status)
            .Int("@UserId", userId);
        await ExecuteAsync("dbo.S_CriticalPartsSetUpdate", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task AddProductAsync(int criticalPartsSetId, int carGroupId, int carModelId, int carClassId, int carSubClassId, int carTrimId, int carToodooziId, int carOptionId, int carColorId, int saloonId)
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
        await ExecuteAsync("dbo.S_CriticalPartsSetProductInsert", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, short saloonId, int indicatorCarId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartsSetId", criticalPartsSetId)
            .SmallInt("@SaloonId", saloonId)
            .Int("@IndicatorCarId", indicatorCarId);
        await ExecuteAsync("dbo.S_CriticalPartsSetSaloonUpdate", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task DeleteAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        await ExecuteAsync("dbo.S_CriticalPartsSetDeleteLogically", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
