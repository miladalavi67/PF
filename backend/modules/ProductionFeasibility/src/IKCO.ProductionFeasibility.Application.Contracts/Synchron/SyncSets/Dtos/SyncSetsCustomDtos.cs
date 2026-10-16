using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;

/// <summary>قطعه‌ای از ست سنکرون</summary>
public class SyncSetPartItem
{
    /// <summary>شناسه قطعه در فهرست مرجع قطعات کسری (T_ShortagePartList.Id)؛ صفر = یافتن بر اساس شماره فنی</summary>
    [Range(0, int.MaxValue)]
    public int ShortagePartListId { get; set; }

    /// <summary>شماره فنی</summary>
    [Required]
    [StringLength(20)]
    public string PartNo { get; set; } = default!;
}

/// <summary>محصولی از ست سنکرون</summary>
public class SyncSetProductItem
{
    public int CarGroupId { get; set; }
    public int CarModelId { get; set; }
    public int CarClassId { get; set; }
    public int CarSubClassId { get; set; }
    public int CarTrimId { get; set; } = -1;
    public int CarToodooziId { get; set; } = -1;
    public int CarOptionId { get; set; } = -1;
    public int CarColorId { get; set; } = -1;
    public int SaloonId { get; set; }
}

/// <summary>ورودی ثبت ترکیب یک بحران سنکرون (قطعات، رنگ‌ها، سالن‌ها و محصولات) در یک تراکنش</summary>
public class SyncSetsSaveCompositionInput
{
    /// <summary>قطعات ست</summary>
    [Required]
    public List<SyncSetPartItem> Parts { get; set; } = new();

    /// <summary>شناسه رنگ‌ها</summary>
    [Required]
    public List<int> ColorIds { get; set; } = new();

    /// <summary>شناسه سالن‌ها</summary>
    [Required]
    public List<int> SaloonIds { get; set; } = new();

    /// <summary>محصولات</summary>
    [Required]
    public List<SyncSetProductItem> Products { get; set; } = new();
}
