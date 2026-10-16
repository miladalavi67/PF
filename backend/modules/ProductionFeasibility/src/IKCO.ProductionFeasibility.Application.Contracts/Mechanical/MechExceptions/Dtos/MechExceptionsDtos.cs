using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions.Dtos;

/// <summary>ورودی «فهرست بدنه‌های استثنا» — <c>dbo.S_CriticalProductExceptionList</c></summary>
public class MechExceptionsGetExceptionListInput
{
    /// <summary>شماره بدنه</summary>
    [StringLength(8)]
    public string? BodyNo { get; set; }

    /// <summary>از تاریخ</summary>
    [StringLength(12)]
    public string? DateFrom { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(12)]
    public string? DateEnd { get; set; }

    /// <summary>وضعیت</summary>
    public int State { get; set; }

}

/// <summary>ورودی «تاریخچه مجوزهای استثنا برای شماره فنی» — <c>dbo.S_CriticalPartsProductExceptionsLisenceLog</c></summary>
public class MechExceptionsGetLicenseLogInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «ثبت مجوز برای استثنا» — <c>dbo.S_InsertCriticalPartsProductExceptionsLicense</c></summary>
public class MechExceptionsCreateLicenseInput
{
    /// <summary>شناسه استثنا</summary>
    public int CriticalPartsProductExceptionsId { get; set; }

    /// <summary>تعداد مجاز</summary>
    public int PossibleLicenseCount { get; set; }

    /// <summary>شماره نامه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string LetterNo { get; set; } = default!;

    /// <summary>توضیحات</summary>
    [StringLength(250)]
    public string? Description { get; set; }

}

/// <summary>ورودی «غیرفعال‌سازی استثناها» — <c>dbo.S_DisableCriticalPartsProductsException</c></summary>
public class MechExceptionsDisableExceptionsInput
{
    /// <summary>شناسه‌ها</summary>
    [Required(AllowEmptyStrings = false)]
    [MinLength(1)]
    public List<int> Ids { get; set; } = new();

}

/// <summary>ورودی «گزارش شماره فنی‌های دارای استثنا» — <c>dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException</c></summary>
public class MechExceptionsGetPartNumbersInExceptionsInput
{
    /// <summary>مقادیر فیلتر (جداشده با ویرگول)</summary>
    [Required(AllowEmptyStrings = false)]
    public string FilterValues { get; set; } = default!;

    /// <summary>سالن مونتاژ</summary>
    public byte TrimId { get; set; }

    /// <summary>نوع فیلتر (1=مدل، 2=کلاس، 3=زیرکلاس، 4=سالن)</summary>
    public byte FilterType { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNumber { get; set; }

}
