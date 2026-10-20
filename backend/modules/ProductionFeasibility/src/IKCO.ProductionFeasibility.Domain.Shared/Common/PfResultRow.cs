using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace IKCO.ProductionFeasibility.Common;

/// <summary>
/// پایه مدل‌های ردیف خروجی رویه‌های ذخیره‌شده. هر خصوصیت مدل با <see cref="JsonPropertyNameAttribute"/> همان نام ستون رویه را
/// دارد (قرارداد فرانت‌اند و عنوان‌های فارسی ستون‌ها بر اساس نام ستون است) و Dapper ستون را به خصوصیت هم‌نام نگاشت می‌کند.
/// ستونی که رویه برمی‌گرداند ولی در مدل تعریف نشده (مثلاً بخش کوتاه‌شده متن رویه در اسنپ‌شات پایگاه داده) حذف نمی‌شود و در
/// <see cref="ExtraColumns"/> با همان نام در JSON خروجی قرار می‌گیرد.
/// </summary>
public abstract class PfResultRow
{
    /// <summary>ستون‌های خروجی رویه که در مدل تعریف نشده‌اند (در JSON کنار سایر ستون‌ها سریال می‌شوند)</summary>
    [JsonExtensionData]
    public Dictionary<string, object?>? ExtraColumns { get; set; }
}
