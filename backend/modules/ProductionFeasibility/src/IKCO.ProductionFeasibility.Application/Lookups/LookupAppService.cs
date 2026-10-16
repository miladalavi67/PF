using System;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Lookups.Dtos;
using IKCO.ProductionFeasibility.Lookups.Repositories;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Lookups;

public class LookupAppService : PfAppService, ILookupAppService
{
    private readonly ILookupRepository _lookups;

    public LookupAppService(ILookupRepository lookups)
    {
        _lookups = lookups;
    }

    public Task<ListResultDto<LookupDescriptor>> GetCatalogAsync()
        => Task.FromResult(new ListResultDto<LookupDescriptor>(_lookups.GetCatalog()
            .Select(d => new LookupDescriptor { Key = d.Key, Title = d.Title, ParentKey = d.ParentKey }).ToList()));

    public async Task<ListResultDto<LookupRow>> GetAsync(string key, GetLookupInput input)
    {
        var items = await _lookups.GetAsync(key, input.ParentId);
        var query = items.AsEnumerable();
        var search = input.Search?.Trim();
        if (!string.IsNullOrEmpty(search))
            query = query.Where(i => i.Title.Contains(search, StringComparison.OrdinalIgnoreCase)
                                     || (i.Code?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false));
        return new ListResultDto<LookupRow>(query.Select(i => new LookupRow { Id = i.Id, Code = i.Code, Title = i.Title }).ToList());
    }
}
