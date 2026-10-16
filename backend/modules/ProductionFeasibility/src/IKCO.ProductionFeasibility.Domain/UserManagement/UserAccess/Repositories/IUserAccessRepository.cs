using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess.Repositories;

/// <summary>دسترسی کاربران به خطوط و سالن‌ها (frmUserManagement) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IUserAccessRepository
{
    /// <summary>فهرست کاربران امکان تولید (frmUserManagement) — <c>PF.GetAllUsers</c></summary>
    Task<TabularResult> GetAllUsersAsync();

    /// <summary>فهرست دسترسی کاربران به خطوط (frmUserManagement) — <c>PF.GetUsersAccessList</c></summary>
    Task<TabularResult> GetUsersAccessListAsync();

    /// <summary>سایت‌های مجازی (frmUserManagement) — <c>PF.GetSiteByDomainId</c></summary>
    Task<TabularResult> GetSitesAsync(string? personalCode, int? saloonTypeId);

    /// <summary>خطوط/سالن‌های یک سایت مجازی (frmUserManagement) — <c>PF.GetSaloonByVirtualPlantId</c></summary>
    Task<TabularResult> GetSaloonsByVirtualPlantAsync(int plantId);

    /// <summary>حذف دسترسی کاربران (frmUserManagement (حذف)) — <c>PF.S_DeleteUserAccess</c></summary>
    Task<GlobalResultDto?> DeleteUserAccessAsync(string idStr);

}
