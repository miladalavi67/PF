using System;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

/// <summary>اطلاعات بدنه (SEXT_BodyInfo_V2)</summary>
public class BodyInfoRow
{
    /// <summary>شماره بدنه</summary>
    public string BodyNO { get; set; } = default!;
    /// <summary>شماره موتور</summary>
    public string? EngineNO { get; set; }
    /// <summary>کد کیفی / VIN</summary>
    public string? VIN { get; set; }
    /// <summary>مشخصات خودرو</summary>
    public string? CarAttribute { get; set; }
    /// <summary>کد زیرکلاس</summary>
    public string? SubClassCode { get; set; }
    /// <summary>رنگ</summary>
    public string? Color { get; set; }
    /// <summary>مرحله فعلی</summary>
    public string? StepDescF { get; set; }
    /// <summary>سالن</summary>
    public string? Saloon { get; set; }
    /// <summary>شناسه محصول</summary>
    public int CarId { get; set; }
    /// <summary>شناسه آخرین مرحله</summary>
    public int? LastProductionStepId { get; set; }
    /// <summary>شناسه آخرین سالن</summary>
    public int? LastSaloonId { get; set; }
    /// <summary>شناسه رنگ</summary>
    public int? ColorId { get; set; }
    /// <summary>تاریخ مرحله</summary>
    public string? StepDate { get; set; }
    /// <summary>ساعت مرحله</summary>
    public string? StepTime { get; set; }
    /// <summary>شناسه خط بدنه‌سازی (BIWSiteId)</summary>
    public int? B_VIN_BIWSiteID { get; set; }
    /// <summary>شرح سالن</summary>
    public string? SaloonDescF { get; set; }
}

/// <summary>محصولات مصرف‌کننده یک شماره فنی (BOM)</summary>
public class PartProductRow
{
    /// <summary>ردیف</summary>
    public int RowNo { get; set; }
    /// <summary>شرح محصول</summary>
    public string? CarDescF { get; set; }
    /// <summary>شرح مدل</summary>
    public string? CarModelDescF { get; set; }
    /// <summary>کد مدل</summary>
    public string? CarModelCode { get; set; }
    /// <summary>ضریب مصرف</summary>
    public int Quantity { get; set; }
    /// <summary>خانواده محصول</summary>
    public string? CarGroupDescF { get; set; }
    /// <summary>کد کلاس</summary>
    public string? CarClassCode { get; set; }
    /// <summary>شناسه کلاس</summary>
    public int CarClassId { get; set; }
    /// <summary>شناسه مدل</summary>
    public string? CarModelId { get; set; }
    /// <summary>زیرکلاس</summary>
    public string? Subclass { get; set; }
}
