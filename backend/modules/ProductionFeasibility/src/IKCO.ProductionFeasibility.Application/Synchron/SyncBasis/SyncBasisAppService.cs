using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Dtos;
using IKCO.ProductionFeasibility.Synchron.SyncBasis.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncBasis;

/// <summary>تعریف مبنای ست‌های سنکرون (frmCriticalPartSetBasis). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class SyncBasisAppService : PfAppService, ISyncBasisAppService
{
    private readonly ISyncBasisRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public SyncBasisAppService(ISyncBasisRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> GetListAsync()
    {
        return await _repository.GetListAsync();
    }

    public async Task<TabularResult> GetByCodeAsync(SyncBasisGetByCodeInput input)
    {
        return await _repository.GetByCodeAsync(input.Code);
    }

    public async Task<TabularResult> GetByIdAsync(string id)
    {
        return await _repository.GetByIdAsync(id);
    }

    public async Task<TabularResult> GetByStatusAsync(SyncBasisGetByStatusInput input)
    {
        return await _repository.GetByStatusAsync(input.Status);
    }

    public async Task<TabularResult> CountBySetCodeAsync(SyncBasisCountBySetCodeInput input)
    {
        return await _repository.CountBySetCodeAsync(input.SetCode);
    }

    public async Task CreateAsync(SyncBasisCreateInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.CreateAsync(input.Code, input.DescF, input.CarClass, input.PartTypeId, input.LevelType, userId, input.IsTrim, input.IsOption, input.IsToodoozi, input.IsColor);
    }

    public async Task UpdateAsync(int id, SyncBasisUpdateInput input)
    {
        await _repository.UpdateAsync(id, input.Code, input.DescF, input.CarClassId, input.PartTypeId, input.LevelType, input.Status, input.IsTrim, input.IsColor, input.IsOption, input.IsToodoozi);
    }

}
