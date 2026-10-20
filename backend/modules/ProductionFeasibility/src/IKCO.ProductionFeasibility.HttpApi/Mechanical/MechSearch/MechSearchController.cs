using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechSearch.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch;

/// <summary>جستجوی بحران‌ها (frmCriticalPartSearch / جستجوی سنکرون و مکانیکی)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/search")]
public partial class MechSearchController : PfController
{
    private readonly IMechSearchAppService _service;

    public MechSearchController(IMechSearchAppService service)
    {
        _service = service;
    }

    /// <summary>جستجوی اقلام بحرانی (مکانیکی و سنکرون)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSearch</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSearchByConditions</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/search/critical-parts
    ///     { "partNo": "string", "setCode": "string", "fromDate": "1405/01/01", "toDate": "1405/01/01", "trimShopIds": "string", "status": 0, "criticalPartTypeId": 0 }
    /// </remarks>
    [HttpPost("critical-parts")]
    [Authorize(PfPermissions.MechSearch.Search)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartSearchRow>))]
    public Task<ListResultDto<CriticalPartSearchRow>> SearchCriticalPartsAsync([FromBody] MechSearchSearchCriticalPartsInput input)
        => _service.SearchCriticalPartsAsync(input);

    /// <summary>جستجوی بحران‌ها بر اساس سالن و بازه تاریخ</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSearch (نسخه قدیم)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSearch</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/search/critical-parts-by-trim-shop
    /// </remarks>
    [HttpGet("critical-parts-by-trim-shop")]
    [Authorize(PfPermissions.MechSearch.Search)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<TrimShopCriticalPartRow>))]
    public Task<ListResultDto<TrimShopCriticalPartRow>> SearchByTrimShopAsync([FromQuery] MechSearchSearchByTrimShopInput input)
        => _service.SearchByTrimShopAsync(input);

    /// <summary>اقلام بحرانی فعال وارد شده به شرکت</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsArrived</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPart_RecievedParts</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/search/received-parts
    /// </remarks>
    [HttpGet("received-parts")]
    [Authorize(PfPermissions.MechSearch.Arrived)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ReceivedPartRow>))]
    public Task<ListResultDto<ReceivedPartRow>> GetReceivedPartsAsync([FromQuery] MechSearchGetReceivedPartsInput input)
        => _service.GetReceivedPartsAsync(input);

}
