using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechExceptions.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions;

/// <summary>استثناهای محصولات بحران و مجوز آن‌ها</summary>
public partial interface IMechExceptionsAppService : IApplicationService
{
    /// <summary>فهرست بدنه‌های استثنا</summary>
    Task<ListResultDto<ProductExceptionRow>> GetExceptionListAsync(MechExceptionsGetExceptionListInput input);

    /// <summary>مجوزهای استثنا</summary>
    Task<ListResultDto<ExceptionLicenseRow>> GetLicensesAsync(int id);

    /// <summary>بدنه‌های مصرف‌کننده مجوز استثنا</summary>
    Task<ListResultDto<ExceptionLicenseBodyRow>> GetLicenseBodiesAsync(int id);

    /// <summary>تاریخچه مجوزهای استثنا برای شماره فنی</summary>
    Task<ListResultDto<ExceptionLicenseRow>> GetLicenseLogAsync(MechExceptionsGetLicenseLogInput input);

    /// <summary>ثبت مجوز برای استثنا</summary>
    Task CreateLicenseAsync(MechExceptionsCreateLicenseInput input);

    /// <summary>غیرفعال‌سازی مجوز استثنا</summary>
    Task DisableLicenseAsync(int id);

    /// <summary>غیرفعال‌سازی استثناها</summary>
    Task DisableExceptionsAsync(MechExceptionsDisableExceptionsInput input);

    /// <summary>گزارش شماره فنی‌های دارای استثنا</summary>
    Task<ListResultDto<ExceptionPartNumberRow>> GetPartNumbersInExceptionsAsync(MechExceptionsGetPartNumbersInExceptionsInput input);

}
