using System.Collections.Generic;

namespace IKCO.ProductionFeasibility.Common;

/// <summary>
/// خروجی جدولی رویه‌هایی که سامانه قدیم آن‌ها را در <c>DataTable</c> دریافت می‌کرد.
/// ترتیب و نام ستون‌ها دقیقاً توسط رویه ذخیره‌شده تعیین می‌شود (اولین result set).
/// </summary>
public class TabularResult
{
    /// <summary>نام ستون‌ها به ترتیب خروجی رویه</summary>
    public List<string> Columns { get; set; } = new();

    /// <summary>ردیف‌ها؛ کلید هر مقدار نام ستون است</summary>
    public List<Dictionary<string, object?>> Rows { get; set; } = new();
}
