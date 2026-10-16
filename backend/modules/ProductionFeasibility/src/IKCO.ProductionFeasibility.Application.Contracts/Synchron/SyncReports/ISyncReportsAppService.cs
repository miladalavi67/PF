using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

/// <summary>گزارش‌های امکان تولید سنکرون</summary>
public partial interface ISyncReportsAppService : IApplicationService
{
    /// <summary>گزارش امکان تولید سنکرون</summary>
    Task<TabularResult> GetProductionPossibilityAsync(SyncReportsGetProductionPossibilityInput input);

}
