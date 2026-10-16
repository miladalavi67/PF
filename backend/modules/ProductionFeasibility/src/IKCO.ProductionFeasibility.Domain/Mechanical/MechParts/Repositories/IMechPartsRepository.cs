using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechPartsRepository
{
    /// <summary>مشخصات بدنه برای ثبت بحران (frmCriticalPart (شماره بدنه)) — <c>dbo.S_CarPropertiesSelectForCriticalParts</c></summary>
    Task<TabularResult> GetBodyPropertiesAsync(string bodyNo, byte plantId, byte? mode);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی (frmCriticalPart (شماره فنی)) — <c>dbo.SIMP_SelectPartDescByPartNo</c></summary>
    Task<string?> GetPartDescriptionAsync(string partNo);

    /// <summary>اطلاعات قطعه در فهرست مرجع قطعات کسری (frmCriticalPart) — <c>dbo.SIMP_SelectParInShortagePartList</c></summary>
    Task<TabularResult> GetPartInShortageListAsync(string partNo);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM) (frmCriticalPart (محصولات)) — <c>dbo.SIMP_SelectProductsUsePartNo</c></summary>
    Task<TabularResult> GetPartProductsAsync(string partNo);

    /// <summary>قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه (frmCriticalPartSetBasis / frmCriticalPart) — <c>dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType</c></summary>
    Task<TabularResult> GetProductsByClassAndPartTypeAsync(string carClassCode, byte partTypeCode);

    /// <summary>بررسی وجود بحران فعال برای شماره فنی در سالن (frmCriticalPart) — <c>dbo.S_CriticalPartsExistPartNo</c></summary>
    Task<TabularResult> ExistsPartNoAsync(string partNo, int trimShopId);

    /// <summary>محصولات شماره فنی از BOP (frmCriticalPart) — <c>dbo.S_CriticalPartsSelectFromBOP</c></summary>
    Task<TabularResult> GetFromBopAsync(string partNo, int trimShopId);

    /// <summary>محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها (frmCriticalPart) — <c>dbo.S_CriticalPartsSelectFromSAP</c></summary>
    Task<TabularResult> GetFromSapAsync(string subClassCode, string partNo);

    /// <summary>اطلاعات بحران برای ویرایش (frmCriticalPart (ویرایش)) — <c>dbo.S_SelectCriticalPartForEdit</c></summary>
    Task<TabularResult> GetForEditAsync(int id);

    /// <summary>جزئیات بحران (frmCriticalPartSearch) — <c>dbo.S_CriticalPartsSelectById</c></summary>
    Task<TabularResult> GetByIdAsync(int id);

    /// <summary>محصولات بحران برای ویرایش (frmCriticalPart (ویرایش محصولات)) — <c>dbo.S_CriticalPartProductsSelectForEdit_V1</c></summary>
    Task<TabularResult> GetProductsForEditAsync(int id, int? shopTrimId);

    /// <summary>محصولات ثبت‌شده یک بحران (frmCriticalPartSearch (جزئیات)) — <c>dbo.S_CriticalPartProductsSelectByCriticalPartId_V2</c></summary>
    Task<TabularResult> GetProductsAsync(int criticalPartId);

    /// <summary>ثبت بحران مکانیکی (frmCriticalPart.btnOk) — <c>dbo.S_CriticalPartsINSERT</c></summary>
    Task<int?> CreateAsync(int carId, string partNo, string partDesc, byte plantId, string partGroup, string? alterNativePartNo, string? criticalPartNote, int stock, int used, int possibleProductionNo, int trimShopId, int usageFactor, int deptId, int userId, bool? isOutOfSap);

    /// <summary>ویرایش بحران مکانیکی (frmCriticalPart (ویرایش)) — <c>dbo.S_CriticalPartsUpdate</c></summary>
    Task UpdateAsync(int id, int stock, int used, string usageFactor, string? alterNativePartNo, string? criticalPartNote, string? productionStateLetterNo, string? productionStateNote, string partGroup, int productionPossibility, int userId, bool? isOutOfSap);

    /// <summary>ثبت/به‌روزرسانی یک محصول بحران (frmCriticalPart (محصولات)) — <c>dbo.S_CriticalPartsPruductUpdate_H_V1</c></summary>
    Task SaveProductAsync(int carId, int criticalPartsId, int carGroupId, int carModelId, int carClassId, int subClassId, int colorId, int? trimId, int optionId, int userId, int isNew, int trimShopId, short plantId, byte quantity);

    /// <summary>افزودن زیرکلاس مغایر به بحران (frmRPTContrastOfSubClasses (افزودن)) — <c>dbo.S_AddContrastOfSubClasses</c></summary>
    Task AddSubClassAsync(int criticalPartsId, string subClassCode, int userId, int trimShopId);

    /// <summary>حذف محصولات بحران مکانیکی (frmCriticalPartSearch (حذف)) — <c>dbo.S_CriticalPartDelete</c></summary>
    Task DeleteAsync(int id);

}
