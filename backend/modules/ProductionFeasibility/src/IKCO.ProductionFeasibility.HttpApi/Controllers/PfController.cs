using Volo.Abp.AspNetCore.Mvc;

namespace IKCO.ProductionFeasibility.Controllers;

/// <summary>پایه کنترلرهای سامانه ردیابی قطعات (کنترلرها نازک هستند و فقط سرویس کاربردی را فراخوانی می‌کنند).</summary>
public abstract class PfController : AbpControllerBase
{
}
