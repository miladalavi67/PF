using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions.Repositories;

/// <summary>استثناهای محصولات بحران و مجوز آن‌ها — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechExceptionsRepository
{
    /// <summary>فهرست بدنه‌های استثنا (frmCriticalPartsProductExceptions) — <c>dbo.S_CriticalProductExceptionList</c></summary>
    Task<List<ProductExceptionRow>> GetExceptionListAsync(string? bodyNo, string? dateFrom, string? dateEnd, int state);

    /// <summary>مجوزهای استثنا (frmCriticalPartsProductExceptionsLicense) — <c>dbo.S_CriticalPartsProductExceptionsLisenceList</c></summary>
    Task<List<ExceptionLicenseRow>> GetLicensesAsync(int id);

    /// <summary>بدنه‌های مصرف‌کننده مجوز استثنا (frmCriticalPartsProductExceptionsLicense) — <c>dbo.S_CriticalPartsProductExceptionsLicenseBodyNoList</c></summary>
    Task<List<ExceptionLicenseBodyRow>> GetLicenseBodiesAsync(int id);

    /// <summary>تاریخچه مجوزهای استثنا برای شماره فنی (frmLicenseLogs) — <c>dbo.S_CriticalPartsProductExceptionsLisenceLog</c></summary>
    Task<List<ExceptionLicenseRow>> GetLicenseLogAsync(string partNo);

    /// <summary>ثبت مجوز برای استثنا (frmCriticalPartsProductExceptionsLicense) — <c>dbo.S_InsertCriticalPartsProductExceptionsLicense</c></summary>
    Task CreateLicenseAsync(int criticalPartsProductExceptionsId, int possibleLicenseCount, string letterNo, string? description, int userId);

    /// <summary>غیرفعال‌سازی مجوز استثنا (frmDisableCriticalPartsProductExceptions) — <c>dbo.S_DisableCriticalPartsProductExceptionsLicense</c></summary>
    Task DisableLicenseAsync(int userId, int id);

    /// <summary>غیرفعال‌سازی استثناها (frmDisableCriticalPartsProductExceptions) — <c>dbo.S_DisableCriticalPartsProductsException</c></summary>
    Task DisableExceptionsAsync(string values, int userId);

    /// <summary>گزارش شماره فنی‌های دارای استثنا (frmRPT_CriticalPartsProductExceptions) — <c>dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException</c></summary>
    Task<List<ExceptionPartNumberRow>> GetPartNumbersInExceptionsAsync(string filterValues, byte trimId, byte filterType, string? partNumber);

}
