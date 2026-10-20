using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechReasons.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons;

/// <summary>دلایل عدم امکان برنامه‌شدن / ورود (frmNotPossibleProductReason)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/not-possible-reasons")]
public partial class MechReasonsController : PfController
{
    private readonly IMechReasonsAppService _service;

    public MechReasonsController(IMechReasonsAppService service)
    {
        _service = service;
    }

    /// <summary>فهرست دلایل عدم امکان تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmNotPossibleProductReason</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ProgNotPossibilityReasonSelectAll</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/not-possible-reasons
    /// </remarks>
    [HttpGet]
    [Authorize(PfPermissions.MechReasons.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<NotPossibleReasonRow>))]
    public Task<ListResultDto<NotPossibleReasonRow>> GetListAsync([FromQuery] MechReasonsGetListInput input)
        => _service.GetListAsync(input);

    /// <summary>ثبت دلیل عدم امکان تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmNotPossibleProductReason</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ProgNotPossibilityReason_Insert</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/not-possible-reasons
    ///     { "code": "string", "descF": "string", "descE": "string", "status": 0 }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.MechReasons.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task CreateAsync([FromBody] MechReasonsCreateInput input)
        => _service.CreateAsync(input);

    /// <summary>ویرایش دلیل عدم امکان تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmNotPossibleProductReason</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ProgNotPossibilityReason_Update</c>.
    ///
    ///     PUT /api/production-feasibility/mechanical/not-possible-reasons/{id:int}
    ///     { "code": "string", "descF": "string", "descE": "string", "status": 0 }
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(PfPermissions.MechReasons.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task UpdateAsync(int id, [FromBody] MechReasonsUpdateInput input)
        => _service.UpdateAsync(id, input);

    /// <summary>حذف منطقی دلیل عدم امکان تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmNotPossibleProductReason</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_ProgNotPossibilityReason_LogicalDelete</c>.
    ///
    ///     DELETE /api/production-feasibility/mechanical/not-possible-reasons/{id:int}
    /// </remarks>
    [HttpDelete("{id:int}")]
    [Authorize(PfPermissions.MechReasons.Delete)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task DeleteAsync(int id)
        => _service.DeleteAsync(id);

}
