using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet)</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.SynchronGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/synchron/sets")]
public partial class SyncSetsController : PfController
{
    private readonly ISyncSetsAppService _service;

    public SyncSetsController(ISyncSetsAppService service)
    {
        _service = service;
    }

    /// <summary>اطلاعات بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetSelectById</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{id:int}
    /// </remarks>
    [HttpGet("{id:int}")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetByIdAsync(int id)
        => _service.GetByIdAsync(id);

    /// <summary>قطعات بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/parts
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/parts")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetPartsAsync(int criticalPartsSetId)
        => _service.GetPartsAsync(criticalPartsSetId);

    /// <summary>قطعات بحران سنکرون به همراه فهرست مرجع کسری</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/parts-with-shortage-list
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/parts-with-shortage-list")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetPartsWithShortageListAsync(int criticalPartsSetId)
        => _service.GetPartsWithShortageListAsync(criticalPartsSetId);

    /// <summary>رنگ‌های بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetColorSelectByCriticalPartSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/colors
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/colors")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetColorsAsync(int criticalPartsSetId)
        => _service.GetColorsAsync(criticalPartsSetId);

    /// <summary>سالن‌های بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetIndicatorCars</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartSetId:int}/saloons
    /// </remarks>
    [HttpGet("{criticalPartSetId:int}/saloons")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetSaloonsAsync(int criticalPartSetId)
        => _service.GetSaloonsAsync(criticalPartSetId);

    /// <summary>محصولات بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/products
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/products")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductsAsync(int criticalPartsSetId)
        => _service.GetProductsAsync(criticalPartsSetId);

    /// <summary>جزئیات محصولات بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/product-details
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/product-details")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductDetailsAsync(int criticalPartsSetId)
        => _service.GetProductDetailsAsync(criticalPartsSetId);

    /// <summary>تعداد سالن‌های بدون خودروی شاخص</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetIndicatorCars</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount</c>.
    ///
    ///     GET /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/not-entered-indicator-cars-count
    /// </remarks>
    [HttpGet("{criticalPartsSetId:int}/not-entered-indicator-cars-count")]
    [Authorize(PfPermissions.SyncSets.View)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId)
        => _service.GetNotEnteredIndicatorCarsCountAsync(criticalPartsSetId);

    /// <summary>ثبت سرآیند بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet.btnSave</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetInsert</c>.
    /// مقدار برگشتی شناسه بحران سنکرون ثبت‌شده (Id) است؛ سپس قطعات، رنگ‌ها، سالن‌ها و محصولات ثبت می‌شوند.
    ///
    ///     POST /api/production-feasibility/synchron/sets
    ///     { "setId": 0, "deptId": 0, "productsColorCode": "string", "productsColorId": 0, "productsTrimId": 0, "productsToodooziId": 0, "productsOptionId": 0, "stock": 0, "note": "string" }
    /// </remarks>
    [HttpPost]
    [Authorize(PfPermissions.SyncSets.Create)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(int))]
    public Task<int> CreateHeaderAsync([FromBody] SyncSetsCreateHeaderInput input)
        => _service.CreateHeaderAsync(input);

    /// <summary>ویرایش بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet (ویرایش)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetUpdate</c>.
    ///
    ///     PUT /api/production-feasibility/synchron/sets/{id:int}
    ///     { "deptId": 0, "stock": 0, "note": "string", "status": 0 }
    /// </remarks>
    [HttpPut("{id:int}")]
    [Authorize(PfPermissions.SyncSets.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task UpdateHeaderAsync(int id, [FromBody] SyncSetsUpdateHeaderInput input)
        => _service.UpdateHeaderAsync(id, input);

    /// <summary>ثبت یک محصول بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet (محصولات)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetProductInsert</c>.
    ///
    ///     POST /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/products
    ///     { "carGroupId": 0, "carModelId": 0, "carClassId": 0, "carSubClassId": 0, "carTrimId": 0, "carToodooziId": 0, "carOptionId": 0, "carColorId": 0, "saloonId": 0 }
    /// </remarks>
    [HttpPost("{criticalPartsSetId:int}/products")]
    [Authorize(PfPermissions.SyncSets.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task AddProductAsync(int criticalPartsSetId, [FromBody] SyncSetsAddProductInput input)
        => _service.AddProductAsync(criticalPartsSetId, input);

    /// <summary>تعیین خودروی شاخص سالن بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSetIndicatorCars</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetSaloonUpdate</c>.
    ///
    ///     PUT /api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/saloons/{id:int}
    ///     { "saloonId": 0, "indicatorCarId": 0 }
    /// </remarks>
    [HttpPut("{criticalPartsSetId:int}/saloons/{id:int}")]
    [Authorize(PfPermissions.SyncSets.Update)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, [FromBody] SyncSetsUpdateSaloonIndicatorCarInput input)
        => _service.UpdateSaloonIndicatorCarAsync(id, criticalPartsSetId, input);

    /// <summary>حذف منطقی بحران سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmCriticalPartSet (حذف)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_CriticalPartsSetDeleteLogically</c>.
    ///
    ///     DELETE /api/production-feasibility/synchron/sets/{id:int}
    /// </remarks>
    [HttpDelete("{id:int}")]
    [Authorize(PfPermissions.SyncSets.Delete)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد")]
    public Task DeleteAsync(int id)
        => _service.DeleteAsync(id);

}
