using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Dtos;
using IKCO.ProductionFeasibility.BodyShop.BodyReports.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyReports;

/// <summary>جستجو و گزارش‌های بحران بدنه‌سازی. مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class BodyReportsAppService : PfAppService, IBodyReportsAppService
{
    private readonly IBodyReportsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public BodyReportsAppService(IBodyReportsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> SearchCriticalPartsAsync(BodyReportsSearchCriticalPartsInput input)
    {
        return await _repository.SearchCriticalPartsAsync(input.PlantId, input.VINBiwSite, PartNumber.OptionalValue(input.PartNumber), input.Status, ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.CarGroupId, input.CarModelId, input.CarClassId, input.TypeOfSearch, input.ProductionPossibilityId);
    }

    public async Task<TabularResult> GetCriticalPartProductsAsync(int criticalPartsId)
    {
        return await _repository.GetCriticalPartProductsAsync(criticalPartsId);
    }

    public async Task<TabularResult> GetTrackingCriticalPartsAsync(BodyReportsGetTrackingCriticalPartsInput input)
    {
        return await _repository.GetTrackingCriticalPartsAsync(input.PlantId, input.VINBiwSite, input.CarGroupId, input.CarModelId, input.CarClassId, input.SubClassId, input.ProductionPossibilityId);
    }

    public async Task<TabularResult> GetTrackingCriticalPartProductsAsync(int criticalPartsId)
    {
        return await _repository.GetTrackingCriticalPartProductsAsync(criticalPartsId);
    }

    public async Task<TabularResult> GetProductsOfSitesAsync()
    {
        return await _repository.GetProductsOfSitesAsync();
    }

    public async Task<TabularResult> GetPreAssignBodyCountsAsync(BodyReportsGetPreAssignBodyCountsInput input)
    {
        return await _repository.GetPreAssignBodyCountsAsync(input.CarId);
    }

    public async Task<TabularResult> GetPreAssignCriticalBodiesAsync(BodyReportsGetPreAssignCriticalBodiesInput input)
    {
        return await _repository.GetPreAssignCriticalBodiesAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo));
    }

    public async Task<TabularResult> GetNotPossibleProductsToStepAssignAsync(BodyReportsGetNotPossibleProductsToStepAssignInput input)
    {
        return await _repository.GetNotPossibleProductsToStepAssignAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo));
    }

    public async Task<TabularResult> GetShortageLicenseTrackingAsync(BodyReportsGetShortageLicenseTrackingInput input)
    {
        return await _repository.GetShortageLicenseTrackingAsync(input.CriticalPartTypeId, input.CriticalPartId);
    }

    public async Task<TabularResult> GetProductionPossibilityStatusAsync(int id)
    {
        return await _repository.GetProductionPossibilityStatusAsync(id);
    }

    public async Task<TabularResult> GetFailedSapTransfersAsync(BodyReportsGetFailedSapTransfersInput input)
    {
        return await _repository.GetFailedSapTransfersAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value);
    }

    public async Task<TabularResult> GetSapDataUpdateTimesAsync()
    {
        return await _repository.GetSapDataUpdateTimesAsync();
    }

    public async Task<string?> GetLastChangeOnLicenseAsync()
    {
        return await _repository.GetLastChangeOnLicenseAsync();
    }

}
