using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Volo.Abp.Http;

namespace IKCO.ProductionFeasibility.Controllers;

/// <summary>
/// پاسخ‌های استاندارد 400/401/403/404/409/500 (با بدنه <see cref="RemoteServiceErrorResponse"/>) را به همه اکشن‌های
/// کنترلرهای <see cref="PfController"/> اضافه می‌کند تا Swagger طبق استاندارد IKCO کامل باشد.
/// </summary>
public class PfStandardResponsesConvention : IApplicationModelConvention
{
    private static readonly int[] StatusCodes =
    {
        Microsoft.AspNetCore.Http.StatusCodes.Status400BadRequest,
        Microsoft.AspNetCore.Http.StatusCodes.Status401Unauthorized,
        Microsoft.AspNetCore.Http.StatusCodes.Status403Forbidden,
        Microsoft.AspNetCore.Http.StatusCodes.Status404NotFound,
        Microsoft.AspNetCore.Http.StatusCodes.Status409Conflict,
        Microsoft.AspNetCore.Http.StatusCodes.Status500InternalServerError,
    };

    public void Apply(ApplicationModel application)
    {
        foreach (var controller in application.Controllers.Where(c => typeof(PfController).IsAssignableFrom(c.ControllerType)))
        foreach (var action in controller.Actions)
        foreach (var status in StatusCodes)
        {
            if (action.Filters.OfType<ProducesResponseTypeAttribute>().Any(f => f.StatusCode == status))
                continue;
            action.Filters.Add(new ProducesResponseTypeAttribute(typeof(RemoteServiceErrorResponse), status));
        }
    }
}
