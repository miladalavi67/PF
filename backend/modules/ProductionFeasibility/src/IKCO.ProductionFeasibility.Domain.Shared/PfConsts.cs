namespace IKCO.ProductionFeasibility;

public static class PfConsts
{
    /// <summary>طول شماره بدنه (char(8))</summary>
    public const int BodyNoLength = 8;

    /// <summary>حداکثر طول شماره فنی</summary>
    public const int PartNoLength = 30;

    /// <summary>طول تاریخ شمسی yyyy/MM/dd</summary>
    public const int ShamsiDateLength = 10;

    /// <summary>طول زمان HH:mm:ss</summary>
    public const int TimeLength = 8;

    /// <summary>حداکثر طول توضیحات کوتاه (varchar(100))</summary>
    public const int NoteLength = 100;

    /// <summary>حداکثر طول توضیحات بلند (nvarchar(1000))</summary>
    public const int LongNoteLength = 1000;

    /// <summary>حداکثر طول شماره نامه</summary>
    public const int LetterNoLength = 50;

    /// <summary>حداکثر تعداد ردیف در ورودی‌های گروهی (اکسل)</summary>
    public const int MaxBatchRows = 5000;
}
