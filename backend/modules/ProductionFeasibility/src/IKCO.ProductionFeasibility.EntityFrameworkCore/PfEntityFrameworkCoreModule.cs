using IKCO.ProductionFeasibility.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Dapper;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.SqlServer;
using Volo.Abp.Modularity;

namespace IKCO.ProductionFeasibility;

[DependsOn(
    typeof(PfDomainModule),
    typeof(AbpEntityFrameworkCoreSqlServerModule),
    typeof(AbpDapperModule)
)]
public class PfEntityFrameworkCoreModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        context.Services.AddAbpDbContext<PfDbContext>();

        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<PfDbContext>(c => c.UseSqlServer());
        });
    }
}
