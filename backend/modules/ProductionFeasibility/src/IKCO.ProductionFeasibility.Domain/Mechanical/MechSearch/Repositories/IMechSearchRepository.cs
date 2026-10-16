using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch.Repositories;

/// <summary>جستجوی بحران‌ها (frmCriticalPartSearch / جستجوی سنکرون و مکانیکی) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechSearchRepository
{
    /// <summary>جستجوی اقلام بحرانی (مکانیکی و سنکرون) (frmCriticalPartSearch) — <c>dbo.S_CriticalPartsSearchByConditions</c></summary>
    Task<TabularResult> SearchCriticalPartsAsync(string? partNo, string? setCode, string? fromDate, string? toDate, string? trimShopIds, int? status, int? criticalPartTypeId);

    /// <summary>جستجوی بحران‌ها بر اساس سالن و بازه تاریخ (frmCriticalPartSearch (نسخه قدیم)) — <c>dbo.S_CriticalPartSearch</c></summary>
    Task<TabularResult> SearchByTrimShopAsync(string? partNo, string? fromDate, string? toDate, int trimShopId, int type);

    /// <summary>اقلام بحرانی فعال وارد شده به شرکت (frmCriticalPartsArrived) — <c>dbo.S_CriticalPart_RecievedParts</c></summary>
    Task<TabularResult> GetReceivedPartsAsync(string fromDate, string toDate);

}
