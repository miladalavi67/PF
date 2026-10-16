using System.ComponentModel.DataAnnotations;

namespace IKCO.ProductionFeasibility.Lookups.Dtos;

/// <summary>یک ردیف فهرست مرجع (کد و شرح فارسی)</summary>
public class LookupRow
{
    /// <summary>شناسه</summary>
    public int Id { get; set; }

    /// <summary>کد (در صورت وجود)</summary>
    public string? Code { get; set; }

    /// <summary>شرح فارسی</summary>
    public string Title { get; set; } = default!;
}

/// <summary>شرح یک فهرست مرجع قابل درخواست</summary>
public class LookupDescriptor
{
    /// <summary>کلید فهرست</summary>
    public string Key { get; set; } = default!;

    /// <summary>عنوان فارسی</summary>
    public string Title { get; set; } = default!;

    /// <summary>کلید فهرست والد (برای فهرست‌های وابسته مانند مدل ← خانواده)</summary>
    public string? ParentKey { get; set; }
}

/// <summary>ورودی دریافت یک فهرست مرجع</summary>
public class GetLookupInput
{
    /// <summary>شناسه ردیف والد برای فهرست‌های وابسته</summary>
    public int? ParentId { get; set; }

    /// <summary>متن جستجو در کد/شرح (اختیاری)</summary>
    [StringLength(LookupConsts.SearchLength)]
    public string? Search { get; set; }
}
