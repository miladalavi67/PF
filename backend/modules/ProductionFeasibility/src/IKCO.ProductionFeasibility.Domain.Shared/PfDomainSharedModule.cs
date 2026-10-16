using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace IKCO.ProductionFeasibility;

[DependsOn(typeof(AbpValidationModule))]
public class PfDomainSharedModule : AbpModule
{
}
