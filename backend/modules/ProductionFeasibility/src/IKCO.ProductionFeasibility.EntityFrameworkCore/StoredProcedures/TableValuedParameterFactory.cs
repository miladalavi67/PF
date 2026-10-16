using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IKCO.ProductionFeasibility.Exceptions;

namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// ساخت پارامتر جدولی برای user-defined table type ها. سامانه قدیم یک <c>DataTable</c> را با ترتیب خصوصیت‌های DTO می‌فرستاد؛
/// تعریف نوع جدولی در sfcDB.md موجود نیست، بنابراین نام نوع از <c>sys.parameters</c> رویه و ستون‌ها از <c>sys.columns</c> هنگام اجرا
/// خوانده می‌شوند و مقادیر بر اساس نام ستون (و با نوع CLR متناظر) قرار می‌گیرند.
/// </summary>
public static class TableValuedParameterFactory
{
    private const string TypeOfParameterSql = @"SELECT SCHEMA_NAME(tt.schema_id) AS SchemaName, tt.name AS TypeName
FROM sys.parameters p
INNER JOIN sys.table_types tt ON tt.user_type_id = p.user_type_id
WHERE p.object_id = OBJECT_ID(@Procedure) AND p.name = @Parameter";

    private const string ColumnsSql = @"SELECT c.name AS Name, TYPE_NAME(c.system_type_id) AS TypeName, c.is_nullable AS IsNullable
FROM sys.table_types tt
INNER JOIN sys.columns c ON c.object_id = tt.type_table_object_id
WHERE tt.name = @TypeName AND SCHEMA_NAME(tt.schema_id) = @Schema
ORDER BY c.column_id";

    private sealed class TypeRef
    {
        public string SchemaName { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
    }

    private sealed class ColumnInfo
    {
        public string Name { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public bool IsNullable { get; set; }
    }

    /// <summary>پارامتر جدولی رویه: نوع جدولی پارامتر <paramref name="parameterName"/> (مثلاً <c>@Table</c>) کشف می‌شود</summary>
    public static async Task<SqlMapper.ICustomQueryParameter> CreateForProcedureAsync(
        DbConnection connection, DbTransaction? transaction, string procedure, string parameterName,
        IEnumerable<IReadOnlyDictionary<string, object?>> rows)
    {
        var type = (await connection.QueryAsync<TypeRef>(TypeOfParameterSql,
                new { Procedure = procedure, Parameter = parameterName.StartsWith('@') ? parameterName : "@" + parameterName }, transaction))
            .FirstOrDefault()
            ?? throw new BaseException(PfErrorCodes.OperationFailed,
                $"نوع جدولی پارامتر {parameterName} رویه {procedure} در پایگاه داده یافت نشد.");
        return await CreateByNameAsync(connection, transaction, type.SchemaName, type.TypeName, rows);
    }

    /// <summary>پارامتر جدولی از یک نوع جدولی مشخص؛ ستون‌ها بر اساس نام (بدون حساسیت به حروف) پر می‌شوند</summary>
    public static async Task<SqlMapper.ICustomQueryParameter> CreateByNameAsync(
        DbConnection connection, DbTransaction? transaction, string schema, string typeName,
        IEnumerable<IReadOnlyDictionary<string, object?>> rows)
    {
        var columns = (await connection.QueryAsync<ColumnInfo>(ColumnsSql, new { TypeName = typeName, Schema = schema }, transaction)).ToList();
        if (columns.Count == 0)
            throw new BaseException(PfErrorCodes.OperationFailed, $"نوع جدولی {schema}.{typeName} در پایگاه داده یافت نشد.");

        var table = new DataTable();
        foreach (var column in columns)
            table.Columns.Add(column.Name, ClrType(column.TypeName));

        var count = 0;
        foreach (var row in rows)
        {
            if (++count > PfConsts.MaxBatchRows)
                throw new BaseException(PfErrorCodes.BatchInputInvalid, $"تعداد ردیف‌های ورودی بیش از {PfConsts.MaxBatchRows} است.");

            var lookup = new Dictionary<string, object?>(row, StringComparer.OrdinalIgnoreCase);
            var values = new object[columns.Count];
            for (var i = 0; i < columns.Count; i++)
            {
                lookup.TryGetValue(columns[i].Name, out var raw);
                if (raw == null || raw is DBNull)
                {
                    if (!columns[i].IsNullable)
                        throw new BaseException(PfErrorCodes.BatchInputInvalid,
                            $"مقدار ستون «{columns[i].Name}» در ردیف {count} الزامی است.");
                    values[i] = DBNull.Value;
                    continue;
                }

                values[i] = Convert(raw, table.Columns[i].DataType, columns[i].Name, count);
            }

            table.Rows.Add(values);
        }

        return table.AsTableValuedParameter($"{schema}.{typeName}");
    }

    private static Type ClrType(string sqlType) => sqlType.ToLowerInvariant() switch
    {
        "int" => typeof(int),
        "smallint" => typeof(short),
        "tinyint" => typeof(byte),
        "bigint" => typeof(long),
        "bit" => typeof(bool),
        "datetime" or "smalldatetime" or "date" or "datetime2" => typeof(DateTime),
        "decimal" or "numeric" or "money" or "smallmoney" => typeof(decimal),
        "float" => typeof(double),
        "real" => typeof(float),
        "uniqueidentifier" => typeof(Guid),
        _ => typeof(string),
    };

    private static object Convert(object value, Type target, string column, int rowNo)
    {
        try
        {
            if (target == typeof(string))
                return System.Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty;
            if (target == typeof(bool) && value is string s)
                return s is "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);
            if (target == typeof(Guid))
                return Guid.Parse(System.Convert.ToString(value, CultureInfo.InvariantCulture)!);
            return System.Convert.ChangeType(value, target, CultureInfo.InvariantCulture)!;
        }
        catch (Exception ex) when (ex is FormatException or InvalidCastException or OverflowException)
        {
            throw new BaseException(PfErrorCodes.BatchInputInvalid, $"مقدار ستون «{column}» در ردیف {rowNo} معتبر نیست.");
        }
    }
}
