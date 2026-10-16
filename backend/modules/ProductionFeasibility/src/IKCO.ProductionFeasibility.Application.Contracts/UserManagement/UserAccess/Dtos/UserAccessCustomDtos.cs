using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;

/// <summary>دسترسی یک کاربر به یک خط بدنه‌سازی</summary>
public class UserAccessItem
{
    /// <summary>شناسه کاربر (PF.B_User.Id)</summary>
    [Range(1, int.MaxValue)]
    public int UserId { get; set; }

    /// <summary>شناسه خط بدنه‌سازی (BIWSiteId)</summary>
    [Range(1, int.MaxValue)]
    public int BIWSiteId { get; set; }
}

/// <summary>ورودی ثبت دسترسی کاربران به خطوط (PF.S_InsertUserAccess)</summary>
public class UserAccessInsertInput
{
    /// <summary>دسترسی‌ها</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(1000)]
    public List<UserAccessItem> Items { get; set; } = new();
}
