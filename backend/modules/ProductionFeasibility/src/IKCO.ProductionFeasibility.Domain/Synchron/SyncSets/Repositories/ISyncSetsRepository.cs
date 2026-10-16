using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Synchron.SyncSets.Repositories;

/// <summary>ثبت و ویرایش بحران سنکرون (frmCriticalPartSet) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface ISyncSetsRepository
{
    /// <summary>اطلاعات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetSelectById</c></summary>
    Task<TabularResult> GetByIdAsync(int id);

    /// <summary>قطعات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId</c></summary>
    Task<TabularResult> GetPartsAsync(int criticalPartsSetId);

    /// <summary>قطعات بحران سنکرون به همراه فهرست مرجع کسری (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId</c></summary>
    Task<TabularResult> GetPartsWithShortageListAsync(int criticalPartsSetId);

    /// <summary>رنگ‌های بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetColorSelectByCriticalPartSetId</c></summary>
    Task<TabularResult> GetColorsAsync(int criticalPartsSetId);

    /// <summary>سالن‌های بحران سنکرون (frmCriticalPartSetIndicatorCars) — <c>dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId</c></summary>
    Task<TabularResult> GetSaloonsAsync(int criticalPartSetId);

    /// <summary>محصولات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId</c></summary>
    Task<TabularResult> GetProductsAsync(int criticalPartsSetId);

    /// <summary>جزئیات محصولات بحران سنکرون (frmCriticalPartSet) — <c>dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId</c></summary>
    Task<TabularResult> GetProductDetailsAsync(int criticalPartsSetId);

    /// <summary>تعداد سالن‌های بدون خودروی شاخص (frmCriticalPartSetIndicatorCars) — <c>dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount</c></summary>
    Task<TabularResult> GetNotEnteredIndicatorCarsCountAsync(int criticalPartsSetId);

    /// <summary>ثبت سرآیند بحران سنکرون (frmCriticalPartSet.btnSave) — <c>dbo.S_CriticalPartsSetInsert</c></summary>
    Task<int> CreateHeaderAsync(int setId, int deptId, string productsColorCode, int productsColorId, int productsTrimId, int productsToodooziId, int productsOptionId, short stock, string? note, int userId);

    /// <summary>ویرایش بحران سنکرون (frmCriticalPartSet (ویرایش)) — <c>dbo.S_CriticalPartsSetUpdate</c></summary>
    Task UpdateHeaderAsync(int id, int deptId, short stock, string? note, byte status, int userId);

    /// <summary>ثبت یک محصول بحران سنکرون (frmCriticalPartSet (محصولات)) — <c>dbo.S_CriticalPartsSetProductInsert</c></summary>
    Task AddProductAsync(int criticalPartsSetId, int carGroupId, int carModelId, int carClassId, int carSubClassId, int carTrimId, int carToodooziId, int carOptionId, int carColorId, int saloonId);

    /// <summary>تعیین خودروی شاخص سالن بحران سنکرون (frmCriticalPartSetIndicatorCars) — <c>dbo.S_CriticalPartsSetSaloonUpdate</c></summary>
    Task UpdateSaloonIndicatorCarAsync(int id, int criticalPartsSetId, short saloonId, int indicatorCarId);

    /// <summary>حذف منطقی بحران سنکرون (frmCriticalPartSet (حذف)) — <c>dbo.S_CriticalPartsSetDeleteLogically</c></summary>
    Task DeleteAsync(int id);

}
