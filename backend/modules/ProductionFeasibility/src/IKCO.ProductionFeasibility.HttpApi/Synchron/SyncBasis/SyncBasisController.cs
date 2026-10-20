using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis;

/// <summary>تعریف مبنای ست‌های سنکرون (frmCriticalPartSetBasis)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.SynchronGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/synchron/basis")]
public partial class SyncBasisController : PfController
{
    private readonly ISyncBasisAppService _service;

    public SyncBasisController(ISyncBasisAppService service)
    {
        _service = service;
    }

    /// <summary>فهرست مبناهای ست سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RptGetCriticalPartsSet</c>.
    ///
    ///     GET /api/production-feasibility/synchron/basis
    /// </remarks>
    [HttpGet]
    [Authorize(PfPermissions.SyncBasis.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SetBasisReportRow>))]
    public Task<ListResultDto<SetBasisReportRow>> GetListAsync()
        => _service.GetListAsync();

    /// <summary>مبنای ست بر اساس کد</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetBasisSelectByCode</c>.
    ///
    ///     GET /api/production-feasibility/synchron/basis/by-code
    /// </remarks>
    [HttpGet("by-code")]
    [Authorize(PfPermissions.SyncBasis.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SetBasisRow>))]
    public Task<ListResultDto<SetBasisRow>> GetByCodeAsync([FromQuery] SyncBasisGetByCodeInput input)
        => _service.GetByCodeAsync(input);

    /// <summary>مبنای ست بر اساس شناسه</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetBasisSelectById</c>.
    ///
    ///     GET /api/production-feasibility/synchron/basis/{id}
    /// </remarks>
    [HttpGet("{id}")]
    [Authorize(PfPermissions.SyncBasis.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SetBasisRow>))]
    public Task<ListResultDto<SetBasisRow>> GetByIdAsync(string id)
        => _service.GetByIdAsync(id);

    /// <summary>مبناهای ست بر اساس وضعیت</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetBasisSelectByStatus</c>.
    ///
    ///     GET /api/production-feasibility/synchron/basis/by-status
    /// </remarks>
    [HttpGet("by-status")]
    [Authorize(PfPermissions.SyncBasis.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SetBasisRow>))]
    public Task<ListResultDto<SetBasisRow>> GetByStatusAsync([FromQuery] SyncBasisGetByStatusInput input)
        => _service.GetByStatusAsync(input);

    /// <summary>تعداد ست‌های ثبت‌شده با کد ست</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetCountSetCode</c>.
    /// تنها ستون خروجی رویه (n) به صورت عدد برگردانده می‌شود.
    ///
    ///     GET /api/production-feasibility/synchron/basis/count-by-code
    /// </remarks>
    [HttpGet("count-by-code")]
    [Authorize(PfPermissions.SyncBasis.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(int))]
    public Task<int> CountBySetCodeAsync([FromQuery] SyncBasisCountBySetCodeInput input)
        => _service.CountBySetCodeAsync(input);

    /// <summary>ثبت ویژگی نوع قطعه سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>SyncParts/SyncPartsInsertCriticalPartTypeProperty (بک‌اند قبلی)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_InsertCriticalPartTypeProperty</c>.
    ///
    ///     POST /api/production-feasibility/synchron/basis/part-type-properties
    ///     { "partTypeCode": 0, "propertyId": 0 }
    /// </remarks>
    [HttpPost("part-type-properties")]
    [Authorize(PfPermissions.SyncBasis.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task AddPartTypePropertyAsync([FromBody] SyncBasisAddPartTypePropertyInput input)
        => _service.AddPartTypePropertyAsync(input);

    /// <summary>ثبت مبنای ست سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis.btnSave</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_InsertCriticalPartSet</c>.
    ///
    ///     POST /api/production-feasibility/synchron/basis
    ///     { "code": "string", "descF": "string", "carClass": 0, "partTypeId": 0, "levelType": 0, "isTrim": false, "isOption": false, "isToodoozi": false, "isColor": false }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.SyncBasis.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task CreateAsync([FromBody] SyncBasisCreateInput input)
        => _service.CreateAsync(input);

    /// <summary>ویرایش مبنای ست سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis.btnSave (ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_UpdateCriticalPartsSet</c>.
    ///
    ///     PUT /api/production-feasibility/synchron/basis/{id:int}
    ///     { "code": "string", "descF": "string", "carClassId": 0, "partTypeId": 0, "levelType": 0, "status": false, "isTrim": false, "isColor": false, "isOption": false, "isToodoozi": false }
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(PfPermissions.SyncBasis.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task UpdateAsync(int id, [FromBody] SyncBasisUpdateInput input)
        => _service.UpdateAsync(id, input);

}
