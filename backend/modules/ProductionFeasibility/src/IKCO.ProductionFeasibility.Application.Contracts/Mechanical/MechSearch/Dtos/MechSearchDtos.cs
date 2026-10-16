using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch.Dtos;

/// <summary>ورودی «جستجوی اقلام بحرانی (مکانیکی و سنکرون)» — <c>dbo.S_CriticalPartsSearchByConditions</c></summary>
public class MechSearchSearchCriticalPartsInput
{
    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

    /// <summary>کد ست</summary>
    [StringLength(15)]
    public string? SetCode { get; set; }

    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>شناسه سالن‌ها (جداشده با ویرگول، -1 = همه)</summary>
    [StringLength(500)]
    public string? TrimShopIds { get; set; }

    /// <summary>وضعیت (1=فعال، -1=همه)</summary>
    public int? Status { get; set; }

    /// <summary>نوع بحران (0=مکانیکی، 1=سنکرون، -1=همه)</summary>
    public int? CriticalPartTypeId { get; set; }

}

/// <summary>ورودی «جستجوی بحران‌ها بر اساس سالن و بازه تاریخ» — <c>dbo.S_CriticalPartSearch</c></summary>
public class MechSearchSearchByTrimShopInput
{
    /// <summary>شماره فنی</summary>
    [StringLength(500)]
    public string? PartNo { get; set; }

    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

    /// <summary>نوع</summary>
    public int Type { get; set; }

}

/// <summary>ورودی «اقلام بحرانی فعال وارد شده به شرکت» — <c>dbo.S_CriticalPart_RecievedParts</c></summary>
public class MechSearchGetReceivedPartsInput
{
    /// <summary>از تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string FromDate { get; set; } = default!;

    /// <summary>تا تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string ToDate { get; set; } = default!;

}
