using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using IKCO.ProductionFeasibility.Mechanical.Results;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsAppService
{
    /// <summary>
    /// مانند <c>MechanicalCriticalPartsProductUpdate</c> بک‌اند قبلی برای هر محصول، با این تفاوت که همه در یک تراکنش‌اند و
    /// اولین نتیجه ناموفق رویه (Result=0 با پیام خطای رویه) کل ثبت را برمی‌گرداند.
    /// </summary>
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> SaveProductsAsync(int criticalPartsId, MechPartsSaveProductsInput input)
    {
        var personId = await _currentUser.GetPersonIdAsync();
        var products = input.Items
            .Select(i => new MechPartProduct(i.CarGroupId, i.CarModelId, i.CarClassId, i.SubClassId, i.ColorId, i.TrimId, i.OptionId, i.Quantity))
            .Distinct()
            .ToList();
        var results = await _repository.SaveProductsAsync(criticalPartsId, input.CarId, input.TrimShopId, input.PlantId, input.IsNew, products, personId);
        foreach (var result in results.Where(r => r != null))
            EnsureSuccess(result, "ثبت محصولات بحران مکانیکی", r => r > 0);
        return new GlobalResultDto { Result = results.Count, Message = "محصولات بحران با موفقیت ثبت شد." };
    }

    public async Task<ListResultDto<SubClassContrastRow>> CompareSubClassesAsync(MechPartsCompareSubClassesInput input)
    {
        var rows = input.Rows
            .Select(r => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
            {
                ["SubClassCode"] = r.SubClassCode.Trim(),
                ["PartNo"] = new PartNumber(r.PartNo).Value,
            })
            .ToList();
        return new ListResultDto<SubClassContrastRow>(await _repository.CompareSubClassesAsync(rows, input.SaloonId, input.Type));
    }
}
