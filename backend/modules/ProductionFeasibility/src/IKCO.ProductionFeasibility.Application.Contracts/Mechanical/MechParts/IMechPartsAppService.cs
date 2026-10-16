using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Dtos;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart)</summary>
public partial interface IMechPartsAppService : IApplicationService
{
    /// <summary>مشخصات بدنه برای ثبت بحران</summary>
    Task<TabularResult> GetBodyPropertiesAsync(MechPartsGetBodyPropertiesInput input);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی</summary>
    Task<string?> GetPartDescriptionAsync(MechPartsGetPartDescriptionInput input);

    /// <summary>اطلاعات قطعه در فهرست مرجع قطعات کسری</summary>
    Task<TabularResult> GetPartInShortageListAsync(MechPartsGetPartInShortageListInput input);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM)</summary>
    Task<TabularResult> GetPartProductsAsync(MechPartsGetPartProductsInput input);

    /// <summary>قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه</summary>
    Task<TabularResult> GetProductsByClassAndPartTypeAsync(MechPartsGetProductsByClassAndPartTypeInput input);

    /// <summary>بررسی وجود بحران فعال برای شماره فنی در سالن</summary>
    Task<TabularResult> ExistsPartNoAsync(MechPartsExistsPartNoInput input);

    /// <summary>محصولات شماره فنی از BOP</summary>
    Task<TabularResult> GetFromBopAsync(MechPartsGetFromBopInput input);

    /// <summary>محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها</summary>
    Task<TabularResult> GetFromSapAsync(MechPartsGetFromSapInput input);

    /// <summary>اطلاعات بحران برای ویرایش</summary>
    Task<TabularResult> GetForEditAsync(int id);

    /// <summary>جزئیات بحران</summary>
    Task<TabularResult> GetByIdAsync(int id);

    /// <summary>محصولات بحران برای ویرایش</summary>
    Task<TabularResult> GetProductsForEditAsync(int id, MechPartsGetProductsForEditInput input);

    /// <summary>محصولات ثبت‌شده یک بحران</summary>
    Task<TabularResult> GetProductsAsync(int criticalPartId);

    /// <summary>ثبت بحران مکانیکی</summary>
    Task<int> CreateAsync(MechPartsCreateInput input);

    /// <summary>ویرایش بحران مکانیکی</summary>
    Task UpdateAsync(int id, MechPartsUpdateInput input);

    /// <summary>ثبت/به‌روزرسانی یک محصول بحران</summary>
    Task SaveProductAsync(int criticalPartsId, MechPartsSaveProductInput input);

    /// <summary>افزودن زیرکلاس مغایر به بحران</summary>
    Task AddSubClassAsync(MechPartsAddSubClassInput input);

    /// <summary>حذف محصولات بحران مکانیکی</summary>
    Task DeleteAsync(int id);

}
