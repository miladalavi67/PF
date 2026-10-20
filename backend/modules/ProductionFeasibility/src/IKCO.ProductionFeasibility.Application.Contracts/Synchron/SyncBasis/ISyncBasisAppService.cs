using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis;

/// <summary>تعریف مبنای ست‌های سنکرون (frmCriticalPartSetBasis)</summary>
public partial interface ISyncBasisAppService : IApplicationService
{
    /// <summary>فهرست مبناهای ست سنکرون</summary>
    Task<ListResultDto<SetBasisReportRow>> GetListAsync();

    /// <summary>مبنای ست بر اساس کد</summary>
    Task<ListResultDto<SetBasisRow>> GetByCodeAsync(SyncBasisGetByCodeInput input);

    /// <summary>مبنای ست بر اساس شناسه</summary>
    Task<ListResultDto<SetBasisRow>> GetByIdAsync(string id);

    /// <summary>مبناهای ست بر اساس وضعیت</summary>
    Task<ListResultDto<SetBasisRow>> GetByStatusAsync(SyncBasisGetByStatusInput input);

    /// <summary>تعداد ست‌های ثبت‌شده با کد ست</summary>
    Task<int> CountBySetCodeAsync(SyncBasisCountBySetCodeInput input);

    /// <summary>ثبت ویژگی نوع قطعه سنکرون</summary>
    Task AddPartTypePropertyAsync(SyncBasisAddPartTypePropertyInput input);

    /// <summary>ثبت مبنای ست سنکرون</summary>
    Task CreateAsync(SyncBasisCreateInput input);

    /// <summary>ویرایش مبنای ست سنکرون</summary>
    Task UpdateAsync(int id, SyncBasisUpdateInput input);

}
