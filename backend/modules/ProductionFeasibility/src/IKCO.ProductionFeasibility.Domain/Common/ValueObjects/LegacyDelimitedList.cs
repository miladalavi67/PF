using System;
using System.Collections.Generic;
using System.Linq;
using IKCO.ProductionFeasibility.Exceptions;
using Volo.Abp.Domain.Values;

namespace IKCO.ProductionFeasibility.Common.ValueObjects;

/// <summary>
/// فهرست رشته‌ای که رویه‌های ذخیره‌شده قدیمی به‌صورت یک پارامتر دریافت می‌کنند.
/// قالب دقیقاً مطابق سامانه قدیم ساخته می‌شود: هر عضو + جداکننده (فاصله یا ویرگول انتهایی باقی می‌ماند)،
/// مثلاً <c>"12 13 "</c> برای ایستگاه‌ها یا <c>"5,7,"</c> برای S_DisableStations.
/// </summary>
public class LegacyDelimitedList : ValueObject
{
    public const char Space = ' ';
    public const char Comma = ',';

    public char Separator { get; }
    public IReadOnlyList<string> Items { get; }

    private LegacyDelimitedList(char separator, IReadOnlyList<string> items)
    {
        Separator = separator;
        Items = items;
    }

    public static LegacyDelimitedList SpaceSeparated(IEnumerable<string>? items) => Create(Space, items);

    public static LegacyDelimitedList CommaSeparated(IEnumerable<string>? items) => Create(Comma, items);

    public static LegacyDelimitedList SpaceSeparated(IEnumerable<int>? ids) => Create(Space, ids?.Select(i => i.ToString()));

    public static LegacyDelimitedList CommaSeparated(IEnumerable<int>? ids) => Create(Comma, ids?.Select(i => i.ToString()));

    private static LegacyDelimitedList Create(char separator, IEnumerable<string>? items)
    {
        var list = (items ?? Array.Empty<string>()).Select(i => i?.Trim() ?? string.Empty).ToList();
        if (list.Any(i => i.Length == 0 || i.IndexOf(Space) >= 0 || i.IndexOf(Comma) >= 0))
            throw new BaseException(PfErrorCodes.IdListInvalid, "اعضای فهرست نباید خالی باشند یا شامل فاصله و ویرگول باشند.");
        return new LegacyDelimitedList(separator, list);
    }

    public bool IsEmpty => Items.Count == 0;

    /// <summary>رشته ارسالی به رویه، با جداکننده انتهایی مانند سامانه قدیم</summary>
    public string ToLegacyString() => string.Concat(Items.Select(i => i + Separator));

    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Separator;
        foreach (var item in Items)
            yield return item;
    }

    public override string ToString() => ToLegacyString();
}
