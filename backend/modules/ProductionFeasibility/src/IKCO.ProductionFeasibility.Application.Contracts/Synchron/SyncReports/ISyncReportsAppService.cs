using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

/// <summary>گزارش‌های امکان تولید سنکرون</summary>
public partial interface ISyncReportsAppService : IApplicationService
{
    /// <summary>گزارش امکان تولید سنکرون</summary>
    Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityAsync(SyncReportsGetProductionPossibilityInput input);

    /// <summary>اقلام بحرانی دریافتی از SAP</summary>
    Task<ListResultDto<SapCriticalPartsMasterRow>> GetSapCriticalPartsMasterAsync(SyncReportsGetSapCriticalPartsMasterInput input);

    /// <summary>رزروهای دریافتی از SAP</summary>
    Task<ListResultDto<SapReserveMasterRow>> GetSapReserveMasterAsync(SyncReportsGetSapReserveMasterInput input);

    /// <summary>گزارش ASN (اعلان ورود محموله)</summary>
    Task<ListResultDto<AsnRow>> GetAsnReportAsync(SyncReportsGetAsnReportInput input);

}
