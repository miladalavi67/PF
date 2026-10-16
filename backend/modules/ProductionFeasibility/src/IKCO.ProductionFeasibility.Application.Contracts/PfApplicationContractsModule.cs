using Volo.Abp.Application;
using Volo.Abp.Authorization;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(PfDomainSharedModule),
    typeof(AbpDddApplicationContractsModule),
    typeof(AbpAuthorizationAbstractionsModule)
)]
public class PfApplicationContractsModule : AbpModule
{
}
