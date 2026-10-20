using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Synchron.Results;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface ISyncSetsRepository
{
    /// <summary>اطلاعات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetSelectById</c></summary>
    Task<List<CriticalPartSetRow>> GetByIdAsync(int id);

    /// <summary>قطعات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId</c></summary>
    Task<List<SetPartRow>> GetPartsAsync(int criticalPartsSetId);

    /// <summary>قطعات بحران سنکرون به همراه فهرست مرجع کسری (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId</c></summary>
    Task<List<SetPartWithShortageRow>> GetPartsWithShortageListAsync(int criticalPartsSetId);

    /// <summary>رنگ‌های بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetColorSelectByCriticalPartSetId</c></summary>
    Task<List<SetColorRow>> GetColorsAsync(int criticalPartsSetId);

    /// <summary>سالن‌های بحران سنکرون (frmCriticalPartSetIndicatorCars) — <c>dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId</c></summary>
    Task<List<SetSaloonRow>> GetSaloonsAsync(int criticalPartSetId);

    /// <summary>محصولات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId</c></summary>
    Task<List<SetProductRow>> GetProductsAsync(int criticalPartsSetId);

    /// <summary>جزئیات محصولات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId</c></summary>
    Task<List<SetProductDetailRow>> GetProductDetailsAsync(int criticalPartsSetId);

    /// <summary>تعداد سالن‌های بدون خودروی شاخص (SyncParts/GetSyncPartsCriticalPartsSetSaloonNotEnteredIndicatorCarsCount (بک‌اند قبلی)) — <c>dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount_Web</c></summary>
    Task<int?> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId);

    /// <summary>سالن‌های ثبت‌شده بحران سنکرون (SyncParts/SyncPartsGetCriticalPartsSet_Saloons (بک‌اند قبلی)) — <c>dbo.S_GetCriticalPartsSet_Saloons</c></summary>
    Task<List<SetSaloonItemRow>> GetSetSaloonsAsync(int criticalPartsSetId);

    /// <summary>اطلاعات شماره فنی از BOM (کلاس، زیرکلاس و نوع قطعه) (SyncParts/GetSyncPartsInfo (بک‌اند قبلی)) — <c>dbo.SIMP_PartGetInfoByPartNo_Web</c></summary>
    Task<List<BomPartInfoRow>> GetPartInfoAsync(string partNo);

    /// <summary>رنگ‌های هم‌کد با کد ۵ رقمی (SyncParts/GetSyncPartsColorsSimilarTo5DigitCode (بک‌اند قبلی)) — <c>dbo.S_SelectColorsSimilarTo5DigitCode</c></summary>
    Task<List<SimilarColorRow>> GetSimilarColorsAsync(string code);

    /// <summary>سالن‌های مونتاژ مدل‌های انتخاب‌شده (SyncParts/GetSyncPartsSaloonsByCarModels (بک‌اند قبلی)) — <c>dbo.S_SelectSaloonsByCarModels</c></summary>
    Task<List<SaloonByCarModelRow>> GetSaloonsByCarModelsAsync(string carModelIdSTR, byte plantId);

    /// <summary>ثبت سرآیند بحران سنکرون (SyncParts/SyncPartsCriticalPartsSetInsert (بک‌اند قبلی) / frmCriticalPartSet.btnSave) — <c>dbo.S_CriticalPartsSetInsert_Web</c></summary>
    Task<GlobalResultDto?> CreateHeaderAsync(int setId, int deptId, string productsColorCode, int productsColorId, int productsTrimId, int productsToodooziId, int productsOptionId, short stock, string? note, int userId);

    /// <summary>ویرایش بحران سنکرون (SyncParts/SyncPartsCriticalPartsSetUpdate (بک‌اند قبلی) / frmCriticalPartSet (ویرایش)) — <c>dbo.S_CriticalPartsSetUpdate_Web</c></summary>
    Task<GlobalResultDto?> UpdateHeaderAsync(int id, int deptId, short stock, string? note, byte status, int userId);

    /// <summary>ثبت یک محصول بحران سنکرون (SyncParts/SyncPartsCriticalPartsSetProductInsert (بک‌اند قبلی) / frmCriticalPartSet (محصولات)) — <c>dbo.S_CriticalPartsSetProductInsert_Web</c></summary>
    Task<GlobalResultDto?> AddProductAsync(int criticalPartsSetId, int carGroupId, int carModelId, int carClassId, int carSubClassId, int carTrimId, int carToodooziId, int carOptionId, int carColorId, int saloonId);

    /// <summary>تعیین خودروی شاخص سالن بحران سنکرون (SyncParts/SyncPartsCriticalPartsSetSaloonUpdate (بک‌اند قبلی) / frmCriticalPartSetIndicatorCars) — <c>dbo.S_CriticalPartsSetSaloonUpdate_Web</c></summary>
    Task<GlobalResultDto?> UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, short saloonId, int indicatorCarId);

    /// <summary>حذف منطقی بحران سنکرون (frmCriticalPartSet (حذف)) — <c>dbo.S_CriticalPartsSetDeleteLogically</c></summary>
    Task DeleteAsync(int id);

}
