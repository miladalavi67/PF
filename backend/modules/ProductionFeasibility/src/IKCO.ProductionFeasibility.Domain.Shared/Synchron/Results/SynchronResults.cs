using System;
using System.Text.Json.Serialization;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Synchron.Results;

/// <summary>
/// اطلاعات شماره فنی از BOM (SIMP_PartGetInfoByPartNo_Web — VIMP_BOM_CarClassPartsWithPartType)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (GetSyncPartsInfo) / متن رویه در SFCDB2.md</para>
/// </summary>
public class BomPartInfoRow : PfResultRow
{
    /// <summary>کد کلاس</summary>
    [JsonPropertyName("ClassCode")]
    public string? ClassCode { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("SubClassCode")]
    public string? SubClassCode { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>کد نوع قطعه</summary>
    [JsonPropertyName("PartTypeCode")]
    public int? PartTypeCode { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartTypeDescF")]
    public string? PartTypeDescF { get; set; }
}

/// <summary>
/// بحران سنکرون (V_CriticalPartSet)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (GetSyncPartsCriticalsById) / متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartSetRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه مبنا</summary>
    [JsonPropertyName("SetId")]
    public int? SetId { get; set; }

    /// <summary>کد ست</summary>
    [JsonPropertyName("expSetCode")]
    public string? ExpSetCode { get; set; }

    /// <summary>شرح ست</summary>
    [JsonPropertyName("expSetDescF")]
    public string? ExpSetDescF { get; set; }

    /// <summary>شناسه کلاس مبنا</summary>
    [JsonPropertyName("expSetBasisCarClassId")]
    public int? ExpSetBasisCarClassId { get; set; }

    /// <summary>کد کلاس مبنا</summary>
    [JsonPropertyName("expSetBasisCarClassCode")]
    public string? ExpSetBasisCarClassCode { get; set; }

    /// <summary>کلاس مبنا</summary>
    [JsonPropertyName("expSetBasisCarClassDescF")]
    public string? ExpSetBasisCarClassDescF { get; set; }

    /// <summary>شناسه نوع قطعه</summary>
    [JsonPropertyName("expSetPartTypeId")]
    public int? ExpSetPartTypeId { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("expPartTypeDescF")]
    public string? ExpPartTypeDescF { get; set; }

    /// <summary>ست</summary>
    [JsonPropertyName("IsSet")]
    public int? IsSet { get; set; }

    /// <summary>واحد مسئول</summary>
    [JsonPropertyName("DeptId")]
    public int? DeptId { get; set; }

    /// <summary>کد واحد</summary>
    [JsonPropertyName("expDeptCode")]
    public string? ExpDeptCode { get; set; }

    /// <summary>واحد مسئول</summary>
    [JsonPropertyName("expDeptDescF")]
    public string? ExpDeptDescF { get; set; }

    /// <summary>کد رنگ</summary>
    [JsonPropertyName("ProductsColorCode")]
    public string? ProductsColorCode { get; set; }

    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("ProductsColorId")]
    public int? ProductsColorId { get; set; }

    /// <summary>شناسه تریم</summary>
    [JsonPropertyName("ProductsTrimId")]
    public int? ProductsTrimId { get; set; }

    /// <summary>شناسه تودوزی</summary>
    [JsonPropertyName("ProductsToodooziId")]
    public int? ProductsToodooziId { get; set; }

    /// <summary>شناسه آپشن</summary>
    [JsonPropertyName("ProductsOptionId")]
    public int? ProductsOptionId { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityId")]
    public int? ProductionPossibilityId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("expProductionPossibilityDescF")]
    public string? ExpProductionPossibilityDescF { get; set; }

    /// <summary>مجاز از تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityFromDate")]
    public string? ProductionPossibilityFromDate { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز از ساعت</summary>
    [JsonPropertyName("ProductionPossibilityFromTime")]
    public string? ProductionPossibilityFromTime { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionPossibilityLetterNo")]
    public string? ProductionPossibilityLetterNo { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionPossibilityNote")]
    public string? ProductionPossibilityNote { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>نوع کاهش تولید</summary>
    [JsonPropertyName("ProductionFallId")]
    public int? ProductionFallId { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Note")]
    public string? Note { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserId")]
    public int? UserId { get; set; }

    /// <summary>کد پرسنلی</summary>
    [JsonPropertyName("expUserPersonelCode")]
    public string? ExpUserPersonelCode { get; set; }

    /// <summary>نام</summary>
    [JsonPropertyName("expUserFirstName")]
    public string? ExpUserFirstName { get; set; }

    /// <summary>نام خانوادگی</summary>
    [JsonPropertyName("expUserLastName")]
    public string? ExpUserLastName { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public DateTime? CreateDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }
}

/// <summary>
/// سالن‌های مونتاژ مدل‌ها (S_SelectSaloonsByCarModels)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SaloonByCarModelRow : PfResultRow
{
    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }
}

/// <summary>
/// اقلام بحرانی دریافتی از SAP (S_Web_SAP_CriticalPartsMaster_SelectAll)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SapCriticalPartsMasterRow : PfResultRow
{
    /// <summary>شماره تلکس</summary>
    [JsonPropertyName("TelNo")]
    public string? TelNo { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public string? Stock { get; set; }

    /// <summary>متن جایگزین</summary>
    [JsonPropertyName("alttext")]
    public string? Alttext { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDesc")]
    public string? SaloonDesc { get; set; }

    /// <summary>تاریخ سرویس</summary>
    [JsonPropertyName("ServiceDate")]
    public string? ServiceDate { get; set; }

    /// <summary>تاریخ ایجاد تلکس</summary>
    [JsonPropertyName("TelNoCreateDate")]
    public string? TelNoCreateDate { get; set; }
}

/// <summary>
/// رزروهای دریافتی از SAP (S_Web_SAP_ReserveMaster_SelectAll)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SapReserveMasterRow : PfResultRow
{
    /// <summary>شماره تلکس</summary>
    [JsonPropertyName("TelNo")]
    public string? TelNo { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>تعداد رزرو</summary>
    [JsonPropertyName("ReserveCount")]
    public decimal? ReserveCount { get; set; }

    /// <summary>شماره رزرو</summary>
    [JsonPropertyName("ReserveNumber")]
    public string? ReserveNumber { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDesc")]
    public string? SaloonDesc { get; set; }

    /// <summary>تاریخ سرویس</summary>
    [JsonPropertyName("ServiceDate")]
    public string? ServiceDate { get; set; }

    /// <summary>تاریخ ایجاد تلکس</summary>
    [JsonPropertyName("TelNoCreateDate")]
    public string? TelNoCreateDate { get; set; }
}

/// <summary>
/// فهرست مبناهای ست سنکرون (S_RptGetCriticalPartsSet)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SetBasisReportRow : PfResultRow
{
    /// <summary>شناسه مبنا</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>کد</summary>
    [JsonPropertyName("Code")]
    public string? Code { get; set; }

    /// <summary>شرح</summary>
    [JsonPropertyName("DescF")]
    public string? DescF { get; set; }

    /// <summary>شناسه کلاس مبنا</summary>
    [JsonPropertyName("BasisCarClassId")]
    public int? BasisCarClassId { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDescF")]
    public string? CarClassDescF { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartTypeDescF")]
    public string? PartTypeDescF { get; set; }

    /// <summary>سطح نوع قطعه</summary>
    [JsonPropertyName("LevelType")]
    public int? LevelType { get; set; }

    /// <summary>شناسه نوع قطعه</summary>
    [JsonPropertyName("PartTypeId")]
    public int? PartTypeId { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateTime")]
    public string? LastUpdateTime { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserId")]
    public string? UserId { get; set; }

    /// <summary>بر اساس تریم</summary>
    [JsonPropertyName("IsTrim")]
    public bool? IsTrim { get; set; }

    /// <summary>بر اساس آپشن</summary>
    [JsonPropertyName("IsOption")]
    public bool? IsOption { get; set; }

    /// <summary>بر اساس تودوزی</summary>
    [JsonPropertyName("IsToodoozi")]
    public bool? IsToodoozi { get; set; }

    /// <summary>بر اساس رنگ</summary>
    [JsonPropertyName("IsColor")]
    public bool? IsColor { get; set; }
}

/// <summary>
/// مبنای ست سنکرون (V_CriticalPartsSetBasis)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (CriticalPartsSetBasis) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SetBasisRow : PfResultRow
{
    /// <summary>شناسه مبنا</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>کد</summary>
    [JsonPropertyName("Code")]
    public string? Code { get; set; }

    /// <summary>شرح</summary>
    [JsonPropertyName("DescF")]
    public string? DescF { get; set; }

    /// <summary>شناسه کلاس مبنا</summary>
    [JsonPropertyName("BasisCarClassId")]
    public int? BasisCarClassId { get; set; }

    /// <summary>کد کلاس مبنا</summary>
    [JsonPropertyName("expBasisCarClassCode")]
    public string? ExpBasisCarClassCode { get; set; }

    /// <summary>کلاس مبنا</summary>
    [JsonPropertyName("expBasisCarClassDescF")]
    public string? ExpBasisCarClassDescF { get; set; }

    /// <summary>شناسه نوع قطعه</summary>
    [JsonPropertyName("PartTypeId")]
    public int? PartTypeId { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("expPartTypeDescF")]
    public string? ExpPartTypeDescF { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("CarModelId")]
    public int? CarModelId { get; set; }

    /// <summary>وضعیت کلاس مبنا</summary>
    [JsonPropertyName("BasisCarClassStatus")]
    public int? BasisCarClassStatus { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("expCriticalPartsSetStatus")]
    public string? ExpCriticalPartsSetStatus { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserId")]
    public int? UserId { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public DateTime? CreateDateTime { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDateShamsi")]
    public string? CreateDateShamsi { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>تاریخ آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateShamsi")]
    public string? LastUpdateDateShamsi { get; set; }
}

/// <summary>
/// رنگ‌های بحران سنکرون (V_CriticalPartsSet_Colors)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SetColorRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("CarColorId")]
    public int? CarColorId { get; set; }

    /// <summary>کد رنگ</summary>
    [JsonPropertyName("CarColorCode")]
    public string? CarColorCode { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("CarColorDescF")]
    public string? CarColorDescF { get; set; }
}

/// <summary>
/// قطعات بحران سنکرون (V_CriticalPartsSet_Parts)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SetPartRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartTypeId")]
    public string? PartTypeId { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartTypeDescF")]
    public string? PartTypeDescF { get; set; }
}

/// <summary>
/// قطعات بحران سنکرون به همراه فهرست مرجع کسری (S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SetPartWithShortageRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }
}

/// <summary>
/// جزئیات محصولات بحران سنکرون (S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (GetSyncPartsProductionPartsSet) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SetProductDetailRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه خانواده</summary>
    [JsonPropertyName("CarGroupId")]
    public int? CarGroupId { get; set; }

    /// <summary>کد خانواده</summary>
    [JsonPropertyName("CarGroupCode")]
    public string? CarGroupCode { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("CarGroupDescF")]
    public string? CarGroupDescF { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("CarModelId")]
    public int? CarModelId { get; set; }

    /// <summary>کد مدل</summary>
    [JsonPropertyName("CarModelCode")]
    public string? CarModelCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDescF")]
    public string? CarModelDescF { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public int? CarClassId { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDescF")]
    public string? CarClassDescF { get; set; }

    /// <summary>شناسه زیرکلاس</summary>
    [JsonPropertyName("CarSubClassId")]
    public int? CarSubClassId { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("CarSubClassCode")]
    public string? CarSubClassCode { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("CarSubClassDescF")]
    public string? CarSubClassDescF { get; set; }

    /// <summary>شناسه تریم</summary>
    [JsonPropertyName("CarTrimId")]
    public int? CarTrimId { get; set; }

    /// <summary>کد تریم</summary>
    [JsonPropertyName("CarTrimCode")]
    public string? CarTrimCode { get; set; }

    /// <summary>تریم</summary>
    [JsonPropertyName("CarTrimDescF")]
    public string? CarTrimDescF { get; set; }

    /// <summary>شناسه تودوزی</summary>
    [JsonPropertyName("CarToodooziId")]
    public int? CarToodooziId { get; set; }

    /// <summary>کد تودوزی</summary>
    [JsonPropertyName("CarToodooziCode")]
    public string? CarToodooziCode { get; set; }

    /// <summary>تودوزی</summary>
    [JsonPropertyName("CarToodooziDescF")]
    public string? CarToodooziDescF { get; set; }

    /// <summary>شناسه آپشن</summary>
    [JsonPropertyName("CarOptionId")]
    public int? CarOptionId { get; set; }

    /// <summary>کد آپشن</summary>
    [JsonPropertyName("CarOptionCode")]
    public string? CarOptionCode { get; set; }

    /// <summary>آپشن</summary>
    [JsonPropertyName("CarOptionDescF")]
    public string? CarOptionDescF { get; set; }

    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("CarColorId")]
    public int? CarColorId { get; set; }

    /// <summary>کد رنگ</summary>
    [JsonPropertyName("CarColorCode")]
    public string? CarColorCode { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("CarColorDescF")]
    public string? CarColorDescF { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>کد سالن</summary>
    [JsonPropertyName("SaloonCode")]
    public string? SaloonCode { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }
}

/// <summary>
/// محصولات بحران سنکرون (S_CriticalPartSetProductSelectByCriticalPartsSetId)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SetProductRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه خانواده</summary>
    [JsonPropertyName("CarGroupId")]
    public int? CarGroupId { get; set; }

    /// <summary>کد خانواده</summary>
    [JsonPropertyName("CarGroupCode")]
    public string? CarGroupCode { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("CarGroupDescF")]
    public string? CarGroupDescF { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("CarModelId")]
    public int? CarModelId { get; set; }

    /// <summary>کد مدل</summary>
    [JsonPropertyName("CarModelCode")]
    public string? CarModelCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDescF")]
    public string? CarModelDescF { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public int? CarClassId { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDescF")]
    public string? CarClassDescF { get; set; }

    /// <summary>شناسه زیرکلاس</summary>
    [JsonPropertyName("CarSubClassId")]
    public int? CarSubClassId { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("CarSubClassCode")]
    public string? CarSubClassCode { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("CarSubClassDescF")]
    public string? CarSubClassDescF { get; set; }
}

/// <summary>
/// سالن‌های ثبت‌شده بحران سنکرون (S_GetCriticalPartsSet_Saloons)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (GetCriticalPartsSet_SaloonsDto) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SetSaloonItemRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }

    /// <summary>شناسه خودروی شاخص</summary>
    [JsonPropertyName("IndicatorCarId")]
    public int? IndicatorCarId { get; set; }
}

/// <summary>
/// سالن‌های بحران سنکرون و خودروی شاخص (V_CriticalPartsSet_Saloons)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (GetSyncSaloonsOfCriticalPartsSet) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SetSaloonRow : PfResultRow
{
    /// <summary>شناسه بحران سنکرون</summary>
    [JsonPropertyName("CriticalPartsSetId")]
    public int? CriticalPartsSetId { get; set; }

    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("SaloonId")]
    public int? SaloonId { get; set; }

    /// <summary>کد سالن</summary>
    [JsonPropertyName("SaloonCode")]
    public string? SaloonCode { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }

    /// <summary>شناسه خودروی شاخص</summary>
    [JsonPropertyName("IndicatorCarId")]
    public int? IndicatorCarId { get; set; }

    /// <summary>بدنه شاخص</summary>
    [JsonPropertyName("IndicatorCarBodyNo")]
    public string? IndicatorCarBodyNo { get; set; }
}

/// <summary>
/// رنگ‌های هم‌کد ۵ رقمی (S_SelectColorsSimilarTo5DigitCode)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class SimilarColorRow : PfResultRow
{
    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("CarColorId")]
    public int? CarColorId { get; set; }

    /// <summary>کد رنگ</summary>
    [JsonPropertyName("CarColorCode")]
    public string? CarColorCode { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("CarColorDescF")]
    public string? CarColorDescF { get; set; }
}
