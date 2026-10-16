using System.Collections.Generic;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Domain.Values;

namespace IKCO.ProductionFeasibility.Common.ValueObjects;

/// <summary>شماره بدنه (حداکثر ۸ کاراکتر)؛ حروف بزرگ/کوچک مانند سامانه قدیم دست‌نخورده می‌ماند.</summary>
public class BodyNumber : ValueObject
{
    public string Value { get; }

    public BodyNumber(string? value)
    {
        var v = (value ?? string.Empty).Trim();
        if (v.Length == 0 || v.Length > PfConsts.BodyNoLength)
            throw new BaseException(PfErrorCodes.BodyNumberInvalid, "شماره بدنه وارد شده معتبر نیست.")
                .WithData("Value", value ?? string.Empty);
        Value = v;
    }

    /// <summary>برای پارامترهای اختیاری شماره بدنه</summary>
    public static string? OptionalValue(string? value) => string.IsNullOrWhiteSpace(value) ? null : new BodyNumber(value).Value;

    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }

    public override string ToString() => Value;
}
