using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis.Dtos;

/// <summary>ورودی «مبنای ست بر اساس کد» — <c>dbo.S_CriticalPartsSetBasisSelectByCode</c></summary>
public class SyncBasisGetByCodeInput
{
    /// <summary>کد ست</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string Code { get; set; } = default!;

}

/// <summary>ورودی «مبناهای ست بر اساس وضعیت» — <c>dbo.S_CriticalPartsSetBasisSelectByStatus</c></summary>
public class SyncBasisGetByStatusInput
{
    /// <summary>وضعیت</summary>
    public byte? Status { get; set; }

}

/// <summary>ورودی «تعداد ست‌های ثبت‌شده با کد ست» — <c>dbo.S_CriticalPartsSetCountSetCode</c></summary>
public class SyncBasisCountBySetCodeInput
{
    /// <summary>کد ست</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string SetCode { get; set; } = default!;

}

/// <summary>ورودی «ثبت ویژگی نوع قطعه سنکرون» — <c>dbo.S_InsertCriticalPartTypeProperty</c></summary>
public class SyncBasisAddPartTypePropertyInput
{
    /// <summary>کد نوع قطعه</summary>
    public int PartTypeCode { get; set; }

    /// <summary>شناسه ویژگی</summary>
    public int PropertyId { get; set; }

}

/// <summary>ورودی «ثبت مبنای ست سنکرون» — <c>dbo.S_InsertCriticalPartSet</c></summary>
public class SyncBasisCreateInput
{
    /// <summary>کد ست</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(10)]
    public string Code { get; set; } = default!;

    /// <summary>شرح فارسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string DescF { get; set; } = default!;

    /// <summary>کلاس مبنا</summary>
    public int CarClass { get; set; }

    /// <summary>نوع قطعه</summary>
    public int PartTypeId { get; set; }

    /// <summary>سطح نوع قطعه</summary>
    public int LevelType { get; set; }

    /// <summary>بر اساس تریم</summary>
    public bool IsTrim { get; set; }

    /// <summary>بر اساس آپشن</summary>
    public bool IsOption { get; set; }

    /// <summary>بر اساس تودوزی</summary>
    public bool IsToodoozi { get; set; }

    /// <summary>بر اساس رنگ</summary>
    public bool IsColor { get; set; }

}

/// <summary>ورودی «ویرایش مبنای ست سنکرون» — <c>dbo.S_UpdateCriticalPartsSet</c></summary>
public class SyncBasisUpdateInput
{
    /// <summary>کد ست</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(10)]
    public string Code { get; set; } = default!;

    /// <summary>شرح فارسی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string DescF { get; set; } = default!;

    /// <summary>کلاس مبنا</summary>
    public int CarClassId { get; set; }

    /// <summary>نوع قطعه</summary>
    public int PartTypeId { get; set; }

    /// <summary>سطح نوع قطعه</summary>
    public int LevelType { get; set; }

    /// <summary>فعال</summary>
    public bool Status { get; set; }

    /// <summary>بر اساس تریم</summary>
    public bool IsTrim { get; set; }

    /// <summary>بر اساس رنگ</summary>
    public bool IsColor { get; set; }

    /// <summary>بر اساس آپشن</summary>
    public bool IsOption { get; set; }

    /// <summary>بر اساس تودوزی</summary>
    public bool IsToodoozi { get; set; }

}
