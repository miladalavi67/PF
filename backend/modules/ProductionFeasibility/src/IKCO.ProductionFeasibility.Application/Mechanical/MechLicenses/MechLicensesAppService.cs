using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

/// <summary>شرایط تولید و مجوزهای تولید با کسری. مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechLicensesAppService : PfAppService, IMechLicensesAppService
{
    private readonly IMechLicensesRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechLicensesAppService(IMechLicensesRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ListResultDto<ShortageLicenseTrackingRow>> GetTrackingInfoAsync(MechLicensesGetTrackingInfoInput input)
    {
        return new ListResultDto<ShortageLicenseTrackingRow>(await _repository.GetTrackingInfoAsync(input.CriticalPartTypeId, input.CriticalPartId));
    }

    public async Task<ListResultDto<ShortageLicenseTrackingRow>> GetTrackingListAsync(MechLicensesGetTrackingListInput input)
    {
        return new ListResultDto<ShortageLicenseTrackingRow>(await _repository.GetTrackingListAsync(input.CriticalPartTypeId, input.CriticalPartId));
    }

    public async Task<ListResultDto<LicenseTrackingSearchRow>> SearchTrackingAsync(MechLicensesSearchTrackingInput input)
    {
        return new ListResultDto<LicenseTrackingSearchRow>(await _repository.SearchTrackingAsync(input.FromDate, input.ToDate, PartNumber.OptionalValue(input.PartNo), input.SetCode));
    }

    public async Task<ListResultDto<LicenseTrackingCriticalRow>> SearchTrackingCriticalsAsync(MechLicensesSearchTrackingCriticalsInput input)
    {
        return new ListResultDto<LicenseTrackingCriticalRow>(await _repository.SearchTrackingCriticalsAsync(input.FromDate, input.ToDate, PartNumber.OptionalValue(input.PartNo), input.SetCode));
    }

    public async Task<ListResultDto<UsedLicenseBodyRow>> GetBodiesRelatedToUsedLicenseAsync(MechLicensesGetBodiesRelatedToUsedLicenseInput input)
    {
        return new ListResultDto<UsedLicenseBodyRow>(await _repository.GetBodiesRelatedToUsedLicenseAsync(input.CriticalPartTypeId, input.CriticalPartId));
    }

}
