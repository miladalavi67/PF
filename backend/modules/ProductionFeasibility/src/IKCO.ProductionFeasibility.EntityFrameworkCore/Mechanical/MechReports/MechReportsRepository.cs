using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports;

public partial class MechReportsRepository : PfDapperRepository<PfDbContext>, IMechReportsRepository, ITransientDependency
{
    public MechReportsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId)
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
        return await QueryTableAsync("dbo.S_RptCriticalPartsProduction_Mechanical", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetProductionPossibilityWithLicenseAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, int carModelId, int carClassId)
    {
        var parameters = new SpParameters()
            .NVarChar("@SaloonId", saloonId, -1)
            .Int("@CrisisTypeId", crisisTypeId)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SubClassId", subClassId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId);
        return await QueryTableAsync("dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetProductionPossibilityByTrimShopAsync(int trimShopId, byte prodState)
    {
        var parameters = new SpParameters()
            .Int("@TrimShopId", trimShopId)
            .TinyInt("@ProdState", prodState);
        return await QueryTableAsync("dbo.S_RPT_DESC_ProdPossibilityByTrimShop", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetGeneralReportAsync(byte? criticalPartTypeId, int saloonId, byte? crisisTypeId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@SaloonId", saloonId);
        if (criticalPartTypeId != null)
            parameters.TinyInt("@CriticalPartTypeId", criticalPartTypeId);
        if (crisisTypeId != null)
            parameters.TinyInt("@CrisisTypeId", crisisTypeId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryTableAsync("dbo.S_CriticalPartsGeneralReport", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetIncomingBodiesByCriticalPartAsync(int id, int criticalPartTypeId, int saloonId, byte plantId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SaloonId", saloonId)
            .TinyInt("@PlantId", plantId);
        return await QueryTableAsync("dbo.S_RPT_IncommingBodiesByCriticalPartId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetIncomingBodyListAsync(int id, int criticalPartTypeId, int saloonId, int productionStepGroupId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SaloonId", saloonId)
            .Int("@ProductionStepGroupId", productionStepGroupId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryTableAsync("dbo.S_RPT_IncommingBodiesListByCriticalPartId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetIncomingBodiesSummaryAsync(int saloonId, byte crisisTypeId, int? productionStepGroupId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@SaloonId", saloonId)
            .TinyInt("@CrisisTypeId", crisisTypeId);
        if (productionStepGroupId != null)
            parameters.Int("@ProductionStepGroupId", productionStepGroupId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryTableAsync("dbo.S_RPT_IncommingBodiesByCriticalParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetStockLogAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 30);
        return await QueryTableAsync("dbo.SEXT_CriticalPartsStockLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetSapCriticalLogAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        return await QueryTableAsync("dbo.S_SapCriticalLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetFailedSapTransfersAsync(string fromDate, string toDate)
    {
        var parameters = new SpParameters()
            .NVarChar("@FromDate", fromDate, 10)
            .NVarChar("@ToDate", toDate, 10);
        return await QueryTableAsync("dbo.S_FailedPartTransferd_Sap_Log", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetAsnReportAsync(string? partNumber, string? fromDate, string? toDate, byte type)
    {
        var parameters = new SpParameters()
            .TinyInt("@Type", type);
        if (partNumber != null)
            parameters.VarChar("@PartNumber", partNumber, 50);
        if (fromDate != null)
            parameters.VarChar("@FromDate", fromDate, 10);
        if (toDate != null)
            parameters.VarChar("@ToDate", toDate, 10);
        return await QueryTableAsync("dbo.S_ASNReport", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetNotPossibleProductsToTrimStepAsync(int carId)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId);
        return await QueryTableAsync("dbo.S_TrimInNotPossibleProductsSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetTrimShopBodyCountAsync(int carId)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId);
        return await QueryTableAsync("dbo.S_TrimShopBodyCount_List", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<TabularResult> GetTrimShopCriticalBodiesAsync(string? fromDate, string? toDate, string? partNo, int criticalPartTypeId, string? setCode)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId);
        if (fromDate != null)
            parameters.VarChar("@FromDate", fromDate, 10);
        if (toDate != null)
            parameters.VarChar("@ToDate", toDate, 10);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        if (setCode != null)
            parameters.NVarChar("@SetCode", setCode, 10);
        return await QueryTableAsync("dbo.S_TrimShopCriticalBody_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
