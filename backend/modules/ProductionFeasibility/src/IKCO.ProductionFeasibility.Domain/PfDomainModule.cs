using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PfDomainSharedModule)
)]
public class PfDomainModule : AbpModule
{
}
