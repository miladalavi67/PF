namespace IKCO.ProductionFeasibility.Permissions;

/// <summary>
/// کلیدهای دسترسی API (همه با حروف کوچک: <c>&lt;entity&gt;.&lt;action&gt;</c>). درخت کامل با شرح فارسی در
/// <see cref="PfPermissionTree"/> تعریف شده و در InfrastructureExtension ثبت می‌شود. کلیدهای UI منوها
/// (<c>&lt;section&gt;.menu.&lt;page&gt;</c>) در <see cref="PfPermissionTree.Ui"/> و mfe-manifest فرانت‌اند هستند.
/// </summary>
public static class PfPermissions
{
    public static class BodyCriticals
    {
        public const string Group = "bodyshop.criticalparts";
        public const string View = "bodyshop.criticalparts.view";
        public const string Create = "bodyshop.criticalparts.create";
        public const string Update = "bodyshop.criticalparts.update";
        public const string Disable = "bodyshop.criticalparts.disable";
        public const string License = "bodyshop.criticalparts.license";
    }

    public static class BodyReports
    {
        public const string Group = "bodyshop.reports";
        public const string Search = "bodyshop.reports.search";
        public const string Tracking = "bodyshop.reports.tracking";
        public const string Productsofsites = "bodyshop.reports.productsofsites";
        public const string Preassigncount = "bodyshop.reports.preassigncount";
        public const string Preassigncritical = "bodyshop.reports.preassigncritical";
        public const string Conflicts = "bodyshop.reports.conflicts";
        public const string Licensetracking = "bodyshop.reports.licensetracking";
        public const string Sap = "bodyshop.reports.sap";
    }

    public static class MechParts
    {
        public const string Group = "mechanical.criticalparts";
        public const string View = "mechanical.criticalparts.view";
        public const string Create = "mechanical.criticalparts.create";
        public const string Update = "mechanical.criticalparts.update";
        public const string Compare = "mechanical.criticalparts.compare";
        public const string Delete = "mechanical.criticalparts.delete";
    }

    public static class MechSearch
    {
        public const string Group = "mechanical.search";
        public const string Search = "mechanical.search.search";
        public const string Arrived = "mechanical.search.arrived";
    }

    public static class MechLicenses
    {
        public const string Group = "mechanical.licenses";
        public const string View = "mechanical.licenses.view";
        public const string Report = "mechanical.licenses.report";
        public const string Change = "mechanical.licenses.change";
    }

    public static class MechExceptions
    {
        public const string Group = "mechanical.exceptions";
        public const string View = "mechanical.exceptions.view";
        public const string Create = "mechanical.exceptions.create";
        public const string Disable = "mechanical.exceptions.disable";
    }

    public static class MechReasons
    {
        public const string Group = "mechanical.notpossiblereasons";
        public const string View = "mechanical.notpossiblereasons.view";
        public const string Create = "mechanical.notpossiblereasons.create";
        public const string Update = "mechanical.notpossiblereasons.update";
        public const string Delete = "mechanical.notpossiblereasons.delete";
    }

    public static class MechReports
    {
        public const string Group = "mechanical.reports";
        public const string Productionpossibility = "mechanical.reports.productionpossibility";
        public const string General = "mechanical.reports.general";
        public const string Incoming = "mechanical.reports.incoming";
        public const string Stocklog = "mechanical.reports.stocklog";
        public const string Sap = "mechanical.reports.sap";
        public const string Conflicts = "mechanical.reports.conflicts";
    }

    public static class SyncBasis
    {
        public const string Group = "synchron.basis";
        public const string View = "synchron.basis.view";
        public const string Create = "synchron.basis.create";
        public const string Update = "synchron.basis.update";
    }

    public static class SyncSets
    {
        public const string Group = "synchron.sets";
        public const string View = "synchron.sets.view";
        public const string Create = "synchron.sets.create";
        public const string Update = "synchron.sets.update";
        public const string Delete = "synchron.sets.delete";
    }

    public static class SyncReports
    {
        public const string Group = "synchron.reports";
        public const string Productionpossibility = "synchron.reports.productionpossibility";
    }

    public static class UserAccess
    {
        public const string Group = "usermanagement.useraccess";
        public const string View = "usermanagement.useraccess.view";
        public const string Delete = "usermanagement.useraccess.delete";
        public const string Create = "usermanagement.useraccess.create";
    }

    public static class Lookups
    {
        public const string Group = "lookups";
        public const string View = "lookups.view";
    }

    public static class CurrentUser
    {
        public const string Group = "currentuser";
        public const string View = "currentuser.view";
    }

}
