using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports;

public partial class BodyReportsRepository : PfDapperRepository<PfDbContext>, IBodyReportsRepository, ITransientDependency
{
    public BodyReportsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<BodyCriticalSearchRow>> SearchCriticalPartsAsync(int? plantId, int? vinBiwSite, string? partNumber, bool? status, string? fromDate, string? toDate, int? carGroupId, int? carModelId, int? carClassId, int? typeOfSearch, int? productionPossibilityId)
    {
        var parameters = new SpParameters()
            .Int("@PlantId", plantId)
            .Int("@VINBiwSite", vinBiwSite)
            .VarChar("@PartNumber", partNumber, 100)
            .Bit("@Status", status)
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10)
            .Int("@CarGroupId", carGroupId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId)
            .Int("@TypeOfSearch", typeOfSearch)
            .Int("@ProductionPossibilityId", productionPossibilityId);
        return await QueryRowsAsync<BodyCriticalSearchRow>("PF.S_BodyCriticalSearch_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<BodyCriticalProductRow>> GetCriticalPartProductsAsync(int criticalPartsId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsId", criticalPartsId);
        return await QueryRowsAsync<BodyCriticalProductRow>("PF.Get_BodyCriticalSearch_Product_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BodyTrackingCriticalPartRow>> GetTrackingCriticalPartsAsync(int? plantId, int? vinBiwSite, int? carGroupId, int? carModelId, int? carClassId, int? subClassId, int? productionPossibilityId)
    {
        var parameters = new SpParameters()
            .Int("@PlantId", plantId)
            .Int("@VINBiwSite", vinBiwSite)
            .Int("@CarGroupId", carGroupId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId)
            .Int("@SubClassId", subClassId)
            .Int("@ProductionPossibilityId", productionPossibilityId);
        return await QueryRowsAsync<BodyTrackingCriticalPartRow>("PF.GetTrackingCriticalParts_V2", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<BodyTrackingProductRow>> GetTrackingCriticalPartProductsAsync(int criticalPartsId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsId", criticalPartsId);
        return await QueryRowsAsync<BodyTrackingProductRow>("PF.GetTrackingCriticalParts_Product", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BodyProductsOfSiteRow>> GetProductsOfSitesAsync()
    {
        var parameters = new SpParameters();
        return await QueryRowsAsync<BodyProductsOfSiteRow>("PF.S_VINBIWSite_CarModel_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<BodyPreAssignCountRow>> GetPreAssignBodyCountsAsync(int carId)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId);
        return await QueryRowsAsync<BodyPreAssignCountRow>("PF.S_PreAssignBodyCoun_List", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BodyPreAssignCriticalRow>> GetPreAssignCriticalBodiesAsync(string? fromDate, string? toDate, string? partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10)
            .VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<BodyPreAssignCriticalRow>("PF.S_PreAssignCriticalBody_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<BodyNotPossibleToAssignRow>> GetNotPossibleProductsToStepAssignAsync(string? fromDate, string? toDate, string? partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10)
            .VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<BodyNotPossibleToAssignRow>("PF.S_NotPossibleProductsToStepAssign_SelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<BodyLicenseTrackingRow>> GetShortageLicenseTrackingAsync(int criticalPartTypeId, int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@CriticalPartId", criticalPartId);
        return await QueryRowsAsync<BodyLicenseTrackingRow>("PF.GetShortageLicenseTrackingSelectAll", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BodyProductionPossibilityStatusRow>> GetProductionPossibilityStatusAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryRowsAsync<BodyProductionPossibilityStatusRow>("PF.GetProductionPssibilityStatus_V1", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<FailedSapTransferRow>> GetFailedSapTransfersAsync(string fromDate, string toDate)
    {
        var parameters = new SpParameters()
            .VarChar("@FromDate", fromDate, 10)
            .VarChar("@ToDate", toDate, 10);
        return await QueryRowsAsync<FailedSapTransferRow>("PF.S_FailedPartTransferd_Sap_Log", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<SapDataUpdateTimesRow>> GetSapDataUpdateTimesAsync()
    {
        var parameters = new SpParameters();
        return await QueryRowsAsync<SapDataUpdateTimesRow>("PF.S_GetSapDataUpdateTimes", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<string?> GetLastChangeOnLicenseAsync()
    {
        var parameters = new SpParameters();
        var scalar = await ExecuteScalarAsync("PF.S_GetLastChangeOnShortageLicense", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
        return scalar is null or DBNull ? null : Convert.ToString(scalar);
    }

}
