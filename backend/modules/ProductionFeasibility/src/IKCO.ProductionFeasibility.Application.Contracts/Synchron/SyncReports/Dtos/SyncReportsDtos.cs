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

/// <summary>ورودی «اقلام بحرانی دریافتی از SAP» — <c>dbo.S_Web_SAP_CriticalPartsMaster_SelectAll</c></summary>
public class SyncReportsGetSapCriticalPartsMasterInput
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

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>شماره بدنه</summary>
    [StringLength(8)]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNO { get; set; }

}

/// <summary>ورودی «رزروهای دریافتی از SAP» — <c>dbo.S_Web_SAP_ReserveMaster_SelectAll</c></summary>
public class SyncReportsGetSapReserveMasterInput
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

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNO { get; set; }

}

/// <summary>ورودی «گزارش ASN (اعلان ورود محموله)» — <c>dbo.S_ASNReport</c></summary>
public class SyncReportsGetAsnReportInput
{
    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNumber { get; set; }

    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>نوع</summary>
    public byte Type { get; set; }

}
