using System;
using System.IO;
using System.Linq;
using System.Net;
using IKCO.Infrastructure;
using IKCO.ProductionFeasibility.Host.Identity;
using IKCO.ProductionFeasibility.Sap;
using IKCO.ProductionFeasibility.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Volo.Abp;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;
using Volo.Abp.Swashbuckle;
using Volo.Abp.Timing;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.Host;

[DependsOn(
    typeof(PfApplicationModule),
    typeof(PfHttpApiModule),
    typeof(PfEntityFrameworkCoreModule),
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(AbpSwashbuckleModule)
)]
public class HostModule : AbpModule
{
    private const string CorsPolicyName = "PfFrontend";

    private static readonly (string Name, string Title)[] SwaggerDocs =
    {
        (PfRemoteServiceConsts.BodyShopGroup, "امکان تولید - بدنه‌سازی"),
        (PfRemoteServiceConsts.MechanicalGroup, "امکان تولید - مونتاژ مکانیکی"),
        (PfRemoteServiceConsts.SynchronGroup, "امکان تولید - مونتاژ سنکرون"),
        (PfRemoteServiceConsts.OptimizationGroup, "الگوریتم رفع کسری (کاندیداها)"),
        (PfRemoteServiceConsts.UserManagementGroup, "مدیریت کاربران امکان تولید"),
        (PfRemoteServiceConsts.CommonGroup, "کاربر جاری و فهرست‌های مرجع"),
    };

    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();

        // ستون‌های تاریخ پایگاه داده موجود به وقت محلی سرور ذخیره شده‌اند (DateTime.Now سامانه قدیم)
        Configure<AbpClockOptions>(options => options.Kind = DateTimeKind.Local);

        // رویه‌های ذخیره‌شده خودشان تراکنش و خطا را مدیریت می‌کنند (مانند سامانه ویندوزی)؛ تراکنش سراسری ABP غیرفعال است
        // و فقط عملیات چندمرحله‌ای صریح با [UnitOfWork(isTransactional: true)] تراکنشی می‌شوند.
        Configure<AbpUnitOfWorkDefaultOptions>(options => options.TransactionBehavior = UnitOfWorkTransactionBehavior.Disabled);

        ConfigureExceptionStatusCodes();
        ConfigureSwagger(context);
        ConfigureCors(context, configuration);

        context.Services.AddHttpContextAccessor();
        context.Services.AddTransient<IPfIdentityProvider, HttpPfIdentityProvider>();
        context.Services.Configure<SapOptions>(configuration.GetSection(SapOptions.SectionName));
    }

    private void ConfigureExceptionStatusCodes()
    {
        Configure<AbpExceptionHttpStatusCodeOptions>(options =>
        {
            options.Map(nameof(PfErrorCodes.UnAuthorized), HttpStatusCode.Unauthorized);
            options.Map(nameof(PfErrorCodes.PfUserNotFound), HttpStatusCode.Forbidden);
            options.Map(nameof(PfErrorCodes.PersonNotFound), HttpStatusCode.Forbidden);
            options.Map(nameof(PfErrorCodes.SaloonAccessDenied), HttpStatusCode.Forbidden);
            options.Map(nameof(PfErrorCodes.SQLServerNetworkNotResponding), HttpStatusCode.ServiceUnavailable);
            options.Map(nameof(PfErrorCodes.ExternalServiceUnavailable), HttpStatusCode.ServiceUnavailable);
            options.Map(nameof(PfErrorCodes.DatabaseCommandTimeout), HttpStatusCode.GatewayTimeout);
            options.Map(nameof(PfErrorCodes.RecordNotFound), HttpStatusCode.NotFound);
            options.Map(nameof(PfErrorCodes.LookupNotFound), HttpStatusCode.NotFound);
            options.Map(nameof(PfErrorCodes.DuplicateRecord), HttpStatusCode.Conflict);
            options.Map(nameof(PfErrorCodes.ReferenceConstraintViolation), HttpStatusCode.Conflict);

            foreach (var code in new[]
                     {
                         PfErrorCodes.StoredProcedureError, PfErrorCodes.PartNumberInvalid, PfErrorCodes.BodyNumberInvalid,
                         PfErrorCodes.InputInvalid, PfErrorCodes.ShamsiDateInvalid, PfErrorCodes.IdListInvalid,
                         PfErrorCodes.OperationFailed, PfErrorCodes.BatchInputInvalid, PfErrorCodes.ExternalServiceResponseInvalid,
                         PfErrorCodes.OptimizationNotAllowed,
                     })
                options.Map(code.ToString(), HttpStatusCode.BadRequest);
        });
    }

    private static void ConfigureSwagger(ServiceConfigurationContext context)
    {
        context.Services.AddAbpSwaggerGen(options =>
        {
            foreach (var (name, title) in SwaggerDocs)
                options.SwaggerDoc(name, new OpenApiInfo { Title = title, Version = "v1" });

            options.DocInclusionPredicate((docName, description) => description.GroupName == docName);
            options.CustomSchemaIds(type => type.FullName);
            options.EnableAnnotations();

            foreach (var xml in Directory.GetFiles(AppContext.BaseDirectory, "IKCO.ProductionFeasibility.*.xml"))
                options.IncludeXmlComments(xml, includeControllerXmlComments: true);
        });
    }

    private static void ConfigureCors(ServiceConfigurationContext context, IConfiguration configuration)
    {
        // به جای AllowAnyOrigin قالب، فقط مبداهای تنظیم‌شده (Daricheh و محیط توسعه) مجازند
        var origins = (configuration["App:CorsOrigins"] ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(o => o.TrimEnd('/'))
            .ToArray();

        context.Services.AddCors(options => options.AddPolicy(CorsPolicyName, builder =>
            builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();

        app.UseRouting();
        app.UseAbpRequestLocalization();
        app.UseCors(CorsPolicyName);
        app.UseCorrelationId();
        // مدیریت خطا، لاگ درخواست/پاسخ سازمانی و احراز هویت/مجوز IKCO.Auth
        app.UseIKCOInfrastructure();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseSwagger();
        app.UseAbpSwaggerUI(options =>
        {
            foreach (var (name, title) in SwaggerDocs)
                options.SwaggerEndpoint($"/swagger/{name}/swagger.json", title);
        });
        app.UseAuditing();
        app.UseAbpSerilogEnrichers();
        app.UseConfiguredEndpoints();
    }
}
