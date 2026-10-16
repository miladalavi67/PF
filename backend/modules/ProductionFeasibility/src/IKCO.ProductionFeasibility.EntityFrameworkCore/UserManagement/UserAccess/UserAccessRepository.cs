using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

public partial class UserAccessRepository : PfDapperRepository<PfDbContext>, IUserAccessRepository, ITransientDependency
{
    public UserAccessRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> GetAllUsersAsync()
    {
        var parameters = new SpParameters();
        return await QueryTableAsync("PF.GetAllUsers", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetUsersAccessListAsync()
    {
        var parameters = new SpParameters();
        return await QueryTableAsync("PF.GetUsersAccessList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetSitesAsync(string? personalCode, int? saloonTypeId)
    {
        var parameters = new SpParameters()
            .VarChar("@PersonalCode", personalCode, 20)
            .Int("@SaloonTypeId", saloonTypeId);
        return await QueryTableAsync("PF.GetSiteByDomainId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetSaloonsByVirtualPlantAsync(int plantId)
    {
        var parameters = new SpParameters()
            .Int("@plantId", plantId);
        return await QueryTableAsync("PF.GetSaloonByVirtualPlantId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<GlobalResultDto?> DeleteUserAccessAsync(string idStr)
    {
        var parameters = new SpParameters()
            .VarChar("@IdStr", idStr, -1);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("PF.S_DeleteUserAccess", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
