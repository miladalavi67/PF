using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;

/// <summary>یک محصول (ترکیب خانواده/مدل/کلاس/زیرکلاس/رنگ/تریم/آپشن) از بحران مکانیکی</summary>
public class MechPartProductItem
{
    /// <summary>خانواده محصول</summary>
    public int CarGroupId { get; set; }

    /// <summary>مدل</summary>
    public int CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int CarClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    public int SubClassId { get; set; }

    /// <summary>رنگ (-1 = همه)</summary>
    public int ColorId { get; set; } = -1;

    /// <summary>تریم (-1 = همه)</summary>
    public int TrimId { get; set; } = -1;

    /// <summary>آپشن (-1 = همه)</summary>
    public int OptionId { get; set; } = -1;

    /// <summary>ضریب مصرف</summary>
    [Range(1, 255)]
    public byte Quantity { get; set; } = 1;
}

/// <summary>ورودی ثبت چند محصول برای یک بحران مکانیکی (S_CriticalPartsPruductUpdate_H_V1 به ازای هر محصول، در یک تراکنش)</summary>
public class MechPartsSaveProductsInput
{
    /// <summary>شناسه محصول (CarId) بدنه ثبت‌کننده بحران</summary>
    [Range(1, int.MaxValue)]
    public int CarId { get; set; }

    /// <summary>سالن مونتاژ</summary>
    [Range(1, int.MaxValue)]
    public int TrimShopId { get; set; }

    /// <summary>سایت</summary>
    [Range(0, short.MaxValue)]
    public short PlantId { get; set; } = 1;

    /// <summary>محصول جدید (1) یا ویرایش (0)</summary>
    [Range(0, 1)]
    public int IsNew { get; set; } = 1;

    /// <summary>محصولات</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(2000)]
    public List<MechPartProductItem> Items { get; set; } = new();
}

/// <summary>یک ردیف فایل SAP برای مقایسه زیرکلاس‌ها</summary>
public class SubClassPartRow
{
    /// <summary>کد زیرکلاس (۷ کاراکتر)</summary>
    [Required]
    [StringLength(7)]
    public string SubClassCode { get; set; } = default!;

    /// <summary>شماره فنی</summary>
    [Required]
    [StringLength(PfConsts.PartNoLength)]
    public string PartNo { get; set; } = default!;
}

/// <summary>ورودی مقایسه زیرکلاس‌های SAP با SFC (S_GetContrastOfSubClasses)</summary>
public class MechPartsCompareSubClassesInput
{
    /// <summary>سالن مونتاژ</summary>
    [Range(1, int.MaxValue)]
    public int SaloonId { get; set; }

    /// <summary>نوع: 1 = موجود در SAP و ناموجود در SFC، 2 = موجود در SFC و ناموجود در SAP</summary>
    [Range(1, 2)]
    public int Type { get; set; } = 1;

    /// <summary>زوج‌های (زیرکلاس، شماره فنی) فایل SAP</summary>
    [Required]
    [MinLength(1)]
    [MaxLength(PfConsts.MaxBatchRows)]
    public List<SubClassPartRow> Rows { get; set; } = new();
}
