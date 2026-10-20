using System;
using System.Text.Json.Serialization;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.UserManagement.Results;

/// <summary>
/// خطوط بدنه‌سازی یک سایت (PF.GetSaloonByVirtualPlantId)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BiwSiteRow : PfResultRow
{
    /// <summary>شناسه خط</summary>
    [JsonPropertyName("BiwSiteId")]
    public int? BiwSiteId { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSiteDescF")]
    public string? BIWSiteDescF { get; set; }
}

/// <summary>
/// کاربران امکان تولید (PF.GetAllUsers)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class PfUserRow : PfResultRow
{
    /// <summary>نام و نام خانوادگی</summary>
    [JsonPropertyName("FullName")]
    public string? FullName { get; set; }

    /// <summary>شناسه کاربر</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }
}

/// <summary>
/// دسترسی کاربران به خطوط (PF.GetUsersAccessList)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class UserAccessRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>نام</summary>
    [JsonPropertyName("FirstName")]
    public string? FirstName { get; set; }

    /// <summary>نام خانوادگی</summary>
    [JsonPropertyName("LastName")]
    public string? LastName { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserName")]
    public string? UserName { get; set; }

    /// <summary>سایت</summary>
    [JsonPropertyName("VirtualPlantDescF")]
    public string? VirtualPlantDescF { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSiteDescF")]
    public string? BIWSiteDescF { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>ثبت‌کننده</summary>
    [JsonPropertyName("CreatorUser")]
    public string? CreatorUser { get; set; }
}

/// <summary>
/// سایت‌های مجازی (PF.GetSiteByDomainId)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class VirtualPlantRow : PfResultRow
{
    /// <summary>شناسه سایت</summary>
    [JsonPropertyName("VirtualPlantId")]
    public int? VirtualPlantId { get; set; }

    /// <summary>سایت</summary>
    [JsonPropertyName("VirtualPlantDescF")]
    public string? VirtualPlantDescF { get; set; }
}
