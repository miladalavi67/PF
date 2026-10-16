using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsAppService
{
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> SaveProductsAsync(int criticalPartsId, MechPartsSaveProductsInput input)
    {
        var personId = await _currentUser.GetPersonIdAsync();
        var products = input.Items
            .Select(i => new MechPartProduct(i.CarGroupId, i.CarModelId, i.CarClassId, i.SubClassId, i.ColorId, i.TrimId, i.OptionId, i.Quantity))
            .Distinct()
            .ToList();
        var count = await _repository.SaveProductsAsync(criticalPartsId, input.CarId, input.TrimShopId, input.PlantId, input.IsNew, products, personId);
        return new GlobalResultDto { Result = count, Message = "محصولات بحران با موفقیت ثبت شد." };
    }

    public Task<TabularResult> CompareSubClassesAsync(MechPartsCompareSubClassesInput input)
    {
        var rows = input.Rows
            .Select(r => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["SubClassCode"] = r.SubClassCode.Trim(),
                ["PartNo"] = new PartNumber(r.PartNo).Value,
            })
            .ToList();
        return _repository.CompareSubClassesAsync(rows, input.SaloonId, input.Type);
    }
}
