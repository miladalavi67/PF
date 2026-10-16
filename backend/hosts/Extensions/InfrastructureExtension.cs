using System.Linq;
using IKCO.Infrastructure;
using IKCO.ProductionFeasibility.Permissions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IKCO.ProductionFeasibility.Host.Extensions;

/// <summary>
/// اتصال به زیرساخت سازمانی (IKCO.Infrastructure 2.0.5 / IKCO.Auth 2.0.4): احراز هویت و مجوز IKCO، لاگ مرکزی و ثبت
/// درخت دسترسی‌ها. IdP در این پروژه پیاده‌سازی نمی‌شود؛ حالت با <c>Oidc:Mode</c> انتخاب می‌شود
/// (Idp در TEST/Production، Embedded در Development با کاربران توسعه تعریف‌شده در تنظیمات).
/// </summary>
public static class InfrastructureExtension
{
    public const string SubsystemCode = "productionfeasibility";
    public const string EmbeddedCompany = "ikco";
    public const string EmbeddedDeveloperRole = "productionfeasibility-developer";
    public const string EmbeddedNoAccessRole = "productionfeasibility-noaccess";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
    {
        services.AddIKCOInfrastructure(configuration, ikco =>
        {
            ikco.ConfigureAuthorization(options => options.SubsystemCode = configuration["Ikco:Authorization:SubsystemCode"] ?? SubsystemCode);

            // کنترلرهای داخلی ABP منتشر نمی‌شوند؛ فقط کنترلرهای صریح HttpApi
            ikco.ConfigureEndpointDiscovery(options => options.ExcludeAbpControllers = true);

            ikco.ConfigurePermissions(p =>
            {
                p.Api(api =>
                {
                    foreach (var group in PfPermissionTree.Api)
                        api.Group(group.Code, group.DisplayName, g =>
                        {
                            foreach (var leaf in group.Leaves)
                                g.Leaf(leaf.Code, leaf.DisplayName);
                        });
                });

                p.Ui(ui =>
                {
                    foreach (var group in PfPermissionTree.Ui)
                        ui.Group(group.Code, group.DisplayName, g =>
                        {
                            foreach (var leaf in group.Leaves)
                                g.Leaf(leaf.Code, leaf.DisplayName);
                        });
                });
            });

            // فقط در Oidc:Mode=Embedded (محیط توسعه) اثر دارد؛ در Idp نادیده گرفته می‌شود
            ikco.ConfigureEmbedded(embedded =>
            {
                var allCodes = PfPermissionTree.Api.Concat(PfPermissionTree.Ui)
                    .SelectMany(g => g.Leaves).Select(l => l.Code).ToList();

                embedded.Catalog(catalog =>
                {
                    catalog.Subsystem(SubsystemCode, "سامانه جامع امکان تولید", s =>
                    {
                        s.Role(EmbeddedDeveloperRole, "توسعه‌دهنده امکان تولید (همه دسترسی‌ها)", allCodes, System.Array.Empty<string>());
                        // برای آزمون 403: ورود به زیرسامانه بدون هیچ دسترسی عملیاتی
                        s.Role(EmbeddedNoAccessRole, "کاربر آزمایشی بدون دسترسی", System.Array.Empty<string>(), System.Array.Empty<string>());
                    });
                    catalog.Company(EmbeddedCompany, company =>
                        company.Position("ROOT", "ریشه", System.Array.Empty<string>(), null!));
                });

                embedded.Assignments(assignments =>
                {
                    assignments.Company(EmbeddedCompany, "ایران خودرو");

                    // کاربران توسعه از تنظیمات (user-secrets / متغیر محیطی) خوانده می‌شوند؛ هیچ کاربری در کد ثابت نشده است
                    foreach (var user in configuration.GetSection("ProductionFeasibility:Embedded:Users").GetChildren())
                    {
                        var userName = user["UserName"];
                        if (string.IsNullOrWhiteSpace(userName))
                            continue;
                        var roles = user.GetSection("Roles").Get<string[]>() ?? new[] { EmbeddedDeveloperRole };
                        assignments.User(userName, user["DisplayName"] ?? userName, user["Email"] ?? string.Empty, user["NationalCode"] ?? string.Empty)
                            .Company(EmbeddedCompany, access => access.Roles(roles));
                    }
                });
            });
        });

        return services;
    }
}
