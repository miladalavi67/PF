using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial interface ISyncSetsAppService
{
    /// <summary>ثبت قطعات، رنگ‌ها، سالن‌ها و محصولات یک بحران سنکرون</summary>
    Task<GlobalResultDto> SaveCompositionAsync(int criticalPartsSetId, SyncSetsSaveCompositionInput input);
}
