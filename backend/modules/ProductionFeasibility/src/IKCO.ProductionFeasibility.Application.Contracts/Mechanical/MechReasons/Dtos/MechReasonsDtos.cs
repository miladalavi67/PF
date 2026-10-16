using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons.Dtos;

/// <summary>ورودی «فهرست دلایل عدم امکان تولید» — <c>dbo.S_ProgNotPossibilityReasonSelectAll</c></summary>
public class MechReasonsGetListInput
{
    /// <summary>نوع (1=فعال، 2=غیرفعال)</summary>
    public byte? SelectType { get; set; }

}

/// <summary>ورودی «ثبت دلیل عدم امکان تولید» — <c>dbo.S_ProgNotPossibilityReason_Insert</c></summary>
public class MechReasonsCreateInput
{
    /// <summary>کد</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(5)]
    public string Code { get; set; } = default!;

    /// <summary>شرح فارسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(500)]
    public string DescF { get; set; } = default!;

    /// <summary>شرح انگلیسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(500)]
    public string DescE { get; set; } = default!;

    /// <summary>وضعیت</summary>
    public byte Status { get; set; }

}

/// <summary>ورودی «ویرایش دلیل عدم امکان تولید» — <c>dbo.S_ProgNotPossibilityReason_Update</c></summary>
public class MechReasonsUpdateInput
{
    /// <summary>کد</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(5)]
    public string Code { get; set; } = default!;

    /// <summary>شرح فارسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(500)]
    public string DescF { get; set; } = default!;

    /// <summary>شرح انگلیسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(500)]
    public string DescE { get; set; } = default!;

    /// <summary>وضعیت</summary>
    public byte Status { get; set; }

}
