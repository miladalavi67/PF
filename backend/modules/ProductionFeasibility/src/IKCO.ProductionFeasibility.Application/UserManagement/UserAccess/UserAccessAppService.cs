using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.UserManagement.Results;
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

    public async Task<ListResultDto<PfUserRow>> GetAllUsersAsync()
    {
        return new ListResultDto<PfUserRow>(await _repository.GetAllUsersAsync());
    }

    public async Task<ListResultDto<UserAccessRow>> GetUsersAccessListAsync()
    {
        return new ListResultDto<UserAccessRow>(await _repository.GetUsersAccessListAsync());
    }

    public async Task<ListResultDto<VirtualPlantRow>> GetSitesAsync(UserAccessGetSitesInput input)
    {
        return new ListResultDto<VirtualPlantRow>(await _repository.GetSitesAsync(input.PersonalCode, input.SaloonTypeId));
    }

    public async Task<ListResultDto<BiwSiteRow>> GetSaloonsByVirtualPlantAsync(UserAccessGetSaloonsByVirtualPlantInput input)
    {
        return new ListResultDto<BiwSiteRow>(await _repository.GetSaloonsByVirtualPlantAsync(input.PlantId));
    }

    public async Task<GlobalResultDto> DeleteUserAccessAsync(UserAccessDeleteUserAccessInput input)
    {
        var result = await _repository.DeleteUserAccessAsync(new IdList(input.Ids).ToCommaSeparated());
        return EnsureSuccess(result, "حذف دسترسی کاربران", r => r > 0);
    }

}
