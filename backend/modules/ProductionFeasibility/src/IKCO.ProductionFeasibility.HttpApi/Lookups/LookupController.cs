using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Lookups.Dtos;
using IKCO.ProductionFeasibility.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Lookups;

/// <summary>فهرست‌های مرجع</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.CommonGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/lookups")]
public class LookupController : PfController
{
    private readonly ILookupAppService _service;

    public LookupController(ILookupAppService service)
    {
        _service = service;
    }

    /// <summary>فهرست کلیدهای فهرست‌های مرجع</summary>
    /// <remarks>
    ///     GET /api/production-feasibility/lookups
    /// </remarks>
    [HttpGet]
    [Authorize(PfPermissions.Lookups.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<LookupDescriptor>))]
    public Task<ListResultDto<LookupDescriptor>> GetCatalogAsync() => _service.GetCatalogAsync();

    /// <summary>ردیف‌های یک فهرست مرجع</summary>
    /// <remarks>
    /// فهرست‌های وابسته (مدل، کلاس، زیرکلاس، خط بدنه‌سازی، سالن مونتاژ) با <c>parentId</c> فیلتر می‌شوند.
    ///
    ///     GET /api/production-feasibility/lookups/car-models?parentId=3
    ///     { "items": [ { "id": 12, "code": "A1", "title": "مدل نمونه" } ] }
    /// </remarks>
    [HttpGet("{key}")]
    [Authorize(PfPermissions.Lookups.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<LookupRow>))]
    public Task<ListResultDto<LookupRow>> GetAsync(string key, [FromQuery] GetLookupInput input) => _service.GetAsync(key, input);
}
