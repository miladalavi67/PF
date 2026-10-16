using IKCO.Auth.Services;
using IKCO.Infrastructure;
using IKCO.ProductionFeasibility.Host;
using IKCO.ProductionFeasibility.Host.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// ترتیب مطابق استاندارد IKCO: لاگ مرکزی → Autofac → ماژول ABP → زیرساخت سازمانی
var context = WebApplication.CreateBuilder(args);
context.Host.UseIKCOLogging();
context.Host.UseAutofac();
await context.Services.AddApplicationAsync<HostModule>();
context.Services.AddInfrastructure(context.Configuration, context.Environment);

var app = context.Build();
await app.InitializeApplicationAsync();
// مسیرهای /_auth/* متناسب با Oidc:Mode (در Embedded: صدور توکن و کاتالوگ دسترسی توسعه)
app.MapIkcoAuthEndpoints();
await app.RunAsync();
