using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(PfApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class PfHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddHttpClientProxies(
            typeof(PfApplicationContractsModule).Assembly,
            PfRemoteServiceConsts.RemoteServiceName);
    }
}
