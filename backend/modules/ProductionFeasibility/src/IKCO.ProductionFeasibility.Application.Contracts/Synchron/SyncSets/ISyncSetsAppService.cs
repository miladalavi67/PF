using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet)</summary>
public partial interface ISyncSetsAppService : IApplicationService
{
    /// <summary>اطلاعات بحران سنکرون</summary>
    Task<ListResultDto<CriticalPartSetRow>> GetByIdAsync(int id);

    /// <summary>قطعات بحران سنکرون</summary>
    Task<ListResultDto<SetPartRow>> GetPartsAsync(int criticalPartsSetId);

    /// <summary>قطعات بحران سنکرون به همراه فهرست مرجع کسری</summary>
    Task<ListResultDto<SetPartWithShortageRow>> GetPartsWithShortageListAsync(int criticalPartsSetId);

    /// <summary>رنگ‌های بحران سنکرون</summary>
    Task<ListResultDto<SetColorRow>> GetColorsAsync(int criticalPartsSetId);

    /// <summary>سالن‌های بحران سنکرون</summary>
    Task<ListResultDto<SetSaloonRow>> GetSaloonsAsync(int criticalPartSetId);

    /// <summary>محصولات بحران سنکرون</summary>
    Task<ListResultDto<SetProductRow>> GetProductsAsync(int criticalPartsSetId);

    /// <summary>جزئیات محصولات بحران سنکرون</summary>
    Task<ListResultDto<SetProductDetailRow>> GetProductDetailsAsync(int criticalPartsSetId);

    /// <summary>تعداد سالن‌های بدون خودروی شاخص</summary>
    Task<int> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId);

    /// <summary>سالن‌های ثبت‌شده بحران سنکرون</summary>
    Task<ListResultDto<SetSaloonItemRow>> GetSetSaloonsAsync(int criticalPartsSetId);

    /// <summary>اطلاعات شماره فنی از BOM (کلاس، زیرکلاس و نوع قطعه)</summary>
    Task<ListResultDto<BomPartInfoRow>> GetPartInfoAsync(SyncSetsGetPartInfoInput input);

    /// <summary>رنگ‌های هم‌کد با کد ۵ رقمی</summary>
    Task<ListResultDto<SimilarColorRow>> GetSimilarColorsAsync(SyncSetsGetSimilarColorsInput input);

    /// <summary>سالن‌های مونتاژ مدل‌های انتخاب‌شده</summary>
    Task<ListResultDto<SaloonByCarModelRow>> GetSaloonsByCarModelsAsync(SyncSetsGetSaloonsByCarModelsInput input);

    /// <summary>ثبت سرآیند بحران سنکرون</summary>
    Task<GlobalResultDto> CreateHeaderAsync(SyncSetsCreateHeaderInput input);

    /// <summary>ویرایش بحران سنکرون</summary>
    Task<GlobalResultDto> UpdateHeaderAsync(int id, SyncSetsUpdateHeaderInput input);

    /// <summary>ثبت یک محصول بحران سنکرون</summary>
    Task<GlobalResultDto> AddProductAsync(int criticalPartsSetId, SyncSetsAddProductInput input);

    /// <summary>تعیین خودروی شاخص سالن بحران سنکرون</summary>
    Task<GlobalResultDto> UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, SyncSetsUpdateSaloonIndicatorCarInput input);

    /// <summary>حذف منطقی بحران سنکرون</summary>
    Task DeleteAsync(int id);

}
