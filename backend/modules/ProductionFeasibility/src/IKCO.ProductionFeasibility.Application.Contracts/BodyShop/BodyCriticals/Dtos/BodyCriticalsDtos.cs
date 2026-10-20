using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;

/// <summary>ورودی «اطلاعات بدنه» — <c>dbo.SEXT_BodyInfo_V2</c></summary>
public class BodyCriticalsGetBodyInfoInput
{
    /// <summary>شماره بدنه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(8)]
    public string BodyNumber { get; set; } = default!;

}

/// <summary>ورودی «شرح فارسی قطعه بر اساس شماره فنی» — <c>dbo.SIMP_SelectPartDescByPartNo</c></summary>
public class BodyCriticalsGetPartDescriptionInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «محصولات مصرف‌کننده یک شماره فنی در یک خط بدنه‌سازی» — <c>PF.SelectPartProductByPartNo</c></summary>
public class BodyCriticalsGetPartProductsInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

    /// <summary>شناسه خط بدنه‌سازی</summary>
    public int BIWSiteId { get; set; }

}

/// <summary>ورودی «کدهای کلاس مصرف‌کننده شماره فنی» — <c>PF.S_GetCodeClassByPartNo</c></summary>
public class BodyCriticalsGetCodeClassByPartNoInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

    /// <summary>شناسه خط بدنه‌سازی</summary>
    public int BIWSiteId { get; set; }

}

/// <summary>ورودی «ثبت بحران بدنه‌سازی» — <c>PF.S_CreateCriticalParts_V3</c></summary>
public class BodyCriticalsCreateCriticalPartInput
{
    /// <summary>شناسه محصول (CarId)</summary>
    public int CarId { get; set; }

    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(30)]
    public string PartNo { get; set; } = default!;

    /// <summary>شرح قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string PartDesc { get; set; } = default!;

    /// <summary>گروه قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(1)]
    public string PartGroup { get; set; } = default!;

    /// <summary>شماره فنی جایگزین</summary>
    [StringLength(1000)]
    public string? AlterNativePartNo { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [StringLength(100)]
    public string? CriticalPartNote { get; set; }

    /// <summary>شناسه خط بدنه‌سازی</summary>
    public int BIWSiteId { get; set; }

    /// <summary>موجودی گروهی</summary>
    [Range(0, 100000000)]
    public int TotalStock { get; set; }

    /// <summary>موجودی خام</summary>
    [Range(0, 100000000)]
    public int RawStock { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [StringLength(1000)]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره نامه</summary>
    [StringLength(1000)]
    public string? LetterNo { get; set; }

    /// <summary>خارج از SAP</summary>
    public bool IsOutOfSAP { get; set; }

}

/// <summary>ورودی «ویرایش بحران بدنه‌سازی» — <c>PF.S_Web_UpdateCriticalParts</c></summary>
public class BodyCriticalsUpdateCriticalPartInput
{
    /// <summary>گروه قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(1)]
    public string PartGroup { get; set; } = default!;

    /// <summary>شماره فنی جایگزین</summary>
    [StringLength(1000)]
    public string? AlterNativePartNo { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [StringLength(100)]
    public string? CriticalPartNote { get; set; }

    /// <summary>موجودی خام</summary>
    [Range(0, 100000000)]
    public int RawStock { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [StringLength(1000)]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره نامه</summary>
    [StringLength(1000)]
    public string? LetterNo { get; set; }

}

/// <summary>ورودی «گزارش رویدادهای یک بحران» — <c>PF.S_EventLogList</c></summary>
public class BodyCriticalsGetEventLogInput
{
    /// <summary>نوع رویداد (نوع بحران)</summary>
    public int EventTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    public int ChangedRecordId { get; set; }

}
