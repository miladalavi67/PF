using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.UserManagement.Results;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

/// <summary>دسترسی کاربران به خطوط و سالن‌ها (frmUserManagement)</summary>
public partial interface IUserAccessAppService : IApplicationService
{
    /// <summary>فهرست کاربران امکان تولید</summary>
    Task<ListResultDto<PfUserRow>> GetAllUsersAsync();

    /// <summary>فهرست دسترسی کاربران به خطوط</summary>
    Task<ListResultDto<UserAccessRow>> GetUsersAccessListAsync();

    /// <summary>سایت‌های مجازی</summary>
    Task<ListResultDto<VirtualPlantRow>> GetSitesAsync(UserAccessGetSitesInput input);

    /// <summary>خطوط/سالن‌های یک سایت مجازی</summary>
    Task<ListResultDto<BiwSiteRow>> GetSaloonsByVirtualPlantAsync(UserAccessGetSaloonsByVirtualPlantInput input);

    /// <summary>حذف دسترسی کاربران</summary>
    Task<GlobalResultDto> DeleteUserAccessAsync(UserAccessDeleteUserAccessInput input);

}
