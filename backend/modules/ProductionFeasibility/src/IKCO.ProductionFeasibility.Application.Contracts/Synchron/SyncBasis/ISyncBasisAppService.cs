using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis;

/// <summary>تعریف مبنای ست‌های سنکرون (frmCriticalPartSetBasis)</summary>
public partial interface ISyncBasisAppService : IApplicationService
{
    /// <summary>فهرست مبناهای ست سنکرون</summary>
    Task<TabularResult> GetListAsync();

    /// <summary>مبنای ست بر اساس کد</summary>
    Task<TabularResult> GetByCodeAsync(SyncBasisGetByCodeInput input);

    /// <summary>مبنای ست بر اساس شناسه</summary>
    Task<TabularResult> GetByIdAsync(string id);

    /// <summary>مبناهای ست بر اساس وضعیت</summary>
    Task<TabularResult> GetByStatusAsync(SyncBasisGetByStatusInput input);

    /// <summary>تعداد ست‌های ثبت‌شده با کد ست</summary>
    Task<TabularResult> CountBySetCodeAsync(SyncBasisCountBySetCodeInput input);

    /// <summary>ثبت مبنای ست سنکرون</summary>
    Task CreateAsync(SyncBasisCreateInput input);

    /// <summary>ویرایش مبنای ست سنکرون</summary>
    Task UpdateAsync(int id, SyncBasisUpdateInput input);

}
