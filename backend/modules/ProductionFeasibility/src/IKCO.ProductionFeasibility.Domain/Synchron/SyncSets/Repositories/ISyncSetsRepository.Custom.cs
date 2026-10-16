using System.Collections.Generic;
using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

public sealed record SyncSetPart(int ShortagePartListId, string PartNo);

public sealed record SyncSetProduct(int CarGroupId, int CarModelId, int CarClassId, int CarSubClassId, int CarTrimId, int CarToodooziId,
    int CarOptionId, int CarColorId, int SaloonId);

public partial interface ISyncSetsRepository
{
    /// <summary>شناسه فعال شماره فنی در فهرست مرجع کسری (T_ShortagePartList)؛ در نبود، null</summary>
    Task<int?> FindShortagePartListIdAsync(string partNo);

    /// <summary>قطعات ست؛ رویه <c>S_InsertCriticalPartsSet_Parts_Web</c> در صورت وجود، وگرنه درج مستقیم</summary>
    Task InsertPartsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetPart> parts);

    /// <summary>رنگ‌های ست؛ رویه <c>S_InsertCriticalPartsSet_Colors_Web</c> در صورت وجود، وگرنه درج مستقیم</summary>
    Task InsertColorsAsync(int criticalPartsSetId, IReadOnlyList<int> colorIds);

    /// <summary>سالن‌های ست؛ رویه <c>S_InsertCriticalPartsSet_Saloons_Web</c> در صورت وجود، وگرنه درج مستقیم</summary>
    Task InsertSaloonsAsync(int criticalPartsSetId, IReadOnlyList<int> saloonIds);

    /// <summary>محصولات ست؛ رویه <c>S_InsertCriticalPartsSet_Products</c> با پارامتر جدولی <c>@dt</c></summary>
    Task InsertProductsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetProduct> products);
}
