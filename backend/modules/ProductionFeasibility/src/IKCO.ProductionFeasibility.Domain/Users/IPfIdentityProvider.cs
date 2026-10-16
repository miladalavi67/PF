namespace IKCO.ProductionFeasibility.Users;

/// <summary>
/// هویت کاربر احراز هویت‌شده از توکن IKCO.Auth: <c>sub</c> (نام کاربری / شماره پرسنلی).
/// جایگزین <c>Ikco.Gate.Relation.Security.GetUserInfo()</c> سامانه ویندوزی.
/// </summary>
public interface IPfIdentityProvider
{
    string? UserName { get; }
}
