using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
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

    public async Task<TabularResult> GetBodyPropertiesAsync(MechPartsGetBodyPropertiesInput input)
    {
        return await _repository.GetBodyPropertiesAsync(new BodyNumber(input.BodyNo).Value, input.PlantId, input.Mode);
    }

    public async Task<string?> GetPartDescriptionAsync(MechPartsGetPartDescriptionInput input)
    {
        return await _repository.GetPartDescriptionAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetPartInShortageListAsync(MechPartsGetPartInShortageListInput input)
    {
        return await _repository.GetPartInShortageListAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetPartProductsAsync(MechPartsGetPartProductsInput input)
    {
        return await _repository.GetPartProductsAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetProductsByClassAndPartTypeAsync(MechPartsGetProductsByClassAndPartTypeInput input)
    {
        return await _repository.GetProductsByClassAndPartTypeAsync(input.CarClassCode, input.PartTypeCode);
    }

    public async Task<TabularResult> ExistsPartNoAsync(MechPartsExistsPartNoInput input)
    {
        return await _repository.ExistsPartNoAsync(new PartNumber(input.PartNo).Value, input.TrimShopId);
    }

    public async Task<TabularResult> GetFromBopAsync(MechPartsGetFromBopInput input)
    {
        return await _repository.GetFromBopAsync(new PartNumber(input.PartNo).Value, input.TrimShopId);
    }

    public async Task<TabularResult> GetFromSapAsync(MechPartsGetFromSapInput input)
    {
        return await _repository.GetFromSapAsync(input.SubClassCode, new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetForEditAsync(int id)
    {
        return await _repository.GetForEditAsync(id);
    }

    public async Task<TabularResult> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TabularResult> GetProductsForEditAsync(int id, MechPartsGetProductsForEditInput input)
    {
        return await _repository.GetProductsForEditAsync(id, input.ShopTrimId);
    }

    public async Task<TabularResult> GetProductsAsync(int criticalPartId)
    {
        return await _repository.GetProductsAsync(criticalPartId);
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

    public async Task SaveProductAsync(int criticalPartsId, MechPartsSaveProductInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.SaveProductAsync(input.CarId, criticalPartsId, input.CarGroupId, input.CarModelId, input.CarClassId, input.SubClassId, input.ColorId, input.TrimId, input.OptionId, userId, input.IsNew, input.TrimShopId, input.PlantId, input.Quantity);
    }

    public async Task AddSubClassAsync(MechPartsAddSubClassInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.AddSubClassAsync(input.CriticalPartsId, input.SubClassCode, userId, input.TrimShopId);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

}
