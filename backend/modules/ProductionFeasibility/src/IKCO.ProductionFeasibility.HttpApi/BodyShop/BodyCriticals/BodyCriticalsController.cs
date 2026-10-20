using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

/// <summary>بحران‌های بدنه‌سازی (ثبت، ویرایش، غیرفعال‌سازی)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.BodyShopGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/body-shop/critical-parts")]
public partial class BodyCriticalsController : PfController
{
    private readonly IBodyCriticalsAppService _service;

    public BodyCriticalsController(IBodyCriticalsAppService service)
    {
        _service = service;
    }

    /// <summary>اطلاعات بدنه</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>uctBodyInfo / DA_Critical.GetCarInfo</c>.
    /// رویه ذخیره‌شده: <c>dbo.SEXT_BodyInfo_V2</c>.
    /// در صورت نبودن بدنه یا نبودن دسترسی کاربر به خط بدنه‌سازی آن، پاسخ خالی (204) است.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/body-info
    /// </remarks>
    [HttpGet("body-info")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(BodyInfoRow))]
    public Task<BodyInfoRow?> GetBodyInfoAsync([FromQuery] BodyCriticalsGetBodyInfoInput input)
        => _service.GetBodyInfoAsync(input);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>uctPartNo / SelectPartDescByPartNo</c>.
    /// رویه ذخیره‌شده: <c>dbo.SIMP_SelectPartDescByPartNo</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/part-description
    /// </remarks>
    [HttpGet("part-description")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(string))]
    public Task<string?> GetPartDescriptionAsync([FromQuery] BodyCriticalsGetPartDescriptionInput input)
        => _service.GetPartDescriptionAsync(input);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی در یک خط بدنه‌سازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>BL_BodyCritical.ProductByPartNo_SelectAll</c>.
    /// رویه ذخیره‌شده: <c>PF.SelectPartProductByPartNo</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/part-products
    /// </remarks>
    [HttpGet("part-products")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<PartProductRow>))]
    public Task<ListResultDto<PartProductRow>> GetPartProductsAsync([FromQuery] BodyCriticalsGetPartProductsInput input)
        => _service.GetPartProductsAsync(input);

    /// <summary>کدهای کلاس مصرف‌کننده شماره فنی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUploadParts (ثبت گروهی)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_GetCodeClassByPartNo</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/code-class-by-part-no
    /// </remarks>
    [HttpGet("code-class-by-part-no")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CodeClassRow>))]
    public Task<ListResultDto<CodeClassRow>> GetCodeClassByPartNoAsync([FromQuery] BodyCriticalsGetCodeClassByPartNoInput input)
        => _service.GetCodeClassByPartNoAsync(input);

    /// <summary>ثبت بحران بدنه‌سازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmBodyCriticalParts.btnSave</c>.
    /// رویه ذخیره‌شده: <c>PF.S_CreateCriticalParts_V3</c>.
    /// مقدار Result در پاسخ، شناسه بحران ثبت‌شده است و برای ثبت محصولات (products) استفاده می‌شود.
    ///
    ///     POST /api/production-feasibility/body-shop/critical-parts
    ///     { "carId": 0, "partNo": "string", "partDesc": "string", "partGroup": "string", "alterNativePartNo": "string", "criticalPartNote": "string", "biwSiteId": 0, "totalStock": 0, "rawStock": 0, "productionStateNote": "string", "letterNo": "string", "isOutOfSAP": false }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.BodyCriticals.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> CreateCriticalPartAsync([FromBody] BodyCriticalsCreateCriticalPartInput input)
        => _service.CreateCriticalPartAsync(input);

    /// <summary>ویرایش بحران بدنه‌سازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmBodyCriticalParts (حالت ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_Web_UpdateCriticalParts</c>.
    ///
    ///     PUT /api/production-feasibility/body-shop/critical-parts/{criticalPartsId:int}
    ///     { "partGroup": "string", "alterNativePartNo": "string", "criticalPartNote": "string", "rawStock": 0, "productionStateNote": "string", "letterNo": "string" }
    /// </remarks>
    [HttpPut("{criticalPartsId:int}")]
    [Authorize(PfPermissions.BodyCriticals.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> UpdateCriticalPartAsync(int criticalPartsId, [FromBody] BodyCriticalsUpdateCriticalPartInput input)
        => _service.UpdateCriticalPartAsync(criticalPartsId, input);

    /// <summary>اطلاعات یک بحران بدنه‌سازی برای ویرایش</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>BL_BodyCritical.CriticalPartsInfo (frmBodyCriticalParts در حالت ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_CriticalPartsInfo</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/{criticalPartsId:int}/info
    /// </remarks>
    [HttpGet("{criticalPartsId:int}/info")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BodyCriticalPartInfoRow>))]
    public Task<ListResultDto<BodyCriticalPartInfoRow>> GetCriticalPartInfoAsync(int criticalPartsId)
        => _service.GetCriticalPartInfoAsync(criticalPartsId);

    /// <summary>گزارش رویدادهای یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmEventLog</c>.
    /// رویه ذخیره‌شده: <c>PF.S_EventLogList</c>.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/event-log
    /// </remarks>
    [HttpGet("event-log")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<EventLogRow>))]
    public Task<ListResultDto<EventLogRow>> GetEventLogAsync([FromQuery] BodyCriticalsGetEventLogInput input)
        => _service.GetEventLogAsync(input);

}
