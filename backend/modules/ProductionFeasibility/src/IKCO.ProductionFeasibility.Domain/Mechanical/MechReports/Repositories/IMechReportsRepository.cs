using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports.Repositories;

/// <summary>گزارش‌های امکان تولید مکانیکی — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechReportsRepository
{
    /// <summary>گزارش امکان تولید مکانیکی (frmRPTProductionPossibility / frmRPTProdpossibility) — <c>dbo.S_RptCriticalPartsProduction_Mechanical</c></summary>
    Task<List<ProductionPossibilityRow>> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId);

    /// <summary>گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده (frmRPTProductionPossibility (با مجوز)) — <c>dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5</c></summary>
    Task<List<ProductionPossibilityRow>> GetProductionPossibilityWithLicenseAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, int carModelId, int carClassId);

    /// <summary>امکان تولید به تفکیک سالن مونتاژ (frmRPTProdpossibility) — <c>dbo.S_RPT_DESC_ProdPossibilityByTrimShop</c></summary>
    Task<List<ProdPossibilityByTrimShopRow>> GetProductionPossibilityByTrimShopAsync(int trimShopId, byte prodState);

    /// <summary>گزارش جامع اقلام بحرانی (مکانیکی و سنکرون) (frmRPTProdpossibility (جامع)) — <c>dbo.S_CriticalPartsGeneralReport_Web</c></summary>
    Task<List<GeneralReportRow>> GetGeneralReportAsync(byte? criticalPartTypeId, int saloonId, byte? crisisTypeId, byte? plantId);

    /// <summary>بدنه‌های در راه / وارد شده به خط برای یک بحران (frm_RPT_Des_IncommingBodiesByCPId) — <c>dbo.S_RPT_IncommingBodiesByCriticalPartId</c></summary>
    Task<List<IncomingBodiesCountRow>> GetIncomingBodiesByCriticalPartAsync(int id, int criticalPartTypeId, int saloonId, byte plantId);

    /// <summary>فهرست بدنه‌های وارد شده برای یک بحران (frm_RPT_ST_IncommingBodiesByCP) — <c>dbo.S_RPT_IncommingBodiesListByCriticalPartId</c></summary>
    Task<List<IncomingBodyRow>> GetIncomingBodyListAsync(int id, int criticalPartTypeId, int saloonId, int productionStepGroupId, byte? plantId);

    /// <summary>خلاصه بدنه‌های وارد شده به خط (frm_RPT_ST_IncommingBodiesByCP) — <c>dbo.S_RPT_IncommingBodiesByCriticalParts</c></summary>
    Task<List<IncomingBodiesSummaryRow>> GetIncomingBodiesSummaryAsync(int saloonId, byte crisisTypeId, int? productionStepGroupId, byte? plantId);

    /// <summary>تاریخچه موجودی شماره فنی (frm_Rpt_CriticalPartsStockLog) — <c>dbo.SEXT_CriticalPartsStockLog</c></summary>
    Task<List<StockLogRow>> GetStockLogAsync(string partNo);

    /// <summary>لاگ انتقال اقلام بحرانی از SAP (frm_Rpt_SapCriticalLog) — <c>dbo.S_SapCriticalLog</c></summary>
    Task<List<SapCriticalLogRow>> GetSapCriticalLogAsync(string partNo);

    /// <summary>قطعات ناموفق در انتقال از SAP (frm_Rpt_FailedTransfer_Sap) — <c>dbo.S_FailedPartTransferd_Sap_Log</c></summary>
    Task<List<FailedSapTransferRow>> GetFailedSapTransfersAsync(string fromDate, string toDate);

    /// <summary>گزارش ASN (اعلان ورود محموله) (frm_Rpt_Sap_Asn / frmIncommingOrdersASNDetail) — <c>dbo.S_ASNReport</c></summary>
    Task<List<AsnRow>> GetAsnReportAsync(string? partNumber, string? fromDate, string? toDate, byte type);

    /// <summary>دلایل عدم امکان ورود خودرو به مونتاژ (frmRPTNotPossibleProductsToStepTrimShop) — <c>dbo.S_TrimInNotPossibleProductsSelectAll</c></summary>
    Task<List<TrimInNotPossibleRow>> GetNotPossibleProductsToTrimStepAsync(int carId);

    /// <summary>تعداد بدنه‌های ورودی به مونتاژ پس از ثبت (frmTrimShopBodyCount) — <c>dbo.S_TrimShopBodyCount_List</c></summary>
    Task<List<TrimShopBodyCountRow>> GetTrimShopBodyCountAsync(int carId);

    /// <summary>بدنه‌های بحرانی ورودی به مونتاژ (frmRPTProductsToStepTrimShop) — <c>dbo.S_TrimShopCriticalBody_SelectAll</c></summary>
    Task<List<TrimShopCriticalBodyRow>> GetTrimShopCriticalBodiesAsync(string? fromDate, string? toDate, string? partNo, int criticalPartTypeId, string? setCode);

}
