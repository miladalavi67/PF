using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports;

/// <summary>جستجو و گزارش‌های بحران بدنه‌سازی</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.BodyShopGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/body-shop/reports")]
public partial class BodyReportsController : PfController
{
    private readonly IBodyReportsAppService _service;

    public BodyReportsController(IBodyReportsAppService service)
    {
        _service = service;
    }

    /// <summary>جستجوی اقلام بحرانی بدنه‌سازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmBodyCriticalSearch / frmRPTBodyCriticalSearch</c>.
    /// رویه ذخیره‌شده: <c>PF.S_BodyCriticalSearch_Web</c>.
    ///
    ///     POST /api/production-feasibility/body-shop/reports/critical-parts/search
    ///     { "plantId": 0, "vinBiwSite": 0, "partNumber": "string", "status": false, "fromDate": "1405/01/01", "toDate": "1405/01/01", "carGroupId": 0, "carModelId": 0, "carClassId": 0, "typeOfSearch": 0, "productionPossibilityId": 0 }
    /// </remarks>
    [HttpPost("critical-parts/search")]
    [Authorize(PfPermissions.BodyReports.Search)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyCriticalSearchRow>))]
    public Task<ListResultDto<BodyCriticalSearchRow>> SearchCriticalPartsAsync([FromBody] BodyReportsSearchCriticalPartsInput input)
        => _service.SearchCriticalPartsAsync(input);

    /// <summary>محصولات یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmBodyCriticalSearch (جزئیات)</c>.
    /// رویه ذخیره‌شده: <c>PF.Get_BodyCriticalSearch_Product_Web</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/critical-parts/{criticalPartsId:int}/products
    /// </remarks>
    [HttpGet("critical-parts/{criticalPartsId:int}/products")]
    [Authorize(PfPermissions.BodyReports.Search)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyCriticalProductRow>))]
    public Task<ListResultDto<BodyCriticalProductRow>> GetCriticalPartProductsAsync(int criticalPartsId)
        => _service.GetCriticalPartProductsAsync(criticalPartsId);

    /// <summary>ردیابی تغییرات اقلام بحرانی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTTrackingCriticalParts / frmRPTTrackingCriticalParts_withTimer</c>.
    /// رویه ذخیره‌شده: <c>PF.GetTrackingCriticalParts_V2</c>.
    ///
    ///     POST /api/production-feasibility/body-shop/reports/tracking-critical-parts
    ///     { "plantId": 0, "vinBiwSite": 0, "carGroupId": 0, "carModelId": 0, "carClassId": 0, "subClassId": 0, "productionPossibilityId": 0 }
    /// </remarks>
    [HttpPost("tracking-critical-parts")]
    [Authorize(PfPermissions.BodyReports.Tracking)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyTrackingCriticalPartRow>))]
    public Task<ListResultDto<BodyTrackingCriticalPartRow>> GetTrackingCriticalPartsAsync([FromBody] BodyReportsGetTrackingCriticalPartsInput input)
        => _service.GetTrackingCriticalPartsAsync(input);

    /// <summary>محصولات بحران در ردیابی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTTrackingCriticalParts (جزئیات)</c>.
    /// رویه ذخیره‌شده: <c>PF.GetTrackingCriticalParts_Product</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/tracking-critical-parts/{criticalPartsId:int}/products
    /// </remarks>
    [HttpGet("tracking-critical-parts/{criticalPartsId:int}/products")]
    [Authorize(PfPermissions.BodyReports.Tracking)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyTrackingProductRow>))]
    public Task<ListResultDto<BodyTrackingProductRow>> GetTrackingCriticalPartProductsAsync(int criticalPartsId)
        => _service.GetTrackingCriticalPartProductsAsync(criticalPartsId);

    /// <summary>محصولات به تفکیک خطوط بدنه‌سازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTGetProductsOfBIWSite</c>.
    /// رویه ذخیره‌شده: <c>PF.S_VINBIWSite_CarModel_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/products-of-sites
    /// </remarks>
    [HttpGet("products-of-sites")]
    [Authorize(PfPermissions.BodyReports.Productsofsites)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyProductsOfSiteRow>))]
    public Task<ListResultDto<BodyProductsOfSiteRow>> GetProductsOfSitesAsync()
        => _service.GetProductsOfSitesAsync();

    /// <summary>تعداد بدنه‌های پیش‌تخصیص‌شده</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTPreassignBodyCount</c>.
    /// رویه ذخیره‌شده: <c>PF.S_PreAssignBodyCoun_List</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/pre-assign-body-counts
    /// </remarks>
    [HttpGet("pre-assign-body-counts")]
    [Authorize(PfPermissions.BodyReports.Preassigncount)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyPreAssignCountRow>))]
    public Task<ListResultDto<BodyPreAssignCountRow>> GetPreAssignBodyCountsAsync([FromQuery] BodyReportsGetPreAssignBodyCountsInput input)
        => _service.GetPreAssignBodyCountsAsync(input);

    /// <summary>بدنه‌های پیش‌تخصیص بحرانی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTPreAssignCriticalBody</c>.
    /// رویه ذخیره‌شده: <c>PF.S_PreAssignCriticalBody_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/pre-assign-critical-bodies
    /// </remarks>
    [HttpGet("pre-assign-critical-bodies")]
    [Authorize(PfPermissions.BodyReports.Preassigncritical)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyPreAssignCriticalRow>))]
    public Task<ListResultDto<BodyPreAssignCriticalRow>> GetPreAssignCriticalBodiesAsync([FromQuery] BodyReportsGetPreAssignCriticalBodiesInput input)
        => _service.GetPreAssignCriticalBodiesAsync(input);

    /// <summary>مغایرت پیش‌تخصیص بدنه‌های بحرانی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTNotPossibleProductsToStepAssign</c>.
    /// رویه ذخیره‌شده: <c>PF.S_NotPossibleProductsToStepAssign_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/not-possible-to-assign
    /// </remarks>
    [HttpGet("not-possible-to-assign")]
    [Authorize(PfPermissions.BodyReports.Conflicts)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyNotPossibleToAssignRow>))]
    public Task<ListResultDto<BodyNotPossibleToAssignRow>> GetNotPossibleProductsToStepAssignAsync([FromQuery] BodyReportsGetNotPossibleProductsToStepAssignInput input)
        => _service.GetNotPossibleProductsToStepAssignAsync(input);

    /// <summary>ردیابی پارامترهای بحرانی (مجوز تولید با کسری)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTShortageLicenseTracking</c>.
    /// رویه ذخیره‌شده: <c>PF.GetShortageLicenseTrackingSelectAll</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/shortage-license-tracking
    /// </remarks>
    [HttpGet("shortage-license-tracking")]
    [Authorize(PfPermissions.BodyReports.Licensetracking)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyLicenseTrackingRow>))]
    public Task<ListResultDto<BodyLicenseTrackingRow>> GetShortageLicenseTrackingAsync([FromQuery] BodyReportsGetShortageLicenseTrackingInput input)
        => _service.GetShortageLicenseTrackingAsync(input);

    /// <summary>وضعیت مجوز تولید یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsProdPosChange</c>.
    /// رویه ذخیره‌شده: <c>PF.GetProductionPssibilityStatus_V1</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/production-possibility-status/{id:int}
    /// </remarks>
    [HttpGet("production-possibility-status/{id:int}")]
    [Authorize(PfPermissions.BodyReports.Licensetracking)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyProductionPossibilityStatusRow>))]
    public Task<ListResultDto<BodyProductionPossibilityStatusRow>> GetProductionPossibilityStatusAsync(int id)
        => _service.GetProductionPossibilityStatusAsync(id);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>BodyShop reports (SAP)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_FailedPartTransferd_Sap_Log</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/failed-sap-transfers
    /// </remarks>
    [HttpGet("failed-sap-transfers")]
    [Authorize(PfPermissions.BodyReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<FailedSapTransferRow>))]
    public Task<ListResultDto<FailedSapTransferRow>> GetFailedSapTransfersAsync([FromQuery] BodyReportsGetFailedSapTransfersInput input)
        => _service.GetFailedSapTransfersAsync(input);

    /// <summary>زمان آخرین به‌روزرسانی داده‌های SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>BodyShop reports (SAP)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_GetSapDataUpdateTimes</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/sap-data-update-times
    /// </remarks>
    [HttpGet("sap-data-update-times")]
    [Authorize(PfPermissions.BodyReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SapDataUpdateTimesRow>))]
    public Task<ListResultDto<SapDataUpdateTimesRow>> GetSapDataUpdateTimesAsync()
        => _service.GetSapDataUpdateTimesAsync();

    /// <summary>شناسه آخرین تغییر مجوزها (برای به‌روزرسانی خودکار گزارش)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>RealTimeDetectChangeOnLicense (جایگزین SignalR)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_GetLastChangeOnShortageLicense</c>.
    /// کلاینت به‌صورت دوره‌ای فراخوانی می‌کند و در صورت تغییر مقدار، گزارش را دوباره بارگذاری می‌کند.
    ///
    ///     GET /api/production-feasibility/body-shop/reports/last-change-on-license
    /// </remarks>
    [HttpGet("last-change-on-license")]
    [Authorize(PfPermissions.BodyReports.Tracking)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(string))]
    public Task<string?> GetLastChangeOnLicenseAsync()
        => _service.GetLastChangeOnLicenseAsync();

}
