namespace IKCO.ProductionFeasibility.Common;

/// <summary>
/// نتیجه عملیات ثبت/ویرایش/حذف رویه‌های ذخیره‌شده که یک ردیف <c>Result</c> و <c>Message</c> برمی‌گردانند
/// (در بک‌اند قبلی <c>GlobalResultDto</c>). مقدار مثبت <c>Result</c> یعنی موفق؛ پیام فارسی رویه به کاربر نمایش داده می‌شود.
/// </summary>
public class GlobalResultDto
{
    /// <summary>کد نتیجه (مثبت = موفق)</summary>
    public int Result { get; set; }

    /// <summary>پیام فارسی رویه</summary>
    public string? Message { get; set; }
}
