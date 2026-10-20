using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechPartsAppService : PfAppService, IMechPartsAppService
{
    private readonly IMechPartsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechPartsAppService(IMechPartsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ListResultDto<CarPropertiesRow>> GetBodyPropertiesAsync(MechPartsGetBodyPropertiesInput input)
    {
        return new ListResultDto<CarPropertiesRow>(await _repository.GetBodyPropertiesAsync(new BodyNumber(input.BodyNo).Value, input.PlantId, input.Mode));
    }

    public async Task<string?> GetPartDescriptionAsync(MechPartsGetPartDescriptionInput input)
    {
        return await _repository.GetPartDescriptionAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<ListResultDto<PartInShortageListRow>> GetPartInShortageListAsync(MechPartsGetPartInShortageListInput input)
    {
        return new ListResultDto<PartInShortageListRow>(await _repository.GetPartInShortageListAsync(new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<BomProductRow>> GetPartProductsAsync(MechPartsGetPartProductsInput input)
    {
        return new ListResultDto<BomProductRow>(await _repository.GetPartProductsAsync(new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<BomProductRow>> GetProductsByClassAndPartTypeAsync(MechPartsGetProductsByClassAndPartTypeInput input)
    {
        return new ListResultDto<BomProductRow>(await _repository.GetProductsByClassAndPartTypeAsync(input.CarClassCode, input.PartTypeCode));
    }

    public async Task<ListResultDto<CriticalPartExistRow>> ExistsPartNoAsync(MechPartsExistsPartNoInput input)
    {
        return new ListResultDto<CriticalPartExistRow>(await _repository.ExistsPartNoAsync(new PartNumber(input.PartNo).Value, input.TrimShopId));
    }

    public async Task<ListResultDto<CriticalProductCandidateRow>> GetFromBopAsync(MechPartsGetFromBopInput input)
    {
        return new ListResultDto<CriticalProductCandidateRow>(await _repository.GetFromBopAsync(new PartNumber(input.PartNo).Value, input.TrimShopId));
    }

    public async Task<ListResultDto<CriticalProductCandidateRow>> GetFromSapAsync(MechPartsGetFromSapInput input)
    {
        return new ListResultDto<CriticalProductCandidateRow>(await _repository.GetFromSapAsync(input.SubClassCode, new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<CriticalPartForEditRow>> GetForEditAsync(int id)
    {
        return new ListResultDto<CriticalPartForEditRow>(await _repository.GetForEditAsync(id));
    }

    public async Task<ListResultDto<CriticalPartRow>> GetByIdAsync(int id)
    {
        return new ListResultDto<CriticalPartRow>(await _repository.GetByIdAsync(id));
    }

    public async Task<ListResultDto<CriticalPartProductForEditRow>> GetProductsForEditAsync(int id, MechPartsGetProductsForEditInput input)
    {
        return new ListResultDto<CriticalPartProductForEditRow>(await _repository.GetProductsForEditAsync(id, input.ShopTrimId));
    }

    public async Task<ListResultDto<CriticalPartProductRow>> GetProductsAsync(int criticalPartId)
    {
        return new ListResultDto<CriticalPartProductRow>(await _repository.GetProductsAsync(criticalPartId));
    }

    public async Task<int> CreateAsync(MechPartsCreateInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        return await _repository.CreateAsync(input.CarId, new PartNumber(input.PartNo).Value, input.PartDesc, input.PlantId, input.PartGroup, input.AlterNativePartNo, input.CriticalPartNote, input.Stock, input.Used, input.PossibleProductionNo, input.TrimShopId, input.UsageFactor, input.DeptId, userId, input.IsOutOfSap) ?? default;
    }

    public async Task UpdateAsync(int id, MechPartsUpdateInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.UpdateAsync(id, input.Stock, input.Used, input.UsageFactor, input.AlterNativePartNo, input.CriticalPartNote, input.ProductionStateLetterNo, input.ProductionStateNote, input.PartGroup, input.ProductionPossibility, userId, input.IsOutOfSap);
    }

    public async Task<GlobalResultDto> SaveProductAsync(int criticalPartsId, MechPartsSaveProductInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        var result = await _repository.SaveProductAsync(input.CarId, criticalPartsId, input.CarGroupId, input.CarModelId, input.CarClassId, input.SubClassId, input.ColorId, input.TrimId, input.OptionId, userId, input.IsNew, input.TrimShopId, input.PlantId, input.Quantity);
        result ??= new GlobalResultDto { Result = 1, Message = "محصول مشابه فعال برای این بحران از قبل ثبت شده است." };
        return EnsureSuccess(result, "ثبت/به‌روزرسانی یک محصول بحران", r => r > 0);
    }

    public async Task AddSubClassAsync(MechPartsAddSubClassInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.AddSubClassAsync(input.CriticalPartsId, input.SubClassCode, userId, input.TrimShopId);
    }

    public async Task<GlobalResultDto> DeleteAsync(int id)
    {
        var result = await _repository.DeleteAsync(id);
        return EnsureSuccess(result, "حذف بحران مکانیکی (به همراه استثناها و مجوزهای آن)", r => r > 0);
    }

}
