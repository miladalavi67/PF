using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;

/// <summary>ورودی «گزارش امکان تولید سنکرون» — <c>dbo.S_RptCriticalPartsProduction_Synchron</c></summary>
public class SyncReportsGetProductionPossibilityInput
{
    /// <summary>شناسه سالن‌ها (جداشده با ویرگول، -1 = همه)</summary>
    [Required(AllowEmptyStrings = false)]
    public string SaloonId { get; set; } = default!;

    /// <summary>نوع بحران (100 = همه)</summary>
    public int CrisisTypeId { get; set; }

    /// <summary>نوع قطعه بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>زیرکلاس</summary>
    public int SubClassId { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

    /// <summary>مدل</summary>
    public int CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int CarClassId { get; set; }

}
