namespace IKCO.ProductionFeasibility;

public static class PfRemoteServiceConsts
{
    public const string RemoteServiceName = "ProductionFeasibility";

    public const string ModuleName = "production-feasibility";

    /// <summary>پیشوند مسیر همه API ها</summary>
    public const string RoutePrefix = "api/production-feasibility";

    /// <summary>گروه‌های Swagger (هر گروه یک SwaggerDoc در HostModule)</summary>
    public const string BodyShopGroup = "bodyshop";
    public const string MechanicalGroup = "mechanical";
    public const string SynchronGroup = "synchron";
    public const string OptimizationGroup = "optimization";
    public const string UserManagementGroup = "usermanagement";
    public const string CommonGroup = "common";
}
