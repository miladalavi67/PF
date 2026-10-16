using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis;

public partial class SyncBasisRepository : PfDapperRepository<PfDbContext>, ISyncBasisRepository, ITransientDependency
{
    public SyncBasisRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> GetListAsync()
    {
        var parameters = new SpParameters();
        return await QueryTableAsync("dbo.S_RptGetCriticalPartsSet", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetByCodeAsync(string code)
    {
        var parameters = new SpParameters()
            .VarChar("@Code", code, 50);
        return await QueryTableAsync("dbo.S_CriticalPartsSetBasisSelectByCode", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetByIdAsync(string id)
    {
        var parameters = new SpParameters()
            .VarChar("@Id", id, 50);
        return await QueryTableAsync("dbo.S_CriticalPartsSetBasisSelectById", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetByStatusAsync(byte? status)
    {
        var parameters = new SpParameters();
        if (status != null)
            parameters.TinyInt("@Status", status);
        return await QueryTableAsync("dbo.S_CriticalPartsSetBasisSelectByStatus", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> CountBySetCodeAsync(string setCode)
    {
        var parameters = new SpParameters()
            .VarChar("@SetCode", setCode, 50);
        return await QueryTableAsync("dbo.S_CriticalPartsSetCountSetCode", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task CreateAsync(string code, string descF, int carClass, int partTypeId, int levelType, int userId, bool isTrim, bool isOption, bool isToodoozi, bool isColor)
    {
        var parameters = new SpParameters()
            .NVarChar("@Code", code, 10)
            .NVarChar("@DescF", descF, 100)
            .Int("@CarClass", carClass)
            .Int("@PartTypeId", partTypeId)
            .Int("@LevelType", levelType)
            .Int("@UserId", userId)
            .Bit("@IsTrim", isTrim)
            .Bit("@IsOption", isOption)
            .Bit("@IsToodoozi", isToodoozi)
            .Bit("@IsColor", isColor);
        await ExecuteAsync("dbo.S_InsertCriticalPartSet", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task UpdateAsync(int id, string code, string descF, int carClassId, int partTypeId, int levelType, bool status, bool isTrim, bool isColor, bool isOption, bool isToodoozi)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .NVarChar("@Code", code, 10)
            .NVarChar("@DescF", descF, 100)
            .Int("@CarClassId", carClassId)
            .Int("@PartTypeId", partTypeId)
            .Int("@LevelType", levelType)
            .Bit("@Status", status)
            .Bit("@IsTrim", isTrim)
            .Bit("@IsColor", isColor)
            .Bit("@IsOption", isOption)
            .Bit("@IsToodoozi", isToodoozi);
        await ExecuteAsync("dbo.S_UpdateCriticalPartsSet", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
