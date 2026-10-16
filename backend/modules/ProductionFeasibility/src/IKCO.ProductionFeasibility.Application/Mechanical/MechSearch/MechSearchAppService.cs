using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.MechSearch.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechSearch.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechSearch;

/// <summary>جستجوی بحران‌ها (frmCriticalPartSearch / جستجوی سنکرون و مکانیکی). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechSearchAppService : PfAppService, IMechSearchAppService
{
    private readonly IMechSearchRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechSearchAppService(IMechSearchRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> SearchCriticalPartsAsync(MechSearchSearchCriticalPartsInput input)
    {
        return await _repository.SearchCriticalPartsAsync(PartNumber.OptionalValue(input.PartNo), input.SetCode, ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.TrimShopIds, input.Status, input.CriticalPartTypeId);
    }

    public async Task<TabularResult> SearchByTrimShopAsync(MechSearchSearchByTrimShopInput input)
    {
        return await _repository.SearchByTrimShopAsync(PartNumber.OptionalValue(input.PartNo), ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.TrimShopId, input.Type);
    }

    public async Task<TabularResult> GetReceivedPartsAsync(MechSearchGetReceivedPartsInput input)
    {
        return await _repository.GetReceivedPartsAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value);
    }

}
