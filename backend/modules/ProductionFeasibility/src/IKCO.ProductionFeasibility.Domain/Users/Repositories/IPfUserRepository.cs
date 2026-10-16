using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Users.Repositories;

/// <summary>ردیف <c>PF.B_User</c> (بدون رمز و اطلاعات حساس)</summary>
public sealed record PfUserRecord(int Id, int IkcoUserId, string UserName, string? FirstName, string? LastName, bool? IsAdmin, int? VirtualPlantId);

/// <summary>خواندن کاربر امکان تولید و شخص کنترل تولید بر اساس نام کاربری توکن.</summary>
public interface IPfUserRepository
{
    /// <summary><c>PF.B_User</c> با <c>UserName</c> برابر نام کاربری توکن</summary>
    Task<PfUserRecord?> FindByUserNameAsync(string userName);

    /// <summary><c>dbo.B_Person.Id</c> با <c>PersonelCode</c> یا <c>UserName</c> برابر نام کاربری توکن؛ در نبودن null</summary>
    Task<int?> FindPersonIdAsync(string userName);
}
