using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial interface IMechPartsAppService
{
    /// <summary>ثبت چند محصول برای یک بحران مکانیکی در یک تراکنش</summary>
    Task<GlobalResultDto> SaveProductsAsync(int criticalPartsId, MechPartsSaveProductsInput input);

    /// <summary>مقایسه زیرکلاس‌های SAP با زیرکلاس‌های ثبت‌شده در SFC</summary>
    Task<TabularResult> CompareSubClassesAsync(MechPartsCompareSubClassesInput input);
}
