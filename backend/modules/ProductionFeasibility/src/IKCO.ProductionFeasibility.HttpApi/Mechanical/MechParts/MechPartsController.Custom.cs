using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsController
{
    /// <summary>ثبت چند محصول برای یک بحران مکانیکی</summary>
    /// <remarks>
    /// رویه <c>dbo.S_CriticalPartsPruductUpdate_Web</c> (همان رویه <c>MechanicalCriticalPartsProductUpdate</c> بک‌اند قبلی) برای هر محصول،
    /// همه در یک تراکنش؛ اگر محصول مشابه فعال باشد تغییری نمی‌دهد. خطای رویه (Result=0) کل ثبت را برمی‌گرداند.
    ///
    ///     POST /api/production-feasibility/mechanical/critical-parts/15/products/batch
    ///     { "carId": 123456, "trimShopId": 16, "plantId": 1, "isNew": 1, "items": [ { "carGroupId": 1, "carModelId": 2, "carClassId": 3, "subClassId": 4, "colorId": -1, "trimId": -1, "optionId": -1, "quantity": 1 } ] }
    /// </remarks>
    [HttpPost("{criticalPartsId:int}/products/batch")]
    [Authorize(PfPermissions.MechParts.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> SaveProductsAsync(int criticalPartsId, [FromBody] MechPartsSaveProductsInput input)
        => _service.SaveProductsAsync(criticalPartsId, input);

    /// <summary>مقایسه زیرکلاس‌های SAP با SFC</summary>
    /// <remarks>
    /// معادل <c>frmRPTContrastOfSubClasses</c> (رویه <c>dbo.S_GetContrastOfSubClasses</c> با پارامتر جدولی از فایل اکسل SAP).
    ///
    ///     POST /api/production-feasibility/mechanical/critical-parts/compare-subclasses
    ///     { "saloonId": 16, "type": 1, "rows": [ { "subClassCode": "1234567", "partNo": "9616837480" } ] }
    /// </remarks>
    [HttpPost("compare-subclasses")]
    [Authorize(PfPermissions.MechParts.Compare)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SubClassContrastRow>))]
    public Task<ListResultDto<SubClassContrastRow>> CompareSubClassesAsync([FromBody] MechPartsCompareSubClassesInput input)
        => _service.CompareSubClassesAsync(input);
}
