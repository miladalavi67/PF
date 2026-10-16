namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>timeout فرمان‌ها (ثانیه) مطابق رفتار سامانه ویندوزی (<c>SqlHelperModern.ExecuteReader</c> = 720).</summary>
public static class PfCommandTimeouts
{
    /// <summary>فهرست‌های مرجع و پرس‌وجوهای سبک</summary>
    public const int Lookup = 30;

    /// <summary>ثبت/ویرایش/حذف</summary>
    public const int Command = 120;

    /// <summary>گزارش‌ها و جستجوها</summary>
    public const int Report = 720;
}
