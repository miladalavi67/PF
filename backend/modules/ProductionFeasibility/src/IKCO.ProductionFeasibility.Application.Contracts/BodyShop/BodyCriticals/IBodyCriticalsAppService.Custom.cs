using System.Threading.Tasks;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Dtos;
using IKCO.ProductionFeasibility.Common;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

public partial interface IBodyCriticalsAppService
{
    /// <summary>ثبت کلاس‌های مصرف‌کننده یک بحران (در یک تراکنش)</summary>
    Task<GlobalResultDto> CreateProductsAsync(int criticalPartsId, BodyCriticalsCreateProductsInput input);

    /// <summary>ثبت مجوز تولید با کسری</summary>
    Task<GlobalResultDto> RegisterLicenseAsync(BodyCriticalsRegisterLicenseInput input);

    /// <summary>ثبت گروهی بحران‌ها از اکسل</summary>
    Task<TabularResult> BatchCreateAsync(BodyCriticalsBatchCreateInput input);

    /// <summary>زیرکلاس‌های مصرف‌کننده شماره فنی از SAP</summary>
    Task<ListResultDto<SapWhereUsedItemDto>> GetSapWhereUsedAsync(BodyCriticalsGetSapWhereUsedInput input);
}
