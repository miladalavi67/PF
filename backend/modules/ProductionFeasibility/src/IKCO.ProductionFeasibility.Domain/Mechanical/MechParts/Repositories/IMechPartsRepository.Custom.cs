using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;

/// <summary>یک محصول بحران مکانیکی برای رویه <c>S_CriticalPartsPruductUpdate_Web</c></summary>
public sealed record MechPartProduct(int CarGroupId, int CarModelId, int CarClassId, int SubClassId, int ColorId, int TrimId, int OptionId, byte Quantity);

public partial interface IMechPartsRepository
{
    /// <summary>
    /// برای هر محصول <c>S_CriticalPartsPruductUpdate_Web</c> را فراخوانی می‌کند (در تراکنش جاری) و نتیجه هر فراخوانی را برمی‌گرداند؛
    /// رویه برای محصول تکراری ردیف نتیجه برنمی‌گرداند (null).
    /// </summary>
    Task<List<GlobalResultDto?>> SaveProductsAsync(int criticalPartsId, int carId, int trimShopId, short plantId, int isNew, IReadOnlyList<MechPartProduct> products, int userId);

    /// <summary><c>S_GetContrastOfSubClasses</c> با پارامتر جدولی <c>@Tbl</c></summary>
    Task<List<SubClassContrastRow>> CompareSubClassesAsync(IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, int saloonId, int type);
}
