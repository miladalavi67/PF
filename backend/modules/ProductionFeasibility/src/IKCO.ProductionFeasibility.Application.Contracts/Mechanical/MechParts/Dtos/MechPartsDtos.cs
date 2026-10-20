using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;

/// <summary>ورودی «مشخصات بدنه برای ثبت بحران» — <c>dbo.S_CarPropertiesSelectForCriticalParts</c></summary>
public class MechPartsGetBodyPropertiesInput
{
    /// <summary>شماره بدنه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(8)]
    public string BodyNo { get; set; } = default!;

    /// <summary>سایت</summary>
    public byte PlantId { get; set; }

    /// <summary>حالت</summary>
    public byte? Mode { get; set; }

}

/// <summary>ورودی «شرح فارسی قطعه بر اساس شماره فنی» — <c>dbo.SIMP_SelectPartDescByPartNo</c></summary>
public class MechPartsGetPartDescriptionInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «اطلاعات قطعه در فهرست مرجع قطعات کسری» — <c>dbo.SIMP_SelectParInShortagePartList</c></summary>
public class MechPartsGetPartInShortageListInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «محصولات مصرف‌کننده یک شماره فنی (BOM)» — <c>dbo.SIMP_SelectProductsUsePartNo</c></summary>
public class MechPartsGetPartProductsInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه» — <c>dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType</c></summary>
public class MechPartsGetProductsByClassAndPartTypeInput
{
    /// <summary>کد کلاس</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(5)]
    public string CarClassCode { get; set; } = default!;

    /// <summary>نوع قطعه</summary>
    public byte PartTypeCode { get; set; }

}

/// <summary>ورودی «بررسی وجود بحران فعال برای شماره فنی در سالن» — <c>dbo.S_CriticalPartsExistPartNo</c></summary>
public class MechPartsExistsPartNoInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(200)]
    public string PartNo { get; set; } = default!;

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

}

/// <summary>ورودی «محصولات شماره فنی از BOP» — <c>dbo.S_CriticalPartsSelectFromBOP</c></summary>
public class MechPartsGetFromBopInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(20)]
    public string PartNo { get; set; } = default!;

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

}

/// <summary>ورودی «محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها» — <c>dbo.S_CriticalPartsSelectFromSAP</c></summary>
public class MechPartsGetFromSapInput
{
    /// <summary>کد زیرکلاس‌ها</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(1000)]
    public string SubClassCode { get; set; } = default!;

    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «محصولات بحران برای ویرایش» — <c>dbo.S_CriticalPartProductsSelectForEdit_V1</c></summary>
public class MechPartsGetProductsForEditInput
{
    /// <summary>سالن مونتاژ</summary>
    public int? ShopTrimId { get; set; }

}

/// <summary>ورودی «ثبت بحران مکانیکی» — <c>dbo.S_CriticalPartsINSERT</c></summary>
public class MechPartsCreateInput
{
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }

    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(30)]
    public string PartNo { get; set; } = default!;

    /// <summary>شرح قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(100)]
    public string PartDesc { get; set; } = default!;

    /// <summary>سایت</summary>
    public byte PlantId { get; set; }

    /// <summary>گروه قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(1)]
    public string PartGroup { get; set; } = default!;

    /// <summary>قطعات آلترنات</summary>
    [StringLength(1000)]
    public string? AlterNativePartNo { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(100)]
    public string? CriticalPartNote { get; set; }

    /// <summary>موجودی</summary>
    public int Stock { get; set; }

    /// <summary>مصرف‌شده</summary>
    public int Used { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    public int PossibleProductionNo { get; set; }

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

    /// <summary>ضریب مصرف</summary>
    public int UsageFactor { get; set; }

    /// <summary>واحد مسئول کسری</summary>
    public int DeptId { get; set; }

    /// <summary>خارج از SAP</summary>
    public bool? IsOutOfSap { get; set; }

}

/// <summary>ورودی «ویرایش بحران مکانیکی» — <c>dbo.S_CriticalPartsUpdate</c></summary>
public class MechPartsUpdateInput
{
    /// <summary>موجودی</summary>
    public int Stock { get; set; }

    /// <summary>مصرف‌شده</summary>
    public int Used { get; set; }

    /// <summary>ضریب مصرف</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(2)]
    public string UsageFactor { get; set; } = default!;

    /// <summary>قطعات آلترنات</summary>
    [StringLength(200)]
    public string? AlterNativePartNo { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(1000)]
    public string? CriticalPartNote { get; set; }

    /// <summary>شماره نامه</summary>
    [StringLength(1000)]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>توضیحات وضعیت</summary>
    [StringLength(300)]
    public string? ProductionStateNote { get; set; }

    /// <summary>گروه قطعه</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(1)]
    public string PartGroup { get; set; } = default!;

    /// <summary>شرایط تولید</summary>
    public int ProductionPossibility { get; set; }

    /// <summary>خارج از SAP</summary>
    public bool? IsOutOfSap { get; set; }

}

/// <summary>ورودی «ثبت/به‌روزرسانی یک محصول بحران» — <c>dbo.S_CriticalPartsPruductUpdate_Web</c></summary>
public class MechPartsSaveProductInput
{
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }

    /// <summary>خانواده</summary>
    public int CarGroupId { get; set; }

    /// <summary>مدل</summary>
    public int CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int CarClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    public int SubClassId { get; set; }

    /// <summary>رنگ</summary>
    public int ColorId { get; set; }

    /// <summary>تریم</summary>
    public int? TrimId { get; set; }

    /// <summary>آپشن</summary>
    public int OptionId { get; set; }

    /// <summary>جدید (۱) یا موجود</summary>
    public int IsNew { get; set; }

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

    /// <summary>سایت</summary>
    public short PlantId { get; set; }

    /// <summary>ضریب مصرف</summary>
    public byte Quantity { get; set; }

}

/// <summary>ورودی «افزودن زیرکلاس مغایر به بحران» — <c>dbo.S_AddContrastOfSubClasses</c></summary>
public class MechPartsAddSubClassInput
{
    /// <summary>شناسه بحران</summary>
    public int CriticalPartsId { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(7)]
    public string SubClassCode { get; set; } = default!;

    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

}
