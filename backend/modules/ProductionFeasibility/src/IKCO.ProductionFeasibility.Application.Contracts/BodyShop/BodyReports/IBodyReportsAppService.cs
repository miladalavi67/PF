using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports;

/// <summary>جستجو و گزارش‌های بحران بدنه‌سازی</summary>
public partial interface IBodyReportsAppService : IApplicationService
{
    /// <summary>جستجوی اقلام بحرانی بدنه‌سازی</summary>
    Task<ListResultDto<BodyCriticalSearchRow>> SearchCriticalPartsAsync(BodyReportsSearchCriticalPartsInput input);

    /// <summary>محصولات یک بحران</summary>
    Task<ListResultDto<BodyCriticalProductRow>> GetCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>ردیابی تغییرات اقلام بحرانی</summary>
    Task<ListResultDto<BodyTrackingCriticalPartRow>> GetTrackingCriticalPartsAsync(BodyReportsGetTrackingCriticalPartsInput input);

    /// <summary>محصولات بحران در ردیابی</summary>
    Task<ListResultDto<BodyTrackingProductRow>> GetTrackingCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>محصولات به تفکیک خطوط بدنه‌سازی</summary>
    Task<ListResultDto<BodyProductsOfSiteRow>> GetProductsOfSitesAsync();

    /// <summary>تعداد بدنه‌های پیش‌تخصیص‌شده</summary>
    Task<ListResultDto<BodyPreAssignCountRow>> GetPreAssignBodyCountsAsync(BodyReportsGetPreAssignBodyCountsInput input);

    /// <summary>بدنه‌های پیش‌تخصیص بحرانی</summary>
    Task<ListResultDto<BodyPreAssignCriticalRow>> GetPreAssignCriticalBodiesAsync(BodyReportsGetPreAssignCriticalBodiesInput input);

    /// <summary>مغایرت پیش‌تخصیص بدنه‌های بحرانی</summary>
    Task<ListResultDto<BodyNotPossibleToAssignRow>> GetNotPossibleProductsToStepAssignAsync(BodyReportsGetNotPossibleProductsToStepAssignInput input);

    /// <summary>ردیابی پارامترهای بحرانی (مجوز تولید با کسری)</summary>
    Task<ListResultDto<BodyLicenseTrackingRow>> GetShortageLicenseTrackingAsync(BodyReportsGetShortageLicenseTrackingInput input);

    /// <summary>وضعیت مجوز تولید یک بحران</summary>
    Task<ListResultDto<BodyProductionPossibilityStatusRow>> GetProductionPossibilityStatusAsync(int id);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    Task<ListResultDto<FailedSapTransferRow>> GetFailedSapTransfersAsync(BodyReportsGetFailedSapTransfersInput input);

    /// <summary>زمان آخرین به‌روزرسانی داده‌های SAP</summary>
    Task<ListResultDto<SapDataUpdateTimesRow>> GetSapDataUpdateTimesAsync();

    /// <summary>شناسه آخرین تغییر مجوزها (برای به‌روزرسانی خودکار گزارش)</summary>
    Task<string?> GetLastChangeOnLicenseAsync();

}
