using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports.Dtos;

/// <summary>ورودی «گزارش امکان تولید مکانیکی» — <c>dbo.S_RptCriticalPartsProduction_Mechanical</c></summary>
public class MechReportsGetProductionPossibilityInput
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

/// <summary>ورودی «گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده» — <c>dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5</c></summary>
public class MechReportsGetProductionPossibilityWithLicenseInput
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

    /// <summary>مدل</summary>
    public int CarModelId { get; set; }

    /// <summary>کلاس</summary>
    public int CarClassId { get; set; }

}

/// <summary>ورودی «امکان تولید به تفکیک سالن مونتاژ» — <c>dbo.S_RPT_DESC_ProdPossibilityByTrimShop</c></summary>
public class MechReportsGetProductionPossibilityByTrimShopInput
{
    /// <summary>سالن مونتاژ</summary>
    public int TrimShopId { get; set; }

    /// <summary>وضعیت تولید (1=با کسری، 2=با قطعه جایگزین، 100=همه)</summary>
    public byte ProdState { get; set; }

}

/// <summary>ورودی «گزارش جامع اقلام بحرانی (مکانیکی و سنکرون)» — <c>dbo.S_CriticalPartsGeneralReport</c></summary>
public class MechReportsGetGeneralReportInput
{
    /// <summary>نوع قطعه بحران (100 = همه)</summary>
    public byte? CriticalPartTypeId { get; set; }

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>نوع بحران (100 = همه)</summary>
    public byte? CrisisTypeId { get; set; }

    /// <summary>سایت</summary>
    public byte? PlantId { get; set; }

}

/// <summary>ورودی «بدنه‌های در راه / وارد شده به خط برای یک بحران» — <c>dbo.S_RPT_IncommingBodiesByCriticalPartId</c></summary>
public class MechReportsGetIncomingBodiesByCriticalPartInput
{
    /// <summary>شناسه بحران</summary>
    public int Id { get; set; }

    /// <summary>نوع بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>سایت</summary>
    public byte PlantId { get; set; }

}

/// <summary>ورودی «فهرست بدنه‌های وارد شده برای یک بحران» — <c>dbo.S_RPT_IncommingBodiesListByCriticalPartId</c></summary>
public class MechReportsGetIncomingBodyListInput
{
    /// <summary>شناسه بحران</summary>
    public int Id { get; set; }

    /// <summary>نوع بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>گروه مرحله تولید</summary>
    public int ProductionStepGroupId { get; set; }

    /// <summary>سایت</summary>
    public byte? PlantId { get; set; }

}

/// <summary>ورودی «خلاصه بدنه‌های وارد شده به خط» — <c>dbo.S_RPT_IncommingBodiesByCriticalParts</c></summary>
public class MechReportsGetIncomingBodiesSummaryInput
{
    /// <summary>سالن</summary>
    public int SaloonId { get; set; }

    /// <summary>نوع بحران</summary>
    public byte CrisisTypeId { get; set; }

    /// <summary>گروه مرحله تولید</summary>
    public int? ProductionStepGroupId { get; set; }

    /// <summary>سایت</summary>
    public byte? PlantId { get; set; }

}

/// <summary>ورودی «تاریخچه موجودی شماره فنی» — <c>dbo.SEXT_CriticalPartsStockLog</c></summary>
public class MechReportsGetStockLogInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(30)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «لاگ انتقال اقلام بحرانی از SAP» — <c>dbo.S_SapCriticalLog</c></summary>
public class MechReportsGetSapCriticalLogInput
{
    /// <summary>شماره فنی</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string PartNo { get; set; } = default!;

}

/// <summary>ورودی «قطعات ناموفق در انتقال از SAP» — <c>dbo.S_FailedPartTransferd_Sap_Log</c></summary>
public class MechReportsGetFailedSapTransfersInput
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

}

/// <summary>ورودی «گزارش ASN (اعلان ورود محموله)» — <c>dbo.S_ASNReport</c></summary>
public class MechReportsGetAsnReportInput
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

/// <summary>ورودی «دلایل عدم امکان ورود خودرو به مونتاژ» — <c>dbo.S_TrimInNotPossibleProductsSelectAll</c></summary>
public class MechReportsGetNotPossibleProductsToTrimStepInput
{
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }

}

/// <summary>ورودی «تعداد بدنه‌های ورودی به مونتاژ پس از ثبت» — <c>dbo.S_TrimShopBodyCount_List</c></summary>
public class MechReportsGetTrimShopBodyCountInput
{
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }

}

/// <summary>ورودی «بدنه‌های بحرانی ورودی به مونتاژ» — <c>dbo.S_TrimShopCriticalBody_SelectAll</c></summary>
public class MechReportsGetTrimShopCriticalBodiesInput
{
    /// <summary>از تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? FromDate { get; set; }

    /// <summary>تا تاریخ</summary>
    [StringLength(10)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

    /// <summary>نوع بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>کد ست</summary>
    [StringLength(10)]
    public string? SetCode { get; set; }

}
