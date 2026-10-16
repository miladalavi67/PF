using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis.Repositories;

/// <summary>تعریف مبنای ست‌های سنکرون (frmCriticalPartSetBasis) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface ISyncBasisRepository
{
    /// <summary>فهرست مبناهای ست سنکرون (frmCriticalPartSetBasis) — <c>dbo.S_RptGetCriticalPartsSet</c></summary>
    Task<TabularResult> GetListAsync();

    /// <summary>مبنای ست بر اساس کد (frmCriticalPartSetBasis) — <c>dbo.S_CriticalPartsSetBasisSelectByCode</c></summary>
    Task<TabularResult> GetByCodeAsync(string code);

    /// <summary>مبنای ست بر اساس شناسه (frmCriticalPartSetBasis) — <c>dbo.S_CriticalPartsSetBasisSelectById</c></summary>
    Task<TabularResult> GetByIdAsync(string id);

    /// <summary>مبناهای ست بر اساس وضعیت (frmCriticalPartSetBasis) — <c>dbo.S_CriticalPartsSetBasisSelectByStatus</c></summary>
    Task<TabularResult> GetByStatusAsync(byte? status);

    /// <summary>تعداد ست‌های ثبت‌شده با کد ست (frmCriticalPartSet) — <c>dbo.S_CriticalPartsSetCountSetCode</c></summary>
    Task<TabularResult> CountBySetCodeAsync(string setCode);

    /// <summary>ثبت مبنای ست سنکرون (frmCriticalPartSetBasis.btnSave) — <c>dbo.S_InsertCriticalPartSet</c></summary>
    Task CreateAsync(string code, string descF, int carClass, int partTypeId, int levelType, int userId, bool isTrim, bool isOption, bool isToodoozi, bool isColor);

    /// <summary>ویرایش مبنای ست سنکرون (frmCriticalPartSetBasis.btnSave (ویرایش)) — <c>dbo.S_UpdateCriticalPartsSet</c></summary>
    Task UpdateAsync(int id, string code, string descF, int carClassId, int partTypeId, int levelType, bool status, bool isTrim, bool isColor, bool isOption, bool isToodoozi);

}
