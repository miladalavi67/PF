using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechReasons.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons;

/// <summary>دلایل عدم امکان برنامه‌شدن / ورود (frmNotPossibleProductReason)</summary>
public partial interface IMechReasonsAppService : IApplicationService
{
    /// <summary>فهرست دلایل عدم امکان تولید</summary>
    Task<TabularResult> GetListAsync(MechReasonsGetListInput input);

    /// <summary>ثبت دلیل عدم امکان تولید</summary>
    Task CreateAsync(MechReasonsCreateInput input);

    /// <summary>ویرایش دلیل عدم امکان تولید</summary>
    Task UpdateAsync(int id, MechReasonsUpdateInput input);

    /// <summary>حذف منطقی دلیل عدم امکان تولید</summary>
    Task DeleteAsync(int id);

}
