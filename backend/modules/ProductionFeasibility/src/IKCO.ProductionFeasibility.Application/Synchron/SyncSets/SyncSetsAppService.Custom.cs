using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

public partial class SyncSetsAppService
{
    /// <summary>
    /// در بک‌اند قبلی هر بخش (قطعات/رنگ/سالن/محصولات) با درخواست جدا و بدون تراکنش ثبت می‌شد و خطای میانه
    /// ست ناقص باقی می‌گذاشت؛ اینجا همه در یک تراکنش ثبت می‌شوند.
    /// </summary>
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> SaveCompositionAsync(int criticalPartsSetId, SyncSetsSaveCompositionInput input)
    {
        if (input.Parts.Count == 0 || input.Products.Count == 0 || input.SaloonIds.Count == 0)
            throw new BaseException(PfErrorCodes.InputInvalid, "حداقل یک قطعه، یک سالن و یک محصول برای ست لازم است.");

        var parts = new List<SyncSetPart>();
        foreach (var item in input.Parts)
        {
            var partNo = new PartNumber(item.PartNo).Value;
            var listId = item.ShortagePartListId > 0 ? item.ShortagePartListId : await _repository.FindShortagePartListIdAsync(partNo);
            if (listId is null)
                throw new BaseException(PfErrorCodes.InputInvalid, $"شماره فنی {partNo} در فهرست مرجع اقلام کسری ثبت نشده است.").WithData("PartNo", partNo);
            parts.Add(new SyncSetPart(listId.Value, partNo));
        }
        parts = parts.Distinct().ToList();
        await _repository.InsertPartsAsync(criticalPartsSetId, parts);
        if (input.ColorIds.Count > 0)
            await _repository.InsertColorsAsync(criticalPartsSetId, input.ColorIds.Distinct().ToList());
        await _repository.InsertSaloonsAsync(criticalPartsSetId, input.SaloonIds.Distinct().ToList());
        var products = input.Products
            .Select(p => new SyncSetProduct(p.CarGroupId, p.CarModelId, p.CarClassId, p.CarSubClassId, p.CarTrimId, p.CarToodooziId, p.CarOptionId, p.CarColorId, p.SaloonId))
            .Distinct().ToList();
        await _repository.InsertProductsAsync(criticalPartsSetId, products);
        return new GlobalResultDto { Result = 1, Message = "ترکیب بحران سنکرون با موفقیت ثبت شد." };
    }
}
