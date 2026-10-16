using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Lookups.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Lookups;

/// <summary>فهرست‌های مرجع فرم‌ها (جایگزین ComboBox/DropDown های پر شده از EF و رویه‌ها در سامانه ویندوزی)</summary>
public interface ILookupAppService : IApplicationService
{
    /// <summary>فهرست کلیدهای قابل درخواست</summary>
    Task<ListResultDto<LookupDescriptor>> GetCatalogAsync();

    /// <summary>ردیف‌های یک فهرست مرجع</summary>
    Task<ListResultDto<LookupRow>> GetAsync(string key, GetLookupInput input);
}
