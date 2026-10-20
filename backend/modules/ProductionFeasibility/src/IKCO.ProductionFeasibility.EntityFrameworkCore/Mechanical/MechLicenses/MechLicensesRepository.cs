using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.Results;
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

    public async Task<List<ShortageLicenseTrackingRow>> GetTrackingInfoAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryRowsAsync<ShortageLicenseTrackingRow>("dbo.S_ShortageLicenseSelectAllTrackingInfo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<ShortageLicenseTrackingRow>> GetTrackingListAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryRowsAsync<ShortageLicenseTrackingRow>("dbo.S_ShortageLicenseTrackingSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<LicenseTrackingSearchRow>> SearchTrackingAsync(string fromDate, string toDate, string? partNo, string? setCode)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 50)
            .VarChar("@ToDate", toDate, 50);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.VarChar("@SetCode", setCode, 50);
        return await QueryRowsAsync<LicenseTrackingSearchRow>("dbo.S_ShortageLicenseTrackingSearch", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<LicenseTrackingCriticalRow>> SearchTrackingCriticalsAsync(string fromDate, string toDate, string? partNo, string? setCode)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 50)
            .VarChar("@ToDate", toDate, 50);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.VarChar("@SetCode", setCode, 50);
        return await QueryRowsAsync<LicenseTrackingCriticalRow>("dbo.S_ShortagelicenseTrackingSearchCriticals", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<UsedLicenseBodyRow>> GetBodiesRelatedToUsedLicenseAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryRowsAsync<UsedLicenseBodyRow>("dbo.S_SelectBodiesRelatedToUsedLicense", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
