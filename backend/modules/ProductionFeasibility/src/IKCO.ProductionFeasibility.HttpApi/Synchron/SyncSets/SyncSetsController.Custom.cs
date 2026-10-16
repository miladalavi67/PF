using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial class SyncSetsController
{
    /// <summary>ثبت قطعات، رنگ‌ها، سالن‌ها و محصولات بحران سنکرون</summary>
    /// <remarks>
    /// پس از «ثبت سرآیند» و با شناسه برگشتی فراخوانی می‌شود. قطعات/رنگ/سالن با رویه‌های <c>_Web</c> بک‌اند قبلی (در صورت وجود در پایگاه داده)
    /// وگرنه با درج مستقیم، و محصولات با <c>dbo.S_InsertCriticalPartsSet_Products</c> ثبت می‌شوند؛ همه در یک تراکنش.
    ///
    ///     POST /api/production-feasibility/synchron/sets/15/composition
    ///     { "parts": [ { "shortagePartListId": 10, "partNo": "9616837480" } ], "colorIds": [3], "saloonIds": [16], "products": [ { "carGroupId": 1, "carModelId": 2, "carClassId": 3, "carSubClassId": 4, "saloonId": 16 } ] }
    /// </remarks>
    [HttpPost("{criticalPartsSetId:int}/composition")]
    [Authorize(PfPermissions.SyncSets.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> SaveCompositionAsync(int criticalPartsSetId, [FromBody] SyncSetsSaveCompositionInput input)
        => _service.SaveCompositionAsync(criticalPartsSetId, input);
}
