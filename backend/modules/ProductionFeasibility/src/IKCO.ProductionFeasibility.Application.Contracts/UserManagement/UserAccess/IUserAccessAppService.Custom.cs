using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.UserManagement.UserAccess.Dtos;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

public partial interface IUserAccessAppService
{
    /// <summary>ثبت دسترسی کاربران به خطوط بدنه‌سازی</summary>
    Task<GlobalResultDto> InsertAsync(UserAccessInsertInput input);
}
