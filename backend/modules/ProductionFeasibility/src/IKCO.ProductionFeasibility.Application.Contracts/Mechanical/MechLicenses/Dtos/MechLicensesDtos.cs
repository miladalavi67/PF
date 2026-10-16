using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;

/// <summary>ورودی «اطلاعات ردیابی مجوزهای یک بحران» — <c>dbo.S_ShortageLicenseSelectAllTrackingInfo</c></summary>
public class MechLicensesGetTrackingInfoInput
{
    /// <summary>نوع بحران (0=مکانیکی، 1=سنکرون)</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    public int CriticalPartId { get; set; }

}

/// <summary>ورودی «تاریخچه مجوزهای یک بحران» — <c>dbo.S_ShortageLicenseTrackingSelectAll</c></summary>
public class MechLicensesGetTrackingListInput
{
    /// <summary>نوع بحران (0=مکانیکی، 1=سنکرون)</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    public int CriticalPartId { get; set; }

}

/// <summary>ورودی «گزارش پارامترهای تولید با کسری تعریف‌شده» — <c>dbo.S_ShortageLicenseTrackingSearch</c></summary>
public class MechLicensesSearchTrackingInput
{
    /// <summary>از تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string FromDate { get; set; } = default!;

    /// <summary>تا تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string ToDate { get; set; } = default!;

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

    /// <summary>کد ست</summary>
    [StringLength(50)]
    public string? SetCode { get; set; }

}

/// <summary>ورودی «بحران‌های دارای مجوز تولید با کسری» — <c>dbo.S_ShortagelicenseTrackingSearchCriticals</c></summary>
public class MechLicensesSearchTrackingCriticalsInput
{
    /// <summary>از تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string FromDate { get; set; } = default!;

    /// <summary>تا تاریخ</summary>
    [Required(AllowEmptyStrings = false)]
    [StringLength(50)]
    public string ToDate { get; set; } = default!;

    /// <summary>شماره فنی</summary>
    [StringLength(50)]
    public string? PartNo { get; set; }

    /// <summary>کد ست</summary>
    [StringLength(50)]
    public string? SetCode { get; set; }

}

/// <summary>ورودی «بدنه‌های مصرف‌کننده مجوز تولید» — <c>dbo.S_SelectBodiesRelatedToUsedLicense</c></summary>
public class MechLicensesGetBodiesRelatedToUsedLicenseInput
{
    /// <summary>نوع بحران</summary>
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    public int CriticalPartId { get; set; }

}
