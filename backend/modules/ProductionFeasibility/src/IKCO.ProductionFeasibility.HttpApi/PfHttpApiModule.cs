using IKCO.ProductionFeasibility.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(PfApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class PfHttpApiModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        Configure<MvcOptions>(options => options.Conventions.Add(new PfStandardResponsesConvention()));
    }
}
