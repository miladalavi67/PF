using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

/// <summary>شرایط تولید و مجوزهای تولید با کسری</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/licenses")]
public partial class MechLicensesController : PfController
{
    private readonly IMechLicensesAppService _service;

    public MechLicensesController(IMechLicensesAppService service)
    {
        _service = service;
    }

    /// <summary>اطلاعات ردیابی مجوزهای یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalShortageLicenseTracking</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ShortageLicenseSelectAllTrackingInfo</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/licenses/tracking-info
    /// </remarks>
    [HttpGet("tracking-info")]
    [Authorize(PfPermissions.MechLicenses.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetTrackingInfoAsync([FromQuery] MechLicensesGetTrackingInfoInput input)
        => _service.GetTrackingInfoAsync(input);

    /// <summary>تاریخچه مجوزهای یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmLicenseLogs</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ShortageLicenseTrackingSelectAll</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/licenses/tracking
    /// </remarks>
    [HttpGet("tracking")]
    [Authorize(PfPermissions.MechLicenses.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetTrackingListAsync([FromQuery] MechLicensesGetTrackingListInput input)
        => _service.GetTrackingListAsync(input);

    /// <summary>گزارش پارامترهای تولید با کسری تعریف‌شده</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmShortageLicenseTracking</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ShortageLicenseTrackingSearch</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/licenses/tracking/search
    ///     { "fromDate": "string", "toDate": "string", "partNo": "string", "setCode": "string" }
    /// </remarks>
    [HttpPost("tracking/search")]
    [Authorize(PfPermissions.MechLicenses.Report)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> SearchTrackingAsync([FromBody] MechLicensesSearchTrackingInput input)
        => _service.SearchTrackingAsync(input);

    /// <summary>بحران‌های دارای مجوز تولید با کسری</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalShortageLicenseTracking</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ShortagelicenseTrackingSearchCriticals</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/licenses/tracking/search-criticals
    ///     { "fromDate": "string", "toDate": "string", "partNo": "string", "setCode": "string" }
    /// </remarks>
    [HttpPost("tracking/search-criticals")]
    [Authorize(PfPermissions.MechLicenses.Report)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> SearchTrackingCriticalsAsync([FromBody] MechLicensesSearchTrackingCriticalsInput input)
        => _service.SearchTrackingCriticalsAsync(input);

    /// <summary>بدنه‌های مصرف‌کننده مجوز تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmBodiesRelatedToUsedLicense</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_SelectBodiesRelatedToUsedLicense</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/licenses/used-license-bodies
    /// </remarks>
    [HttpGet("used-license-bodies")]
    [Authorize(PfPermissions.MechLicenses.Report)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetBodiesRelatedToUsedLicenseAsync([FromQuery] MechLicensesGetBodiesRelatedToUsedLicenseInput input)
        => _service.GetBodiesRelatedToUsedLicenseAsync(input);

}
