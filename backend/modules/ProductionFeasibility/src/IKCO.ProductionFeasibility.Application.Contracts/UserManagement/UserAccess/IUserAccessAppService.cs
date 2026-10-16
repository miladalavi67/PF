using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

/// <summary>دسترسی کاربران به خطوط و سالن‌ها (frmUserManagement)</summary>
public partial interface IUserAccessAppService : IApplicationService
{
    /// <summary>فهرست کاربران امکان تولید</summary>
    Task<TabularResult> GetAllUsersAsync();

    /// <summary>فهرست دسترسی کاربران به خطوط</summary>
    Task<TabularResult> GetUsersAccessListAsync();

    /// <summary>سایت‌های مجازی</summary>
    Task<TabularResult> GetSitesAsync(UserAccessGetSitesInput input);

    /// <summary>خطوط/سالن‌های یک سایت مجازی</summary>
    Task<TabularResult> GetSaloonsByVirtualPlantAsync(UserAccessGetSaloonsByVirtualPlantInput input);

    /// <summary>حذف دسترسی کاربران</summary>
    Task<GlobalResultDto> DeleteUserAccessAsync(UserAccessDeleteUserAccessInput input);

}
