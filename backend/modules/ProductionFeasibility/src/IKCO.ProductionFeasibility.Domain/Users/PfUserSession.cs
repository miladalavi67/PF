namespace IKCO.ProductionFeasibility.Users;

/// <summary>
/// معادل <c>GlobalVariables.CurrentUser</c> سامانه ویندوزی امکان تولید برای درخواست جاری.
/// </summary>
public sealed class PfUserSession
{
    public PfUserSession(int id, int ikcoUserId, string userName, string fullName, int? virtualPlantId, bool isAdmin)
    {
        Id = id;
        IkcoUserId = ikcoUserId;
        UserName = userName;
        FullName = fullName;
        VirtualPlantId = virtualPlantId;
        IsAdmin = isAdmin;
    }

    /// <summary><c>PF.B_User.Id</c> — مقداری که به پارامترهای <c>@UserId</c> رویه‌های schema PF ارسال می‌شود</summary>
    public int Id { get; }

    /// <summary><c>PF.B_User.UserID</c> — شناسه کاربر در دریچه / Ikco.Gate</summary>
    public int IkcoUserId { get; }

    /// <summary>نام کاربری (شماره پرسنلی)</summary>
    public string UserName { get; }

    public string FullName { get; }

    /// <summary>سایت مجازی پیش‌فرض کاربر (<c>PF.B_User.VirtualPlantId</c>)</summary>
    public int? VirtualPlantId { get; }

    public bool IsAdmin { get; }
}
