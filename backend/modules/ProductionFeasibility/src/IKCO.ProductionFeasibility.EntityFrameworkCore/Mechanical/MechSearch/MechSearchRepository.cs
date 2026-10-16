using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.MechSearch.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch;

public partial class MechSearchRepository : PfDapperRepository<PfDbContext>, IMechSearchRepository, ITransientDependency
{
    public MechSearchRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> SearchCriticalPartsAsync(string? partNo, string? setCode, string? fromDate, string? toDate, string? trimShopIds, int? status, int? criticalPartTypeId)
    {
        var parameters = new SpParameters();
        if (partNo != null)
            parameters.NVarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.NVarChar("@SetCode", setCode, 15);
        if (fromDate != null)
            parameters.VarChar("@FromDate", fromDate, 10);
        if (toDate != null)
            parameters.VarChar("@ToDate", toDate, 10);
        if (trimShopIds != null)
            parameters.VarChar("@TrimShopIds", trimShopIds, 500);
        if (status != null)
            parameters.Int("@Status", status);
        if (criticalPartTypeId != null)
            parameters.Int("@CriticalPartTypeId", criticalPartTypeId);
        return await QueryTableAsync("dbo.S_CriticalPartsSearchByConditions", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> SearchByTrimShopAsync(string? partNo, string? fromDate, string? toDate, int trimShopId, int type)
    {
        var parameters = new SpParameters()
            .Int("@TrimShopId", trimShopId)
            .Int("@Type", type);
        if (partNo != null)
            parameters.NVarChar("@PartNo", partNo, 500);
        if (fromDate != null)
            parameters.VarChar("@FromDate", fromDate, 10);
        if (toDate != null)
            parameters.VarChar("@ToDate", toDate, 10);
        return await QueryTableAsync("dbo.S_CriticalPartSearch", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetReceivedPartsAsync(string fromDate, string toDate)
    {
        var parameters = new SpParameters()
            .NVarChar("@FromDate", fromDate, 10)
            .NVarChar("@ToDate", toDate, 10);
        return await QueryTableAsync("dbo.S_CriticalPart_RecievedParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
