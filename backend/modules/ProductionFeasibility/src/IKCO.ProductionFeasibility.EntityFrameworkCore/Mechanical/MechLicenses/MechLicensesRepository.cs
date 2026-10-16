using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

public partial class MechLicensesRepository : PfDapperRepository<PfDbContext>, IMechLicensesRepository, ITransientDependency
{
    public MechLicensesRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> GetTrackingInfoAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryTableAsync("dbo.S_ShortageLicenseSelectAllTrackingInfo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetTrackingListAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryTableAsync("dbo.S_ShortageLicenseTrackingSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> SearchTrackingAsync(string fromDate, string toDate, string? partNo, string? setCode)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 50)
            .VarChar("@ToDate", toDate, 50);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.VarChar("@SetCode", setCode, 50);
        return await QueryTableAsync("dbo.S_ShortageLicenseTrackingSearch", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> SearchTrackingCriticalsAsync(string fromDate, string toDate, string? partNo, string? setCode)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 50)
            .VarChar("@ToDate", toDate, 50);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.VarChar("@SetCode", setCode, 50);
        return await QueryTableAsync("dbo.S_ShortagelicenseTrackingSearchCriticals", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetBodiesRelatedToUsedLicenseAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryTableAsync("dbo.S_SelectBodiesRelatedToUsedLicense", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
