using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Synchron.Results;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Dtos;
using IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class SyncSetsAppService : PfAppService, ISyncSetsAppService
{
    private readonly ISyncSetsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public SyncSetsAppService(ISyncSetsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ListResultDto<CriticalPartSetRow>> GetByIdAsync(int id)
    {
        return new ListResultDto<CriticalPartSetRow>(await _repository.GetByIdAsync(id));
    }

    public async Task<ListResultDto<SetPartRow>> GetPartsAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetPartRow>(await _repository.GetPartsAsync(criticalPartsSetId));
    }

    public async Task<ListResultDto<SetPartWithShortageRow>> GetPartsWithShortageListAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetPartWithShortageRow>(await _repository.GetPartsWithShortageListAsync(criticalPartsSetId));
    }

    public async Task<ListResultDto<SetColorRow>> GetColorsAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetColorRow>(await _repository.GetColorsAsync(criticalPartsSetId));
    }

    public async Task<ListResultDto<SetSaloonRow>> GetSaloonsAsync(int criticalPartSetId)
    {
        return new ListResultDto<SetSaloonRow>(await _repository.GetSaloonsAsync(criticalPartSetId));
    }

    public async Task<ListResultDto<SetProductRow>> GetProductsAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetProductRow>(await _repository.GetProductsAsync(criticalPartsSetId));
    }

    public async Task<ListResultDto<SetProductDetailRow>> GetProductDetailsAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetProductDetailRow>(await _repository.GetProductDetailsAsync(criticalPartsSetId));
    }

    public async Task<int> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId)
    {
        return await _repository.GetNotEnteredIndicatorCarsCountAsync(criticalPartsSetId) ?? default;
    }

    public async Task<ListResultDto<SetSaloonItemRow>> GetSetSaloonsAsync(int criticalPartsSetId)
    {
        return new ListResultDto<SetSaloonItemRow>(await _repository.GetSetSaloonsAsync(criticalPartsSetId));
    }

    public async Task<ListResultDto<BomPartInfoRow>> GetPartInfoAsync(SyncSetsGetPartInfoInput input)
    {
        return new ListResultDto<BomPartInfoRow>(await _repository.GetPartInfoAsync(new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<SimilarColorRow>> GetSimilarColorsAsync(SyncSetsGetSimilarColorsInput input)
    {
        return new ListResultDto<SimilarColorRow>(await _repository.GetSimilarColorsAsync(input.Code));
    }

    public async Task<ListResultDto<SaloonByCarModelRow>> GetSaloonsByCarModelsAsync(SyncSetsGetSaloonsByCarModelsInput input)
    {
        return new ListResultDto<SaloonByCarModelRow>(await _repository.GetSaloonsByCarModelsAsync(input.CarModelIds, input.PlantId));
    }

    public async Task<GlobalResultDto> CreateHeaderAsync(SyncSetsCreateHeaderInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        var result = await _repository.CreateHeaderAsync(input.SetId, input.DeptId, input.ProductsColorCode, input.ProductsColorId, input.ProductsTrimId, input.ProductsToodooziId, input.ProductsOptionId, input.Stock, input.Note, userId);
        return EnsureSuccess(result, "ثبت سرآیند بحران سنکرون", r => r > 0);
    }

    public async Task<GlobalResultDto> UpdateHeaderAsync(int id, SyncSetsUpdateHeaderInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        var result = await _repository.UpdateHeaderAsync(id, input.DeptId, input.Stock, input.Note, input.Status, userId);
        return EnsureSuccess(result, "ویرایش بحران سنکرون", r => r > 0);
    }

    public async Task<GlobalResultDto> AddProductAsync(int criticalPartsSetId, SyncSetsAddProductInput input)
    {
        var result = await _repository.AddProductAsync(criticalPartsSetId, input.CarGroupId, input.CarModelId, input.CarClassId, input.CarSubClassId, input.CarTrimId, input.CarToodooziId, input.CarOptionId, input.CarColorId, input.SaloonId);
        return EnsureSuccess(result, "ثبت یک محصول بحران سنکرون", r => r == 1);
    }

    public async Task<GlobalResultDto> UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, SyncSetsUpdateSaloonIndicatorCarInput input)
    {
        var result = await _repository.UpdateSaloonIndicatorCarAsync(id, criticalPartsSetId, input.SaloonId, input.IndicatorCarId);
        return EnsureSuccess(result, "تعیین خودروی شاخص سالن بحران سنکرون", r => r == 1);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

}
