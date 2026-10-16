namespace IKCO.ProductionFeasibility.EntityFrameworkCore;

public static class PfDbProperties
{
    /// <summary>schema رویه‌ها و جداول امکان تولید در پایگاه داده ProductionControl (رویه‌های کنترل تولید در dbo هستند)</summary>
    public const string DbSchema = "PF";

    /// <summary>پایگاه داده اصلی ProductionControl (کلید <c>ConnectionStrings:Default</c>)</summary>
    public const string ConnectionStringName = "Default";
}
