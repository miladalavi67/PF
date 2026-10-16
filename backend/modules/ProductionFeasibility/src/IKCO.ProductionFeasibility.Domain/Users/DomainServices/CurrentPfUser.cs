using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Users.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Services;

namespace IKCO.ProductionFeasibility.Users.DomainServices;

/// <summary>
/// بازسازی منطق <c>Program.SetApplication</c> سامانه ویندوزی برای هر درخواست: شناسه کاربر امکان تولید
/// (<c>B_User.Id</c>) از روی نام کاربری توکن به دست می‌آید و هرگز از کلاینت پذیرفته نمی‌شود
/// (در بک‌اند قبلی <c>UserId</c> در بدنه درخواست می‌آمد و قابل جعل بود).
/// </summary>
public class CurrentPfUser : DomainService, ICurrentPfUser, IScopedDependency
{
    private readonly IPfIdentityProvider _identity;
    private readonly IPfUserRepository _users;
    private PfUserSession? _cached;
    private int? _personId;

    public CurrentPfUser(IPfIdentityProvider identity, IPfUserRepository users)
    {
        _identity = identity;
        _users = users;
    }

    public async Task<PfUserSession> GetAsync()
    {
        if (_cached != null)
            return _cached;

        var userName = _identity.UserName?.Trim();
        if (string.IsNullOrEmpty(userName))
            throw new BaseException(PfErrorCodes.UnAuthorized, "کاربر احراز هویت نشده است.");

        var user = await _users.FindByUserNameAsync(userName)
                   ?? throw new BaseException(PfErrorCodes.PfUserNotFound,
                           $"کاربر '{userName}' در سامانه امکان تولید تعریف نشده است.")
                       .WithData("UserName", userName);

        var fullName = string.Join(" ", new[] { user.FirstName, user.LastName }.Where(n => !string.IsNullOrWhiteSpace(n)));
        _cached = new PfUserSession(user.Id, user.IkcoUserId, user.UserName, fullName, user.VirtualPlantId, user.IsAdmin == true);
        return _cached;
    }

    public async Task<int> GetIdAsync() => (await GetAsync()).Id;

    public async Task<int> GetPersonIdAsync()
    {
        if (_personId.HasValue)
            return _personId.Value;

        var userName = _identity.UserName?.Trim();
        if (string.IsNullOrEmpty(userName))
            throw new BaseException(PfErrorCodes.UnAuthorized, "کاربر احراز هویت نشده است.");

        var id = await _users.FindPersonIdAsync(userName)
                 ?? throw new BaseException(PfErrorCodes.PersonNotFound,
                         $"کاربر '{userName}' در جدول اشخاص کنترل تولید تعریف نشده است.")
                     .WithData("UserName", userName);
        _personId = id;
        return id;
    }
}
