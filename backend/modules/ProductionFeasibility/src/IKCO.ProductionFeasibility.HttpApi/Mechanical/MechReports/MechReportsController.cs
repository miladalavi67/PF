using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports;

/// <summary>گزارش‌های امکان تولید مکانیکی</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/reports")]
public partial class MechReportsController : PfController
{
    private readonly IMechReportsAppService _service;

    public MechReportsController(IMechReportsAppService service)
    {
        _service = service;
    }

    /// <summary>گزارش امکان تولید مکانیکی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProductionPossibility / frmRPTProdpossibility</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RptCriticalPartsProduction_Mechanical</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/reports/production-possibility
    ///     { "saloonId": "string", "crisisTypeId": 0, "criticalPartTypeId": 0, "subClassId": 0, "partNo": "string", "carModelId": 0, "carClassId": 0 }
    /// </remarks>
    [HttpPost("production-possibility")]
    [Authorize(PfPermissions.MechReports.Productionpossibility)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductionPossibilityAsync([FromBody] MechReportsGetProductionPossibilityInput input)
        => _service.GetProductionPossibilityAsync(input);

    /// <summary>گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProductionPossibility (با مجوز)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/reports/production-possibility-with-license
    ///     { "saloonId": "string", "crisisTypeId": 0, "criticalPartTypeId": 0, "subClassId": 0, "carModelId": 0, "carClassId": 0 }
    /// </remarks>
    [HttpPost("production-possibility-with-license")]
    [Authorize(PfPermissions.MechReports.Productionpossibility)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductionPossibilityWithLicenseAsync([FromBody] MechReportsGetProductionPossibilityWithLicenseInput input)
        => _service.GetProductionPossibilityWithLicenseAsync(input);

    /// <summary>امکان تولید به تفکیک سالن مونتاژ</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProdpossibility</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RPT_DESC_ProdPossibilityByTrimShop</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/production-possibility-by-trim-shop
    /// </remarks>
    [HttpGet("production-possibility-by-trim-shop")]
    [Authorize(PfPermissions.MechReports.Productionpossibility)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductionPossibilityByTrimShopAsync([FromQuery] MechReportsGetProductionPossibilityByTrimShopInput input)
        => _service.GetProductionPossibilityByTrimShopAsync(input);

    /// <summary>گزارش جامع اقلام بحرانی (مکانیکی و سنکرون)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProdpossibility (جامع)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsGeneralReport</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/general
    /// </remarks>
    [HttpGet("general")]
    [Authorize(PfPermissions.MechReports.General)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetGeneralReportAsync([FromQuery] MechReportsGetGeneralReportInput input)
        => _service.GetGeneralReportAsync(input);

    /// <summary>بدنه‌های در راه / وارد شده به خط برای یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_RPT_Des_IncommingBodiesByCPId</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RPT_IncommingBodiesByCriticalPartId</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/incoming-bodies
    /// </remarks>
    [HttpGet("incoming-bodies")]
    [Authorize(PfPermissions.MechReports.Incoming)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetIncomingBodiesByCriticalPartAsync([FromQuery] MechReportsGetIncomingBodiesByCriticalPartInput input)
        => _service.GetIncomingBodiesByCriticalPartAsync(input);

    /// <summary>فهرست بدنه‌های وارد شده برای یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_RPT_ST_IncommingBodiesByCP</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RPT_IncommingBodiesListByCriticalPartId</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/incoming-body-list
    /// </remarks>
    [HttpGet("incoming-body-list")]
    [Authorize(PfPermissions.MechReports.Incoming)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetIncomingBodyListAsync([FromQuery] MechReportsGetIncomingBodyListInput input)
        => _service.GetIncomingBodyListAsync(input);

    /// <summary>خلاصه بدنه‌های وارد شده به خط</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_RPT_ST_IncommingBodiesByCP</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RPT_IncommingBodiesByCriticalParts</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/incoming-bodies-summary
    /// </remarks>
    [HttpGet("incoming-bodies-summary")]
    [Authorize(PfPermissions.MechReports.Incoming)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetIncomingBodiesSummaryAsync([FromQuery] MechReportsGetIncomingBodiesSummaryInput input)
        => _service.GetIncomingBodiesSummaryAsync(input);

    /// <summary>تاریخچه موجودی شماره فنی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_Rpt_CriticalPartsStockLog</c>.
    /// رویه ذخیره‌شده: <c>dbo.SEXT_CriticalPartsStockLog</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/stock-log
    /// </remarks>
    [HttpGet("stock-log")]
    [Authorize(PfPermissions.MechReports.Stocklog)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetStockLogAsync([FromQuery] MechReportsGetStockLogInput input)
        => _service.GetStockLogAsync(input);

    /// <summary>لاگ انتقال اقلام بحرانی از SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_Rpt_SapCriticalLog</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_SapCriticalLog</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/sap-critical-log
    /// </remarks>
    [HttpGet("sap-critical-log")]
    [Authorize(PfPermissions.MechReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetSapCriticalLogAsync([FromQuery] MechReportsGetSapCriticalLogInput input)
        => _service.GetSapCriticalLogAsync(input);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_Rpt_FailedTransfer_Sap</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_FailedPartTransferd_Sap_Log</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/failed-sap-transfers
    /// </remarks>
    [HttpGet("failed-sap-transfers")]
    [Authorize(PfPermissions.MechReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetFailedSapTransfersAsync([FromQuery] MechReportsGetFailedSapTransfersInput input)
        => _service.GetFailedSapTransfersAsync(input);

    /// <summary>گزارش ASN (اعلان ورود محموله)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frm_Rpt_Sap_Asn / frmIncommingOrdersASNDetail</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ASNReport</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/asn
    /// </remarks>
    [HttpGet("asn")]
    [Authorize(PfPermissions.MechReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetAsnReportAsync([FromQuery] MechReportsGetAsnReportInput input)
        => _service.GetAsnReportAsync(input);

    /// <summary>دلایل عدم امکان ورود خودرو به مونتاژ</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTNotPossibleProductsToStepTrimShop</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_TrimInNotPossibleProductsSelectAll</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/not-possible-to-trim-in
    /// </remarks>
    [HttpGet("not-possible-to-trim-in")]
    [Authorize(PfPermissions.MechReports.Conflicts)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetNotPossibleProductsToTrimStepAsync([FromQuery] MechReportsGetNotPossibleProductsToTrimStepInput input)
        => _service.GetNotPossibleProductsToTrimStepAsync(input);

    /// <summary>تعداد بدنه‌های ورودی به مونتاژ پس از ثبت</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmTrimShopBodyCount</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_TrimShopBodyCount_List</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/trim-shop-body-count
    /// </remarks>
    [HttpGet("trim-shop-body-count")]
    [Authorize(PfPermissions.MechReports.Conflicts)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetTrimShopBodyCountAsync([FromQuery] MechReportsGetTrimShopBodyCountInput input)
        => _service.GetTrimShopBodyCountAsync(input);

    /// <summary>بدنه‌های بحرانی ورودی به مونتاژ</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProductsToStepTrimShop</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_TrimShopCriticalBody_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/reports/trim-shop-critical-bodies
    /// </remarks>
    [HttpGet("trim-shop-critical-bodies")]
    [Authorize(PfPermissions.MechReports.Conflicts)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetTrimShopCriticalBodiesAsync([FromQuery] MechReportsGetTrimShopCriticalBodiesInput input)
        => _service.GetTrimShopCriticalBodiesAsync(input);

}
