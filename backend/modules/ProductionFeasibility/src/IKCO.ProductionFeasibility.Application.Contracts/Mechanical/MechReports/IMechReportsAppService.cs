using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports;

/// <summary>گزارش‌های امکان تولید مکانیکی</summary>
public partial interface IMechReportsAppService : IApplicationService
{
    /// <summary>گزارش امکان تولید مکانیکی</summary>
    Task<TabularResult> GetProductionPossibilityAsync(MechReportsGetProductionPossibilityInput input);

    /// <summary>گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده</summary>
    Task<TabularResult> GetProductionPossibilityWithLicenseAsync(MechReportsGetProductionPossibilityWithLicenseInput input);

    /// <summary>امکان تولید به تفکیک سالن مونتاژ</summary>
    Task<TabularResult> GetProductionPossibilityByTrimShopAsync(MechReportsGetProductionPossibilityByTrimShopInput input);

    /// <summary>گزارش جامع اقلام بحرانی (مکانیکی و سنکرون)</summary>
    Task<TabularResult> GetGeneralReportAsync(MechReportsGetGeneralReportInput input);

    /// <summary>بدنه‌های در راه / وارد شده به خط برای یک بحران</summary>
    Task<TabularResult> GetIncomingBodiesByCriticalPartAsync(MechReportsGetIncomingBodiesByCriticalPartInput input);

    /// <summary>فهرست بدنه‌های وارد شده برای یک بحران</summary>
    Task<TabularResult> GetIncomingBodyListAsync(MechReportsGetIncomingBodyListInput input);

    /// <summary>خلاصه بدنه‌های وارد شده به خط</summary>
    Task<TabularResult> GetIncomingBodiesSummaryAsync(MechReportsGetIncomingBodiesSummaryInput input);

    /// <summary>تاریخچه موجودی شماره فنی</summary>
    Task<TabularResult> GetStockLogAsync(MechReportsGetStockLogInput input);

    /// <summary>لاگ انتقال اقلام بحرانی از SAP</summary>
    Task<TabularResult> GetSapCriticalLogAsync(MechReportsGetSapCriticalLogInput input);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    Task<TabularResult> GetFailedSapTransfersAsync(MechReportsGetFailedSapTransfersInput input);

    /// <summary>گزارش ASN (اعلان ورود محموله)</summary>
    Task<TabularResult> GetAsnReportAsync(MechReportsGetAsnReportInput input);

    /// <summary>دلایل عدم امکان ورود خودرو به مونتاژ</summary>
    Task<TabularResult> GetNotPossibleProductsToTrimStepAsync(MechReportsGetNotPossibleProductsToTrimStepInput input);

    /// <summary>تعداد بدنه‌های ورودی به مونتاژ پس از ثبت</summary>
    Task<TabularResult> GetTrimShopBodyCountAsync(MechReportsGetTrimShopBodyCountInput input);

    /// <summary>بدنه‌های بحرانی ورودی به مونتاژ</summary>
    Task<TabularResult> GetTrimShopCriticalBodiesAsync(MechReportsGetTrimShopCriticalBodiesInput input);

}
