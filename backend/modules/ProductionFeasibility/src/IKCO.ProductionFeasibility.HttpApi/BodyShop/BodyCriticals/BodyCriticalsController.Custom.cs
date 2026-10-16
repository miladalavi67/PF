using System.Threading.Tasks;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

public partial class BodyCriticalsController
{
    /// <summary>ثبت کلاس‌های مصرف‌کننده یک بحران بدنه‌سازی</summary>
    /// <remarks>
    /// معادل <c>DA_Critical.CreateCriticalPartsProduct</c> (رویه <c>PF.S_CreateCritocalPartsProduct</c> برای هر کلاس) در یک تراکنش.
    /// پس از «ثبت بحران» با شناسه برگشتی (Result) فراخوانی می‌شود.
    ///
    ///     POST /api/production-feasibility/body-shop/critical-parts/15/products
    ///     { "carId": 123456, "items": [ { "carClassId": 7, "usageFactor": 1 } ] }
    /// </remarks>
    [HttpPost("{criticalPartsId:int}/products")]
    [Authorize(PfPermissions.BodyCriticals.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> CreateProductsAsync(int criticalPartsId, [FromBody] BodyCriticalsCreateProductsInput input)
        => _service.CreateProductsAsync(criticalPartsId, input);

    /// <summary>ثبت مجوز تولید با کسری (تغییر شرایط تولید)</summary>
    /// <remarks>
    /// معادل <c>DA_Critical.RegisterShortageLicense</c> (رویه <c>PF.RegisterShortageLicense_V1</c>)؛ کاربر ثبت‌کننده و سازنده از سرور تعیین می‌شود.
    ///
    ///     POST /api/production-feasibility/body-shop/critical-parts/licenses
    ///     { "criticalPartId": 15, "criticalPartTypeId": 1, "productionPossibilityId": 1, "possibleProductionNo": 50, "productionPossibilityToDate": "1405/02/10", "productionPossibilityToTime": "12:00:00", "note": "توضیحات" }
    /// </remarks>
    [HttpPost("licenses")]
    [Authorize(PfPermissions.BodyCriticals.License)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> RegisterLicenseAsync([FromBody] BodyCriticalsRegisterLicenseInput input)
        => _service.RegisterLicenseAsync(input);

    /// <summary>ثبت گروهی بحران‌ها از اکسل</summary>
    /// <remarks>
    /// معادل <c>frmUploadParts</c> (رویه <c>PF.S_CreateBatchCriticalPartProduct</c>)؛ خروجی نتیجه هر ردیف است.
    ///
    ///     POST /api/production-feasibility/body-shop/critical-parts/batch
    ///     { "biwSiteId": 1, "rows": [ { "bodyNo": "12345678", "partNo": "9616837480", "partGroup": "A", "groupStock": 10, "rawStock": 5, "carClassCode": 7, "quantity": 1 } ] }
    /// </remarks>
    [HttpPost("batch")]
    [Authorize(PfPermissions.BodyCriticals.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> BatchCreateAsync([FromBody] BodyCriticalsBatchCreateInput input) => _service.BatchCreateAsync(input);

    /// <summary>زیرکلاس‌های مصرف‌کننده شماره فنی از SAP</summary>
    /// <remarks>
    /// معادل <c>GetSubClassFromSAP</c> بک‌اند قبلی (سرویس SOAP ZMAT_WHERE_USED)؛ نشانی و اعتبار از تنظیمات
    /// <c>ProductionFeasibility:Sap</c> خوانده می‌شود.
    ///
    ///     GET /api/production-feasibility/body-shop/critical-parts/sap/where-used?partNo=9616837480&amp;saloonCode=B1
    /// </remarks>
    [HttpGet("sap/where-used")]
    [Authorize(PfPermissions.BodyCriticals.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<SapWhereUsedItemDto>))]
    public Task<ListResultDto<SapWhereUsedItemDto>> GetSapWhereUsedAsync([FromQuery] BodyCriticalsGetSapWhereUsedInput input)
        => _service.GetSapWhereUsedAsync(input);
}
