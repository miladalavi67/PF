using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet)</summary>
public partial interface ISyncSetsAppService : IApplicationService
{
    /// <summary>اطلاعات بحران سنکرون</summary>
    Task<TabularResult> GetByIdAsync(int id);

    /// <summary>قطعات بحران سنکرون</summary>
    Task<TabularResult> GetPartsAsync(int criticalPartsSetId);

    /// <summary>قطعات بحران سنکرون به همراه فهرست مرجع کسری</summary>
    Task<TabularResult> GetPartsWithShortageListAsync(int criticalPartsSetId);

    /// <summary>رنگ‌های بحران سنکرون</summary>
    Task<TabularResult> GetColorsAsync(int criticalPartsSetId);

    /// <summary>سالن‌های بحران سنکرون</summary>
    Task<TabularResult> GetSaloonsAsync(int criticalPartSetId);

    /// <summary>محصولات بحران سنکرون</summary>
    Task<TabularResult> GetProductsAsync(int criticalPartsSetId);

    /// <summary>جزئیات محصولات بحران سنکرون</summary>
    Task<TabularResult> GetProductDetailsAsync(int criticalPartsSetId);

    /// <summary>تعداد سالن‌های بدون خودروی شاخص</summary>
    Task<TabularResult> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId);

    /// <summary>ثبت سرآیند بحران سنکرون</summary>
    Task<int> CreateHeaderAsync(SyncSetsCreateHeaderInput input);

    /// <summary>ویرایش بحران سنکرون</summary>
    Task UpdateHeaderAsync(int id, SyncSetsUpdateHeaderInput input);

    /// <summary>ثبت یک محصول بحران سنکرون</summary>
    Task AddProductAsync(int criticalPartsSetId, SyncSetsAddProductInput input);

    /// <summary>تعیین خودروی شاخص سالن بحران سنکرون</summary>
    Task UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, SyncSetsUpdateSaloonIndicatorCarInput input);

    /// <summary>حذف منطقی بحران سنکرون</summary>
    Task DeleteAsync(int id);

}
