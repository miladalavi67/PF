using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.Results;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;

/// <summary>ثبت و ویرایش بحران مکانیکی (frmCriticalPart) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IMechPartsRepository
{
    /// <summary>مشخصات بدنه برای ثبت بحران (frmCriticalPart (شماره بدنه)) — <c>dbo.S_CarPropertiesSelectForCriticalParts</c></summary>
    Task<List<CarPropertiesRow>> GetBodyPropertiesAsync(string bodyNo, byte plantId, byte? mode);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی (frmCriticalPart (شماره فنی)) — <c>dbo.SIMP_SelectPartDescByPartNo</c></summary>
    Task<string?> GetPartDescriptionAsync(string partNo);

    /// <summary>اطلاعات قطعه در فهرست مرجع قطعات کسری (frmCriticalPart) — <c>dbo.SIMP_SelectParInShortagePartList</c></summary>
    Task<List<PartInShortageListRow>> GetPartInShortageListAsync(string partNo);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM) (frmCriticalPart (محصولات)) — <c>dbo.SIMP_SelectProductsUsePartNo</c></summary>
    Task<List<BomProductRow>> GetPartProductsAsync(string partNo);

    /// <summary>قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه (frmCriticalPartSetBasis / frmCriticalPart) — <c>dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType</c></summary>
    Task<List<BomProductRow>> GetProductsByClassAndPartTypeAsync(string carClassCode, byte partTypeCode);

    /// <summary>بررسی وجود بحران فعال برای شماره فنی در سالن (frmCriticalPart) — <c>dbo.S_CriticalPartsExistPartNo</c></summary>
    Task<List<CriticalPartExistRow>> ExistsPartNoAsync(string partNo, int trimShopId);

    /// <summary>محصولات شماره فنی از BOP (frmCriticalPart) — <c>dbo.S_CriticalPartsSelectFromBOP</c></summary>
    Task<List<CriticalProductCandidateRow>> GetFromBopAsync(string partNo, int trimShopId);

    /// <summary>محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها (frmCriticalPart) — <c>dbo.S_CriticalPartsSelectFromSAP</c></summary>
    Task<List<CriticalProductCandidateRow>> GetFromSapAsync(string subClassCode, string partNo);

    /// <summary>اطلاعات بحران برای ویرایش (frmCriticalPart (ویرایش)) — <c>dbo.S_SelectCriticalPartForEdit</c></summary>
    Task<List<CriticalPartForEditRow>> GetForEditAsync(int id);

    /// <summary>جزئیات بحران (frmCriticalPartSearch) — <c>dbo.S_CriticalPartsSelectById</c></summary>
    Task<List<CriticalPartRow>> GetByIdAsync(int id);

    /// <summary>محصولات بحران برای ویرایش (frmCriticalPart (ویرایش محصولات)) — <c>dbo.S_CriticalPartProductsSelectForEdit_V1</c></summary>
    Task<List<CriticalPartProductForEditRow>> GetProductsForEditAsync(int id, int? shopTrimId);

    /// <summary>محصولات ثبت‌شده یک بحران (frmCriticalPartSearch (جزئیات)) — <c>dbo.S_CriticalPartProductsSelectByCriticalPartId_V2</c></summary>
    Task<List<CriticalPartProductRow>> GetProductsAsync(int criticalPartId);

    /// <summary>ثبت بحران مکانیکی (frmCriticalPart.btnOk) — <c>dbo.S_CriticalPartsINSERT</c></summary>
    Task<int?> CreateAsync(int carId, string partNo, string partDesc, byte plantId, string partGroup, string? alterNativePartNo, string? criticalPartNote, int stock, int used, int possibleProductionNo, int trimShopId, int usageFactor, int deptId, int userId, bool? isOutOfSap);

    /// <summary>ویرایش بحران مکانیکی (frmCriticalPart (ویرایش)) — <c>dbo.S_CriticalPartsUpdate</c></summary>
    Task UpdateAsync(int id, int stock, int used, string usageFactor, string? alterNativePartNo, string? criticalPartNote, string? productionStateLetterNo, string? productionStateNote, string partGroup, int productionPossibility, int userId, bool? isOutOfSap);

    /// <summary>ثبت/به‌روزرسانی یک محصول بحران (MechanicalParts/MechanicalCriticalPartsProductUpdate (بک‌اند قبلی) / frmCriticalPart (محصولات)) — <c>dbo.S_CriticalPartsPruductUpdate_Web</c></summary>
    Task<GlobalResultDto?> SaveProductAsync(int carId, int criticalPartsId, int carGroupId, int carModelId, int carClassId, int subClassId, int colorId, int? trimId, int optionId, int userId, int isNew, int trimShopId, short plantId, byte quantity);

    /// <summary>افزودن زیرکلاس مغایر به بحران (frmRPTContrastOfSubClasses (افزودن)) — <c>dbo.S_AddContrastOfSubClasses</c></summary>
    Task AddSubClassAsync(int criticalPartsId, string subClassCode, int userId, int trimShopId);

    /// <summary>حذف بحران مکانیکی (به همراه استثناها و مجوزهای آن) (MechanicalParts/MechanicalCriticalPartDelete (بک‌اند قبلی) / frmCriticalPartSearch (حذف)) — <c>dbo.S_CriticalPartDelete_Web</c></summary>
    Task<GlobalResultDto?> DeleteAsync(int id);

}
