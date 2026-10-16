using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;

/// <summary>یک کلاس مصرف‌کننده بحران با ضریب مصرف</summary>
public class CriticalPartProductItem
{
    /// <summary>شناسه کلاس</summary>
    [Range(1, int.MaxValue)]
    public int CarClassId { get; set; }

    /// <summary>ضریب مصرف</summary>
    [Range(1, 100)]
    public int UsageFactor { get; set; }
}

/// <summary>ورودی ثبت محصولات یک بحران بدنه‌سازی (فراخوانی PF.S_CreateCritocalPartsProduct برای هر کلاس)</summary>
public class BodyCriticalsCreateProductsInput
{
    /// <summary>شناسه محصول (CarId) بدنه ثبت‌کننده بحران</summary>
    [Range(1, int.MaxValue)]
    public int CarId { get; set; }

    /// <summary>کلاس‌های انتخاب‌شده</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(200)]
    public List<CriticalPartProductItem> Items { get; set; } = new();
}

/// <summary>ورودی ثبت مجوز تولید با کسری برای بحران بدنه‌سازی</summary>
public class BodyCriticalsRegisterLicenseInput
{
    /// <summary>شناسه بحران</summary>
    [Range(1, int.MaxValue)]
    public int CriticalPartId { get; set; }

    /// <summary>نوع بحران (PF.B_CriticalPartType)</summary>
    [Range(0, 255)]
    public int CriticalPartTypeId { get; set; }

    /// <summary>شرایط تولید (0=عدم ورود، 1=تولید با کسری، 2=تولید با قطعه جایگزین)</summary>
    [Range(0, 2)]
    public int ProductionPossibilityId { get; set; }

    /// <summary>تعداد مجاز تولید با کسری (خالی = نامحدود)</summary>
    [Range(0, 100000000)]
    public int? PossibleProductionNo { get; set; }

    /// <summary>مجاز تا تاریخ (شمسی yyyy/MM/dd)</summary>
    [StringLength(PfConsts.ShamsiDateLength)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت (HH:mm:ss)</summary>
    [StringLength(PfConsts.TimeLength)]
    [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d:[0-5]\d$", ErrorMessage = "ساعت باید با قالب HH:mm:ss باشد.")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(250)]
    public string? Note { get; set; }
}

/// <summary>یک ردیف ثبت گروهی بحران‌ها (اکسل)</summary>
public class BatchCriticalPartRow
{
    /// <summary>شماره بدنه</summary>
    [Required]
    [StringLength(PfConsts.BodyNoLength)]
    public string BodyNo { get; set; } = default!;

    /// <summary>شماره فنی</summary>
    [Required]
    [StringLength(PfConsts.PartNoLength)]
    public string PartNo { get; set; } = default!;

    /// <summary>شرح قطعه</summary>
    [StringLength(250)]
    public string? PartDescF { get; set; }

    /// <summary>گروه قطعه</summary>
    [Required]
    [StringLength(1)]
    public string PartGroup { get; set; } = default!;

    /// <summary>شماره فنی جایگزین</summary>
    [StringLength(1000)]
    public string? Alternative { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [StringLength(100)]
    public string? PartDescription { get; set; }

    /// <summary>موجودی گروهی</summary>
    [Range(0, 100000000)]
    public int GroupStock { get; set; }

    /// <summary>موجودی خام</summary>
    [Range(0, 100000000)]
    public int RawStock { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [StringLength(1000)]
    public string? Description { get; set; }

    /// <summary>شماره نامه</summary>
    [StringLength(1000)]
    public string? LetterNo { get; set; }

    /// <summary>کد کلاس</summary>
    [Range(0, int.MaxValue)]
    public int CarClassCode { get; set; }

    /// <summary>ضریب مصرف</summary>
    [Range(1, 100)]
    public int Quantity { get; set; }
}

/// <summary>ورودی ثبت گروهی بحران‌های بدنه‌سازی (PF.S_CreateBatchCriticalPartProduct)</summary>
public class BodyCriticalsBatchCreateInput
{
    /// <summary>شناسه خط بدنه‌سازی</summary>
    [Range(1, int.MaxValue)]
    public int BIWSiteId { get; set; }

    /// <summary>ردیف‌ها</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(PfConsts.MaxBatchRows)]
    public List<BatchCriticalPartRow> Rows { get; set; } = new();
}

/// <summary>ورودی جستجوی «استفاده‌شده در» SAP</summary>
public class BodyCriticalsGetSapWhereUsedInput
{
    /// <summary>شماره فنی</summary>
    [Required]
    [StringLength(PfConsts.PartNoLength)]
    public string PartNo { get; set; } = default!;

    /// <summary>کد سالن / خط بدنه‌سازی در SAP (P_SFCSALON)</summary>
    [Required]
    [StringLength(15)]
    public string SaloonCode { get; set; } = default!;
}

/// <summary>یک ردیف پاسخ SAP</summary>
public class SapWhereUsedItemDto
{
    /// <summary>کد زیرکلاس</summary>
    public string SubClassCode { get; set; } = default!;

    /// <summary>مقدار مصرف</summary>
    public decimal? Quantity { get; set; }
}
