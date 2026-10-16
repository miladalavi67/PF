using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

public partial class UserAccessController
{
    /// <summary>ثبت دسترسی کاربران به خطوط بدنه‌سازی</summary>
    /// <remarks>
    /// معادل <c>DA_UserManagment.InsertUserAccess</c> (رویه <c>PF.S_InsertUserAccess</c>)؛ وضعیت، تاریخ و کاربر ثبت‌کننده از سرور تعیین می‌شود.
    ///
    ///     POST /api/production-feasibility/user-management/user-access
    ///     { "items": [ { "userId": 3, "biwSiteId": 2 } ] }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.UserAccess.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> InsertAsync([FromBody] UserAccessInsertInput input) => _service.InsertAsync(input);
}
