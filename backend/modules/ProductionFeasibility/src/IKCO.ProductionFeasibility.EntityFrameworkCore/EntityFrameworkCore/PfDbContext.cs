using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.EntityFrameworkCore;

/// <summary>
/// DbContext روی پایگاه داده موجود ProductionControl. پایگاه داده مرجع است و توسط سامانه‌های دیگر مدیریت می‌شود؛
/// هیچ migration یا جدولی توسط این پروژه ساخته نمی‌شود و همه عملیات داده از رویه‌های ذخیره‌شده (Dapper) انجام می‌شود.
/// DbContext فقط مدیریت اتصال و تراکنش (unit of work) را به عهده دارد.
/// </summary>
[ConnectionStringName(PfDbProperties.ConnectionStringName)]
public class PfDbContext : AbpDbContext<PfDbContext>
{
    public PfDbContext(DbContextOptions<PfDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
