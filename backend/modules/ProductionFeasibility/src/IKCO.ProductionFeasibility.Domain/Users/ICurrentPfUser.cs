using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Users;

/// <summary>کاربر امکان تولید متناظر با کاربر احراز هویت‌شده درخواست جاری.</summary>
public interface ICurrentPfUser
{
    /// <summary>اطلاعات کاربر در <c>PF.B_User</c>؛ در صورت تعریف نشدن خطای <see cref="PfErrorCodes.PfUserNotFound"/></summary>
    Task<PfUserSession> GetAsync();

    /// <summary><c>PF.B_User.Id</c> برای رویه‌های schema PF</summary>
    Task<int> GetIdAsync();

    /// <summary>
    /// <c>dbo.B_Person.Id</c> برای رویه‌های کنترل تولید (dbo) که <c>@UserId</c> آن‌ها شناسه شخص است
    /// (مثلاً <c>S_SS_Auth_Person_AccessRights_VirtualPlant</c>)؛ در صورت نبودن خطای <see cref="PfErrorCodes.PersonNotFound"/>
    /// </summary>
    Task<int> GetPersonIdAsync();
}
