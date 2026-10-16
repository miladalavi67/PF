using System;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility;

/// <summary>
/// پایه سرویس‌های کاربردی. سرویس‌ها DTO را به value object ترجمه و به مخزن/سرویس دامنه واگذار می‌کنند؛
/// قاعده کسب‌وکار در لایه دامنه یا رویه ذخیره‌شده است. فقط کنترلرهای صریح HttpApi منتشر می‌شوند و [Authorize] با کلیدهای IKCO
/// فقط روی کنترلرها قرار می‌گیرد (رهگیر ABP سیاست‌های پویای IKCO.Auth را نمی‌شناسد).
/// </summary>
[Volo.Abp.RemoteService(IsEnabled = false)]
public abstract class PfAppService : ApplicationService
{
    protected PfAppService()
    {
        ObjectMapperContext = typeof(PfApplicationModule);
    }

    /// <summary>
    /// نتیجه رویه‌های ثبت/ویرایش/حذف (ردیف <c>Result</c>/<c>Message</c>) را بررسی می‌کند. نتیجه منفی با پیام فارسی همان رویه
    /// به خطای منطقی 400 تبدیل می‌شود (در بک‌اند قبلی با کد 200 و StatusCode داخل بدنه برمی‌گشت).
    /// </summary>
    protected static GlobalResultDto EnsureSuccess(GlobalResultDto? result, string operation, Func<int, bool> isSuccess)
    {
        if (result == null)
            throw new BaseException(PfErrorCodes.OperationFailed, $"عملیات «{operation}» نتیجه‌ای برنگرداند.");
        if (!isSuccess(result.Result))
            throw new BaseException(PfErrorCodes.OperationFailed,
                    string.IsNullOrWhiteSpace(result.Message) ? $"عملیات «{operation}» انجام نشد." : result.Message!)
                .WithData("Result", result.Result);
        return result;
    }
}
