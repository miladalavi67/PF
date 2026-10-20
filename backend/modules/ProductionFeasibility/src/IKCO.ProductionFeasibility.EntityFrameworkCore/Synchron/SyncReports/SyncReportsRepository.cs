using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

public partial class SyncReportsRepository : PfDapperRepository<PfDbContext>, ISyncReportsRepository, ITransientDependency
{
    public SyncReportsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<ProductionPossibilityRow>> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId)
    {
        var parameters = new SpParameters()
            .NVarChar("@SaloonId", saloonId, -1)
            .Int("@CrisisTypeId", crisisTypeId)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SubClassId", subClassId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<ProductionPossibilityRow>("dbo.S_RptCriticalPartsProduction_Synchron", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<SapCriticalPartsMasterRow>> GetSapCriticalPartsMasterAsync(string fromDate, string toDate, int saloonId, string? bodyNO, string? partNO)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10)
            .Int("@SaloonId", saloonId);
        if (bodyNO != null)
            parameters.VarChar("@BodyNO", bodyNO, 8);
        if (partNO != null)
            parameters.NVarChar("@PartNO", partNO, 50);
        return await QueryRowsAsync<SapCriticalPartsMasterRow>("dbo.S_Web_SAP_CriticalPartsMaster_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<SapReserveMasterRow>> GetSapReserveMasterAsync(string fromDate, string toDate, int saloonId, string? partNO)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10)
            .Int("@SaloonId", saloonId);
        if (partNO != null)
            parameters.NVarChar("@PartNO", partNO, 50);
        return await QueryRowsAsync<SapReserveMasterRow>("dbo.S_Web_SAP_ReserveMaster_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<AsnRow>> GetAsnReportAsync(string? partNumber, string? fromDate, string? toDate, byte type)
    {
        var parameters = new SpParameters()
            .TinyInt("@Type", type);
        if (partNumber != null)
            parameters.VarChar("@PartNumber", partNumber, 50);
        if (fromDate != null)
            parameters.VarChar("@FromDate", fromDate, 10);
        if (toDate != null)
            parameters.VarChar("@ToDate", toDate, 10);
        return await QueryRowsAsync<AsnRow>("dbo.S_ASNReport", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
