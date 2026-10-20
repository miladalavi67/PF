using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports;

/// <summary>گزارش‌های امکان تولید مکانیکی</summary>
public partial interface IMechReportsAppService : IApplicationService
{
    /// <summary>گزارش امکان تولید مکانیکی</summary>
    Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityAsync(MechReportsGetProductionPossibilityInput input);

    /// <summary>گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده</summary>
    Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityWithLicenseAsync(MechReportsGetProductionPossibilityWithLicenseInput input);

    /// <summary>امکان تولید به تفکیک سالن مونتاژ</summary>
    Task<ListResultDto<ProdPossibilityByTrimShopRow>> GetProductionPossibilityByTrimShopAsync(MechReportsGetProductionPossibilityByTrimShopInput input);

    /// <summary>گزارش جامع اقلام بحرانی (مکانیکی و سنکرون)</summary>
    Task<ListResultDto<GeneralReportRow>> GetGeneralReportAsync(MechReportsGetGeneralReportInput input);

    /// <summary>بدنه‌های در راه / وارد شده به خط برای یک بحران</summary>
    Task<ListResultDto<IncomingBodiesCountRow>> GetIncomingBodiesByCriticalPartAsync(MechReportsGetIncomingBodiesByCriticalPartInput input);

    /// <summary>فهرست بدنه‌های وارد شده برای یک بحران</summary>
    Task<ListResultDto<IncomingBodyRow>> GetIncomingBodyListAsync(MechReportsGetIncomingBodyListInput input);

    /// <summary>خلاصه بدنه‌های وارد شده به خط</summary>
    Task<ListResultDto<IncomingBodiesSummaryRow>> GetIncomingBodiesSummaryAsync(MechReportsGetIncomingBodiesSummaryInput input);

    /// <summary>تاریخچه موجودی شماره فنی</summary>
    Task<ListResultDto<StockLogRow>> GetStockLogAsync(MechReportsGetStockLogInput input);

    /// <summary>لاگ انتقال اقلام بحرانی از SAP</summary>
    Task<ListResultDto<SapCriticalLogRow>> GetSapCriticalLogAsync(MechReportsGetSapCriticalLogInput input);

    /// <summary>قطعات ناموفق در انتقال از SAP</summary>
    Task<ListResultDto<FailedSapTransferRow>> GetFailedSapTransfersAsync(MechReportsGetFailedSapTransfersInput input);

    /// <summary>گزارش ASN (اعلان ورود محموله)</summary>
    Task<ListResultDto<AsnRow>> GetAsnReportAsync(MechReportsGetAsnReportInput input);

    /// <summary>دلایل عدم امکان ورود خودرو به مونتاژ</summary>
    Task<ListResultDto<TrimInNotPossibleRow>> GetNotPossibleProductsToTrimStepAsync(MechReportsGetNotPossibleProductsToTrimStepInput input);

    /// <summary>تعداد بدنه‌های ورودی به مونتاژ پس از ثبت</summary>
    Task<ListResultDto<TrimShopBodyCountRow>> GetTrimShopBodyCountAsync(MechReportsGetTrimShopBodyCountInput input);

    /// <summary>بدنه‌های بحرانی ورودی به مونتاژ</summary>
    Task<ListResultDto<TrimShopCriticalBodyRow>> GetTrimShopCriticalBodiesAsync(MechReportsGetTrimShopCriticalBodiesInput input);

}
