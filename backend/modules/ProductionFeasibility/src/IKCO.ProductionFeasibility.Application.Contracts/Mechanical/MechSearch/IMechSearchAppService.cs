using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechSearch.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch;

/// <summary>جستجوی بحران‌ها (frmCriticalPartSearch / جستجوی سنکرون و مکانیکی)</summary>
public partial interface IMechSearchAppService : IApplicationService
{
    /// <summary>جستجوی اقلام بحرانی (مکانیکی و سنکرون)</summary>
    Task<ListResultDto<CriticalPartSearchRow>> SearchCriticalPartsAsync(MechSearchSearchCriticalPartsInput input);

    /// <summary>جستجوی بحران‌ها بر اساس سالن و بازه تاریخ</summary>
    Task<ListResultDto<TrimShopCriticalPartRow>> SearchByTrimShopAsync(MechSearchSearchByTrimShopInput input);

    /// <summary>اقلام بحرانی فعال وارد شده به شرکت</summary>
    Task<ListResultDto<ReceivedPartRow>> GetReceivedPartsAsync(MechSearchGetReceivedPartsInput input);

}
