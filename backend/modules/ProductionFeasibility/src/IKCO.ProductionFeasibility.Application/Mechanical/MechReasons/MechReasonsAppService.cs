using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechReasons.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechReasons.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechReasons;

/// <summary>دلایل عدم امکان برنامه‌شدن / ورود (frmNotPossibleProductReason). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechReasonsAppService : PfAppService, IMechReasonsAppService
{
    private readonly IMechReasonsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechReasonsAppService(IMechReasonsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<ListResultDto<NotPossibleReasonRow>> GetListAsync(MechReasonsGetListInput input)
    {
        return new ListResultDto<NotPossibleReasonRow>(await _repository.GetListAsync(input.SelectType));
    }

    public async Task CreateAsync(MechReasonsCreateInput input)
    {
        var userId = await _currentUser.GetPersonIdAsync();
        await _repository.CreateAsync(input.Code, input.DescF, input.DescE, input.Status, userId);
    }

    public async Task UpdateAsync(int id, MechReasonsUpdateInput input)
    {
        await _repository.UpdateAsync(id, input.Code, input.DescF, input.DescE, input.Status);
    }

    public async Task DeleteAsync(int id)
    {
        await _repository.DeleteAsync(id);
    }

}
