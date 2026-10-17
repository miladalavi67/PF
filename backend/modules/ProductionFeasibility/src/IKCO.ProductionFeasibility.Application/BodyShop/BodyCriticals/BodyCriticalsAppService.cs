using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

/// <summary>بحران‌های بدنه‌سازی (ثبت، ویرایش، غیرفعال‌سازی). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class BodyCriticalsAppService : PfAppService, IBodyCriticalsAppService
{
    private readonly IBodyCriticalsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public BodyCriticalsAppService(IBodyCriticalsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<BodyInfoRow?> GetBodyInfoAsync(BodyCriticalsGetBodyInfoInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        return await _repository.GetBodyInfoAsync(new BodyNumber(input.BodyNumber).Value, userId);
    }

    public async Task<string?> GetPartDescriptionAsync(BodyCriticalsGetPartDescriptionInput input)
    {
        return await _repository.GetPartDescriptionAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<ListResultDto<PartProductRow>> GetPartProductsAsync(BodyCriticalsGetPartProductsInput input)
    {
        return new ListResultDto<PartProductRow>(await _repository.GetPartProductsAsync(new PartNumber(input.PartNo).Value, input.BIWSiteId));
    }

    public async Task<TabularResult> GetCodeClassByPartNoAsync(BodyCriticalsGetCodeClassByPartNoInput input)
    {
        return await _repository.GetCodeClassByPartNoAsync(new PartNumber(input.PartNo).Value, input.BIWSiteId);
    }

    public async Task<GlobalResultDto> CreateCriticalPartAsync(BodyCriticalsCreateCriticalPartInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        var result = await _repository.CreateCriticalPartAsync(input.CarId, new PartNumber(input.PartNo).Value, input.PartDesc, input.PartGroup, input.AlterNativePartNo, input.CriticalPartNote, input.BIWSiteId, input.TotalStock, input.RawStock, userId, input.ProductionStateNote, input.LetterNo, input.IsOutOfSAP);
        return EnsureSuccess(result, "ثبت بحران بدنه‌سازی", r => r > 0);
    }

    public async Task<GlobalResultDto> UpdateCriticalPartAsync(int criticalPartsId, BodyCriticalsUpdateCriticalPartInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        var result = await _repository.UpdateCriticalPartAsync(criticalPartsId, input.PartGroup, input.AlterNativePartNo, input.CriticalPartNote, input.RawStock, userId, input.ProductionStateNote, input.LetterNo);
        return EnsureSuccess(result, "ویرایش بحران بدنه‌سازی", r => r > 0);
    }

    public async Task<GlobalResultDto> DisableCriticalPartsAsync(BodyCriticalsDisableCriticalPartsInput input)
    {
        var userId = await _currentUser.GetIdAsync();
        if (!string.IsNullOrWhiteSpace(input.Reason))
        {
            foreach (var id in input.Ids)
            {
                await _repository.InsertDisableReasonAsync(input.EventTypeId, id, input.Reason.Trim() + " (حذف بحران)", userId);
            }
        }

        var result = await _repository.DisableCriticalPartsAsync(new IdList(input.Ids).ToCommaSeparated(), userId);
        return EnsureSuccess(result, "غیرفعال‌سازی بحران‌های بدنه‌سازی", r => r > 0);
    }

    public async Task<TabularResult> GetCriticalPartInfoAsync(int criticalPartsId)
    {
        return await _repository.GetCriticalPartInfoAsync(criticalPartsId);
    }

    public async Task<TabularResult> GetEventLogAsync(BodyCriticalsGetEventLogInput input)
    {
        return await _repository.GetEventLogAsync(input.EventTypeId, input.ChangedRecordId);
    }

}
