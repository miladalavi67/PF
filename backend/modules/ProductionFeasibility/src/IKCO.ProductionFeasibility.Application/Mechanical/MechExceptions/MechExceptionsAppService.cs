using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.MechExceptions.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechExceptions.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions;

/// <summary>استثناهای محصولات بحران و مجوز آن‌ها. مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechExceptionsAppService : PfAppService, IMechExceptionsAppService
{
    private readonly IMechExceptionsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechExceptionsAppService(IMechExceptionsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> GetExceptionListAsync(MechExceptionsGetExceptionListInput input)
    {
        return await _repository.GetExceptionListAsync(BodyNumber.OptionalValue(input.BodyNo), input.DateFrom, input.DateEnd, input.State);
    }

    public async Task<TabularResult> GetLicensesAsync(int id)
    {
        return await _repository.GetLicensesAsync(id);
    }

    public async Task<TabularResult> GetLicenseBodiesAsync(int id)
    {
        return await _repository.GetLicenseBodiesAsync(id);
    }

    public async Task<TabularResult> GetLicenseLogAsync(MechExceptionsGetLicenseLogInput input)
    {
        return await _repository.GetLicenseLogAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task CreateLicenseAsync(MechExceptionsCreateLicenseInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.CreateLicenseAsync(input.CriticalPartsProductExceptionsId, input.PossibleLicenseCount, input.LetterNo, input.Description, userId);
    }

    public async Task DisableLicenseAsync(int id)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.DisableLicenseAsync(userId, id);
    }

    public async Task DisableExceptionsAsync(MechExceptionsDisableExceptionsInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.DisableExceptionsAsync(new IdList(input.Ids).ToCommaSeparated(), userId);
    }

    public async Task<TabularResult> GetPartNumbersInExceptionsAsync(MechExceptionsGetPartNumbersInExceptionsInput input)
    {
        return await _repository.GetPartNumbersInExceptionsAsync(input.FilterValues, input.TrimId, input.FilterType, PartNumber.OptionalValue(input.PartNumber));
    }

}
