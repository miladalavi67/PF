using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;
using IKCO.ProductionFeasibility.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

public partial class MechLicensesController
{
    /// <summary>تغییر شرایط تولید / ثبت مجوز تولید با کسری</summary>
    /// <remarks>
    /// معادل <c>frmCriticalPartsProdPosChange</c>: نوع بحران 0 = مکانیکی، 1 = سنکرون. تعداد و تاریخ مجاز فقط برای «تولید با کسری» (۱) است؛
    /// خالی بودن هر دو یعنی نامحدود. <c>editLast=true</c> آخرین مجوز فعال را ویرایش می‌کند.
    ///
    ///     POST /api/production-feasibility/mechanical/licenses/change
    ///     { "criticalPartTypeId": 0, "criticalPartId": 15, "productionPossibility": 1, "possibleProductionNo": 50, "toDate": "1405/02/10", "toTime": "12:00:00", "letterNo": "123", "note": "توضیحات", "editLast": false }
    /// </remarks>
    [HttpPost("change")]
    [Authorize(PfPermissions.MechLicenses.Change)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> ChangeAsync([FromBody] MechLicensesChangeInput input) => _service.ChangeAsync(input);

    /// <summary>مجوز مصرف‌شده و باقی‌مانده‌های یک بحران</summary>
    /// <remarks>
    /// توابع <c>U_UsedLicenseOfCritical(Set)PossibleProductionNo</c>، <c>U_GetValidRemainingOfAllShouldPreviousShortageLicenses</c> و
    /// <c>U_GetValidRemainingOfLastLimitedShortageLicense</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/licenses/summary?criticalPartTypeId=0&amp;criticalPartId=15
    /// </remarks>
    [HttpGet("summary")]
    [Authorize(PfPermissions.MechLicenses.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(LicenseSummaryDto))]
    public Task<LicenseSummaryDto> GetSummaryAsync([FromQuery] MechLicensesGetSummaryInput input) => _service.GetSummaryAsync(input);
}
