/**
 * ستون‌های قابل نمایش هر گرید، دقیقاً مطابق تعریف گرید در فرم ویندوزی (فقط ستون‌های IsVisible = true، به همان ترتیب و عنوان).
 * گرید فقط همین ستون‌ها را نشان می‌دهد و سایر ستون‌های خروجی رویه (شناسه‌ها، ستون‌های کمکی و ExtraColumns) نمایش داده نمی‌شوند.
 *
 * منبع: فرم‌های PF از کد پشت فرم (متدهای SetGrv*، استخراج با `_src/gen/grid_cols.py`)؛ فرم‌های SFC (مکانیکی/سنکرون) که فقط resx دارند
 * از تصویر فرم (`pics/`) و در نبود عنوان ستون در تصویر از فرم متناظر PF (frmTrimCriticalSearch، frmRPTTrackingTrimCriticalPart و …).
 * نام فیلد همان نام ستون خروجی رویه است (تطبیق بدون حساسیت به حروف کوچک/بزرگ). فیلد `__rowId` شماره ردیف سمت کلاینت است.
 */
export interface IGridColumn {
  field: string;
  header: string;
  width?: number;
}

const c = (field: string, header: string, width?: number): IGridColumn => ({ field, header, width });

/** ردیف سمت کلاینت برای خروجی‌هایی که ستون ردیف ندارند (مانند ستون «ردیف» گرید ویندوزی) */
const rowNo = c("__rowId", "ردیف", 70);

export const gridColumns = {
  // ───────── بدنه‌سازی ─────────
  /** frmBodyCriticalSearch / frmRPTBodyCriticalSearch — grvParts */
  bodyCriticalSearch: [
    c("PartNo", "شماره فنی"),
    c("PartDescF", "شرح فنی"),
    c("TotalStock", "موجودی مجموعه (مقاومت)"),
    c("RawStock", "موجودی خام"),
    c("CriticalPartNote", "توضیحات"),
    c("ProductionPosDesF", "وضعیت امکان تولید"),
    c("BIWSiteDescF", "سالن"),
    c("AlternativePartNo", "قطعه جایگزین"),
    c("CreateDateTime", "تاریخ ثبت"),
    c("LastUpdateDateTime", "تاریخ ویرایش"),
    c("StatusDescF", "وضعیت"),
  ],
  /** frmBodyCriticalSearch / frmRPTBodyCriticalSearch / frmBodyCriticalParts (ویرایش) — grvProduct */
  bodyCriticalProducts: [
    c("UsageFactor", "ضریب مصرف"),
    c("CarGroupeDesc", "گروه محصول"),
    c("CarGroupeCode", "کد محصول"),
    c("CarModelDesc", "محصول"),
    c("CarClassCode", "کد کلاس"),
    c("CarClassDesc", "کلاس"),
  ],
  /** frmRPTTrackingCriticalParts — grvCritical */
  bodyTracking: [
    c("RowNumber", "ردیف", 70),
    c("PartType", "نوع قطعه"),
    c("PartNo", "شماره فنی"),
    c("PartDescF", "شرح فنی"),
    c("TotalStock", "موجودی مجموعه(مقاومت)"),
    c("RawStock", "موجودی خام"),
    c("Total", "ت.ب پیش تخصیص شده"),
    c("Description", "وضعیت امکان تولید"),
    c("ProductionPos", "امکان تولید"),
    c("PossibleProductionNo", "تعداد مجوز"),
    c("UsedLicense", "تعداد مجوز مانده"),
    c("CriticalDescription", "توضیحات"),
    c("CreateDateTime", "تاریخ ثبت"),
    c("LastUpdateDateTime", "تاریخ آخرین بروز رسانی"),
  ],
  /** frmRPTTrackingCriticalParts — grvProducts */
  bodyTrackingProducts: [
    c("RowNumber", "ردیف", 70),
    c("BodyNO", "شماره بدنه شاخص"),
    c("CarGroupeDesc", "گروه محصول"),
    c("CarGroupeCode", "کد محصول"),
    c("CarModelDesc", "محصول"),
    c("CarClassCode", "کد کلاس"),
    c("CarClassDesc", "کلاس"),
  ],
  /** frmRPTPreAssignCriticalBody — grvCritical */
  bodyPreAssignCritical: [c("RowNumber", "ردیف", 70), c("BodyNO", "شماره بدنه"), c("PartNo", "شماره فنی"), c("PartDescF", "شرح فنی"), c("CreateDate", "تاریخ ایجاد")],
  /** frmRPTNotPossibleProductsToStepAssign — grvCritical */
  bodyConflicts: [
    c("RowNumber", "ردیف", 70),
    c("CriticalPartType", "نوع بحران"),
    c("BodyNO", "شماره بدنه"),
    c("PartNo", "شماره فنی"),
    c("PartDescF", "شرح فنی"),
    c("ErrorDescF", "شرح خطا"),
    c("CreateDate", "تاریخ ایجاد"),
  ],
  /** frmRPTGetProductsOfBIWSite — grdShowResult */
  bodyProductsOfSites: [c("Row", "ردیف", 70), c("CarModelCode", "کد محصول"), c("CarModelDescF", "شرح محصول"), c("VINBIWSiteDescF", "نام سالن")],
  /** frmRPTPreassignBodyCount — dgvShowResult */
  bodyPreAssignCount: [c("BIWSite", "سالن ساخت بدنه"), c("ClassCode", "کد کلاس"), c("PreAssignBodyCount", "تعداد بدنه پیش تخصیص شده")],
  /** frmRPTShortageLicenseTracking / frmCriticalPartsProdPosChange — grvCriticals */
  licenseTracking: [
    c("ProductionPossibilityDescF", "شرایط تولید"),
    c("PossibleProductionNo", "مجوز تولید با کسری"),
    c("ProductionPossibilityToDate", "تا تاریخ"),
    c("ProductionPossibilityToTime", "تا زمان"),
    c("UsedLicense", "کسری مصرفی مجوز"),
    c("Status", "وضعیت"),
    c("CreateDateTime", "زمان ثبت"),
    c("UserName", "کاربر ثبت کننده"),
    c("LastUpdateByName", "اصلاح آخرین مجوز توسط"),
    c("LastUpdateDateTime", "تاریخ آخرین تغییرات"),
  ],
  /** frmEventLog — grvCriticals */
  eventLog: [
    c("AtrributeDescF", "خصیصه"),
    c("Oldvalue", "مقدار قبلی"),
    c("Newvalue", "مقدار جدید"),
    c("Requestno", "شماره نامه"),
    c("ProductionStateNote", "توضیحات تغییر بحران"),
    c("PartNoDesc", "توضیحات قطعه بحرانی"),
    c("EventDate", "تاریخ ایجاد"),
    c("UserName", "کاربر"),
  ],
  /** frmCriticalExcelLogError */
  bodyExcelErrors: [c("BodyNO", "شماره بدنه"), c("CarClassCode", "کد کلاس"), c("PartNo", "شماره فنی"), c("PartDescF", "شرح فنی"), c("Message", "پیام")],

  // ───────── مونتاژ - مکانیکی ─────────
  /** frmCriticalPartSearch (SFC؛ ستون‌ها از frmTrimCriticalSearch.grvParts به‌علاوه کد/شرح ست برای بحران سنکرون) */
  mechCriticalSearch: [
    c("CriticalPartTypeDescF", "نوع بحران"),
    c("SetCode", "کد ست"),
    c("SetDescF", "شرح ست"),
    c("PartNo", "شماره فنی"),
    c("Partdesc", "شرح فنی"),
    c("AlterNativePartNo", "قطعه جایگزین"),
    c("UsageFactor", "ضریب مصرف"),
    c("Stock", "موجودی"),
    c("SaloonDescF", "سالن"),
    c("ProductionPossibility", "وضعیت امکان تولید"),
    c("RegisterDate", "تاریخ ثبت"),
    c("UpdateDate", "تاریخ ویرایش"),
    c("StatusDescF", "وضعیت"),
  ],
  /** محصولات بحران مکانیکی (frmTrimCriticalSearch.grvProduct و frmMechanicalTrimCriticalParts) */
  mechCriticalProducts: [
    c("CarGroupDescF", "گروه محصول"),
    c("CarModelDescF", "محصول"),
    c("CarModelCode", "کد محصول"),
    c("CarClassCode", "کد کلاس"),
    c("CarSubClassCode", "کد زیرکلاس"),
    c("Quantity", "ضریب مصرف"),
  ],
  /** frmCriticalPart (SFC، تصویر): خانواده محصول، محصول، کد کلاس، کلاس، زیرکلاس، رنگ، تریم */
  mechProductCandidates: [
    c("GroupDescf", "خانواده محصول"),
    c("Model", "محصول"),
    c("ClassCode", "کد کلاس"),
    c("Class", "کلاس"),
    c("SubClassDesc", "زیرکلاس"),
    c("Color", "رنگ"),
    c("Trim", "تریم"),
  ],
  /** frmRPTProductionPossibility (SFC، تصویر) — ردیف، فنی/ست، شرح فنی/شرح ست، کدرنگ، شرح رنگ، موجودی، ت.ب، امکان تولید، فنی آلترناتیو، توضیحات، وضعیت، … */
  mechProductionPossibility: [
    rowNo,
    c("SetCodePartNo", "فنی/ست"),
    c("SetDescfPartDesc", "شرح فنی/شرح ست"),
    c("ProductColorCode", "کدرنگ"),
    c("ProductColorName", "شرح رنگ"),
    c("Stock", "موجودی"),
    c("BodyCount", "ت.ب"),
    c("BodyProdPossibilityNo", "امکان تولید"),
    c("AlterNativePartNo", "فنی آلترناتیو"),
    c("Note", "توضیحات"),
    c("ProductionPossibilityDescF", "وضعیت"),
    c("CriticalPartTypeDescF", "نوع"),
    c("SaloonDescF", "سالن"),
    c("RegisterShamsiDateTime", "تاریخ ثبت"),
    c("LastUpdateShamsiDateTime", "تاریخ آخرین بروزرسانی"),
  ],
  /** frm_RPT_ST_IncommingBodiesByCP (SFC، تصویر و راهنمای اختصارها) */
  mechGeneralReport: [
    rowNo,
    c("CriticalPartTypeDescF", "نوع"),
    c("SetCode", "کد ست"),
    c("SetDescF", "شرح ست"),
    c("PartNo", "شماره فنی"),
    c("PartDesc", "شرح فنی"),
    c("Stock", "موجودی"),
    c("BodyCount", "ت.ب.و.ش.ب.خ"),
    c("ProdPossibilityNo", "ا.ت"),
    c("BodyStockNo", "م.ف.بدنه"),
    c("PaintStockNo", "م.ف.رنگ"),
    c("PBSStockNo", "م.ف.PBS"),
    c("ProductionPossibility", "وضعیت امکان تولید"),
    c("RegisterShamsiDateTime", "تاریخ ثبت"),
    c("LastUpdateShamsiDateTime", "تاریخ آخرین بروزرسانی"),
  ],
  /** frm_RPT_ST_IncommingBodiesByCP — جدول «محصولات» (بدنه‌های وارد شده) */
  mechIncomingBodies: [
    rowNo,
    c("BodyNo", "شماره بدنه"),
    c("CarModelClassCode", "کد کلاس"),
    c("ModelDescF", "محصول"),
    c("LastStepName", "آخرین مرحله"),
    c("LastSaloonDescF", "آخرین سالن"),
    c("LastStepDate", "تاریخ آخرین مرحله"),
    c("OrderTypeDescF", "نوع سفارش"),
  ],
  /** frmTrimShopBodyCount — dgvShowResult */
  mechTrimShopBodyCount: [c("BIWSite", "سالن ساخت بدنه"), c("Subclass", "کد زیرکلاس"), c("TrimShopBodyCount", "تعداد بدنه ورود به مونتاژ شده")],
  /** frmRPTContrastOfSubClasses (SFC، تصویر): کد زیرکلاس، شماره فنی، شرح فنی، وضعیت */
  mechSubClassContrast: [c("SubClassCode", "کد زیرکلاس"), c("PartNo", "شماره فنی"), c("PartDesc", "شرح فنی"), c("Status", "وضعیت")],
  /** لاگ مجوزهای بحران (frmCriticalShortageLicenseTracking؛ ستون‌ها مطابق frmCriticalPartsProdPosChange.grvCriticals) */
  mechLicenseTrackingList: [
    c("CriticalPartTypeDescF", "نوع بحران"),
    c("SetCode", "کد ست"),
    c("PartNo", "شماره فنی"),
    c("PartDesc", "شرح فنی"),
    c("ProductionPossibilityDescF", "شرایط تولید"),
    c("PossibleProductionNo", "مجوز تولید با کسری"),
    c("ProductionPossibilityToDate", "تا تاریخ"),
    c("ProductionPossibilityToTime", "تا زمان"),
    c("UsedLicense", "کسری مصرفی مجوز"),
    c("LicenseStatusDescF", "وضعیت"),
    c("CreateDateTime", "زمان ثبت"),
    c("LastUpdateDateTime", "تاریخ آخرین تغییرات"),
  ],
  /** frm_Rpt_CriticalPartsStockLog */
  mechStockLog: [
    c("PartNo", "شماره فنی"),
    c("BodyNO", "شماره بدنه"),
    c("OldStock", "موجودی قبلی"),
    c("NewStock", "موجودی جدید"),
    c("TrimInCount", "ت.ب ورود به مونتاژ"),
    c("ProductionPossibillity", "امکان تولید"),
    c("EventDate", "تاریخ رویداد"),
    c("CriticalDate", "تاریخ بحران"),
  ],
  /** frm_Rpt_SapCriticalLog */
  mechSapLog: [c("PartNo", "شماره فنی"), c("PartDesc", "شرح فنی"), c("Total", "تعداد"), c("Status", "وضعیت"), c("CreateDate", "تاریخ"), c("CreatTime", "ساعت")],
  /** لاگ مغایرت SAP */
  mechFailedSap: [c("PartNo", "شماره فنی"), c("PartDesc", "شرح فنی"), c("CreateDateTime", "تاریخ")],
  /** frm_Rpt_Sap_Asn */
  mechAsn: [c("PartNo", "شماره فنی"), c("Total", "تعداد"), c("Unit", "واحد"), c("WarehouseCode", "انبار"), c("CreateDate", "تاریخ"), c("CreatTime", "ساعت")],

  // ───────── مونتاژ - سنکرون ─────────
  /** frmCriticalPartSet — «ریز قطعات» */
  syncSetParts: [c("PartNo", "شماره فنی"), c("PartDescF", "شرح فنی")],
  /** frmCriticalPartSet — «رنگ» */
  syncSetColors: [c("CarColorCode", "کد رنگ"), c("CarColorDescF", "رنگ")],
  /** frmCriticalPartSet — «سالن‌های مشترک» / frmCriticalPartSetIndicatorCars */
  syncSetSaloons: [c("SaloonCode", "کد سالن"), c("SaloonDescF", "سالن"), c("IndicatorCarBodyNo", "بدنه شاخص")],
  /** frmCriticalPartSet — «خانواده/محصول/کلاس/زیرکلاس/تریم/تودوزی/آپشن/رنگ/سالن» */
  syncSetProducts: [
    c("CarGroupDescF", "خانواده"),
    c("CarModelDescF", "محصول"),
    c("CarClassCode", "کلاس"),
    c("CarSubClassCode", "زیرکلاس"),
    c("CarTrimDescF", "تریم"),
    c("CarToodooziDescF", "تودوزی"),
    c("CarOptionDescF", "آپشن"),
    c("CarColorDescF", "رنگ"),
    c("SaloonDescF", "سالن"),
  ],

  // ───────── مدیریت سیستم ─────────
  /** frmUserManagement — grvShowUsers */
  userAccess: [
    c("FirstName", "نام"),
    c("LastName", "نام خانوادگی"),
    c("UserName", "شماره پرسنلی"),
    c("VirtualPlantDescF", "سایت"),
    c("BIWSiteDescF", "سالن"),
    c("CreateDateTime", "تاریخ ثبت"),
    c("CreatorUser", "توسط کاربر"),
  ],
} satisfies Record<string, IGridColumn[]>;

export type GridKey = keyof typeof gridColumns;
