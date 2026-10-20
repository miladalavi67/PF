using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

/// <summary>گزارش‌های امکان تولید سنکرون</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.SynchronGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/synchron/reports")]
public partial class SyncReportsController : PfController
{
    private readonly ISyncReportsAppService _service;

    public SyncReportsController(ISyncReportsAppService service)
    {
        _service = service;
    }

    /// <summary>گزارش امکان تولید سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProductionPossibility (سنکرون)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RptCriticalPartsProduction_Synchron</c>.
    ///
    ///     POST /api/production-feasibility/synchron/reports/production-possibility
    ///     { "saloonId": "string", "crisisTypeId": 0, "criticalPartTypeId": 0, "subClassId": 0, "partNo": "string", "carModelId": 0, "carClassId": 0 }
    /// </remarks>
    [HttpPost("production-possibility")]
    [Authorize(PfPermissions.SyncReports.Productionpossibility)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ProductionPossibilityRow>))]
    public Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityAsync([FromBody] SyncReportsGetProductionPossibilityInput input)
        => _service.GetProductionPossibilityAsync(input);

    /// <summary>اقلام بحرانی دریافتی از SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>SyncPartsReports/Rpt_SyncCriticalPartsSetBasis (بک‌اند قبلی)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_Web_SAP_CriticalPartsMaster_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/synchron/reports/sap-critical-parts-master
    /// </remarks>
    [HttpGet("sap-critical-parts-master")]
    [Authorize(PfPermissions.SyncReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SapCriticalPartsMasterRow>))]
    public Task<ListResultDto<SapCriticalPartsMasterRow>> GetSapCriticalPartsMasterAsync([FromQuery] SyncReportsGetSapCriticalPartsMasterInput input)
        => _service.GetSapCriticalPartsMasterAsync(input);

    /// <summary>رزروهای دریافتی از SAP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>SyncPartsReports/RptSyncParts_SAP_ReserveMaster (بک‌اند قبلی)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_Web_SAP_ReserveMaster_SelectAll</c>.
    ///
    ///     GET /api/production-feasibility/synchron/reports/sap-reserve-master
    /// </remarks>
    [HttpGet("sap-reserve-master")]
    [Authorize(PfPermissions.SyncReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SapReserveMasterRow>))]
    public Task<ListResultDto<SapReserveMasterRow>> GetSapReserveMasterAsync([FromQuery] SyncReportsGetSapReserveMasterInput input)
        => _service.GetSapReserveMasterAsync(input);

    /// <summary>گزارش ASN (اعلان ورود محموله)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>SyncPartsReports/Rpt_SyncPartsASN (بک‌اند قبلی)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ASNReport</c>.
    ///
    ///     GET /api/production-feasibility/synchron/reports/asn
    /// </remarks>
    [HttpGet("asn")]
    [Authorize(PfPermissions.SyncReports.Sap)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<AsnRow>))]
    public Task<ListResultDto<AsnRow>> GetAsnReportAsync([FromQuery] SyncReportsGetAsnReportInput input)
        => _service.GetAsnReportAsync(input);

}
