using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports.Repositories;

/// <summary>جستجو و گزارش‌های بحران بدنه‌سازی — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IBodyReportsRepository
{
    /// <summary>جستجوی اقلام بحرانی بدنه‌سازی (frmBodyCriticalSearch / frmRPTBodyCriticalSearch) — <c>PF.S_BodyCriticalSearch_Web</c></summary>
    Task<List<BodyCriticalSearchRow>> SearchCriticalPartsAsync(int? plantId, int? vinBiwSite, string? partNumber, bool? status, string? fromDate, string? toDate, int? carGroupId, int? carModelId, int? carClassId, int? typeOfSearch, int? productionPossibilityId);

    /// <summary>محصولات یک بحران (frmBodyCriticalSearch (جزئیات)) — <c>PF.Get_BodyCriticalSearch_Product_Web</c></summary>
    Task<List<BodyCriticalProductRow>> GetCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>ردیابی تغییرات اقلام بحرانی (frmRPTTrackingCriticalParts / frmRPTTrackingCriticalParts_withTimer) — <c>PF.GetTrackingCriticalParts_V2</c></summary>
    Task<List<BodyTrackingCriticalPartRow>> GetTrackingCriticalPartsAsync(int? plantId, int? vinBiwSite, int? carGroupId, int? carModelId, int? carClassId, int? subClassId, int? productionPossibilityId);

    /// <summary>محصولات بحران در ردیابی (frmRPTTrackingCriticalParts (جزئیات)) — <c>PF.GetTrackingCriticalParts_Product</c></summary>
    Task<List<BodyTrackingProductRow>> GetTrackingCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>محصولات به تفکیک خطوط بدنه‌سازی (frmRPTGetProductsOfBIWSite) — <c>PF.S_VINBIWSite_CarModel_SelectAll</c></summary>
    Task<List<BodyProductsOfSiteRow>> GetProductsOfSitesAsync();

    /// <summary>تعداد بدنه‌های پیش‌تخصیص‌شده (frmRPTPreassignBodyCount) — <c>PF.S_PreAssignBodyCoun_List</c></summary>
    Task<List<BodyPreAssignCountRow>> GetPreAssignBodyCountsAsync(int carId);

    /// <summary>بدنه‌های پیش‌تخصیص بحرانی (frmRPTPreAssignCriticalBody) — <c>PF.S_PreAssignCriticalBody_SelectAll</c></summary>
    Task<List<BodyPreAssignCriticalRow>> GetPreAssignCriticalBodiesAsync(string? fromDate, string? toDate, string? partNo);

    /// <summary>مغایرت پیش‌تخصیص بدنه‌های بحرانی (frmRPTNotPossibleProductsToStepAssign) — <c>PF.S_NotPossibleProductsToStepAssign_SelectAll</c></summary>
    Task<List<BodyNotPossibleToAssignRow>> GetNotPossibleProductsToStepAssignAsync(string? fromDate, string? toDate, string? partNo);

    /// <summary>ردیابی پارامترهای بحرانی (مجوز تولید با کسری) (frmRPTShortageLicenseTracking) — <c>PF.GetShortageLicenseTrackingSelectAll</c></summary>
    Task<List<BodyLicenseTrackingRow>> GetShortageLicenseTrackingAsync(int criticalPartTypeId, int criticalPartId);

    /// <summary>وضعیت مجوز تولید یک بحران (frmCriticalPartsProdPosChange) — <c>PF.GetProductionPssibilityStatus_V1</c></summary>
    Task<List<BodyProductionPossibilityStatusRow>> GetProductionPossibilityStatusAsync(int id);

    /// <summary>قطعات ناموفق در انتقال از SAP (BodyShop reports (SAP)) — <c>PF.S_FailedPartTransferd_Sap_Log</c></summary>
    Task<List<FailedSapTransferRow>> GetFailedSapTransfersAsync(string fromDate, string toDate);

    /// <summary>زمان آخرین به‌روزرسانی داده‌های SAP (BodyShop reports (SAP)) — <c>PF.S_GetSapDataUpdateTimes</c></summary>
    Task<List<SapDataUpdateTimesRow>> GetSapDataUpdateTimesAsync();

    /// <summary>شناسه آخرین تغییر مجوزها (برای به‌روزرسانی خودکار گزارش) (RealTimeDetectChangeOnLicense (جایگزین SignalR)) — <c>PF.S_GetLastChangeOnShortageLicense</c></summary>
    Task<string?> GetLastChangeOnLicenseAsync();

}
