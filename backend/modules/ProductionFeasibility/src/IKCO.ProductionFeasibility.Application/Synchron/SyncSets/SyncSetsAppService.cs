using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
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

    public async Task<TabularResult> GetByIdAsync(int id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TabularResult> GetPartsAsync(int criticalPartsSetId)
    {
        return await _repository.GetPartsAsync(criticalPartsSetId);
    }

    public async Task<TabularResult> GetPartsWithShortageListAsync(int criticalPartsSetId)
    {
        return await _repository.GetPartsWithShortageListAsync(criticalPartsSetId);
    }

    public async Task<TabularResult> GetColorsAsync(int criticalPartsSetId)
    {
        return await _repository.GetColorsAsync(criticalPartsSetId);
    }

    public async Task<TabularResult> GetSaloonsAsync(int criticalPartSetId)
    {
        return await _repository.GetSaloonsAsync(criticalPartSetId);
    }

    public async Task<TabularResult> GetProductsAsync(int criticalPartsSetId)
    {
        return await _repository.GetProductsAsync(criticalPartsSetId);
    }

    public async Task<TabularResult> GetProductDetailsAsync(int criticalPartsSetId)
    {
        return await _repository.GetProductDetailsAsync(criticalPartsSetId);
    }

    public async Task<TabularResult> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId)
    {
        return await _repository.GetNotEnteredIndicatorCarsCountAsync(criticalPartsSetId);
    }

    public async Task<int> CreateHeaderAsync(SyncSetsCreateHeaderInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        return await _repository.CreateHeaderAsync(input.SetId, input.DeptId, input.ProductsColorCode, input.ProductsColorId, input.ProductsTrimId, input.ProductsToodooziId, input.ProductsOptionId, input.Stock, input.Note, userId);
    }

    public async Task UpdateHeaderAsync(int id, SyncSetsUpdateHeaderInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.UpdateHeaderAsync(id, input.DeptId, input.Stock, input.Note, input.Status, userId);
    }

    public async Task AddProductAsync(int criticalPartsSetId, SyncSetsAddProductInput input)
    {
        await _repository.AddProductAsync(criticalPartsSetId, input.CarGroupId, input.CarModelId, input.CarClassId, input.CarSubClassId, input.CarTrimId, input.CarToodooziId, input.CarOptionId, input.CarColorId, input.SaloonId);
    }

    public async Task UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, SyncSetsUpdateSaloonIndicatorCarInput input)
    {
        await _repository.UpdateSaloonIndicatorCarAsync(id, criticalPartsSetId, input.SaloonId, input.IndicatorCarId);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

}
