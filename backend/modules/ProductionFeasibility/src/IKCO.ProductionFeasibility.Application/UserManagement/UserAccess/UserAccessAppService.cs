using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

/// <summary>دسترسی کاربران به خطوط و سالن‌ها (frmUserManagement). مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class UserAccessAppService : PfAppService, IUserAccessAppService
{
    private readonly IUserAccessRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public UserAccessAppService(IUserAccessRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> GetAllUsersAsync()
    {
        return await _repository.GetAllUsersAsync();
    }

    public async Task<TabularResult> GetUsersAccessListAsync()
    {
        return await _repository.GetUsersAccessListAsync();
    }

    public async Task<TabularResult> GetSitesAsync(UserAccessGetSitesInput input)
    {
        return await _repository.GetSitesAsync(input.PersonalCode, input.SaloonTypeId);
    }

    public async Task<TabularResult> GetSaloonsByVirtualPlantAsync(UserAccessGetSaloonsByVirtualPlantInput input)
    {
        return await _repository.GetSaloonsByVirtualPlantAsync(input.PlantId);
    }

    public async Task<GlobalResultDto> DeleteUserAccessAsync(UserAccessDeleteUserAccessInput input)
    {
        var result = await _repository.DeleteUserAccessAsync(new IdList(input.Ids).ToCommaSeparated());
        return EnsureSuccess(result, "حذف دسترسی کاربران", r => r > 0);
    }

}
