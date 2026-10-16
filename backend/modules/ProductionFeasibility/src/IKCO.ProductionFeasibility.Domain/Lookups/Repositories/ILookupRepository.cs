using System.Collections.Generic;
using System.Threading.Tasks;

namespace IKCO.ProductionFeasibility.Lookups.Repositories;

public sealed record LookupDefinition(string Key, string Title, string? ParentKey);

public sealed record LookupItem(int Id, string? Code, string Title);

public interface ILookupRepository
{
    IReadOnlyList<LookupDefinition> GetCatalog();

    /// <summary>ردیف‌های فهرست؛ کلید نامعتبر خطای <see cref="PfErrorCodes.LookupNotFound"/> می‌دهد</summary>
    Task<List<LookupItem>> GetAsync(string key, int? parentId);
}
