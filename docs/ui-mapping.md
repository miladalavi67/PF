# نگاشت تصاویر فرم‌های ویندوزی به صفحه / modal

منبع: `PFWorkspace\pics\{BodyShop,Mechanical,Sync}`. منو دقیقاً مطابق `PFMenu.PNG` است، بجز «بهینه‌سازی فرآیند تجاری‌سازی…» و
«مدیریت کاربران تجاری‌سازی» که خارج از محدوده‌اند. فرم‌هایی که در سامانه ویندوزی از دکمه یک فرم دیگر باز می‌شدند modal هستند.

## بدنه‌سازی

| تصویر | پیاده‌سازی | نوع |
|---|---|---|
| `frmBodyCriticalParts` | `pages/body-shop/BodyCriticalRegister.tsx` | منو: بدنه سازی ← عملیات ← ثبت بحران |
| `frmBodyCriticalSearch` | `BodyCriticalSearch.tsx` + `BodyCriticalSearchPanel.tsx` | منو: جستجوی اقلام بحرانی |
| `frmRPTTrackingCriticalParts` | `reports/BodyTrackingReport.tsx` | منو: گزارشات ← ردیابی تغییرات اقلام بحرانی |
| `frmRPTBodyCriticalSearch` | `reports/BodyCriticalPartsReport.tsx` (همان Panel با `mode="report"`) | منو: گزارش اقلام بحرانی |
| `frmRPTPreAssignCriticalBody` | `reports/BodyPreAssignCriticalReport.tsx` | منو: گزارش پیش تخصیص بدنه‌های بحرانی |
| `frmRPTNotPossibleProductsToStepAssign` | `reports/BodyConflictsReport.tsx` | منو: گزارش مغایرت پیش تخصیص |
| `frmRPTGetProductsOfBIWSite` | `reports/BodyProductsOfSitesReport.tsx` | منو: گزارش محصولات به تفکیک خطوط بدنه سازی |
| `frmCriticalExcelLogError` | `dialogs/BodyExcelImportDialog.tsx` (modal «گزارش خطاها» پس از ثبت گروهی) | modal از «ایجاد با اکسل» |
| `frmEventLog` | `TabularDialog` (دکمه LOG در ثبت/جستجو/ردیابی) | modal |
| `frmRPTPreassignBodyCount` | `dialogs/BodyPreAssignCountDialog.tsx` | modal از «تعداد بدنه‌های پیش تخصیص شده» |
| `frmRPTShortageLicenseTracking` | `TabularDialog` «ردیابی پارامترهای بحرانی» | modal از جستجو / گزارش اقلام بحرانی |
| (ثبت/ویرایش) | `dialogs/BodyCriticalSearchDialog.tsx`, `BodyCriticalEditDialog.tsx`, `BodyLicenseDialog.tsx` | modal از «جستجو»، «تغییر در مشخصات»، «تغییر در شرایط تولید» |

## مونتاژ - مکانیکی

| تصویر | پیاده‌سازی | نوع |
|---|---|---|
| `frmCriticalPart` | `pages/mechanical/MechCriticalRegister.tsx` | منو: عملیات ← تعریف اقلام بحرانی |
| `frmCriticalPartSearch` | `MechCriticalSearch.tsx` + `MechCriticalSearchPanel.tsx` (همچنین `dialogs/MechCriticalSearchDialog.tsx`) | منو: جستجوی اقلام بحرانی؛ modal از فرم‌های تعریف |
| `frmRPTProductionPossibility` | `reports/MechProductionPossibilityReport.tsx` | منو: گزارشات ← ردیابی اقلام بحرانی |
| `frm_RPT_ST_IncommingBodiesByCP` | `reports/MechGeneralReport.tsx` (جدول «محصولات» = بدنه‌های وارد شده به تفکیک بحران) | منو: گزارش جامع اقلام بحرانی |
| `frm_Rpt_Sap_Asn` | `reports/MechAsnReport.tsx` / `MechAsnPanel.tsx` / `dialogs/MechAsnDialog.tsx` | منو + modal از ردیابی |
| `frmCriticalPartsProdPosChange` | `dialogs/MechLicenseChangeDialog.tsx` | modal (جستجو، ردیابی، سنکرون) |
| `frmRPTContrastOfSubClasses` | `dialogs/MechContrastDialog.tsx` | modal از ردیابی |
| `frmTrimShopBodyCount` | `dialogs/MechTrimShopBodyCountDialog.tsx` | modal از «تعداد بدنه‌های ورود به مونتاژ شده» |

## مونتاژ - سنکرون

| تصویر | پیاده‌سازی | نوع |
|---|---|---|
| `frmCriticalPartSet` | `pages/synchron/SyncSetManagement.tsx` | منو: عملیات ← تعریف اقلام بحرانی سنکرون |
| `frmCriticalPartSetIndicatorCars` | `pages/synchron/dialogs/SyncIndicatorCarsDialog.tsx` | modal از «بدنه (های) شاخص» |

## نکات

- صفحات بدون تصویر از منو و `App.tsx` خارج شدند (کد Backend و سرویس‌ها سر جایشان هستند): `MechExceptions`، `MechNotPossibleReasons`،
  `SyncBasisManagement` (فایل‌ها هنوز در `src/pages` هستند ولی route ندارند) و گزارش‌های تولیدشده‌ی بدون تصویر حذف شدند.
- مقدارهای فیلتر «درحال تکمیل / فعال / غیرفعال» در جستجوی مکانیکی/سنکرون: `0 / 1 / 2` (بر اساس lookup `critical-statuses`)؛ مقدار «درحال تکمیل»
  را با داده واقعی SFC تأیید کنید.
- رنگ‌آمیزی ردیف‌های ردیابی (بحران جدید، <15 و …) فعلاً فقط به صورت راهنمای رنگ (Chip) است؛ نام ستون‌های وضعیت رویه‌ها در اسنپ‌شات نیست.
- `npm i` روی شبکه ایران‌خودرو: `frontend/.npmrc` رجیستری Nexus را تنظیم می‌کند و `replace-registry-host=always` آدرس‌های
  `registry.npmjs.org` داخل `package-lock.json` را به همان رجیستری می‌برد. توصیه: `npm ci`.

## قواعد فعال/غیرفعال و نمایش (برگرفته از کد پشت فرم‌های ویندوزی)

منبع: `PFWinform.txt`. برای فرم‌های SFC (مکانیکی/سنکرون) فقط فایل resx در دسترس است؛ قواعد آن‌ها از فرم‌های متناظر PF
(`frmMechanicalTrimCriticalParts`، `frmTrimCriticalSearch`) و تصاویر گرفته شده است.

| فرم | قاعده |
|---|---|
| ثبت بحران (`frmBodyCriticalParts`، `frmMechanicalTrimCriticalParts`) | ماشین حالت `formState`: عادی ← «جدید» (فقط بدنه فعال) ← بدنه معتبر (گروه قطعه فعال) ← شماره فنی معتبر (محصولات، شماره نامه و توضیحات بحران فعال، «ذخیره» فعال). «جدید»/«جستجو»/«اکسل» فقط در حالت عادی؛ «انصراف» در حالت عادی غیرفعال؛ بدنه نامعتبر = پاک‌سازی کامل؛ شماره فنی بدون محصول (بدنه‌سازی) = پاک شدن شماره فنی و بازگشت به «بدنه معتبر»؛ حالت ویرایش مستقیماً «شماره فنی معتبر» است و بدنه/شماره فنی قفل‌اند. |
| جستجوی بحران (`frmBodyCriticalSearch`، `frmTrimCriticalSearch`) | تاریخ‌ها فقط با «انتخاب تاریخ» و محصول/مدل/کلاس فقط با «انتخاب محصول» فعال؛ «تغییر در مشخصات»، «تغییر در شرایط تولید»، «حذف»، «ردیابی پارامترها»، LOG فقط وقتی جدول ردیف دارد. بدنه‌سازی: «حذف» ← تأیید ← پنل «توضیح دلیل حذف قطعه بحرانی» (دلیل اجباری، با پسوند «(حذف بحران)» در لاگ ثبت می‌شود) و تا پایان یا انصراف سایر دکمه‌ها غیرفعال؛ بک‌اند `PF.InsertEventLog` را برای هر بحران فراخوانی می‌کند. در جستجوی مکانیکی دلیل حذف ثبت نمی‌شود (در SFC رویه‌ی لاگ مشخص نیست). |
| تغییر شرایط تولید (`frmCriticalPartsProdPosChange`) | فیلدها تا «جدید» غیرفعال؛ «تایید/انصراف» فقط در حالت جدید؛ «عدم امکان تولید» و «قطعه جایگزین» تعداد/تاریخ/ساعت/بدون‌محدودیت را غیرفعال می‌کنند؛ «بدون محدودیت» تعداد/تاریخ/ساعت را غیرفعال می‌کند؛ نوع ۳ فقط تعداد. |
| گزارش پیش‌تخصیص / مغایرت | شماره فنی فقط در حالت «بر اساس شماره فنی» و تاریخ‌ها فقط در حالت «بر اساس تاریخ» فعال؛ در جستجوی تاریخی شماره فنی ارسال نمی‌شود. |
| مدیریت کاربران (`frmUserManagement`) | پنل ثبت پس از «افزودن» نمایان؛ «افزودن»، «بازخوانی» و «غیرفعال‌کردن» هنگام باز بودن پنل غیرفعال؛ «غیرفعال‌کردن» فقط با انتخاب ردیف. |

## چیدمان

نوار دکمه‌ها با `components/ActionBar.tsx` (flex-wrap) و گروه‌بندی فیلدها با `components/SectionPanel.tsx` ساخته می‌شود؛ هر دو فقط از
کامپوننت‌های `Ikco*` استفاده می‌کنند (بدون `sx`/style). دکمه‌های modal در `FormDialog` نیز wrap می‌شوند.
