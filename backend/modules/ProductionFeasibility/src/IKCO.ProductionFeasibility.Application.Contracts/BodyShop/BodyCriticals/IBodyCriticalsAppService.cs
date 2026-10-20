using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

/// <summary>بحران‌های بدنه‌سازی (ثبت، ویرایش، غیرفعال‌سازی)</summary>
public partial interface IBodyCriticalsAppService : IApplicationService
{
    /// <summary>اطلاعات بدنه</summary>
    Task<BodyInfoRow?> GetBodyInfoAsync(BodyCriticalsGetBodyInfoInput input);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی</summary>
    Task<string?> GetPartDescriptionAsync(BodyCriticalsGetPartDescriptionInput input);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی در یک خط بدنه‌سازی</summary>
    Task<ListResultDto<PartProductRow>> GetPartProductsAsync(BodyCriticalsGetPartProductsInput input);

    /// <summary>کدهای کلاس مصرف‌کننده شماره فنی</summary>
    Task<ListResultDto<CodeClassRow>> GetCodeClassByPartNoAsync(BodyCriticalsGetCodeClassByPartNoInput input);

    /// <summary>ثبت بحران بدنه‌سازی</summary>
    Task<GlobalResultDto> CreateCriticalPartAsync(BodyCriticalsCreateCriticalPartInput input);

    /// <summary>ویرایش بحران بدنه‌سازی</summary>
    Task<GlobalResultDto> UpdateCriticalPartAsync(int criticalPartsId, BodyCriticalsUpdateCriticalPartInput input);

    /// <summary>اطلاعات یک بحران بدنه‌سازی برای ویرایش</summary>
    Task<ListResultDto<BodyCriticalPartInfoRow>> GetCriticalPartInfoAsync(int criticalPartsId);

    /// <summary>گزارش رویدادهای یک بحران</summary>
    Task<ListResultDto<EventLogRow>> GetEventLogAsync(BodyCriticalsGetEventLogInput input);

}
