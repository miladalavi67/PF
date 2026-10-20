using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechReasons.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons;

public partial class MechReasonsRepository : PfDapperRepository<PfDbContext>, IMechReasonsRepository, ITransientDependency
{
    public MechReasonsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<NotPossibleReasonRow>> GetListAsync(byte? selectType)
    {
        var parameters = new SpParameters();
        if (selectType != null)
            parameters.TinyInt("@SelectType", selectType);
        return await QueryRowsAsync<NotPossibleReasonRow>("dbo.S_ProgNotPossibilityReasonSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task CreateAsync(string code, string descF, string descE, byte status, int userId)
    {
        var parameters = new SpParameters()
            .Char("@Code", code, 5)
            .NVarChar("@DescF", descF, 500)
            .NVarChar("@DescE", descE, 500)
            .TinyInt("@Status", status)
            .Int("@UserId", userId);
        await ExecuteAsync("dbo.S_ProgNotPossibilityReason_Insert", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task UpdateAsync(int id, string code, string descF, string descE, byte status)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Char("@Code", code, 5)
            .NVarChar("@DescF", descF, 500)
            .NVarChar("@DescE", descE, 500)
            .TinyInt("@Status", status);
        await ExecuteAsync("dbo.S_ProgNotPossibilityReason_Update", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task DeleteAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        await ExecuteAsync("dbo.S_ProgNotPossibilityReason_LogicalDelete", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
