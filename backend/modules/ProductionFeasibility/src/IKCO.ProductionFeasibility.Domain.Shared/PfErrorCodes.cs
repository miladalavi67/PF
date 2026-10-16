namespace IKCO.ProductionFeasibility;

/// <summary>
/// کدهای خطای استاندارد IKCO برای سامانه جامع امکان تولید.
/// 0 موفق، 401 عدم احراز هویت، 601 تا 700 خطای زیرساخت، 2000 تا 9999 خطای منطقی.
/// </summary>
public enum PfErrorCodes
{
    /// <summary>عملیات با موفقیت انجام شد</summary>
    RequestSuccessful = 0,

    /// <summary>کاربر احراز هویت نشده است</summary>
    UnAuthorized = 401,

    #region Network Error from 601 to 700
    /// <summary>ارتباط با سرور پایگاه داده برقرار نشد</summary>
    SQLServerNetworkNotResponding = 602,

    /// <summary>زمان اجرای درخواست در پایگاه داده به پایان رسید</summary>
    DatabaseCommandTimeout = 603,

    /// <summary>ارتباط با سرویس خارجی (SAP) برقرار نشد</summary>
    ExternalServiceUnavailable = 604,
    #endregion

    #region Logical Error from 2000 to 9999
    /// <summary>کاربر جاری در جدول کاربران امکان تولید (PF.B_User) تعریف نشده است</summary>
    PfUserNotFound = 2000,

    /// <summary>کاربر جاری در جدول اشخاص کنترل تولید (dbo.B_Person) تعریف نشده است</summary>
    PersonNotFound = 2001,

    /// <summary>کاربر به سالن یا سایت انتخاب‌شده دسترسی ندارد</summary>
    SaloonAccessDenied = 2002,

    /// <summary>پیام خطای برگشتی از رویه ذخیره‌شده (RAISERROR / THROW)</summary>
    StoredProcedureError = 2003,

    /// <summary>رکورد تکراری است (نقض کلید یکتا در پایگاه داده)</summary>
    DuplicateRecord = 2004,

    /// <summary>رکورد وابسته وجود دارد یا کلید خارجی نامعتبر است</summary>
    ReferenceConstraintViolation = 2005,

    /// <summary>رکورد مورد نظر یافت نشد</summary>
    RecordNotFound = 2006,

    /// <summary>شماره فنی معتبر نیست</summary>
    PartNumberInvalid = 2007,

    /// <summary>شماره بدنه معتبر نیست</summary>
    BodyNumberInvalid = 2008,

    /// <summary>ورودی عملیات معتبر نیست</summary>
    InputInvalid = 2009,

    /// <summary>تاریخ شمسی معتبر نیست</summary>
    ShamsiDateInvalid = 2010,

    /// <summary>فهرست شناسه‌ها معتبر نیست</summary>
    IdListInvalid = 2011,

    /// <summary>عملیات انجام نشد (نتیجه منفی رویه ذخیره‌شده)</summary>
    OperationFailed = 2012,

    /// <summary>کلید فهرست مرجع معتبر نیست</summary>
    LookupNotFound = 2014,

    /// <summary>ورودی گروهی (اکسل) معتبر نیست</summary>
    BatchInputInvalid = 2015,

    /// <summary>پاسخ سرویس SAP قابل پردازش نیست</summary>
    ExternalServiceResponseInvalid = 2016,

    /// <summary>الگوریتم رفع کسری در حال اجرا است یا شرایط اجرا برقرار نیست</summary>
    OptimizationNotAllowed = 2017,
    #endregion
}
