using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports.Dtos;

/// <summary>ورودی «جستجوی اقلام بحرانی بدنه‌سازی» — <c>PF.S_BodyCriticalSearch_Web</c></summary>
public class BodyReportsSearchCriticalPartsInput
{
    /// <summary>سایت</summary>
    public int? PlantId { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    public int? VINBiwSite { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(100)]
    public string? PartNumber { get; set; }

    /// <summary>فعال</summary>
    public bool? Status { get; set; }

    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>خانواده محصول</summary>
    public int? CarGroupId { get; set; }

    /// <summary>مدل</summary>
    public int? CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int? CarClassId { get; set; }

    /// <summary>نوع جستجو</summary>
    public int? TypeOfSearch { get; set; }

    /// <summary>شرایط تولید</summary>
    public int? ProductionPossibilityId { get; set; }

}

/// <summary>ورودی «ردیابی تغییرات اقلام بحرانی» — <c>PF.GetTrackingCriticalParts_V2</c></summary>
public class BodyReportsGetTrackingCriticalPartsInput
{
    /// <summary>سایت</summary>
    public int? PlantId { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    public int? VINBiwSite { get; set; }

    /// <summary>خانواده محصول</summary>
    public int? CarGroupId { get; set; }

    /// <summary>مدل</summary>
    public int? CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int? CarClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    public int? SubClassId { get; set; }

    /// <summary>شرایط تولید</summary>
    public int? ProductionPossibilityId { get; set; }

}

/// <summary>ورودی «تعداد بدنه‌های پیش‌تخصیص‌شده» — <c>PF.S_PreAssignBodyCoun_List</c></summary>
public class BodyReportsGetPreAssignBodyCountsInput
{
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }

}

/// <summary>ورودی «بدنه‌های پیش‌تخصیص بحرانی» — <c>PF.S_PreAssignCriticalBody_SelectAll</c></summary>
public class BodyReportsGetPreAssignCriticalBodiesInput
{
    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

}

/// <summary>ورودی «مغایرت پیش‌تخصیص بدنه‌های بحرانی» — <c>PF.S_NotPossibleProductsToStepAssign_SelectAll</c></summary>
public class BodyReportsGetNotPossibleProductsToStepAssignInput
{
    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

}

/// <summary>ورودی «ردیابی پارامترهای بحرانی (مجوز تولید با کسری)» — <c>PF.GetShortageLicenseTrackingSelectAll</c></summary>
public class BodyReportsGetShortageLicenseTrackingInput
{
    /// <summary>نوع بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    public int CriticalPartId { get; set; }

}

/// <summary>ورودی «قطعات ناموفق در انتقال از SAP» — <c>PF.S_FailedPartTransferd_Sap_Log</c></summary>
public class BodyReportsGetFailedSapTransfersInput
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
