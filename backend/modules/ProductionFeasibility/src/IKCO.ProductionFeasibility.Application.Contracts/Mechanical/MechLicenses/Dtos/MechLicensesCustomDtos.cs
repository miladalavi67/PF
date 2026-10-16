using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;

/// <summary>ورودی تغییر شرایط تولید / اصلاح مجوز تولید با کسری برای بحران مکانیکی یا سنکرون</summary>
public class MechLicensesChangeInput
{
    /// <summary>نوع بحران: 0 = مکانیکی، 1 = سنکرون (ست)</summary>
    [Range(0, 1)]
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران (T_CriticalParts.Id یا T_CriticalPartsSet.Id)</summary>
    [Range(1, int.MaxValue)]
    public int CriticalPartId { get; set; }

    /// <summary>شرایط تولید: 0 = عدم ورود، 1 = تولید با کسری، 2 = تولید با قطعه جایگزین</summary>
    [Range(0, 2)]
    public byte ProductionPossibility { get; set; }

    /// <summary>تعداد مجاز تولید با کسری (خالی = نامحدود)</summary>
    [Range(0, 32767)]
    public short? PossibleProductionNo { get; set; }

    /// <summary>مجاز تا تاریخ (شمسی yyyy/MM/dd؛ خالی = نامحدود)</summary>
    [StringLength(PfConsts.ShamsiDateLength)]
    [RegularExpression(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", ErrorMessage = "تاریخ باید با قالب yyyy/MM/dd شمسی باشد.")]
    public string? ToDate { get; set; }

    /// <summary>مجاز تا ساعت (HH:mm:ss)</summary>
    [StringLength(PfConsts.TimeLength)]
    [RegularExpression(@"^([01]\d|2[0-3]):[0-5]\d:[0-5]\d$", ErrorMessage = "ساعت باید با قالب HH:mm:ss باشد.")]
    public string? ToTime { get; set; }

    /// <summary>شماره نامه مجوز</summary>
    [StringLength(30)]
    public string? LetterNo { get; set; }

    /// <summary>توضیحات</summary>
    [StringLength(100)]
    public string? Note { get; set; }

    /// <summary>علت افت تولید (ProductionFallId)</summary>
    [Range(0, 255)]
    public byte? ProductionFallId { get; set; }

    /// <summary>true = ویرایش آخرین مجوز فعال (ControlFlag=3)، false = ثبت مجوز جدید</summary>
    public bool EditLast { get; set; }
}

/// <summary>خلاصه مجوز یک بحران (فرم تغییر شرایط تولید)</summary>
public class LicenseSummaryDto
{
    /// <summary>مجوز مصرف‌شده</summary>
    public int? UsedLicense { get; set; }

    /// <summary>باقی‌مانده معتبر همه مجوزهای قبلی</summary>
    public int? ValidRemainingOfAllShouldPreviousShortageLicenses { get; set; }

    /// <summary>باقی‌مانده معتبر آخرین مجوز محدود</summary>
    public int? ValidRemainingOfLastLimitedShortageLicense { get; set; }
}

/// <summary>ورودی خلاصه مجوز</summary>
public class MechLicensesGetSummaryInput
{
    /// <summary>نوع بحران: 0 = مکانیکی، 1 = سنکرون</summary>
    [Range(0, 1)]
    public int CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    [Range(1, int.MaxValue)]
    public int CriticalPartId { get; set; }
}
