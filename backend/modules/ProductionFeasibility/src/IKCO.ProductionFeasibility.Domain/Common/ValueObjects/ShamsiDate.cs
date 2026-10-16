using System.Collections.Generic;
using System.Text.RegularExpressions;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Domain.Values;

namespace IKCO.ProductionFeasibility.Common.ValueObjects;

/// <summary>تاریخ شمسی رشته‌ای با قالب yyyy/MM/dd که رویه‌های PF دریافت می‌کنند.</summary>
public class ShamsiDate : ValueObject
{
    private static readonly Regex Pattern = new(@"^1[3-5]\d{2}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01])$", RegexOptions.Compiled);

    public string Value { get; }

    public ShamsiDate(string? value)
    {
        var v = (value ?? string.Empty).Trim();
        if (!Pattern.IsMatch(v))
            throw new BaseException(PfErrorCodes.ShamsiDateInvalid, $"تاریخ '{value}' معتبر نیست؛ قالب صحیح yyyy/MM/dd است.")
                .WithData("Value", value ?? string.Empty);
        Value = v;
    }

    /// <summary>برای پارامترهای اختیاری تاریخ: رشته خالی یا null بدون اعتبارسنجی null برمی‌گرداند</summary>
    public static string? OptionalValue(string? value) => string.IsNullOrWhiteSpace(value) ? null : new ShamsiDate(value).Value;

    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }

    public override string ToString() => Value;
}
