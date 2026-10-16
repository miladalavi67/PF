using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;

/// <summary>ورودی «سایت‌های مجازی» — <c>PF.GetSiteByDomainId</c></summary>
public class UserAccessGetSitesInput
{
    /// <summary>کد پرسنلی</summary>
    [StringLength(20)]
    public string? PersonalCode { get; set; }

    /// <summary>نوع سالن</summary>
    public int? SaloonTypeId { get; set; }

}

/// <summary>ورودی «خطوط/سالن‌های یک سایت مجازی» — <c>PF.GetSaloonByVirtualPlantId</c></summary>
public class UserAccessGetSaloonsByVirtualPlantInput
{
    /// <summary>سایت مجازی</summary>
    public int PlantId { get; set; }

}

/// <summary>ورودی «حذف دسترسی کاربران» — <c>PF.S_DeleteUserAccess</c></summary>
public class UserAccessDeleteUserAccessInput
{
    /// <summary>شناسه ردیف‌های دسترسی</summary>
    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    public List<int> Ids { get; set; } = new();

}
