using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;

/// <summary>ورودی «اطلاعات شماره فنی از BOM (کلاس، زیرکلاس و نوع قطعه)» — <c>dbo.SIMP_PartGetInfoByPartNo_Web</c></summary>
public class SyncSetsGetPartInfoInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «رنگ‌های هم‌کد با کد ۵ رقمی» — <c>dbo.S_SelectColorsSimilarTo5DigitCode</c></summary>
public class SyncSetsGetSimilarColorsInput
{
    /// <summary>کد رنگ ۵ رقمی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(5)]
    public string Code { get; set; } = default!;

}

/// <summary>ورودی «سالن‌های مونتاژ مدل‌های انتخاب‌شده» — <c>dbo.S_SelectSaloonsByCarModels</c></summary>
public class SyncSetsGetSaloonsByCarModelsInput
{
    /// <summary>شناسه مدل‌ها (جداشده با ویرگول)</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(2000)]
    public string CarModelIds { get; set; } = default!;

    /// <summary>سایت</summary>
    public byte PlantId { get; set; }

}

/// <summary>ورودی «ثبت سرآیند بحران سنکرون» — <c>dbo.S_CriticalPartsSetInsert_Web</c></summary>
public class SyncSetsCreateHeaderInput
{
    /// <summary>شناسه مبنای ست</summary>
    public int SetId { get; set; }

    /// <summary>واحد مسئول</summary>
    public int DeptId { get; set; }

    /// <summary>کد رنگ محصول (۵ رقمی)</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(5)]
    public string ProductsColorCode { get; set; } = default!;

    /// <summary>شناسه رنگ</summary>
    public int ProductsColorId { get; set; }

    /// <summary>شناسه تریم</summary>
    public int ProductsTrimId { get; set; }

    /// <summary>شناسه تودوزی</summary>
    public int ProductsToodooziId { get; set; }

    /// <summary>شناسه آپشن</summary>
    public int ProductsOptionId { get; set; }

    /// <summary>موجودی</summary>
    public short Stock { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(100)]
    public string? Note { get; set; }

}

/// <summary>ورودی «ویرایش بحران سنکرون» — <c>dbo.S_CriticalPartsSetUpdate_Web</c></summary>
public class SyncSetsUpdateHeaderInput
{
    /// <summary>واحد مسئول</summary>
    public int DeptId { get; set; }

    /// <summary>موجودی</summary>
    public short Stock { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(100)]
    public string? Note { get; set; }

    /// <summary>وضعیت (0=در حال تکمیل، 1=فعال، 2=غیرفعال)</summary>
    public byte Status { get; set; }

}

/// <summary>ورودی «ثبت یک محصول بحران سنکرون» — <c>dbo.S_CriticalPartsSetProductInsert_Web</c></summary>
public class SyncSetsAddProductInput
{
    /// <summary>خانواده</summary>
    public int CarGroupId { get; set; }

    /// <summary>مدل</summary>
    public int CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int CarClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    public int CarSubClassId { get; set; }

    /// <summary>تریم</summary>
    public int CarTrimId { get; set; }

    /// <summary>تودوزی</summary>
    public int CarToodooziId { get; set; }

    /// <summary>آپشن</summary>
    public int CarOptionId { get; set; }

    /// <summary>رنگ</summary>
    public int CarColorId { get; set; }

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

}

/// <summary>ورودی «تعیین خودروی شاخص سالن بحران سنکرون» — <c>dbo.S_CriticalPartsSetSaloonUpdate_Web</c></summary>
public class SyncSetsUpdateSaloonIndicatorCarInput
{
    /// <summary>سالن</summary>
    public short SaloonId { get; set; }

    /// <summary>شناسه خودروی شاخص</summary>
    public int IndicatorCarId { get; set; }

}
