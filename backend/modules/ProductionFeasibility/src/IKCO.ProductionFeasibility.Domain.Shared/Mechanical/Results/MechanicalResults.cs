using System;
using System.Text.Json.Serialization;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.Mechanical.Results;

/// <summary>
/// گزارش ASN (S_ASNReport)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (Rpt_SyncPartsASN) / متن رویه در SFCDB2.md</para>
/// </summary>
public class AsnRow : PfResultRow
{
    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>تعداد</summary>
    [JsonPropertyName("Total")]
    public decimal? Total { get; set; }

    /// <summary>کد انبار</summary>
    [JsonPropertyName("WarehouseCode")]
    public string? WarehouseCode { get; set; }

    /// <summary>واحد</summary>
    [JsonPropertyName("Unit")]
    public string? Unit { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }

    /// <summary>ساعت</summary>
    [JsonPropertyName("CreatTime")]
    public string? CreatTime { get; set; }
}

/// <summary>
/// محصولات مصرف‌کننده شماره فنی از BOM (SIMP_SelectProductsUsePartNo / SIMP_SelectProductsUseAllPartNosByClassCodePartType)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class BomProductRow : PfResultRow
{
    /// <summary>ردیف</summary>
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
/// مشخصات بدنه برای ثبت بحران (S_CarPropertiesSelectForCriticalParts)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class CarPropertiesRow : PfResultRow
{
    /// <summary>شناسه محصول (CarId)</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>VIN</summary>
    [JsonPropertyName("VIN")]
    public string? VIN { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("CarModelId")]
    public int? CarModelId { get; set; }

    /// <summary>محصول</summary>
    [JsonPropertyName("CarModelDesc")]
    public string? CarModelDesc { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public int? CarClassId { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDesc")]
    public string? CarClassDesc { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("CarColorDescf")]
    public string? CarColorDescf { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("CarGroupDescf")]
    public string? CarGroupDescf { get; set; }

    /// <summary>شناسه آخرین مرحله</summary>
    [JsonPropertyName("LastProductionStepId")]
    public int? LastProductionStepId { get; set; }

    /// <summary>مرحله جاری</summary>
    [JsonPropertyName("LastStepDescF")]
    public string? LastStepDescF { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("LastSaloonDescF")]
    public string? LastSaloonDescF { get; set; }

    /// <summary>شناسه آخرین ردیابی تولید</summary>
    [JsonPropertyName("LastCarManufacturingTraceId")]
    public int? LastCarManufacturingTraceId { get; set; }

    /// <summary>تاریخ ورود</summary>
    [JsonPropertyName("LastStepdate")]
    public string? LastStepdate { get; set; }

    /// <summary>ساعت ورود</summary>
    [JsonPropertyName("LastSteptime")]
    public string? LastSteptime { get; set; }

    /// <summary>شناسه سالن</summary>
    [JsonPropertyName("LastSaloonId")]
    public int? LastSaloonId { get; set; }
}

/// <summary>
/// بحران فعال شماره فنی در سالن (S_CriticalPartsExistPartNo — cp.*, tsp.*)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartExistRow : PfResultRow
{
    /// <summary>شماره فنی</summary>
    [JsonPropertyName("Partno")]
    public string? Partno { get; set; }

    /// <summary>سالن مونتاژ</summary>
    [JsonPropertyName("trimShopid")]
    public int? TrimShopid { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه محصول</summary>
    [JsonPropertyName("CarId")]
    public int? CarId { get; set; }

    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public string? UsageFactor { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>موجودی قبلی</summary>
    [JsonPropertyName("PreviousStock")]
    public int? PreviousStock { get; set; }

    /// <summary>مصرف‌شده</summary>
    [JsonPropertyName("Used")]
    public int? Used { get; set; }

    /// <summary>بدنه‌های وارد شده پس از ثبت</summary>
    [JsonPropertyName("TrimInBodyCountAfterRegisteration")]
    public int? TrimInBodyCountAfterRegisteration { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public int? ProductionPossibility { get; set; }

    /// <summary>مجاز از تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityFromDate")]
    public string? ProductionPossibilityFromDate { get; set; }

    /// <summary>مجاز از ساعت</summary>
    [JsonPropertyName("ProductionPossibilityFromTime")]
    public string? ProductionPossibilityFromTime { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>واحد مسئول کسری</summary>
    [JsonPropertyName("DeptId")]
    public int? DeptId { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionStateNote")]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserId")]
    public int? UserId { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public DateTime? CreateDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>نوع کاهش تولید</summary>
    [JsonPropertyName("ProductionFallId")]
    public int? ProductionFallId { get; set; }

    /// <summary>زمان به‌روزرسانی موجودی</summary>
    [JsonPropertyName("StockUpdateDateTime")]
    public DateTime? StockUpdateDateTime { get; set; }

    /// <summary>خارج از SAP</summary>
    [JsonPropertyName("IsOutOfSap")]
    public bool? IsOutOfSap { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastChange")]
    public string? LastChange { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("CarModelId")]
    public int? CarModelId { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public int? CarClassId { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>نوع قطعه کسری</summary>
    [JsonPropertyName("ShortagePartTypeId")]
    public int? ShortagePartTypeId { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Note")]
    public string? Note { get; set; }

    /// <summary>مجاز به تکمیل‌کاری (بلااستفاده)</summary>
    [JsonPropertyName("TakIsPossible_NotUsed")]
    public int? TakIsPossible_NotUsed { get; set; }

    /// <summary>سایت</summary>
    [JsonPropertyName("PlantId")]
    public int? PlantId { get; set; }
}

/// <summary>
/// اطلاعات بحران مکانیکی برای ویرایش (S_SelectCriticalPartForEdit)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartForEditRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("Partno")]
    public string? Partno { get; set; }

    /// <summary>شرح فنی</summary>
    [JsonPropertyName("Partdesc")]
    public string? Partdesc { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>واحد مسئول کسری</summary>
    [JsonPropertyName("DeptId")]
    public int? DeptId { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public int? ProductionPossibility { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>مصرف‌شده</summary>
    [JsonPropertyName("USED")]
    public int? USED { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionStateNote")]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public string? UsageFactor { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>خارج از SAP</summary>
    [JsonPropertyName("IsOutOfSap")]
    public bool? IsOutOfSap { get; set; }
}

/// <summary>
/// محصولات بحران مکانیکی برای ویرایش (S_CriticalPartProductsSelectForEdit_V1)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartProductForEditRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>شناسه خانواده</summary>
    [JsonPropertyName("GroupId")]
    public int? GroupId { get; set; }

    /// <summary>خانواده</summary>
    [JsonPropertyName("GroupDescf")]
    public string? GroupDescf { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("ModelId")]
    public int? ModelId { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModelDesc")]
    public string? CarModelDesc { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("CarClassId")]
    public int? CarClassId { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarClassCode")]
    public string? CarClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("CarClassDesc")]
    public string? CarClassDesc { get; set; }

    /// <summary>شناسه زیرکلاس</summary>
    [JsonPropertyName("SubClassId")]
    public int? SubClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("SubClassDesc")]
    public string? SubClassDesc { get; set; }

    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("ColorId")]
    public int? ColorId { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("ColorDesc")]
    public string? ColorDesc { get; set; }

    /// <summary>شناسه تریم</summary>
    [JsonPropertyName("TrimId")]
    public int? TrimId { get; set; }

    /// <summary>تریم</summary>
    [JsonPropertyName("TrimDescf")]
    public string? TrimDescf { get; set; }

    /// <summary>شناسه آپشن</summary>
    [JsonPropertyName("OptionId")]
    public int? OptionId { get; set; }

    /// <summary>آپشن</summary>
    [JsonPropertyName("OptionDescf")]
    public string? OptionDescf { get; set; }

    /// <summary>شناسه محصول بحران</summary>
    [JsonPropertyName("PID")]
    public int? PID { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("Quantity")]
    public int? Quantity { get; set; }
}

/// <summary>
/// محصولات ثبت‌شده یک بحران (S_CriticalPartProductsSelectByCriticalPartId_V2)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (CriticalProductDto) / متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartProductRow : PfResultRow
{
    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شناسه</summary>
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

    /// <summary>شناسه مدل (شاخه «همه موارد»)</summary>
    [JsonPropertyName("CarMoedlId")]
    public int? CarMoedlId { get; set; }

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

    /// <summary>نوع عبور</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>ABS</summary>
    [JsonPropertyName("ABS")]
    public string? ABS { get; set; }

    /// <summary>EMS</summary>
    [JsonPropertyName("EMS")]
    public string? EMS { get; set; }

    /// <summary>کیسه هوا</summary>
    [JsonPropertyName("AirBag")]
    public string? AirBag { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("Quantity")]
    public int? Quantity { get; set; }
}

/// <summary>
/// رکورد بحران مکانیکی (S_CriticalPartsSelectById — T_CriticalParts.*)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه محصول</summary>
    [JsonPropertyName("CarId")]
    public int? CarId { get; set; }

    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public string? UsageFactor { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>موجودی قبلی</summary>
    [JsonPropertyName("PreviousStock")]
    public int? PreviousStock { get; set; }

    /// <summary>مصرف‌شده</summary>
    [JsonPropertyName("Used")]
    public int? Used { get; set; }

    /// <summary>بدنه‌های وارد شده پس از ثبت</summary>
    [JsonPropertyName("TrimInBodyCountAfterRegisteration")]
    public int? TrimInBodyCountAfterRegisteration { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public int? ProductionPossibility { get; set; }

    /// <summary>مجاز از تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityFromDate")]
    public string? ProductionPossibilityFromDate { get; set; }

    /// <summary>مجاز از ساعت</summary>
    [JsonPropertyName("ProductionPossibilityFromTime")]
    public string? ProductionPossibilityFromTime { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>واحد مسئول کسری</summary>
    [JsonPropertyName("DeptId")]
    public int? DeptId { get; set; }

    /// <summary>توضیحات وضعیت تولید</summary>
    [JsonPropertyName("ProductionStateNote")]
    public string? ProductionStateNote { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("ProductionStateLetterNo")]
    public string? ProductionStateLetterNo { get; set; }

    /// <summary>سالن مونتاژ</summary>
    [JsonPropertyName("TrimShopId")]
    public int? TrimShopId { get; set; }

    /// <summary>کاربر</summary>
    [JsonPropertyName("UserId")]
    public int? UserId { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public DateTime? CreateDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>نوع کاهش تولید</summary>
    [JsonPropertyName("ProductionFallId")]
    public int? ProductionFallId { get; set; }

    /// <summary>زمان به‌روزرسانی موجودی</summary>
    [JsonPropertyName("StockUpdateDateTime")]
    public DateTime? StockUpdateDateTime { get; set; }

    /// <summary>خارج از SAP</summary>
    [JsonPropertyName("IsOutOfSap")]
    public bool? IsOutOfSap { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastChange")]
    public string? LastChange { get; set; }
}

/// <summary>
/// جستجوی اقلام بحرانی مکانیکی و سنکرون (S_CriticalPartsSearchByConditions)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (CriticalPartsSearchByConditionsDto) / متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalPartSearchRow : PfResultRow
{
    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeId")]
    public int? CriticalPartTypeId { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>کد ست</summary>
    [JsonPropertyName("SetCode")]
    public string? SetCode { get; set; }

    /// <summary>شرح ست</summary>
    [JsonPropertyName("SetDescF")]
    public string? SetDescF { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("Partdesc")]
    public string? Partdesc { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public string? ProductionPossibility { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public string? UsageFactor { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>مجاز از تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityFromDate")]
    public string? ProductionPossibilityFromDate { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("RegisterDate")]
    public string? RegisterDate { get; set; }

    /// <summary>ساعت ثبت</summary>
    [JsonPropertyName("RegisterTime")]
    public string? RegisterTime { get; set; }

    /// <summary>تاریخ ویرایش</summary>
    [JsonPropertyName("UpdateDate")]
    public string? UpdateDate { get; set; }

    /// <summary>ساعت ویرایش</summary>
    [JsonPropertyName("UpdateTime")]
    public string? UpdateTime { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("StatusDescF")]
    public string? StatusDescF { get; set; }

    /// <summary>نوع کاهش تولید</summary>
    [JsonPropertyName("ProductionFallId")]
    public int? ProductionFallId { get; set; }
}

/// <summary>
/// محصولات شماره فنی از BOP / SAP (S_CriticalPartsSelectFromBOP / S_CriticalPartsSelectFromSAP)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (MechanicalPartsGetProductsFromSAPDto) / متن رویه در SFCDB2.md</para>
/// </summary>
public class CriticalProductCandidateRow : PfResultRow
{
    /// <summary>شناسه خانواده</summary>
    [JsonPropertyName("GroupId")]
    public int? GroupId { get; set; }

    /// <summary>خانواده</summary>
    [JsonPropertyName("GroupDescf")]
    public string? GroupDescf { get; set; }

    /// <summary>شناسه مدل</summary>
    [JsonPropertyName("ModelId")]
    public int? ModelId { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("Model")]
    public string? Model { get; set; }

    /// <summary>شناسه کلاس</summary>
    [JsonPropertyName("ClassId")]
    public int? ClassId { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("ClassCode")]
    public string? ClassCode { get; set; }

    /// <summary>کلاس</summary>
    [JsonPropertyName("Class")]
    public string? Class { get; set; }

    /// <summary>شناسه زیرکلاس</summary>
    [JsonPropertyName("SubClassId")]
    public int? SubClassId { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("SubClassDesc")]
    public string? SubClassDesc { get; set; }

    /// <summary>شناسه رنگ</summary>
    [JsonPropertyName("ColorId")]
    public int? ColorId { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("Color")]
    public string? Color { get; set; }

    /// <summary>شناسه تریم</summary>
    [JsonPropertyName("TrimId")]
    public int? TrimId { get; set; }

    /// <summary>تریم</summary>
    [JsonPropertyName("Trim")]
    public string? Trim { get; set; }

    /// <summary>شناسه آپشن</summary>
    [JsonPropertyName("OptionId")]
    public int? OptionId { get; set; }

    /// <summary>آپشن</summary>
    [JsonPropertyName("Option")]
    public string? Option { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("Quantity")]
    public int? Quantity { get; set; }
}

/// <summary>
/// بدنه‌های مصرف‌کننده مجوز استثنا (S_CriticalPartsProductExceptionsLicenseBodyNoList)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ExceptionLicenseBodyRow : PfResultRow
{
    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModel")]
    public string? CarModel { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("Salone")]
    public string? Salone { get; set; }
}

/// <summary>
/// مجوزهای استثنا (S_CriticalPartsProductExceptionsLisenceList / ...LisenceLog)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ExceptionLicenseRow : PfResultRow
{
    /// <summary>شناسه مجوز</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("SubClass")]
    public string? SubClass { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModel")]
    public string? CarModel { get; set; }

    /// <summary>شماره نامه</summary>
    [JsonPropertyName("LetterNo")]
    public string? LetterNo { get; set; }

    /// <summary>تعداد مجاز</summary>
    [JsonPropertyName("PossibleLicenseCount")]
    public int? PossibleLicenseCount { get; set; }

    /// <summary>مجوز مصرف‌شده</summary>
    [JsonPropertyName("UsedLicense")]
    public int? UsedLicense { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Description")]
    public string? Description { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }
}

/// <summary>
/// شماره فنی‌های دارای استثنا (S_RPT_GetPartNumbersInCriticalPartsProductsException)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ExceptionPartNumberRow : PfResultRow
{
    /// <summary>شناسه استثنا</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("SubClass")]
    public string? SubClass { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModel")]
    public string? CarModel { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>مجوز</summary>
    [JsonPropertyName("License")]
    public string? License { get; set; }

    /// <summary>ظرفیت مجوز</summary>
    [JsonPropertyName("LicenseCapacity")]
    public int? LicenseCapacity { get; set; }
}

/// <summary>
/// گزارش جامع اقلام بحرانی (S_CriticalPartsGeneralReport_Web)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) (توضیح CriticalPartsGeneralReportDto؛ انتهای رویه در SFCDB2 کوتاه شده است)</para>
/// </summary>
public class GeneralReportRow : PfResultRow
{
    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeId")]
    public int? CriticalPartTypeId { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public string? ProductionPossibility { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>بدنه وارد شده به خط</summary>
    [JsonPropertyName("BodyCount")]
    public int? BodyCount { get; set; }

    /// <summary>امکان تولید</summary>
    [JsonPropertyName("ProdPossibilityNo")]
    public int? ProdPossibilityNo { get; set; }

    /// <summary>تعداد استثنا</summary>
    [JsonPropertyName("CriticalPartsExceptionCount")]
    public int? CriticalPartsExceptionCount { get; set; }

    /// <summary>در راه بدنه‌سازی</summary>
    [JsonPropertyName("BodyStockNo")]
    public int? BodyStockNo { get; set; }

    /// <summary>در راه رنگ</summary>
    [JsonPropertyName("PaintStockNo")]
    public int? PaintStockNo { get; set; }

    /// <summary>در راه PBS</summary>
    [JsonPropertyName("PBSStockNo")]
    public int? PBSStockNo { get; set; }

    /// <summary>سفارش در راه</summary>
    [JsonPropertyName("IncommingItemsNo")]
    public int? IncommingItemsNo { get; set; }

    /// <summary>سفارش در سازنده</summary>
    [JsonPropertyName("OnTruckItemsNo")]
    public int? OnTruckItemsNo { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("STATUS")]
    public int? STATUS { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("RegisterShamsiDateTime")]
    public string? RegisterShamsiDateTime { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateShamsiDateTime")]
    public string? LastUpdateShamsiDateTime { get; set; }

    /// <summary>کد ست</summary>
    [JsonPropertyName("SetCode")]
    public string? SetCode { get; set; }

    /// <summary>شرح ست</summary>
    [JsonPropertyName("SetDescF")]
    public string? SetDescF { get; set; }
}

/// <summary>
/// تعداد بدنه‌های در راه یک بحران (S_RPT_IncommingBodiesByCriticalPartId؛ SELECT نهایی در SFCDB2 کوتاه شده — نام ستون‌ها مطابق رویه هم‌خانواده)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class IncomingBodiesCountRow : PfResultRow
{
    /// <summary>در راه بدنه‌سازی</summary>
    [JsonPropertyName("BodyStockNo")]
    public int? BodyStockNo { get; set; }

    /// <summary>در راه رنگ</summary>
    [JsonPropertyName("PaintStockNo")]
    public int? PaintStockNo { get; set; }

    /// <summary>در راه PBS</summary>
    [JsonPropertyName("PbsStockNo")]
    public int? PbsStockNo { get; set; }
}

/// <summary>
/// خلاصه بدنه‌های در راه (S_RPT_IncommingBodiesByCriticalParts؛ SELECT نهایی در SFCDB2 کوتاه شده)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class IncomingBodiesSummaryRow : PfResultRow
{
    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeId")]
    public int? CriticalPartTypeId { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>در راه بدنه‌سازی</summary>
    [JsonPropertyName("BodyStockNo")]
    public int? BodyStockNo { get; set; }

    /// <summary>در راه رنگ</summary>
    [JsonPropertyName("PaintStockNo")]
    public int? PaintStockNo { get; set; }

    /// <summary>در راه PBS</summary>
    [JsonPropertyName("PbsStockNo")]
    public int? PbsStockNo { get; set; }
}

/// <summary>
/// بدنه‌های در راه یک بحران (S_RPT_IncommingBodiesListByCriticalPartId)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class IncomingBodyRow : PfResultRow
{
    /// <summary>شناسه محصول</summary>
    [JsonPropertyName("CarId")]
    public int? CarId { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNo")]
    public string? BodyNo { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("CarModelClassCode")]
    public string? CarModelClassCode { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("ModelDescF")]
    public string? ModelDescF { get; set; }

    /// <summary>آخرین مرحله</summary>
    [JsonPropertyName("LastStepName")]
    public string? LastStepName { get; set; }

    /// <summary>آخرین سالن</summary>
    [JsonPropertyName("LastSaloonDescF")]
    public string? LastSaloonDescF { get; set; }

    /// <summary>تاریخ آخرین مرحله</summary>
    [JsonPropertyName("LastStepDate")]
    public string? LastStepDate { get; set; }

    /// <summary>نوع سفارش</summary>
    [JsonPropertyName("OrderTypeDescF")]
    public string? OrderTypeDescF { get; set; }
}

/// <summary>
/// بحران‌های دارای مجوز تولید با کسری (S_ShortagelicenseTrackingSearchCriticals)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class LicenseTrackingCriticalRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("CriticalPartId")]
    public int? CriticalPartId { get; set; }

    /// <summary>تاریخ ثبت بحران</summary>
    [JsonPropertyName("CriticalCreateDateTime")]
    public string? CriticalCreateDateTime { get; set; }

    /// <summary>وضعیت بحران</summary>
    [JsonPropertyName("CriticalStatusDescF")]
    public string? CriticalStatusDescF { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>کد ست</summary>
    [JsonPropertyName("SetCode")]
    public string? SetCode { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityDescF")]
    public string? ProductionPossibilityDescF { get; set; }
}

/// <summary>
/// گزارش پارامترهای تولید با کسری تعریف‌شده (S_ShortageLicenseTrackingSearch)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class LicenseTrackingSearchRow : PfResultRow
{
    /// <summary>شناسه مجوز</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("CriticalPartId")]
    public int? CriticalPartId { get; set; }

    /// <summary>تاریخ ثبت بحران</summary>
    [JsonPropertyName("CriticalCreateDateTime")]
    public string? CriticalCreateDateTime { get; set; }

    /// <summary>وضعیت بحران</summary>
    [JsonPropertyName("CriticalStatusDescF")]
    public string? CriticalStatusDescF { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>کد ست</summary>
    [JsonPropertyName("SetCode")]
    public string? SetCode { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>تاریخ و زمان ثبت</summary>
    [JsonPropertyName("CreateDateTime")]
    public string? CreateDateTime { get; set; }

    /// <summary>ساعت ثبت</summary>
    [JsonPropertyName("CreateTime")]
    public string? CreateTime { get; set; }

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

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public string? LastUpdateDateTime { get; set; }

    /// <summary>وضعیت مجوز</summary>
    [JsonPropertyName("LicenseStatus")]
    public int? LicenseStatus { get; set; }

    /// <summary>وضعیت مجوز</summary>
    [JsonPropertyName("LicenseStatusDescF")]
    public string? LicenseStatusDescF { get; set; }
}

/// <summary>
/// دلایل عدم امکان تولید (S_ProgNotPossibilityReasonSelectAll)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class NotPossibleReasonRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>کد</summary>
    [JsonPropertyName("Code")]
    public string? Code { get; set; }

    /// <summary>شرح</summary>
    [JsonPropertyName("DescF")]
    public string? DescF { get; set; }

    /// <summary>شرح انگلیسی</summary>
    [JsonPropertyName("DescE")]
    public string? DescE { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }
}

/// <summary>
/// قطعه در فهرست مرجع قطعات کسری (SIMP_SelectParInShortagePartList)
/// <para>منبع ستون‌ها: DTO پاسخ بک‌اند قبلی (SFC.Domain.Shared) / متن رویه در SFCDB2.md</para>
/// </summary>
public class PartInShortageListRow : PfResultRow
{
    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>نوع قطعه</summary>
    [JsonPropertyName("PartTypeDescF")]
    public string? PartTypeDescF { get; set; }
}

/// <summary>
/// امکان تولید به تفکیک سالن مونتاژ (S_RPT_DESC_ProdPossibilityByTrimShop)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ProdPossibilityByTrimShopRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("CriticalPartsId")]
    public int? CriticalPartsId { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>بدنه وارد شده به خط</summary>
    [JsonPropertyName("BodyCount")]
    public int? BodyCount { get; set; }

    /// <summary>امکان تولید</summary>
    [JsonPropertyName("ProdPossibilityNo")]
    public int? ProdPossibilityNo { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProdByShortageNo")]
    public int? PossibleProdByShortageNo { get; set; }

    /// <summary>مجاز تا</summary>
    [JsonPropertyName("ProdPosToDateTime")]
    public string? ProdPosToDateTime { get; set; }

    /// <summary>نوع قطعه کسری</summary>
    [JsonPropertyName("ShortagePartType")]
    public string? ShortagePartType { get; set; }

    /// <summary>واحد مسئول</summary>
    [JsonPropertyName("DeptDescF")]
    public string? DeptDescF { get; set; }

    /// <summary>تکمیل‌کاری</summary>
    [JsonPropertyName("TakIsPossible")]
    public string? TakIsPossible { get; set; }

    /// <summary>زمان ثبت</summary>
    [JsonPropertyName("RegisterDateTime")]
    public DateTime? RegisterDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("RegisterShamsiDateTime")]
    public string? RegisterShamsiDateTime { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateShamsiDateTime")]
    public string? LastUpdateShamsiDateTime { get; set; }
}

/// <summary>
/// بدنه‌های استثنا (S_CriticalProductExceptionList)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ProductExceptionRow : PfResultRow
{
    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("CarModel")]
    public string? CarModel { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("Salone")]
    public string? Salone { get; set; }
}

/// <summary>
/// امکان تولید (S_RptCriticalPartsProduction_Mechanical / _Synchron / S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ProductionPossibilityRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>کد ست / شماره فنی</summary>
    [JsonPropertyName("SetCodePartNo")]
    public string? SetCodePartNo { get; set; }

    /// <summary>شرح ست / شرح فنی</summary>
    [JsonPropertyName("SetDescfPartDesc")]
    public string? SetDescfPartDesc { get; set; }

    /// <summary>کد رنگ محصول</summary>
    [JsonPropertyName("ProductColorCode")]
    public string? ProductColorCode { get; set; }

    /// <summary>رنگ محصول</summary>
    [JsonPropertyName("ProductColorName")]
    public string? ProductColorName { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>بدنه وارد شده به خط</summary>
    [JsonPropertyName("BodyCount")]
    public int? BodyCount { get; set; }

    /// <summary>امکان تولید</summary>
    [JsonPropertyName("ProdPossibilityNo")]
    public int? ProdPossibilityNo { get; set; }

    /// <summary>امکان تولید (بدنه)</summary>
    [JsonPropertyName("BodyProdPossibilityNo")]
    public int? BodyProdPossibilityNo { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibilityDescF")]
    public string? ProductionPossibilityDescF { get; set; }

    /// <summary>مجوز مصرف‌شده</summary>
    [JsonPropertyName("CriticalUsedLicense")]
    public string? CriticalUsedLicense { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Note")]
    public string? Note { get; set; }

    /// <summary>مجوز باقی‌مانده</summary>
    [JsonPropertyName("RemainingShortageLicense")]
    public string? RemainingShortageLicense { get; set; }

    /// <summary>سالن</summary>
    [JsonPropertyName("SaloonDescF")]
    public string? SaloonDescF { get; set; }

    /// <summary>آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateShamsiDateTime")]
    public string? LastUpdateShamsiDateTime { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("RegisterShamsiDateTime")]
    public string? RegisterShamsiDateTime { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeId")]
    public int? CriticalPartTypeId { get; set; }

    /// <summary>نوع بحران</summary>
    [JsonPropertyName("CriticalPartTypeDescF")]
    public string? CriticalPartTypeDescF { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>زمان ثبت</summary>
    [JsonPropertyName("RegisterDateTime")]
    public DateTime? RegisterDateTime { get; set; }

    /// <summary>تاریخ و زمان آخرین تغییر</summary>
    [JsonPropertyName("LastUpdateDateTime")]
    public DateTime? LastUpdateDateTime { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>تعداد استثنا</summary>
    [JsonPropertyName("CriticalPartsExceptionCount")]
    public int? CriticalPartsExceptionCount { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }
}

/// <summary>
/// اقلام بحرانی فعال وارد شده به شرکت (S_CriticalPart_RecievedParts — بدنه رویه فعلاً RETURN است؛ ستون‌ها از SELECT غیرفعال‌شده)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ReceivedPartRow : PfResultRow
{
    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNumber")]
    public string? PartNumber { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>تعداد</summary>
    [JsonPropertyName("Quantity")]
    public string? Quantity { get; set; }

    /// <summary>تاریخ دریافت</summary>
    [JsonPropertyName("ReceiptDate")]
    public string? ReceiptDate { get; set; }

    /// <summary>ساعت دریافت</summary>
    [JsonPropertyName("ReceiptTime")]
    public string? ReceiptTime { get; set; }
}

/// <summary>
/// لاگ انتقال اقلام بحرانی از SAP (S_SapCriticalLog)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SapCriticalLogRow : PfResultRow
{
    /// <summary>شناسه</summary>
    [JsonPropertyName("Id")]
    public int? Id { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }

    /// <summary>ساعت</summary>
    [JsonPropertyName("CreatTime")]
    public string? CreatTime { get; set; }

    /// <summary>تعداد</summary>
    [JsonPropertyName("Total")]
    public string? Total { get; set; }
}

/// <summary>
/// مجوزهای تولید با کسری یک بحران (S_ShortageLicenseSelectAllTrackingInfo / S_ShortageLicenseTrackingSelectAll)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class ShortageLicenseTrackingRow : PfResultRow
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

    /// <summary>مجوز باقی‌مانده</summary>
    [JsonPropertyName("RemainingLicense")]
    public string? RemainingLicense { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }

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
/// تاریخچه موجودی شماره فنی (SEXT_CriticalPartsStockLog)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class StockLogRow : PfResultRow
{
    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>موجودی قبلی</summary>
    [JsonPropertyName("OldStock")]
    public string? OldStock { get; set; }

    /// <summary>موجودی جدید</summary>
    [JsonPropertyName("NewStock")]
    public string? NewStock { get; set; }

    /// <summary>تاریخ رویداد</summary>
    [JsonPropertyName("EventDate")]
    public string? EventDate { get; set; }

    /// <summary>تاریخ ثبت بحران</summary>
    [JsonPropertyName("CriticalDate")]
    public string? CriticalDate { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>ورود به مونتاژ</summary>
    [JsonPropertyName("TrimInCount")]
    public int? TrimInCount { get; set; }

    /// <summary>امکان تولید</summary>
    [JsonPropertyName("ProductionPossibillity")]
    public int? ProductionPossibillity { get; set; }
}

/// <summary>
/// مقایسه زیرکلاس‌های SAP با SFC (S_GetContrastOfSubClasses؛ نوع ۱ ستون‌های @Tbl و نوع ۲ ستون‌های @Temp1 را برمی‌گرداند)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class SubClassContrastRow : PfResultRow
{
    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("SubClassCode")]
    public string? SubClassCode { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("CriticalPartsId")]
    public int? CriticalPartsId { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>نوع مغایرت</summary>
    [JsonPropertyName("Status")]
    public string? Status { get; set; }
}

/// <summary>
/// دلایل عدم امکان ورود خودرو به مونتاژ (S_TrimInNotPossibleProductsSelectAll)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class TrimInNotPossibleRow : PfResultRow
{
    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public string? ProductionPossibility { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>توضیحات</summary>
    [JsonPropertyName("Note")]
    public string? Note { get; set; }

    /// <summary>توضیحات قطعه</summary>
    [JsonPropertyName("CriticalPartNote")]
    public string? CriticalPartNote { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("GroupDesc")]
    public string? GroupDesc { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("ModelDescF")]
    public string? ModelDescF { get; set; }

    /// <summary>کد کلاس</summary>
    [JsonPropertyName("ModelClassCode")]
    public string? ModelClassCode { get; set; }

    /// <summary>رنگ</summary>
    [JsonPropertyName("ColorDescF")]
    public string? ColorDescF { get; set; }

    /// <summary>تریم</summary>
    [JsonPropertyName("TrimDescF")]
    public string? TrimDescF { get; set; }
}

/// <summary>
/// تعداد بدنه‌های ورودی به مونتاژ پس از ثبت (S_TrimShopBodyCount_List)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class TrimShopBodyCountRow : PfResultRow
{
    /// <summary>خط بدنه‌سازی</summary>
    [JsonPropertyName("BIWSite")]
    public string? BIWSite { get; set; }

    /// <summary>زیرکلاس</summary>
    [JsonPropertyName("Subclass")]
    public string? Subclass { get; set; }

    /// <summary>تعداد بدنه</summary>
    [JsonPropertyName("TrimShopBodyCount")]
    public int? TrimShopBodyCount { get; set; }
}

/// <summary>
/// بدنه‌های بحرانی ورودی به مونتاژ (S_TrimShopCriticalBody_SelectAll)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class TrimShopCriticalBodyRow : PfResultRow
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
    [JsonPropertyName("PartDesc")]
    public string? PartDesc { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("PartDescF")]
    public string? PartDescF { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("CreateDate")]
    public string? CreateDate { get; set; }
}

/// <summary>
/// بحران‌های یک سالن در بازه تاریخ (S_CriticalPartSearch)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class TrimShopCriticalPartRow : PfResultRow
{
    /// <summary>شناسه بحران</summary>
    [JsonPropertyName("id")]
    public int? Id { get; set; }

    /// <summary>شناسه قطعه کسری</summary>
    [JsonPropertyName("ShortagePartListId")]
    public int? ShortagePartListId { get; set; }

    /// <summary>شماره فنی</summary>
    [JsonPropertyName("PartNo")]
    public string? PartNo { get; set; }

    /// <summary>شرح قطعه</summary>
    [JsonPropertyName("Partdesc")]
    public string? Partdesc { get; set; }

    /// <summary>موجودی</summary>
    [JsonPropertyName("Stock")]
    public int? Stock { get; set; }

    /// <summary>ضریب مصرف</summary>
    [JsonPropertyName("UsageFactor")]
    public string? UsageFactor { get; set; }

    /// <summary>شماره فنی جایگزین</summary>
    [JsonPropertyName("AlterNativePartNo")]
    public string? AlterNativePartNo { get; set; }

    /// <summary>گروه قطعه</summary>
    [JsonPropertyName("PartGroup")]
    public string? PartGroup { get; set; }

    /// <summary>شرایط تولید</summary>
    [JsonPropertyName("ProductionPossibility")]
    public string? ProductionPossibility { get; set; }

    /// <summary>مجاز تا تاریخ</summary>
    [JsonPropertyName("ProductionPossibilityToDate")]
    public string? ProductionPossibilityToDate { get; set; }

    /// <summary>مجاز تا ساعت</summary>
    [JsonPropertyName("ProductionPossibilityToTime")]
    public string? ProductionPossibilityToTime { get; set; }

    /// <summary>تعداد مجاز تولید با کسری</summary>
    [JsonPropertyName("PossibleProductionNo")]
    public int? PossibleProductionNo { get; set; }

    /// <summary>تاریخ ثبت</summary>
    [JsonPropertyName("RegisterDate")]
    public string? RegisterDate { get; set; }

    /// <summary>ساعت ثبت</summary>
    [JsonPropertyName("RegisterTime")]
    public string? RegisterTime { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("Status")]
    public int? Status { get; set; }

    /// <summary>وضعیت</summary>
    [JsonPropertyName("StatusDesc")]
    public string? StatusDesc { get; set; }
}

/// <summary>
/// بدنه‌های مصرف‌کننده مجوز تولید (S_SelectBodiesRelatedToUsedLicense)
/// <para>منبع ستون‌ها: متن رویه در SFCDB2.md</para>
/// </summary>
public class UsedLicenseBodyRow : PfResultRow
{
    /// <summary>شناسه محصول</summary>
    [JsonPropertyName("CarId")]
    public int? CarId { get; set; }

    /// <summary>شماره بدنه</summary>
    [JsonPropertyName("BodyNO")]
    public string? BodyNO { get; set; }

    /// <summary>کد زیرکلاس</summary>
    [JsonPropertyName("SubClassCode")]
    public string? SubClassCode { get; set; }

    /// <summary>خانواده محصول</summary>
    [JsonPropertyName("GroupDescF")]
    public string? GroupDescF { get; set; }

    /// <summary>مدل</summary>
    [JsonPropertyName("ModelDescF")]
    public string? ModelDescF { get; set; }

    /// <summary>کد آخرین مرحله</summary>
    [JsonPropertyName("LastStepCode")]
    public string? LastStepCode { get; set; }

    /// <summary>آخرین مرحله</summary>
    [JsonPropertyName("LastStepDescF")]
    public string? LastStepDescF { get; set; }

    /// <summary>تاریخ آخرین مرحله</summary>
    [JsonPropertyName("LastStepDate")]
    public string? LastStepDate { get; set; }

    /// <summary>شیفت</summary>
    [JsonPropertyName("LastShiftDescF")]
    public string? LastShiftDescF { get; set; }
}
