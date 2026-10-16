using System.Collections.Generic;
using P = IKCO.ProductionFeasibility.Permissions.PfPermissions;

namespace IKCO.ProductionFeasibility.Permissions;

/// <summary>یک گروه دسترسی با برگ‌های آن (شرح فارسی برای پنل مدیریت دسترسی).</summary>
public sealed record PermissionGroupDefinition(string Code, string DisplayName, IReadOnlyList<(string Code, string DisplayName)> Leaves);

/// <summary>
/// درخت کامل دسترسی‌های سامانه؛ منبع واحد برای ثبت در IKCO.Auth (<c>ConfigurePermissions</c>).
/// کلیدهای UI منوها با منوی فرانت‌اند (mfe-manifest.ts) عیناً یکی هستند.
/// </summary>
public static class PfPermissionTree
{
    public static IReadOnlyList<PermissionGroupDefinition> Api { get; } = new List<PermissionGroupDefinition>
    {
        new(P.BodyCriticals.Group, "بحران‌های بدنه‌سازی", new[]
        {
            (P.BodyCriticals.View, "مشاهده اطلاعات بدنه و شماره فنی"),
            (P.BodyCriticals.Create, "ثبت بحران بدنه‌سازی"),
            (P.BodyCriticals.Update, "ویرایش بحران بدنه‌سازی"),
            (P.BodyCriticals.Disable, "غیرفعال‌سازی بحران بدنه‌سازی"),
            (P.BodyCriticals.License, "ثبت مجوز تولید با کسری برای بحران بدنه‌سازی"),
        }),
        new(P.BodyReports.Group, "گزارش‌های بدنه‌سازی", new[]
        {
            (P.BodyReports.Search, "جستجوی اقلام بحرانی بدنه‌سازی"),
            (P.BodyReports.Tracking, "گزارش ردیابی تغییرات اقلام بحرانی"),
            (P.BodyReports.Productsofsites, "گزارش محصولات به تفکیک خطوط بدنه‌سازی"),
            (P.BodyReports.Preassigncount, "گزارش تعداد بدنه‌های پیش‌تخصیص‌شده"),
            (P.BodyReports.Preassigncritical, "گزارش بدنه‌های پیش‌تخصیص بحرانی"),
            (P.BodyReports.Conflicts, "گزارش مغایرت پیش‌تخصیص بدنه‌های بحرانی"),
            (P.BodyReports.Licensetracking, "گزارش ردیابی پارامترهای بحرانی"),
            (P.BodyReports.Sap, "گزارش قطعات ناموفق انتقال از SAP"),
        }),
        new(P.MechParts.Group, "بحران‌های مکانیکی", new[]
        {
            (P.MechParts.View, "مشاهده اطلاعات پایه ثبت بحران مکانیکی"),
            (P.MechParts.Create, "ثبت بحران مکانیکی"),
            (P.MechParts.Update, "ویرایش بحران مکانیکی"),
            (P.MechParts.Compare, "افزودن زیرکلاس مغایر به بحران"),
            (P.MechParts.Delete, "حذف بحران مکانیکی"),
        }),
        new(P.MechSearch.Group, "جستجوی بحران‌ها", new[]
        {
            (P.MechSearch.Search, "جستجوی اقلام بحرانی مکانیکی و سنکرون"),
            (P.MechSearch.Arrived, "اقلام بحرانی وارد شده به شرکت"),
        }),
        new(P.MechLicenses.Group, "مجوزهای تولید با کسری", new[]
        {
            (P.MechLicenses.View, "اطلاعات ردیابی مجوزهای یک بحران"),
            (P.MechLicenses.Report, "گزارش ردیابی مجوزهای تولید با کسری"),
            (P.MechLicenses.Change, "تغییر شرایط تولید و ثبت مجوز (مکانیکی و سنکرون)"),
        }),
        new(P.MechExceptions.Group, "استثناهای محصولات بحران", new[]
        {
            (P.MechExceptions.View, "مشاهده استثناهای محصولات بحران"),
            (P.MechExceptions.Create, "ثبت مجوز استثنا"),
            (P.MechExceptions.Disable, "غیرفعال‌سازی مجوز و استثنا"),
        }),
        new(P.MechReasons.Group, "دلایل عدم امکان تولید", new[]
        {
            (P.MechReasons.View, "مشاهده دلایل عدم امکان تولید"),
            (P.MechReasons.Create, "ثبت دلیل عدم امکان تولید"),
            (P.MechReasons.Update, "ویرایش دلیل عدم امکان تولید"),
            (P.MechReasons.Delete, "حذف دلیل عدم امکان تولید"),
        }),
        new(P.MechReports.Group, "گزارش‌های امکان تولید مکانیکی", new[]
        {
            (P.MechReports.Productionpossibility, "گزارش امکان تولید مکانیکی"),
            (P.MechReports.General, "گزارش جامع اقلام بحرانی"),
            (P.MechReports.Incoming, "گزارش بدنه‌های وارد شده به خط به تفکیک بحران"),
            (P.MechReports.Stocklog, "گزارش تاریخچه موجودی اقلام بحرانی"),
            (P.MechReports.Sap, "گزارش‌های ارتباط با SAP"),
            (P.MechReports.Conflicts, "گزارش مغایرت ورود به مونتاژ"),
        }),
        new(P.SyncBasis.Group, "مبنای ست‌های سنکرون", new[]
        {
            (P.SyncBasis.View, "مشاهده مبناهای ست سنکرون"),
            (P.SyncBasis.Create, "ثبت مبنای ست سنکرون"),
            (P.SyncBasis.Update, "ویرایش مبنای ست سنکرون"),
        }),
        new(P.SyncSets.Group, "بحران‌های سنکرون", new[]
        {
            (P.SyncSets.View, "مشاهده بحران سنکرون"),
            (P.SyncSets.Create, "ثبت بحران سنکرون"),
            (P.SyncSets.Update, "ویرایش بحران سنکرون"),
            (P.SyncSets.Delete, "حذف بحران سنکرون"),
        }),
        new(P.SyncReports.Group, "گزارش‌های امکان تولید سنکرون", new[]
        {
            (P.SyncReports.Productionpossibility, "گزارش امکان تولید سنکرون"),
        }),
        new(P.UserAccess.Group, "دسترسی کاربران به خطوط", new[]
        {
            (P.UserAccess.View, "مشاهده کاربران و دسترسی‌ها"),
            (P.UserAccess.Delete, "حذف دسترسی کاربر به خط"),
            (P.UserAccess.Create, "ثبت دسترسی کاربر به خط بدنه‌سازی"),
        }),
        new(P.Lookups.Group, "فهرست‌های مرجع", new[]
        {
            (P.Lookups.View, "مشاهده فهرست‌های مرجع"),
        }),
        new(P.CurrentUser.Group, "کاربر جاری", new[]
        {
            (P.CurrentUser.View, "مشاهده اطلاعات کاربر جاری"),
        }),
    };

    public static IReadOnlyList<PermissionGroupDefinition> Ui { get; } = new List<PermissionGroupDefinition>
    {
        new("bodyshop.menu", "منوی بدنه‌سازی", new[]
        {
            ("bodyshop.menu.register", "ثبت بحران بدنه‌سازی"),
            ("bodyshop.menu.search", "جستجوی اقلام بحرانی"),
            ("bodyshop.menu.tracking", "گزارش ردیابی تغییرات اقلام بحرانی"),
            ("bodyshop.menu.criticalreport", "گزارش اقلام بحرانی"),
            ("bodyshop.menu.licensetracking", "ردیابی پارامترهای بحرانی"),
            ("bodyshop.menu.productsofsites", "محصولات به تفکیک خطوط بدنه‌سازی"),
            ("bodyshop.menu.preassigncount", "تعداد بدنه‌های پیش‌تخصیص‌شده"),
            ("bodyshop.menu.preassigncritical", "بدنه‌های پیش‌تخصیص بحرانی"),
            ("bodyshop.menu.conflicts", "مغایرت پیش‌تخصیص بدنه‌های بحرانی"),
            ("bodyshop.menu.eventlog", "گزارش رویدادها"),
            ("bodyshop.menu.sap", "قطعات ناموفق در انتقال از SAP"),
        }),
        new("mechanical.menu", "منوی مونتاژ مکانیکی", new[]
        {
            ("mechanical.menu.register", "ثبت بحران مکانیکی"),
            ("mechanical.menu.search", "جستجوی اقلام بحرانی (مکانیکی و سنکرون)"),
            ("mechanical.menu.exceptions", "استثناهای محصولات بحران"),
            ("mechanical.menu.reasons", "دلایل عدم امکان تولید"),
            ("mechanical.menu.contrast", "مغایرت زیرکلاس‌ها با SAP"),
            ("mechanical.menu.arrived", "اقلام بحرانی وارد شده به شرکت"),
            ("mechanical.menu.productionpossibility", "گزارش امکان تولید"),
            ("mechanical.menu.productionpossibilitylicense", "گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده"),
            ("mechanical.menu.productionpossibilitytrimshop", "امکان تولید به تفکیک سالن مونتاژ"),
            ("mechanical.menu.general", "گزارش جامع اقلام بحرانی"),
            ("mechanical.menu.incoming", "بدنه‌های وارد شده به خط به تفکیک بحران"),
            ("mechanical.menu.incomingsummary", "خلاصه بدنه‌های وارد شده به خط"),
            ("mechanical.menu.licensetracking", "ردیابی مجوزهای تولید با کسری"),
            ("mechanical.menu.licensecriticals", "بحران‌های دارای مجوز تولید با کسری"),
            ("mechanical.menu.usedlicensebodies", "بدنه‌های مرتبط با مجوز مصرف‌شده"),
            ("mechanical.menu.licenselog", "تاریخچه مجوزهای یک بحران"),
            ("mechanical.menu.exceptionparts", "شماره فنی‌های دارای استثنا"),
            ("mechanical.menu.exceptionlog", "تاریخچه مجوزهای استثنا"),
            ("mechanical.menu.stocklog", "تاریخچه موجودی اقلام بحرانی"),
            ("mechanical.menu.sapcritical", "لاگ انتقال اقلام بحرانی از SAP"),
            ("mechanical.menu.sapfailed", "قطعات ناموفق در انتقال از SAP"),
            ("mechanical.menu.asn", "گزارش ASN"),
            ("mechanical.menu.trimconflicts", "دلایل عدم امکان ورود خودرو به مونتاژ"),
            ("mechanical.menu.trimbodycount", "تعداد بدنه‌های ورودی به مونتاژ"),
            ("mechanical.menu.trimcriticalbodies", "بدنه‌های بحرانی ورودی به مونتاژ"),
        }),
        new("synchron.menu", "منوی مونتاژ سنکرون", new[]
        {
            ("synchron.menu.basis", "تعریف مبنای ست‌ها"),
            ("synchron.menu.sets", "ثبت و ویرایش بحران سنکرون"),
            ("synchron.menu.indicatorcars", "تعیین خودروی شاخص سالن‌ها"),
            ("synchron.menu.productionpossibility", "گزارش امکان تولید سنکرون"),
        }),
        new("usermanagement.menu", "منوی مدیریت کاربران", new[]
        {
            ("usermanagement.menu.access", "مدیریت کاربران"),
        }),
    };
}
