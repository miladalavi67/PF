using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.MechanicalGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/mechanical/critical-parts")]
public partial class MechPartsController : PfController
{
    private readonly IMechPartsAppService _service;

    public MechPartsController(IMechPartsAppService service)
    {
        _service = service;
    }

    /// <summary>مشخصات بدنه برای ثبت بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (شماره بدنه)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CarPropertiesSelectForCriticalParts</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/body-properties
    /// </remarks>
    [HttpGet("body-properties")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CarPropertiesRow>))]
    public Task<ListResultDto<CarPropertiesRow>> GetBodyPropertiesAsync([FromQuery] MechPartsGetBodyPropertiesInput input)
        => _service.GetBodyPropertiesAsync(input);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (شماره فنی)</c>.
    /// رویه ذخیره‌شده: <c>dbo.SIMP_SelectPartDescByPartNo</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/part-description
    /// </remarks>
    [HttpGet("part-description")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(string))]
    public Task<string?> GetPartDescriptionAsync([FromQuery] MechPartsGetPartDescriptionInput input)
        => _service.GetPartDescriptionAsync(input);

    /// <summary>اطلاعات قطعه در فهرست مرجع قطعات کسری</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart</c>.
    /// رویه ذخیره‌شده: <c>dbo.SIMP_SelectParInShortagePartList</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/part-in-shortage-list
    /// </remarks>
    [HttpGet("part-in-shortage-list")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<PartInShortageListRow>))]
    public Task<ListResultDto<PartInShortageListRow>> GetPartInShortageListAsync([FromQuery] MechPartsGetPartInShortageListInput input)
        => _service.GetPartInShortageListAsync(input);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (محصولات)</c>.
    /// رویه ذخیره‌شده: <c>dbo.SIMP_SelectProductsUsePartNo</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/part-products
    /// </remarks>
    [HttpGet("part-products")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BomProductRow>))]
    public Task<ListResultDto<BomProductRow>> GetPartProductsAsync([FromQuery] MechPartsGetPartProductsInput input)
        => _service.GetPartProductsAsync(input);

    /// <summary>قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetBasis / frmCriticalPart</c>.
    /// رویه ذخیره‌شده: <c>dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/products-by-class-part-type
    /// </remarks>
    [HttpGet("products-by-class-part-type")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<BomProductRow>))]
    public Task<ListResultDto<BomProductRow>> GetProductsByClassAndPartTypeAsync([FromQuery] MechPartsGetProductsByClassAndPartTypeInput input)
        => _service.GetProductsByClassAndPartTypeAsync(input);

    /// <summary>بررسی وجود بحران فعال برای شماره فنی در سالن</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsExistPartNo</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/exists-part-no
    /// </remarks>
    [HttpGet("exists-part-no")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartExistRow>))]
    public Task<ListResultDto<CriticalPartExistRow>> ExistsPartNoAsync([FromQuery] MechPartsExistsPartNoInput input)
        => _service.ExistsPartNoAsync(input);

    /// <summary>محصولات شماره فنی از BOP</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSelectFromBOP</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/from-bop
    /// </remarks>
    [HttpGet("from-bop")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalProductCandidateRow>))]
    public Task<ListResultDto<CriticalProductCandidateRow>> GetFromBopAsync([FromQuery] MechPartsGetFromBopInput input)
        => _service.GetFromBopAsync(input);

    /// <summary>محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSelectFromSAP</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/from-sap
    /// </remarks>
    [HttpGet("from-sap")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalProductCandidateRow>))]
    public Task<ListResultDto<CriticalProductCandidateRow>> GetFromSapAsync([FromQuery] MechPartsGetFromSapInput input)
        => _service.GetFromSapAsync(input);

    /// <summary>اطلاعات بحران برای ویرایش</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_SelectCriticalPartForEdit</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/{id:int}/for-edit
    /// </remarks>
    [HttpGet("{id:int}/for-edit")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartForEditRow>))]
    public Task<ListResultDto<CriticalPartForEditRow>> GetForEditAsync(int id)
        => _service.GetForEditAsync(id);

    /// <summary>جزئیات بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSearch</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSelectById</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/{id:int}
    /// </remarks>
    [HttpGet("{id:int}")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartRow>))]
    public Task<ListResultDto<CriticalPartRow>> GetByIdAsync(int id)
        => _service.GetByIdAsync(id);

    /// <summary>محصولات بحران برای ویرایش</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (ویرایش محصولات)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartProductsSelectForEdit_V1</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/{id:int}/products-for-edit
    /// </remarks>
    [HttpGet("{id:int}/products-for-edit")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartProductForEditRow>))]
    public Task<ListResultDto<CriticalPartProductForEditRow>> GetProductsForEditAsync(int id, [FromQuery] MechPartsGetProductsForEditInput input)
        => _service.GetProductsForEditAsync(id, input);

    /// <summary>محصولات ثبت‌شده یک بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSearch (جزئیات)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartProductsSelectByCriticalPartId_V2</c>.
    ///
    ///     GET /api/production-feasibility/mechanical/critical-parts/{criticalPartId:int}/products
    /// </remarks>
    [HttpGet("{criticalPartId:int}/products")]
    [Authorize(PfPermissions.MechParts.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(ListResultDto<CriticalPartProductRow>))]
    public Task<ListResultDto<CriticalPartProductRow>> GetProductsAsync(int criticalPartId)
        => _service.GetProductsAsync(criticalPartId);

    /// <summary>ثبت بحران مکانیکی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart.btnOk</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsINSERT</c>.
    /// مقدار برگشتی شناسه بحران ثبت‌شده است. خطاهای رویه (مثلاً بحران تکراری) با پیام فارسی B_SqlMessages برگردانده می‌شود.
    ///
    ///     POST /api/production-feasibility/mechanical/critical-parts
    ///     { "carId": 0, "partNo": "string", "partDesc": "string", "plantId": 0, "partGroup": "string", "alterNativePartNo": "string", "criticalPartNote": "string", "stock": 0, "used": 0, "possibleProductionNo": 0, "trimShopId": 0, "usageFactor": 0, "deptId": 0, "isOutOfSap": false }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.MechParts.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(int))]
    public Task<int> CreateAsync([FromBody] MechPartsCreateInput input)
        => _service.CreateAsync(input);

    /// <summary>ویرایش بحران مکانیکی</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPart (ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsUpdate</c>.
    ///
    ///     PUT /api/production-feasibility/mechanical/critical-parts/{id:int}
    ///     { "stock": 0, "used": 0, "usageFactor": "string", "alterNativePartNo": "string", "criticalPartNote": "string", "productionStateLetterNo": "string", "productionStateNote": "string", "partGroup": "string", "productionPossibility": 0, "isOutOfSap": false }
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(PfPermissions.MechParts.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task UpdateAsync(int id, [FromBody] MechPartsUpdateInput input)
        => _service.UpdateAsync(id, input);

    /// <summary>ثبت/به‌روزرسانی یک محصول بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>MechanicalParts/MechanicalCriticalPartsProductUpdate (بک‌اند قبلی) / frmCriticalPart (محصولات)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsPruductUpdate_Web</c>.
    /// رویه در صورت وجود محصول مشابه فعال تغییری نمی‌دهد؛ فراخوانی برای هر محصول جداگانه است. برای ثبت چندمحصولی از POST …/products/batch استفاده شود.
    ///
    ///     POST /api/production-feasibility/mechanical/critical-parts/{criticalPartsId:int}/products
    ///     { "carId": 0, "carGroupId": 0, "carModelId": 0, "carClassId": 0, "subClassId": 0, "colorId": 0, "trimId": 0, "optionId": 0, "isNew": 0, "trimShopId": 0, "plantId": 0, "quantity": 0 }
    /// </remarks>
    [HttpPost("{criticalPartsId:int}/products")]
    [Authorize(PfPermissions.MechParts.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> SaveProductAsync(int criticalPartsId, [FromBody] MechPartsSaveProductInput input)
        => _service.SaveProductAsync(criticalPartsId, input);

    /// <summary>افزودن زیرکلاس مغایر به بحران</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTContrastOfSubClasses (افزودن)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_AddContrastOfSubClasses</c>.
    ///
    ///     POST /api/production-feasibility/mechanical/critical-parts/add-subclass
    ///     { "criticalPartsId": 0, "subClassCode": "string", "trimShopId": 0 }
    /// </remarks>
    [HttpPost("add-subclass")]
    [Authorize(PfPermissions.MechParts.Compare)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task AddSubClassAsync([FromBody] MechPartsAddSubClassInput input)
        => _service.AddSubClassAsync(input);

    /// <summary>حذف بحران مکانیکی (به همراه استثناها و مجوزهای آن)</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>MechanicalParts/MechanicalCriticalPartDelete (بک‌اند قبلی) / frmCriticalPartSearch (حذف)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartDelete_Web</c>.
    ///
    ///     DELETE /api/production-feasibility/mechanical/critical-parts/{id:int}
    /// </remarks>
    [HttpDelete("{id:int}")]
    [Authorize(PfPermissions.MechParts.Delete)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(GlobalResultDto))]
    public Task<GlobalResultDto> DeleteAsync(int id)
        => _service.DeleteAsync(id);

}
