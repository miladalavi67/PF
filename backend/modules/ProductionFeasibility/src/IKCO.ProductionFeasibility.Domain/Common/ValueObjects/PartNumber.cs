using System.Collections.Generic;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Domain.Values;

namespace IKCO.ProductionFeasibility.Common.ValueObjects;

/// <summary>شماره فنی قطعه؛ برش فاصله‌های ابتدا/انتها و اعتبارسنجی غیرخالی بودن و حداکثر طول.</summary>
public class PartNumber : ValueObject
{
    public const int MaxLength = 50;

    public string Value { get; }

    public PartNumber(string? value)
    {
        var v = (value ?? string.Empty).Trim();
        if (v.Length == 0 || v.Length > MaxLength || HasControlCharacters(v))
            throw new BaseException(PfErrorCodes.PartNumberInvalid, "شماره فنی وارد شده معتبر نیست.")
                .WithData("Value", value ?? string.Empty);
        Value = v;
    }

    /// <summary>برای پارامترهای اختیاری شماره فنی: رشته خالی یا null به null تبدیل می‌شود</summary>
    public static string? OptionalValue(string? value) => string.IsNullOrWhiteSpace(value) ? null : new PartNumber(value).Value;

    private static bool HasControlCharacters(string v)
    {
        foreach (var ch in v)
            if (char.IsControl(ch))
                return true;
        return false;
    }

    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }

    public override string ToString() => Value;
}
