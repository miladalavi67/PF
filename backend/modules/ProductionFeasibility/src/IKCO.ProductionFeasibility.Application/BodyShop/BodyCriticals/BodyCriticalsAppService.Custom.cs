using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Sap;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

public partial class BodyCriticalsAppService
{
    /// <summary>
    /// در بک‌اند قبلی خطای هر کلاس بلعیده می‌شد و ثبت نیمه‌کاره می‌ماند؛ اینجا همه کلاس‌ها در یک تراکنش ثبت می‌شوند
    /// (خطای رویه ← 400 و بازگشت کامل).
    /// </summary>
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> CreateProductsAsync(int criticalPartsId, BodyCriticalsCreateProductsInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        var items = input.Items.GroupBy(i => i.CarClassId).Select(g => (g.Key, g.Last().UsageFactor)).ToList();
        var count = await _repository.CreateProductsAsync(criticalPartsId, input.CarId, items, userId);
        return new GlobalResultDto { Result = count, Message = "محصولات بحران با موفقیت ثبت شد." };
    }

    public async Task<GlobalResultDto> RegisterLicenseAsync(BodyCriticalsRegisterLicenseInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        var result = await _repository.RegisterLicenseAsync(input.CriticalPartId, input.CriticalPartTypeId, input.ProductionPossibilityId,
            input.PossibleProductionNo, ShamsiDate.OptionalValue(input.ProductionPossibilityToDate), input.ProductionPossibilityToTime,
            input.Note, userId);
        return EnsureSuccess(result, "ثبت مجوز تولید", r => r == 1);
    }

    [UnitOfWork(isTransactional: true)]
    public async Task<ListResultDto<BodyBatchCreateResultRow>> BatchCreateAsync(BodyCriticalsBatchCreateInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        var rows = input.Rows.Select(r => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["BodyNo"] = new BodyNumber(r.BodyNo).Value,
            ["PartNo"] = new PartNumber(r.PartNo).Value,
            ["PartDescF"] = r.PartDescF,
            ["PartGroup"] = r.PartGroup,
            ["Alternative"] = r.Alternative,
            ["PartDescription"] = r.PartDescription,
            ["GroupStock"] = r.GroupStock,
            ["RawStock"] = r.RawStock,
            ["Description"] = r.Description,
            ["LetterNo"] = r.LetterNo,
            ["CarClassCode"] = r.CarClassCode,
            ["Quantity"] = r.Quantity,
        }).ToList();
        return new ListResultDto<BodyBatchCreateResultRow>(await _repository.BatchCreateAsync(input.BIWSiteId, rows, userId));
    }

    public async Task<ListResultDto<SapWhereUsedItemDto>> GetSapWhereUsedAsync(BodyCriticalsGetSapWhereUsedInput input)
    {
        var client = LazyServiceProvider.LazyGetRequiredService<ISapWhereUsedClient>();
        var items = await client.GetWhereUsedAsync(new PartNumber(input.PartNo).Value, input.SaloonCode.Trim());
        return new ListResultDto<SapWhereUsedItemDto>(items
            .Select(i => new SapWhereUsedItemDto { SubClassCode = i.SubClassCode, Quantity = i.Quantity }).ToList());
    }

    /// <summary>
    /// معادل حذف بحران در frmBodyCriticalSearch: دلیل حذف برای هر بحران با پسوند «(حذف بحران)» در لاگ رویدادها ثبت و سپس بحران‌ها
    /// غیرفعال می‌شوند؛ همه در یک تراکنش.
    /// </summary>
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> DisableCriticalPartsAsync(BodyCriticalsDisableCriticalPartsInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        if (!string.IsNullOrWhiteSpace(input.Reason))
        {
            foreach (var id in input.Ids)
                await _repository.InsertDisableReasonAsync(input.EventTypeId, id, input.Reason.Trim() + " (حذف بحران)", userId);
        }

        var result = await _repository.DisableCriticalPartsAsync(new IdList(input.Ids).ToCommaSeparated(), userId);
        return EnsureSuccess(result, "غیرفعال‌سازی بحران‌های بدنه‌سازی", r => r > 0);
    }
}
