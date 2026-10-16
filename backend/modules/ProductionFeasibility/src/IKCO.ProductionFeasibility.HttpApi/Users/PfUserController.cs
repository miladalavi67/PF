using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Users.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Annotations;

namespace IKCO.ProductionFeasibility.Users;

/// <summary>کاربر جاری و کلیدهای دسترسی او</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.CommonGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/users")]
public class PfUserController : PfController
{
    private readonly IPfUserAppService _service;

    public PfUserController(IPfUserAppService service)
    {
        _service = service;
    }

    /// <summary>اطلاعات کاربر جاری در PF.B_User</summary>
    /// <remarks>
    /// کاربر از نام کاربری توکن (<c>sub</c>) به <c>PF.B_User</c> نگاشت می‌شود؛ تعریف‌نشده ← 403.
    ///
    ///     GET /api/production-feasibility/users/me
    ///     { "id": 3, "ikcoUserId": 45012, "userName": "77203085", "fullName": "علی رضایی", "isAdmin": false, "virtualPlantId": 1 }
    /// </remarks>
    [HttpGet("me")]
    [Authorize(PfPermissions.CurrentUser.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(PfUserInfoDto))]
    public Task<PfUserInfoDto> GetCurrentAsync() => _service.GetCurrentAsync();

    /// <summary>کلیدهای دسترسی کاربر جاری</summary>
    /// <remarks>
    /// برگ‌های درخت <c>PfPermissionTree</c> (API و UI) که کاربر جاری دارد؛ برای نمایش منوها و دکمه‌ها در کلاینت.
    /// ارزیابی با همان سیاست‌های IKCO.Auth که روی کنترلرها اعمال می‌شود.
    ///
    ///     GET /api/production-feasibility/users/me/permissions
    ///     [ "mechparts.view", "mechanical.menu.register" ]
    /// </remarks>
    [HttpGet("me/permissions")]
    [Authorize]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(List<string>))]
    public async Task<List<string>> GetCurrentPermissionsAsync()
    {
        var authorizationService = HttpContext.RequestServices.GetRequiredService<IAuthorizationService>();
        var granted = new List<string>();
        foreach (var code in PfPermissionTree.Api.Concat(PfPermissionTree.Ui).SelectMany(g => g.Leaves).Select(l => l.Code))
        {
            if ((await authorizationService.AuthorizeAsync(User, code)).Succeeded)
                granted.Add(code);
        }

        return granted;
    }
}
