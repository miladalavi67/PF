using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Synchron.Results;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports.Repositories;

/// <summary>گزارش‌های امکان تولید سنکرون — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface ISyncReportsRepository
{
    /// <summary>گزارش امکان تولید سنکرون (frmRPTProductionPossibility (سنکرون)) — <c>dbo.S_RptCriticalPartsProduction_Synchron</c></summary>
    Task<List<ProductionPossibilityRow>> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId);

    /// <summary>اقلام بحرانی دریافتی از SAP (SyncPartsReports/Rpt_SyncCriticalPartsSetBasis (بک‌اند قبلی)) — <c>dbo.S_Web_SAP_CriticalPartsMaster_SelectAll</c></summary>
    Task<List<SapCriticalPartsMasterRow>> GetSapCriticalPartsMasterAsync(string fromDate, string toDate, int saloonId, string? bodyNO, string? partNO);

    /// <summary>رزروهای دریافتی از SAP (SyncPartsReports/RptSyncParts_SAP_ReserveMaster (بک‌اند قبلی)) — <c>dbo.S_Web_SAP_ReserveMaster_SelectAll</c></summary>
    Task<List<SapReserveMasterRow>> GetSapReserveMasterAsync(string fromDate, string toDate, int saloonId, string? partNO);

    /// <summary>گزارش ASN (اعلان ورود محموله) (SyncPartsReports/Rpt_SyncPartsASN (بک‌اند قبلی)) — <c>dbo.S_ASNReport</c></summary>
    Task<List<AsnRow>> GetAsnReportAsync(string? partNumber, string? fromDate, string? toDate, byte type);

}
