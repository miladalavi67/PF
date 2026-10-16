using System.Data;
using Dapper;

namespace IKCO.ProductionFeasibility.StoredProcedures;

/// <summary>
/// سازنده پارامترهای Dapper با نوع و طول صریح مطابق تعریف رویه در schema.md
/// (varchar → AnsiString، char → AnsiStringFixedLength، nvarchar → String، nchar → StringFixedLength).
/// مقدار null به صورت DBNull ارسال می‌شود.
/// </summary>
public sealed class SpParameters
{
    public DynamicParameters Dapper { get; } = new();

    public SpParameters Int(string name, int? value) => Add(name, value, DbType.Int32);

    public SpParameters SmallInt(string name, short? value) => Add(name, value, DbType.Int16);

    public SpParameters TinyInt(string name, byte? value) => Add(name, value, DbType.Byte);

    public SpParameters BigInt(string name, long? value) => Add(name, value, DbType.Int64);

    public SpParameters Bit(string name, bool? value) => Add(name, value, DbType.Boolean);

    public SpParameters DateTime(string name, System.DateTime? value) => Add(name, value, DbType.DateTime);

    public SpParameters Decimal(string name, decimal? value, byte precision, byte scale)
    {
        Dapper.Add(name, value, DbType.Decimal, ParameterDirection.Input, precision: precision, scale: scale);
        return this;
    }

    /// <summary>varchar(size)؛ برای varchar(max) مقدار -1</summary>
    public SpParameters VarChar(string name, string? value, int size) => Add(name, value, DbType.AnsiString, size);

    /// <summary>char(size)</summary>
    public SpParameters Char(string name, string? value, int size) => Add(name, value, DbType.AnsiStringFixedLength, size);

    /// <summary>nvarchar(size)؛ برای nvarchar(max) مقدار -1</summary>
    public SpParameters NVarChar(string name, string? value, int size) => Add(name, value, DbType.String, size);

    /// <summary>nchar(size)</summary>
    public SpParameters NChar(string name, string? value, int size) => Add(name, value, DbType.StringFixedLength, size);

    /// <summary>پارامتر جدولی (user-defined table type) ساخته‌شده با <see cref="TableValuedParameterFactory"/></summary>
    public SpParameters Table(string name, SqlMapper.ICustomQueryParameter value)
    {
        Dapper.Add(name, value);
        return this;
    }

    public SpParameters Output(string name, DbType dbType, int? size = null)
    {
        Dapper.Add(name, dbType: dbType, direction: ParameterDirection.Output, size: size);
        return this;
    }

    public T Get<T>(string name) => Dapper.Get<T>(name);

    private SpParameters Add(string name, object? value, DbType dbType, int? size = null)
    {
        Dapper.Add(name, value, dbType, ParameterDirection.Input, size);
        return this;
    }
}
