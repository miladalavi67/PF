using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Controllers;
using IKCO.ProductionFeasibility.Permissions;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

/// <summary>گزارش‌های امکان تولید سنکرون</summary>
[ApiExplorerSettings(GroupName = PfRemoteServiceConsts.SynchronGroup)]
[Route(PfRemoteServiceConsts.RoutePrefix + "/synchron/reports")]
public partial class SyncReportsController : PfController
{
    private readonly ISyncReportsAppService _service;

    public SyncReportsController(ISyncReportsAppService service)
    {
        _service = service;
    }

    /// <summary>گزارش امکان تولید سنکرون</summary>
    /// <remarks>
    /// معادل فرم/متد سامانه ویندوزی: <c>frmRPTProductionPossibility (سنکرون)</c>.
    /// رویه ذخیره‌شده: <c>dbo.S_RptCriticalPartsProduction_Synchron</c>.
    ///
    ///     POST /api/production-feasibility/synchron/reports/production-possibility
    ///     { "saloonId": "string", "crisisTypeId": 0, "criticalPartTypeId": 0, "subClassId": 0, "partNo": "string", "carModelId": 0, "carClassId": 0 }
    /// </remarks>
    [HttpPost("production-possibility")]
    [Authorize(PfPermissions.SyncReports.Productionpossibility)]
    [SwaggerResponse(StatusCodes.Status200OK, "عملیات با موفقیت انجام شد", typeof(TabularResult))]
    public Task<TabularResult> GetProductionPossibilityAsync([FromBody] SyncReportsGetProductionPossibilityInput input)
        => _service.GetProductionPossibilityAsync(input);

}
