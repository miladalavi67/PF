using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// تبدیل ردیف‌های Dapper (<c>QueryAsync</c> بدون نوع) به مدل‌های <see cref="PfResultRow"/>.
/// نگاشت بر اساس نام ستون (<see cref="JsonPropertyNameAttribute"/> یا نام خصوصیت، بدون حساسیت به حروف) است. تبدیل نوع
/// مدارا دارد: رویه‌های قدیمی گاهی در شاخه‌های IF مختلف یک ستون را عددی و گاهی متنی ('-1') برمی‌گردانند؛ مقداری که به نوع
/// مدل تبدیل نشود همراه ستون‌های تعریف‌نشده در <see cref="PfResultRow.ExtraColumns"/> حفظ می‌شود تا هیچ داده‌ای از دست نرود.
/// </summary>
public static class PfResultRowReader
{
    private sealed record Member(PropertyInfo Property, string Column, Type Target);

    private static readonly ConcurrentDictionary<Type, Member[]> Members = new();

    public static List<T> Map<T>(IEnumerable<object> rows) where T : PfResultRow, new()
    {
        var members = Members.GetOrAdd(typeof(T), Describe);
        var result = new List<T>();
        foreach (var raw in rows)
        {
            var values = new Dictionary<string, object?>((IDictionary<string, object?>)raw, StringComparer.OrdinalIgnoreCase);
            var item = new T();
            foreach (var member in members)
            {
                if (!values.TryGetValue(member.Column, out var value))
                    continue;
                if (TryConvert(value, member.Target, out var converted))
                {
                    member.Property.SetValue(item, converted);
                    values.Remove(member.Column);
                }
            }

            if (values.Count > 0)
                item.ExtraColumns = values.ToDictionary(v => v.Key, v => v.Value is DBNull ? null : v.Value);
            result.Add(item);
        }

        return result;
    }

    private static Member[] Describe(Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite && p.Name != nameof(PfResultRow.ExtraColumns))
            .Select(p => new Member(p, p.GetCustomAttribute<JsonPropertyNameAttribute>()?.Name ?? p.Name,
                Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType))
            .ToArray();

    private static bool TryConvert(object? value, Type target, out object? converted)
    {
        converted = null;
        if (value is null or DBNull)
            return true;
        if (target.IsInstanceOfType(value))
        {
            converted = value;
            return true;
        }

        if (target == typeof(string))
        {
            converted = value is DateTime d ? d.ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture) : Convert.ToString(value, CultureInfo.InvariantCulture);
            return true;
        }

        if (value is string s)
        {
            s = s.Trim();
            if (s.Length == 0)
                return true;
            value = s;
        }

        try
        {
            converted = target == typeof(bool) && value is string text
                ? text is "1" || bool.Parse(text)
                : Convert.ChangeType(value, target, CultureInfo.InvariantCulture);
            return true;
        }
        catch (Exception e) when (e is FormatException or InvalidCastException or OverflowException)
        {
            return false;
        }
    }
}
