using System.Collections.Generic;

namespace IKCO.ProductionFeasibility.LiveUpdates;

/// <summary>یک کانال به‌روزرسانی زنده و جدول‌هایی که تغییرشان گزارش متصل را تازه می‌کند.</summary>
/// <param name="Name">نام کانال (گروه SignalR)</param>
/// <param name="Title">شرح فارسی</param>
/// <param name="Queries">
/// پرس‌وجوهای پایش؛ مطابق قواعد Query Notification سرویس بروکر (نام دوبخشی جدول، ستون‌های صریح، بدون NOLOCK و تابع تجمیعی)
/// تا هم برای SqlDependency و هم برای پایش دوره‌ای (checksum) قابل استفاده باشند.
/// </param>
public sealed record PfLiveChannel(string Name, string Title, IReadOnlyList<string> Queries);

/// <summary>
/// کانال‌های به‌روزرسانی زنده گزارش‌های ردیابی اقلام بحرانی (PFReview بند ۵). فرم‌های ویندوزی این گزارش‌ها را با Service Broker
/// (<c>RealTimeDetectChangeOnLicense</c>، <c>RealTimeDetectChangeOnLicense_Trim</c>، <c>RealTimeDetectChangeOnPreAssign</c>،
/// <c>RealTimeReportCriticalPartSearch</c>) تازه می‌کردند؛ همان پرس‌وجوها اینجا پایش و تغییرشان با SignalR به صفحه‌ها اعلام می‌شود.
/// </summary>
public static class PfLiveChannels
{
    /// <summary>ردیابی تغییرات اقلام بحرانی بدنه‌سازی (frmRPTTrackingCriticalParts)</summary>
    public const string BodyTracking = "body-tracking";

    /// <summary>گزارش پیش‌تخصیص بدنه‌های بحرانی (frmRPTPreAssignCriticalBody)</summary>
    public const string BodyPreAssign = "body-preassign";

    /// <summary>ردیابی اقلام بحرانی مونتاژ — مکانیکی و سنکرون (frmRPTProductionPossibility)</summary>
    public const string TrimTracking = "trim-tracking";

    public static IReadOnlyList<PfLiveChannel> All { get; } = new[]
    {
        new PfLiveChannel(BodyTracking, "ردیابی تغییرات اقلام بحرانی بدنه‌سازی", new[]
        {
            "SELECT tslt.CreateDateTime, tslt.LastUpdateDateTime FROM PF.T_ShortageLicenseTracking AS tslt WHERE tslt.[Status] = 1 AND tslt.CriticalPartTypeId = 3",
            "SELECT tbcp.Status FROM PF.T_BodyCriticalParts AS tbcp",
        }),
        new PfLiveChannel(BodyPreAssign, "پیش‌تخصیص بدنه‌های بحرانی", new[]
        {
            "SELECT tpacb.Id FROM PF.T_PreAssignCriticalBody AS tpacb",
        }),
        new PfLiveChannel(TrimTracking, "ردیابی اقلام بحرانی مونتاژ", new[]
        {
            "SELECT slt.CreateDateTime, slt.LastUpdateDateTime FROM dbo.T_ShortageLicenseTracking AS slt WHERE slt.[Status] = 1",
            "SELECT cp.Status, cp.Stock, cp.LastUpdateDateTime FROM dbo.T_CriticalParts AS cp",
            "SELECT cps.Status, cps.Stock, cps.LastUpdateDateTime FROM dbo.T_CriticalPartsSet AS cps",
        }),
    };
}
