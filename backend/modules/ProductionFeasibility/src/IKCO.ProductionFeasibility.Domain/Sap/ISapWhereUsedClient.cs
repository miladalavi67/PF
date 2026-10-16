using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Sap;

/// <summary>یک ردیف خروجی SAP: زیرکلاس مصرف‌کننده شماره فنی و مقدار مصرف</summary>
public sealed record SapWhereUsedItem(string SubClassCode, decimal? Quantity);

/// <summary>سرویس SAP «استفاده‌شده در» (ZMAT_WHERE_USED) برای یافتن زیرکلاس‌های مصرف‌کننده یک شماره فنی.</summary>
public interface ISapWhereUsedClient
{
    Task<IReadOnlyList<SapWhereUsedItem>> GetWhereUsedAsync(string partNo, string saloonCode, CancellationToken cancellationToken = default);
}
