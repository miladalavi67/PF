using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Repositories;

public partial interface IBodyCriticalsRepository
{
    /// <summary>برای هر کلاس <c>PF.S_CreateCritocalPartsProduct</c> را فراخوانی می‌کند؛ تعداد کلاس‌های ثبت‌شده را برمی‌گرداند</summary>
    Task<int> CreateProductsAsync(int criticalPartsId, int carId, IReadOnlyList<(int CarClassId, int UsageFactor)> items, int userId);

    /// <summary>
    /// <c>PF.RegisterShortageLicense_V1</c>؛ سازنده اصلی بحران (کاربر اولین ردیف ردیابی مجوز) به‌صورت <c>@CreatorUser</c>
    /// از پایگاه داده خوانده می‌شود، نه از کلاینت.
    /// </summary>
    Task<GlobalResultDto?> RegisterLicenseAsync(int criticalPartId, int criticalPartTypeId, int productionPossibilityId, int? possibleProductionNo,
        string? toDate, string? toTime, string? note, int userId);

    /// <summary><c>PF.S_CreateBatchCriticalPartProduct</c> با پارامتر جدولی</summary>
    Task<TabularResult> BatchCreateAsync(int biwSiteId, IReadOnlyList<IReadOnlyDictionary<string, object?>> rows, int userId);
}
