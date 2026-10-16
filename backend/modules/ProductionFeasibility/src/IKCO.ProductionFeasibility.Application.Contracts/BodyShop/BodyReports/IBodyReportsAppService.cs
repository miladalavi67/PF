using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports;

/// <summary>جستجو و گزارش‌های بحران بدنه‌سازی</summary>
public partial interface IBodyReportsAppService : IApplicationService
{
    /// <summary>جستجوی اقلام بحرانی بدنه‌سازی</summary>
    Task<TabularResult> SearchCriticalPartsAsync(BodyReportsSearchCriticalPartsInput input);

    /// <summary>محصولات یک بحران</summary>
    Task<TabularResult> GetCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>ردیابی تغییرات اقلام بحرانی</summary>
    Task<TabularResult> GetTrackingCriticalPartsAsync(BodyReportsGetTrackingCriticalPartsInput input);

    /// <summary>محصولات بحران در ردیابی</summary>
    Task<TabularResult> GetTrackingCriticalPartProductsAsync(int criticalPartsId);

    /// <summary>محصولات به تفکیک خطوط بدنه‌سازی</summary>
    Task<TabularResult> GetProductsOfSitesAsync();

    /// <summary>تعداد بدنه‌های پیش‌تخصیص‌شده</summary>
    Task<TabularResult> GetPreAssignBodyCountsAsync(BodyReportsGetPreAssignBodyCountsInput input);

    /// <summary>بدنه‌های پیش‌تخصیص بحرانی</summary>
    Task<TabularResult> GetPreAssignCriticalBodiesAsync(BodyReportsGetPreAssignCriticalBodiesInput input);

    /// <summary>مغایرت پیش‌تخصیص بدنه‌های بحرانی</summary>
    Task<TabularResult> GetNotPossibleProductsToStepAssignAsync(BodyReportsGetNotPossibleProductsToStepAssignInput input);

    /// <summary>ردیابی پارامترهای بحرانی (مجوز تولید با کسری)</summary>
    Task<TabularResult> GetShortageLicenseTrackingAsync(BodyReportsGetShortageLicenseTrackingInput input);

    /// <summary>وضعیت مجوز تولید یک بحران</summary>
    Task<TabularResult> GetProductionPossibilityStatusAsync(int id);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    Task<TabularResult> GetFailedSapTransfersAsync(BodyReportsGetFailedSapTransfersInput input);

    /// <summary>زمان آخرین به‌روزرسانی داده‌های SAP</summary>
    Task<TabularResult> GetSapDataUpdateTimesAsync();

    /// <summary>شناسه آخرین تغییر مجوزها (برای به‌روزرسانی خودکار گزارش)</summary>
    Task<string?> GetLastChangeOnLicenseAsync();

}
