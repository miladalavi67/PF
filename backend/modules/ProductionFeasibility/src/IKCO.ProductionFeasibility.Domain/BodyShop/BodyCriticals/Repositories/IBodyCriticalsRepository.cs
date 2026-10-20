using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.BodyShop.Results;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Repositories;

/// <summary>بحران‌های بدنه‌سازی (ثبت، ویرایش، غیرفعال‌سازی) — دسترسی به رویه‌های ذخیره‌شده (Dapper).</summary>
public partial interface IBodyCriticalsRepository
{
    /// <summary>اطلاعات بدنه (uctBodyInfo / DA_Critical.GetCarInfo) — <c>dbo.SEXT_BodyInfo_V2</c></summary>
    Task<BodyInfoRow?> GetBodyInfoAsync(string bodyNumber, int userId);

    /// <summary>شرح فارسی قطعه بر اساس شماره فنی (uctPartNo / SelectPartDescByPartNo) — <c>dbo.SIMP_SelectPartDescByPartNo</c></summary>
    Task<string?> GetPartDescriptionAsync(string partNo);

    /// <summary>محصولات مصرف‌کننده یک شماره فنی در یک خط بدنه‌سازی (BL_BodyCritical.ProductByPartNo_SelectAll) — <c>PF.SelectPartProductByPartNo</c></summary>
    Task<List<PartProductRow>> GetPartProductsAsync(string partNo, int biwSiteId);

    /// <summary>کدهای کلاس مصرف‌کننده شماره فنی (frmUploadParts (ثبت گروهی)) — <c>PF.S_GetCodeClassByPartNo</c></summary>
    Task<List<CodeClassRow>> GetCodeClassByPartNoAsync(string partNo, int biwSiteId);

    /// <summary>ثبت بحران بدنه‌سازی (frmBodyCriticalParts.btnSave) — <c>PF.S_CreateCriticalParts_V3</c></summary>
    Task<GlobalResultDto?> CreateCriticalPartAsync(int carId, string partNo, string partDesc, string partGroup, string? alterNativePartNo, string? criticalPartNote, int biwSiteId, int totalStock, int rawStock, int userId, string? productionStateNote, string? letterNo, bool isOutOfSAP);

    /// <summary>ویرایش بحران بدنه‌سازی (frmBodyCriticalParts (حالت ویرایش)) — <c>PF.S_Web_UpdateCriticalParts</c></summary>
    Task<GlobalResultDto?> UpdateCriticalPartAsync(int criticalPartsId, string partGroup, string? alterNativePartNo, string? criticalPartNote, int rawStock, int userId, string? productionStateNote, string? letterNo);

    /// <summary>اطلاعات یک بحران بدنه‌سازی برای ویرایش (BL_BodyCritical.CriticalPartsInfo (frmBodyCriticalParts در حالت ویرایش)) — <c>PF.S_CriticalPartsInfo</c></summary>
    Task<List<BodyCriticalPartInfoRow>> GetCriticalPartInfoAsync(int criticalPartsId);

    /// <summary>گزارش رویدادهای یک بحران (frmEventLog) — <c>PF.S_EventLogList</c></summary>
    Task<List<EventLogRow>> GetEventLogAsync(int eventTypeId, int changedRecordId);

}
