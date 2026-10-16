using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports.Repositories;

/// <summary>گزارش‌های امکان تولید سنکرون — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface ISyncReportsRepository
{
    /// <summary>گزارش امکان تولید سنکرون (frmRPTProductionPossibility (سنکرون)) — <c>dbo.S_RptCriticalPartsProduction_Synchron</c></summary>
    Task<TabularResult> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId);

}
