using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

public partial class UserAccessAppService
{
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> InsertAsync(UserAccessInsertInput input)
    {
        var createUserId = await _currentUser.GetIdAsync();
        var items = input.Items.Select(i => (i.UserId, i.BIWSiteId)).Distinct().ToList();
        var result = await _repository.InsertAsync(items, createUserId, Clock.Now);
        return EnsureSuccess(result, "ثبت دسترسی کاربران", r => r > 0);
    }
}
