using Volo.Abp;

namespace IKCO.ProductionFeasibility.Exceptions;

/// <summary>
/// خطای منطقی استاندارد IKCO با کد عددی <see cref="PfErrorCodes"/>. پیام‌ها فارسی و برای نمایش به کاربر هستند
/// (سامانه قدیم پیام خطا را مستقیم به کاربر نشان می‌داد)؛ جزئیات فنی در پیام قرار نمی‌گیرد.
/// </summary>
public class BaseException : BusinessException, IUserFriendlyException
{
    public PfErrorCodes ErrorCode { get; }

    public BaseException(PfErrorCodes code, string message, string? details = null)
        : base(code.ToString(), message, details)
    {
        ErrorCode = code;
        WithData("NumericCode", (int)code);
        WithData("Message", message);
    }
}
