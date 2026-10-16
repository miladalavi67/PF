namespace IKCO.ProductionFeasibility.Users.Dtos;

/// <summary>کاربر جاری در PF.B_User (بدون رمز و اطلاعات حساس)</summary>
public class PfUserInfoDto
{
    /// <summary>PF.B_User.Id</summary>
    public int Id { get; set; }

    /// <summary>شناسه کاربر در دریچه (B_User.UserID)</summary>
    public int IkcoUserId { get; set; }

    /// <summary>نام کاربری (شماره پرسنلی)</summary>
    public string UserName { get; set; } = default!;

    /// <summary>نام و نام خانوادگی</summary>
    public string FullName { get; set; } = default!;

    /// <summary>مدیر سامانه</summary>
    public bool IsAdmin { get; set; }

    /// <summary>سایت مجازی پیش‌فرض</summary>
    public int? VirtualPlantId { get; set; }
}
