using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.UserManagement.Results;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

/// <summary>دسترسی کاربران به خطوط و سالن‌ها (frmUserManagement)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.UserManagementGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/user-management/user-access")]
public partial class UserAccessController : PfController
{
    private readonly IUserAccessAppService _service;

    public UserAccessController(IUserAccessAppService service)
    {
        _service = service;
    }

    /// <summary>فهرست کاربران امکان تولید</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUserManagement</c>.
    /// رویه ذخیره‌شده: <c>PF.GetAllUsers</c>.
    ///
    ///     GET /api/production-feasibility/user-management/user-access/users
    /// </remarks>
    [HttpGet("users")]
    [Authorize(PfPermissions.UserAccess.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<PfUserRow>))]
    public Task<ListResultDto<PfUserRow>> GetAllUsersAsync()
        => _service.GetAllUsersAsync();

    /// <summary>فهرست دسترسی کاربران به خطوط</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUserManagement</c>.
    /// رویه ذخیره‌شده: <c>PF.GetUsersAccessList</c>.
    ///
    ///     GET /api/production-feasibility/user-management/user-access
    /// </remarks>
    [HttpGet]
    [Authorize(PfPermissions.UserAccess.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<UserAccessRow>))]
    public Task<ListResultDto<UserAccessRow>> GetUsersAccessListAsync()
        => _service.GetUsersAccessListAsync();

    /// <summary>سایت‌های مجازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUserManagement</c>.
    /// رویه ذخیره‌شده: <c>PF.GetSiteByDomainId</c>.
    ///
    ///     GET /api/production-feasibility/user-management/user-access/sites
    /// </remarks>
    [HttpGet("sites")]
    [Authorize(PfPermissions.UserAccess.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<VirtualPlantRow>))]
    public Task<ListResultDto<VirtualPlantRow>> GetSitesAsync([FromQuery] UserAccessGetSitesInput input)
        => _service.GetSitesAsync(input);

    /// <summary>خطوط/سالن‌های یک سایت مجازی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUserManagement</c>.
    /// رویه ذخیره‌شده: <c>PF.GetSaloonByVirtualPlantId</c>.
    ///
    ///     GET /api/production-feasibility/user-management/user-access/saloons-by-virtual-plant
    /// </remarks>
    [HttpGet("saloons-by-virtual-plant")]
    [Authorize(PfPermissions.UserAccess.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BiwSiteRow>))]
    public Task<ListResultDto<BiwSiteRow>> GetSaloonsByVirtualPlantAsync([FromQuery] UserAccessGetSaloonsByVirtualPlantInput input)
        => _service.GetSaloonsByVirtualPlantAsync(input);

    /// <summary>حذف دسترسی کاربران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmUserManagement (حذف)</c>.
    /// رویه ذخیره‌شده: <c>PF.S_DeleteUserAccess</c>.
    ///
    ///     POST /api/production-feasibility/user-management/user-access/delete
    ///     { "ids": "string" }
    /// </remarks>
    [HttpPost("delete")]
    [Authorize(PfPermissions.UserAccess.Delete)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> DeleteUserAccessAsync([FromBody] UserAccessDeleteUserAccessInput input)
        => _service.DeleteUserAccessAsync(input);

}
