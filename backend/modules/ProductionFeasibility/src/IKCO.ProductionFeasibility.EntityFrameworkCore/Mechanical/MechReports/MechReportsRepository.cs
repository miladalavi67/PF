using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.Mechanical.Results;
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
        return await QueryRowsAsync<ProductionPossibilityRow>("dbo.S_RptCriticalPartsProduction_Mechanical", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<ProductionPossibilityRow>> GetProductionPossibilityWithLicenseAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, int carModelId, int carClassId)
    {
        var parameters = new SpParameters()
            .NVarChar("@SaloonId", saloonId, -1)
            .Int("@CrisisTypeId", crisisTypeId)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SubClassId", subClassId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId);
        return await QueryRowsAsync<ProductionPossibilityRow>("dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<ProdPossibilityByTrimShopRow>> GetProductionPossibilityByTrimShopAsync(int trimShopId, byte prodState)
    {
        var parameters = new SpParameters()
            .Int("@TrimShopId", trimShopId)
            .TinyInt("@ProdState", prodState);
        return await QueryRowsAsync<ProdPossibilityByTrimShopRow>("dbo.S_RPT_DESC_ProdPossibilityByTrimShop", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<GeneralReportRow>> GetGeneralReportAsync(byte? criticalPartTypeId, int saloonId, byte? crisisTypeId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@SaloonId", saloonId);
        if (criticalPartTypeId != null)
            parameters.TinyInt("@CriticalPartTypeId", criticalPartTypeId);
        if (crisisTypeId != null)
            parameters.TinyInt("@CrisisTypeId", crisisTypeId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryRowsAsync<GeneralReportRow>("dbo.S_CriticalPartsGeneralReport_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<IncomingBodiesCountRow>> GetIncomingBodiesByCriticalPartAsync(int id, int criticalPartTypeId, int saloonId, byte plantId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SaloonId", saloonId)
            .TinyInt("@PlantId", plantId);
        return await QueryRowsAsync<IncomingBodiesCountRow>("dbo.S_RPT_IncommingBodiesByCriticalPartId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<IncomingBodyRow>> GetIncomingBodyListAsync(int id, int criticalPartTypeId, int saloonId, int productionStepGroupId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SaloonId", saloonId)
            .Int("@ProductionStepGroupId", productionStepGroupId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryRowsAsync<IncomingBodyRow>("dbo.S_RPT_IncommingBodiesListByCriticalPartId", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<IncomingBodiesSummaryRow>> GetIncomingBodiesSummaryAsync(int saloonId, byte crisisTypeId, int? productionStepGroupId, byte? plantId)
    {
        var parameters = new SpParameters()
            .Int("@SaloonId", saloonId)
            .TinyInt("@CrisisTypeId", crisisTypeId);
        if (productionStepGroupId != null)
            parameters.Int("@ProductionStepGroupId", productionStepGroupId);
        if (plantId != null)
            parameters.TinyInt("@PlantId", plantId);
        return await QueryRowsAsync<IncomingBodiesSummaryRow>("dbo.S_RPT_IncommingBodiesByCriticalParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<StockLogRow>> GetStockLogAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 30);
        return await QueryRowsAsync<StockLogRow>("dbo.SEXT_CriticalPartsStockLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<SapCriticalLogRow>> GetSapCriticalLogAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<SapCriticalLogRow>("dbo.S_SapCriticalLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<FailedSapTransferRow>> GetFailedSapTransfersAsync(string fromDate, string toDate)
    {
        var parameters = new SpParameters()
            .NVarChar("@FromDate", fromDate, 10)
            .NVarChar("@ToDate", toDate, 10);
        return await QueryRowsAsync<FailedSapTransferRow>("dbo.S_FailedPartTransferd_Sap_Log", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
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

    public async Task<List<TrimInNotPossibleRow>> GetNotPossibleProductsToTrimStepAsync(int carId)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId);
        return await QueryRowsAsync<TrimInNotPossibleRow>("dbo.S_TrimInNotPossibleProductsSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<TrimShopBodyCountRow>> GetTrimShopBodyCountAsync(int carId)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId);
        return await QueryRowsAsync<TrimShopBodyCountRow>("dbo.S_TrimShopBodyCount_List", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<TrimShopCriticalBodyRow>> GetTrimShopCriticalBodiesAsync(string? fromDate, string? toDate, string? partNo, int criticalPartTypeId, string? setCode)
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
        return await QueryRowsAsync<TrimShopCriticalBodyRow>("dbo.S_TrimShopCriticalBody_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
