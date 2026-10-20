using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

public sealed record SyncSetPart(int ShortagePartListId, string PartNo);

public sealed record SyncSetProduct(int CarGroupId, int CarModelId, int CarClassId, int CarSubClassId, int CarTrimId, int CarToodooziId,
    int CarOptionId, int CarColorId, int SaloonId);

/// <summary>
/// ثبت ترکیب بحران سنکرون با همان رویه‌های بک‌اند قبلی (<c>SyncPartsRepository</c>)؛ هر متد نتیجه رویه (Result/Message) را برمی‌گرداند
/// و بررسی موفقیت در سرویس کاربردی انجام می‌شود.
/// </summary>
public partial interface ISyncSetsRepository
{
    /// <summary>شناسه قطعه در فهرست مرجع کسری با رویه <c>SIMP_SelectParInShortagePartList</c>؛ در نبود، null</summary>
    Task<int?> FindShortagePartListIdAsync(string partNo);

    /// <summary>قطعات ست؛ رویه <c>S_InsertCriticalPartsSet_Parts_Web</c> با پارامتر جدولی</summary>
    Task<GlobalResultDto?> InsertPartsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetPart> parts);

    /// <summary>رنگ‌های ست؛ رویه <c>S_InsertCriticalPartsSet_Colors_Web</c> با پارامتر جدولی</summary>
    Task<GlobalResultDto?> InsertColorsAsync(int criticalPartsSetId, IReadOnlyList<int> colorIds);

    /// <summary>سالن‌های ست؛ رویه <c>S_InsertCriticalPartsSet_Saloons_Web</c> با پارامتر جدولی</summary>
    Task<GlobalResultDto?> InsertSaloonsAsync(int criticalPartsSetId, IReadOnlyList<int> saloonIds);

    /// <summary>محصولات ست؛ رویه <c>S_CriticalPartsSetProductInsert_Web</c> برای هر محصول (مانند بک‌اند قبلی)</summary>
    Task<List<GlobalResultDto?>> InsertProductsAsync(int criticalPartsSetId, IReadOnlyList<SyncSetProduct> products);
}
