using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart)</summary>
public partial interface IMechPartsAppService : IApplicationService
{
    /// <summary>مشخصات بدنه برای ثبت بحران</summary>
    Task<ListResultDto<CarPropertiesRow>> GetBodyPropertiesAsync(MechPartsGetBodyPropertiesInput input);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی</summary>
    Task<string?> GetPartDescriptionAsync(MechPartsGetPartDescriptionInput input);

    /// <summary>اطلاعات قطعه در فهرست مرجع قطعات کسری</summary>
    Task<ListResultDto<PartInShortageListRow>> GetPartInShortageListAsync(MechPartsGetPartInShortageListInput input);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM)</summary>
    Task<ListResultDto<BomProductRow>> GetPartProductsAsync(MechPartsGetPartProductsInput input);

    /// <summary>قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه</summary>
    Task<ListResultDto<BomProductRow>> GetProductsByClassAndPartTypeAsync(MechPartsGetProductsByClassAndPartTypeInput input);

    /// <summary>بررسی وجود بحران فعال برای شماره فنی در سالن</summary>
    Task<ListResultDto<CriticalPartExistRow>> ExistsPartNoAsync(MechPartsExistsPartNoInput input);

    /// <summary>محصولات شماره فنی از BOP</summary>
    Task<ListResultDto<CriticalProductCandidateRow>> GetFromBopAsync(MechPartsGetFromBopInput input);

    /// <summary>محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها</summary>
    Task<ListResultDto<CriticalProductCandidateRow>> GetFromSapAsync(MechPartsGetFromSapInput input);

    /// <summary>اطلاعات بحران برای ویرایش</summary>
    Task<ListResultDto<CriticalPartForEditRow>> GetForEditAsync(int id);

    /// <summary>جزئیات بحران</summary>
    Task<ListResultDto<CriticalPartRow>> GetByIdAsync(int id);

    /// <summary>محصولات بحران برای ویرایش</summary>
    Task<ListResultDto<CriticalPartProductForEditRow>> GetProductsForEditAsync(int id, MechPartsGetProductsForEditInput input);

    /// <summary>محصولات ثبت‌شده یک بحران</summary>
    Task<ListResultDto<CriticalPartProductRow>> GetProductsAsync(int criticalPartId);

    /// <summary>ثبت بحران مکانیکی</summary>
    Task<int> CreateAsync(MechPartsCreateInput input);

    /// <summary>ویرایش بحران مکانیکی</summary>
    Task UpdateAsync(int id, MechPartsUpdateInput input);

    /// <summary>ثبت/به‌روزرسانی یک محصول بحران</summary>
    Task<GlobalResultDto> SaveProductAsync(int criticalPartsId, MechPartsSaveProductInput input);

    /// <summary>افزودن زیرکلاس مغایر به بحران</summary>
    Task AddSubClassAsync(MechPartsAddSubClassInput input);

    /// <summary>حذف بحران مکانیکی (به همراه استثناها و مجوزهای آن)</summary>
    Task<GlobalResultDto> DeleteAsync(int id);

}
