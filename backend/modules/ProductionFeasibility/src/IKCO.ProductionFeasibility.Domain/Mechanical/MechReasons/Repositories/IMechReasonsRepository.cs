using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons.Repositories;

/// <summary>دلایل عدم امکان برنامه‌شدن / ورود (frmNotPossibleProductReason) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechReasonsRepository
{
    /// <summary>فهرست دلایل عدم امکان تولید (frmNotPossibleProductReason) — <c>dbo.S_ProgNotPossibilityReasonSelectAll</c></summary>
    Task<List<NotPossibleReasonRow>> GetListAsync(byte? selectType);

    /// <summary>ثبت دلیل عدم امکان تولید (frmNotPossibleProductReason) — <c>dbo.S_ProgNotPossibilityReason_Insert</c></summary>
    Task CreateAsync(string code, string descF, string descE, byte status, int userId);

    /// <summary>ویرایش دلیل عدم امکان تولید (frmNotPossibleProductReason) — <c>dbo.S_ProgNotPossibilityReason_Update</c></summary>
    Task UpdateAsync(int id, string code, string descF, string descE, byte status);

    /// <summary>حذف منطقی دلیل عدم امکان تولید (frmNotPossibleProductReason) — <c>dbo.S_ProgNotPossibilityReason_LogicalDelete</c></summary>
    Task DeleteAsync(int id);

}
