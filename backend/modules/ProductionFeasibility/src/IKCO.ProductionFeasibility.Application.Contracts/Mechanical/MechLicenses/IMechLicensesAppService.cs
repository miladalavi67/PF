using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

/// <summary>شرایط تولید و مجوزهای تولید با کسری</summary>
public partial interface IMechLicensesAppService : IApplicationService
{
    /// <summary>اطلاعات ردیابی مجوزهای یک بحران</summary>
    Task<TabularResult> GetTrackingInfoAsync(MechLicensesGetTrackingInfoInput input);

    /// <summary>تاریخچه مجوزهای یک بحران</summary>
    Task<TabularResult> GetTrackingListAsync(MechLicensesGetTrackingListInput input);

    /// <summary>گزارش پارامترهای تولید با کسری تعریف‌شده</summary>
    Task<TabularResult> SearchTrackingAsync(MechLicensesSearchTrackingInput input);

    /// <summary>بحران‌های دارای مجوز تولید با کسری</summary>
    Task<TabularResult> SearchTrackingCriticalsAsync(MechLicensesSearchTrackingCriticalsInput input);

    /// <summary>بدنه‌های مصرف‌کننده مجوز تولید</summary>
    Task<TabularResult> GetBodiesRelatedToUsedLicenseAsync(MechLicensesGetBodiesRelatedToUsedLicenseInput input);

}
