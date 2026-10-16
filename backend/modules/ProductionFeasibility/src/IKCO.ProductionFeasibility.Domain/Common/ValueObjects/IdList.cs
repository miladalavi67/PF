using System.Collections.Generic;
using System.Linq;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Domain.Values;

namespace IKCO.ProductionFeasibility.Common.ValueObjects;

/// <summary>
/// فهرست شناسه‌های عددی که رویه‌های ذخیره‌شده به‌صورت رشته جداشده با ویرگول دریافت می‌کنند (بدون ویرگول انتهایی).
/// شناسه تکراری حذف و ترتیب ورودی حفظ می‌شود.
/// </summary>
public class IdList : ValueObject
{
    public IReadOnlyList<int> Ids { get; }

    public IdList(IEnumerable<int>? ids)
    {
        var list = (ids ?? Enumerable.Empty<int>()).Distinct().ToList();
        if (list.Count == 0 || list.Any(i => i <= 0) || list.Count > PfConsts.MaxBatchRows)
            throw new BaseException(PfErrorCodes.IdListInvalid, "فهرست شناسه‌ها خالی یا نامعتبر است.");
        Ids = list;
    }

    public string ToCommaSeparated() => string.Join(",", Ids);

    protected override IEnumerable<object> GetAtomicValues()
    {
        foreach (var id in Ids)
            yield return id;
    }

    public override string ToString() => ToCommaSeparated();
}
