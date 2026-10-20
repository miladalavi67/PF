using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechExceptions.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions;

/// <summary>استثناهای محصولات بحران و مجوز آن‌ها</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/exceptions")]
public partial class MechExceptionsController : PfController
{
    private readonly IMechExceptionsAppService _service;

    public MechExceptionsController(IMechExceptionsAppService service)
    {
        _service = service;
    }

    /// <summary>فهرست بدنه‌های استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsProductExceptions</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalProductExceptionList</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/exceptions/search
    ///     { "bodyNo": "string", "dateFrom": "string", "dateEnd": "string", "state": 0 }
    /// </remarks>
    [HttpPost("search")]
    [Authorize(PfPermissions.MechExceptions.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ProductExceptionRow>))]
    public Task<ListResultDto<ProductExceptionRow>> GetExceptionListAsync([FromBody] MechExceptionsGetExceptionListInput input)
        => _service.GetExceptionListAsync(input);

    /// <summary>مجوزهای استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsProductExceptionsLicense</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsProductExceptionsLisenceList</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/exceptions/{id:int}/licenses
    /// </remarks>
    [HttpGet("{id:int}/licenses")]
    [Authorize(PfPermissions.MechExceptions.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ExceptionLicenseRow>))]
    public Task<ListResultDto<ExceptionLicenseRow>> GetLicensesAsync(int id)
        => _service.GetLicensesAsync(id);

    /// <summary>بدنه‌های مصرف‌کننده مجوز استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsProductExceptionsLicense</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsProductExceptionsLicenseBodyNoList</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/exceptions/{id:int}/license-bodies
    /// </remarks>
    [HttpGet("{id:int}/license-bodies")]
    [Authorize(PfPermissions.MechExceptions.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ExceptionLicenseBodyRow>))]
    public Task<ListResultDto<ExceptionLicenseBodyRow>> GetLicenseBodiesAsync(int id)
        => _service.GetLicenseBodiesAsync(id);

    /// <summary>تاریخچه مجوزهای استثنا برای شماره فنی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmLicenseLogs</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsProductExceptionsLisenceLog</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/exceptions/license-log
    /// </remarks>
    [HttpGet("license-log")]
    [Authorize(PfPermissions.MechExceptions.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ExceptionLicenseRow>))]
    public Task<ListResultDto<ExceptionLicenseRow>> GetLicenseLogAsync([FromQuery] MechExceptionsGetLicenseLogInput input)
        => _service.GetLicenseLogAsync(input);

    /// <summary>ثبت مجوز برای استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartsProductExceptionsLicense</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_InsertCriticalPartsProductExceptionsLicense</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/exceptions/licenses
    ///     { "criticalPartsProductExceptionsId": 0, "possibleLicenseCount": 0, "letterNo": "string", "description": "string" }
    /// </remarks>
    [HttpPost("licenses")]
    [Authorize(PfPermissions.MechExceptions.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task CreateLicenseAsync([FromBody] MechExceptionsCreateLicenseInput input)
        => _service.CreateLicenseAsync(input);

    /// <summary>غیرفعال‌سازی مجوز استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmDisableCriticalPartsProductExceptions</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_DisableCriticalPartsProductExceptionsLicense</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/exceptions/licenses/{id:int}/disable
    /// </remarks>
    [HttpPost("licenses/{id:int}/disable")]
    [Authorize(PfPermissions.MechExceptions.Disable)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task DisableLicenseAsync(int id)
        => _service.DisableLicenseAsync(id);

    /// <summary>غیرفعال‌سازی استثناها</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmDisableCriticalPartsProductExceptions</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_DisableCriticalPartsProductsException</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/exceptions/disable
    ///     { "ids": "string" }
    /// </remarks>
    [HttpPost("disable")]
    [Authorize(PfPermissions.MechExceptions.Disable)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task DisableExceptionsAsync([FromBody] MechExceptionsDisableExceptionsInput input)
        => _service.DisableExceptionsAsync(input);

    /// <summary>گزارش شماره فنی‌های دارای استثنا</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPT_CriticalPartsProductExceptions</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/exceptions/part-numbers-report
    ///     { "filterValues": "string", "trimId": 0, "filterType": 0, "partNumber": "string" }
    /// </remarks>
    [HttpPost("part-numbers-report")]
    [Authorize(PfPermissions.MechExceptions.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<ExceptionPartNumberRow>))]
    public Task<ListResultDto<ExceptionPartNumberRow>> GetPartNumbersInExceptionsAsync([FromBody] MechExceptionsGetPartNumbersInExceptionsInput input)
        => _service.GetPartNumbersInExceptionsAsync(input);

}
