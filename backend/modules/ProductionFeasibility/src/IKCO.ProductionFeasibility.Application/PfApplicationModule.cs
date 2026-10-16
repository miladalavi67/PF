using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(PfDomainModule),
    typeof(PfApplicationContractsModule),
    typeof(AbpDddApplicationModule)
)]
public class PfApplicationModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // HttpClient با HttpClientFactory برای کلاینت SAP
        context.Services.AddHttpClient();
    }
}
