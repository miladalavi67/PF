using System.Linq;
using IKCO.Infrastructure.Services;
using IKCO.ProductionFeasibility.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IKCO.ProductionFeasibility.Host.Identity;

/// <summary>
/// هویت کاربر از توکن IKCO.Auth: نام کاربری از <see cref="ICurrentUserService"/> (یا claim <c>sub</c>).
/// فقط در محیط Development و وقتی درخواست احراز هویت نشده باشد (Oidc:Mode=Inactive) مقدار
/// <c>ProductionFeasibility:Development:UserName</c> استفاده می‌شود.
/// </summary>
public class HttpPfIdentityProvider : IPfIdentityProvider
{
    public const string SubjectClaim = "sub";

    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;
    private readonly IHostEnvironment _environment;

    public HttpPfIdentityProvider(IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IHostEnvironment environment)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _environment = environment;
    }

    public string? UserName
    {
        get
        {
            var principal = _httpContextAccessor.HttpContext?.User;
            if (principal?.Identity?.IsAuthenticated == true)
            {
                var service = _httpContextAccessor.HttpContext!.RequestServices.GetService<ICurrentUserService>();
                var name = service?.UserName;
                return string.IsNullOrWhiteSpace(name) ? principal.Claims.FirstOrDefault(c => c.Type == SubjectClaim)?.Value : name;
            }

            if (!_environment.IsDevelopment())
                return null;
            var value = _configuration["ProductionFeasibility:Development:UserName"];
            return string.IsNullOrWhiteSpace(value) ? null : value;
        }
    }
}
