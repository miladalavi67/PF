using System;
using System.Text.Json.Serialization;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.BodyShop.Results;

/// <summary>
/// نتیجه ثبت گروهی بحران با اکسل (PF.S_CreateBatchCriticalPartProduct)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyBatchCreateResultRow : PfResultRow
{
    /// <summary>پیام</summary>
    [JsonPropertyName("Message")]
    public string? Message { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }
}

/// <summary>
/// اطلاعات بحران بدنه‌سازی برای ویرایش (PF.S_CriticalPartsInfo)
/// <para>منبع ستون‌ها: کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyCriticalPartInfoRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه محصول</summary>
    [JsonPropertyName("CarId")]
    public int? CarId { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>موجودی گروهی</summary>
    [JsonPropertyName("TotalStock")]
    public int? TotalStock { get; set; }

    /// <summary>موجودی خام</summary>
    [JsonPropertyName("RawStock")]
    public int? RawStock { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlternativePartNo")]
    public string? AlternativePartNo { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionStateNote")]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>شناسه خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSiteId")]
    public int? BIWSiteId { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public string? CarClassId { get; set; }
}

/// <summary>
/// محصولات یک بحران بدنه‌سازی (PF.Get_BodyCriticalSearch_Product_Web)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyCriticalProductRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("CarGroupeDesc")]
    public string? CarGroupeDesc { get; set; }

    /// <summary>کد خانواده</summary>
    [JsonPropertyName("CarGroupeCode")]
    public string? CarGroupeCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDesc")]
    public string? CarModelDesc { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDesc")]
    public string? CarClassDesc { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public int? UsageFactor { get; set; }
}

/// <summary>
/// جستجوی اقلام بحرانی بدنه‌سازی (PF.S_BodyCriticalSearch_Web)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyCriticalSearchRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>موجودی گروهی</summary>
    [JsonPropertyName("TotalStock")]
    public int? TotalStock { get; set; }

    /// <summary>موجودی خام</summary>
    [JsonPropertyName("RawStock")]
    public int? RawStock { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPosDesF")]
    public string? ProductionPosDesF { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>سایت</summary>
    [JsonPropertyName("VirtualPlantDescF")]
    public string? VirtualPlantDescF { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSiteDescF")]
    public string? BIWSiteDescF { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlternativePartNo")]
    public string? AlternativePartNo { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public string? LastUpdateDateTime { get; set; }

    /// <summary>فعال</summary>
    [JsonPropertyName("Status")]
    public bool? Status { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("StatusDescF")]
    public string? StatusDescF { get; set; }
}

/// <summary>
/// ردیابی پارامترهای بحرانی (PF.GetShortageLicenseTrackingSelectAll)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyLicenseTrackingRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeId")]
    public int? CriticalPartTypeId { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("CriticalPartId")]
    public int? CriticalPartId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityId")]
    public int? ProductionPossibilityId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityDescF")]
    public string? ProductionPossibilityDescF { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>مجوز مصرف‌شده</summary>
    [JsonPropertyName("UsedLicense")]
    public int? UsedLicense { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("STATUS")]
    public string? STATUS { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserName")]
    public string? UserName { get; set; }

    /// <summary>آخرین ویرایش‌کننده</summary>
    [JsonPropertyName("LastUpdateByName")]
    public string? LastUpdateByName { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public string? LastUpdateDateTime { get; set; }
}

/// <summary>
/// مغایرت پیش‌تخصیص بدنه‌های بحرانی (PF.S_NotPossibleProductsToStepAssign_SelectAll)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyNotPossibleToAssignRow : PfResultRow
{
    /// <summary>ردیف</summary>
    [JsonPropertyName("RowNumber")]
    public long? RowNumber { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }

    /// <summary>شرح مغایرت</summary>
    [JsonPropertyName("ErrorDescF")]
    public string? ErrorDescF { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartType")]
    public string? CriticalPartType { get; set; }
}

/// <summary>
/// تعداد بدنه‌های پیش‌تخصیص‌شده (PF.S_PreAssignBodyCoun_List)
/// <para>منبع ستون‌ها: کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyPreAssignCountRow : PfResultRow
{
    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSite")]
    public string? BIWSite { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("ClassCode")]
    public string? ClassCode { get; set; }

    /// <summary>تعداد بدنه پیش‌تخصیص</summary>
    [JsonPropertyName("PreAssignBodyCount")]
    public int? PreAssignBodyCount { get; set; }
}

/// <summary>
/// بدنه‌های پیش‌تخصیص بحرانی (PF.S_PreAssignCriticalBody_SelectAll)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyPreAssignCriticalRow : PfResultRow
{
    /// <summary>ردیف</summary>
    [JsonPropertyName("RowNumber")]
    public long? RowNumber { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }
}

/// <summary>
/// وضعیت مجوز تولید یک بحران (PF.GetProductionPssibilityStatus_V1)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyProductionPossibilityStatusRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>نوع کاهش تولید</summary>
    [JsonPropertyName("ProductionFallId")]
    public int? ProductionFallId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityId")]
    public int? ProductionPossibilityId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityDescF")]
    public string? ProductionPossibilityDescF { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>مجوز مصرف‌شده</summary>
    [JsonPropertyName("UsedLicense")]
    public int? UsedLicense { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Note")]
    public string? Note { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }
}

/// <summary>
/// محصولات به تفکیک خطوط بدنه‌سازی (PF.S_VINBIWSite_CarModel_SelectAll)
/// <para>منبع ستون‌ها: کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyProductsOfSiteRow : PfResultRow
{
    /// <summary>ردیف</summary>
    [JsonPropertyName("Row")]
    public long? Row { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("VINBIWSiteDescF")]
    public string? VINBIWSiteDescF { get; set; }

    /// <summary>شناسه خط</summary>
    [JsonPropertyName("BIWSIteId")]
    public int? BIWSIteId { get; set; }

    /// <summary>کد مدل</summary>
    [JsonPropertyName("CarModelCode")]
    public string? CarModelCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDescF")]
    public string? CarModelDescF { get; set; }
}

/// <summary>
/// ردیابی تغییرات اقلام بحرانی بدنه‌سازی (PF.GetTrackingCriticalParts_V2)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class BodyTrackingCriticalPartRow : PfResultRow
{
    /// <summary>ردیف</summary>
    [JsonPropertyName("RowNumber")]
    public long? RowNumber { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>موجودی گروهی</summary>
    [JsonPropertyName("TotalStock")]
    public int? TotalStock { get; set; }

    /// <summary>موجودی خام</summary>
    [JsonPropertyName("RawStock")]
    public int? RawStock { get; set; }

    /// <summary>توضیحات بحران</summary>
    [JsonPropertyName("CriticalDescription")]
    public string? CriticalDescription { get; set; }

    /// <summary>مجموع</summary>
    [JsonPropertyName("Total")]
    public int? Total { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("DescF")]
    public string? DescF { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>امکان تولید</summary>
    [JsonPropertyName("ProductionPos")]
    public int? ProductionPos { get; set; }

    /// <summary>امکان تولید به ازای بدنه</summary>
    [JsonPropertyName("ProductionPosPerBody")]
    public int? ProductionPosPerBody { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>مجوز مصرف‌شده</summary>
    [JsonPropertyName("UsedLicense")]
    public int? UsedLicense { get; set; }

    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSiteDescF")]
    public string? BIWSiteDescF { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlternativePartNo")]
    public string? AlternativePartNo { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public string? LastUpdateDateTime { get; set; }

    /// <summary>تعداد بدنه پیش‌تخصیص</summary>
    [JsonPropertyName("PreAssignBodyCount")]
    public int? PreAssignBodyCount { get; set; }

    /// <summary>تعداد بدنه پیش‌تخصیص پس از ثبت</summary>
    [JsonPropertyName("PreAssignBodyCountAfterRegistration")]
    public int? PreAssignBodyCountAfterRegistration { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartType")]
    public string? PartType { get; set; }
}

/// <summary>
/// محصولات بحران در ردیابی (PF.GetTrackingCriticalParts_Product)
/// <para>منبع ستون‌ها: کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class BodyTrackingProductRow : PfResultRow
{
    /// <summary>ردیف</summary>
    [JsonPropertyName("RowNumber")]
    public long? RowNumber { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("CarGroupeDesc")]
    public string? CarGroupeDesc { get; set; }

    /// <summary>کد خانواده</summary>
    [JsonPropertyName("CarGroupeCode")]
    public string? CarGroupeCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDesc")]
    public string? CarModelDesc { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDesc")]
    public string? CarClassDesc { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("CarSubClassDesc")]
    public string? CarSubClassDesc { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("CarSubClassCode")]
    public string? CarSubClassCode { get; set; }
}

/// <summary>
/// کد کلاس مصرف‌کننده شماره فنی (PF.S_GetCodeClassByPartNo)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class CodeClassRow : PfResultRow
{
    /// <summary>کد کلاس</summary>
    [JsonPropertyName("Class_Code")]
    public string? Class_Code { get; set; }
}

/// <summary>
/// رویدادهای یک بحران (PF.S_EventLogList)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / کلاس _Result مدل EF فرم ویندوزی PF</para>
/// </summary>
public class EventLogRow : PfResultRow
{
    /// <summary>شناسه رویداد</summary>
    [JsonPropertyName("EventLogId")]
    public int? EventLogId { get; set; }

    /// <summary>نوع رویداد</summary>
    [JsonPropertyName("AtrributeDescF")]
    public string? AtrributeDescF { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionStateNote")]
    public string? ProductionStateNote { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartNoDesc")]
    public string? PartNoDesc { get; set; }

    /// <summary>مقدار قبلی</summary>
    [JsonPropertyName("oldvalue")]
    public string? Oldvalue { get; set; }

    /// <summary>مقدار جدید</summary>
    [JsonPropertyName("newvalue")]
    public string? Newvalue { get; set; }

    /// <summary>شماره درخواست</summary>
    [JsonPropertyName("Requestno")]
    public string? Requestno { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("note")]
    public string? Note { get; set; }

    /// <summary>تاریخ رویداد</summary>
    [JsonPropertyName("EventDate")]
    public string? EventDate { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserName")]
    public string? UserName { get; set; }
}

/// <summary>
/// قطعات ناموفق در انتقال از SAP (S_FailedPartTransferd_Sap_Log)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class FailedSapTransferRow : PfResultRow
{
    /// <summary>تاریخ و زمان</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }
}

/// <summary>
/// زمان آخرین به‌روزرسانی داده‌های SAP (S_GetSapDataUpdateTimes)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared)</para>
/// </summary>
public class SapDataUpdateTimesRow : PfResultRow
{
    /// <summary>آخرین به‌روزرسانی اقلام بحرانی SAP</summary>
    [JsonPropertyName("CriticalMasterUpdateDateTime")]
    public string? CriticalMasterUpdateDateTime { get; set; }

    /// <summary>آخرین به‌روزرسانی رزرو SAP</summary>
    [JsonPropertyName("ReserveMasterUpdateTime")]
    public string? ReserveMasterUpdateTime { get; set; }
}
