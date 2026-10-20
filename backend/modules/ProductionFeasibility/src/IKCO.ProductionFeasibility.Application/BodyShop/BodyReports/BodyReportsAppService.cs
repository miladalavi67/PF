using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.BodyShop.Results;
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

    public async Task<ListResultDto<BodyCriticalSearchRow>> SearchCriticalPartsAsync(BodyReportsSearchCriticalPartsInput input)
    {
        return new ListResultDto<BodyCriticalSearchRow>(await _repository.SearchCriticalPartsAsync(input.PlantId, input.VINBiwSite, PartNumber.OptionalValue(input.PartNumber), input.Status, ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.CarGroupId, input.CarModelId, input.CarClassId, input.TypeOfSearch, input.ProductionPossibilityId));
    }

    public async Task<ListResultDto<BodyCriticalProductRow>> GetCriticalPartProductsAsync(int criticalPartsId)
    {
        return new ListResultDto<BodyCriticalProductRow>(await _repository.GetCriticalPartProductsAsync(criticalPartsId));
    }

    public async Task<ListResultDto<BodyTrackingCriticalPartRow>> GetTrackingCriticalPartsAsync(BodyReportsGetTrackingCriticalPartsInput input)
    {
        return new ListResultDto<BodyTrackingCriticalPartRow>(await _repository.GetTrackingCriticalPartsAsync(input.PlantId, input.VINBiwSite, input.CarGroupId, input.CarModelId, input.CarClassId, input.SubClassId, input.ProductionPossibilityId));
    }

    public async Task<ListResultDto<BodyTrackingProductRow>> GetTrackingCriticalPartProductsAsync(int criticalPartsId)
    {
        return new ListResultDto<BodyTrackingProductRow>(await _repository.GetTrackingCriticalPartProductsAsync(criticalPartsId));
    }

    public async Task<ListResultDto<BodyProductsOfSiteRow>> GetProductsOfSitesAsync()
    {
        return new ListResultDto<BodyProductsOfSiteRow>(await _repository.GetProductsOfSitesAsync());
    }

    public async Task<ListResultDto<BodyPreAssignCountRow>> GetPreAssignBodyCountsAsync(BodyReportsGetPreAssignBodyCountsInput input)
    {
        return new ListResultDto<BodyPreAssignCountRow>(await _repository.GetPreAssignBodyCountsAsync(input.CarId));
    }

    public async Task<ListResultDto<BodyPreAssignCriticalRow>> GetPreAssignCriticalBodiesAsync(BodyReportsGetPreAssignCriticalBodiesInput input)
    {
        return new ListResultDto<BodyPreAssignCriticalRow>(await _repository.GetPreAssignCriticalBodiesAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo)));
    }

    public async Task<ListResultDto<BodyNotPossibleToAssignRow>> GetNotPossibleProductsToStepAssignAsync(BodyReportsGetNotPossibleProductsToStepAssignInput input)
    {
        return new ListResultDto<BodyNotPossibleToAssignRow>(await _repository.GetNotPossibleProductsToStepAssignAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo)));
    }

    public async Task<ListResultDto<BodyLicenseTrackingRow>> GetShortageLicenseTrackingAsync(BodyReportsGetShortageLicenseTrackingInput input)
    {
        return new ListResultDto<BodyLicenseTrackingRow>(await _repository.GetShortageLicenseTrackingAsync(input.CriticalPartTypeId, input.CriticalPartId));
    }

    public async Task<ListResultDto<BodyProductionPossibilityStatusRow>> GetProductionPossibilityStatusAsync(int id)
    {
        return new ListResultDto<BodyProductionPossibilityStatusRow>(await _repository.GetProductionPossibilityStatusAsync(id));
    }

    public async Task<ListResultDto<FailedSapTransferRow>> GetFailedSapTransfersAsync(BodyReportsGetFailedSapTransfersInput input)
    {
        return new ListResultDto<FailedSapTransferRow>(await _repository.GetFailedSapTransfersAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value));
    }

    public async Task<ListResultDto<SapDataUpdateTimesRow>> GetSapDataUpdateTimesAsync()
    {
        return new ListResultDto<SapDataUpdateTimesRow>(await _repository.GetSapDataUpdateTimesAsync());
    }

    public async Task<string?> GetLastChangeOnLicenseAsync()
    {
        return await _repository.GetLastChangeOnLicenseAsync();
    }

}
