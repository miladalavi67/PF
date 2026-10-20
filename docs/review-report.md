# گزارش بررسی اسکلت قدیمی بدنه‌سازی (PFback / PFfront) نسبت به استاندارد پلتفرم IKCO

نتیجه: اسکلت قدیمی با استاندارد سازگار نبود و به‌جای وصله‌کاری، روی قالب استاندارد بازنویسی شد (منطق رویه‌ها حفظ شد).

## یافته‌های Backend

| یافته | وضعیت قدیمی | اقدام |
|---|---|---|
| نسخه | .NET 9 / ABP 9.1.1 | .NET 10 / ABP 10.6.0، IKCO.Infrastructure 2.0.5 و IKCO.Auth 2.0.4 |
| احراز هویت و مجوز | بدون `[Authorize]` و بدون زیرساخت IKCO | `[Authorize]` روی همه کنترلرها؛ درخت مجوز واحد `PfPermissionTree` (کلیدهای حروف کوچک) |
| اسرار | رمز و رشته اتصال در `appsettings` و اعتبار SAP ثابت در کد | مقادیر خالی در appsettings، user-secrets؛ هیچ رمز قدیمی کپی نشد (**باید در منبع قدیمی چرخانده شود**) |
| شناسه کاربر | `UserId` از کلاینت پذیرفته می‌شد | از کاربر جاری سمت سرور ساخته می‌شود |
| SQL | الحاق رشته | رویه‌ذخیره‌شده/SQL پارامتری و کاتالوگ سفید برای Lookup |
| تراکنش | حلقه‌های N+1 بدون تراکنش | `[UnitOfWork(isTransactional: true)]` برای جریان‌های چندمرحله‌ای |
| قرارداد پاسخ | `TResult` با HTTP 200 حتی در خطا | کد HTTP و `BaseException` با `PfErrorCodes`؛ `GlobalResultDto` فقط برای رویه‌های فرمان |
| CORS/CSRF | AllowAnyOrigin و antiforgery خاموش | Origin صریح از پیکربندی |
| مسیریابی | `[Route]` Blazor در `SyncPartsReportsController` | مسیرهای ABP-ی استاندارد |
| نام‌گذاری | ویژگی بی‌معنی `MyProProductsColorCodeperty` | حذف |

## یافته‌های Frontend

- بسته‌های بلااستفاده (`react-signalr`، `react-toastify`، `sweetalert2`، `xlsx`) و peerهای اعلام‌نشده حذف/اعلام شدند.
- مسیرها در `mfe-manifest.ts` و `App.tsx` هر دو و با کلید مجوز UI سازگار تولید می‌شوند (`npm run check:manifest`).
- نام هر کامپوننت یکتاست (mfe-core با `./${componentName}` اکسپورت می‌کند).
- تاریخ شمسی به‌صورت رشته `yyyy/MM/dd`.

## باگ‌ها و نکته‌های سمت پایگاه داده

- `S_CriticalPartsSetUpdateProductionPossibility`: ستون Note را به‌روز نمی‌کند (تغییر داده نشد).
- ثبت‌های چندمرحله‌ای قدیمی (قطعات/رنگ/سالن/محصول ست) با درخواست‌های جدا و بدون تراکنش انجام می‌شد؛ اکنون یک تراکنش است.
- NuGet audit: NU1902/NU1903 برای `System.Security.Cryptography.Pkcs 9.0.4` (گذرا از ABP) — پس از دسترسی به فید داخلی با ارتقای ABP رفع شود.

## موارد نیازمند اقدام انسانی

۱. رمزها و کلیدهای موجود در فایل‌های export قدیمی را بچرخانید. ۲. رویه‌های `PF.*` و `_Web` را روی پایگاه داده مقصد راستی‌آزمایی کنید (`sp-matrix.md`). ۳. پیش از تولید، مقادیر `Oidc`/`Ikco` را از واحد زیرساخت بگیرید.

## اعمال بازبینی PFReview.docx (۱۴۰۵/۰۷/۲۷)

| بند | وضعیت | پیاده‌سازی |
|---|---|---|
| ۱ — مقدار پیش‌فرض combobox | انجام شد | `LookupAutocomplete` اولین گزینه را انتخاب می‌کند (هنگام بارگذاری، تغییر والد و پاک‌سازی)؛ در فیلترهای گزارش که «خالی = همه» است اولین گزینه «همه موارد» است (`allLabel`). |
| ۲ — فاصله عناصر | انجام شد | سلول چک‌باکس/سوئیچ به اندازه محتوا (`size="auto"`): جستجو/گزارش اقلام بحرانی، گزارش پیش‌تخصیص و مغایرت (`DateOrPartReport`)، چک‌باکس SAP در تعریف اقلام مکانیکی. |
| ۳ — کامپوننت شماره فنی / بدنه | انجام شد | `PartNumberField` (شرح از Backend، `SIMP_SelectPartDescByPartNo`) و `BodyNumberField` (اطلاعات بدنه، `SEXT_BodyInfo_V2`). فرم‌های ثبت بحران (بدنه/مکانیکی/سنکرون) جریان حالت‌دار خود را دارند و هنوز منطق شرح را مستقیم صدا می‌زنند. |
| ۴ — Dapper و مدل | انجام شد | خروجی همه رویه‌ها مدل نوع‌دار (`*Results.cs`، پایه `PfResultRow`) است؛ منبع ستون‌ها: DTO بک‌اند قبلی ← کلاس `_Result` فرم ویندوزی ← SELECT نهایی متن رویه در SFCDB2 (`_src/gen/spec_rows.py`). ستون تعریف‌نشده (مثلاً بخش کوتاه‌شده متن رویه در اسنپ‌شات) در `ExtraColumns` حفظ و در JSON با همان نام برمی‌گردد. فهرست‌های پایه‌ای که بک‌اند قبلی با رویه می‌خواند (سایت، سالن مونتاژ، واحد، تریم، تودوزی، آپشن، رنگ) با همان رویه‌ها خوانده می‌شوند. |
| ۵ — رفرش زنده | انجام شد | هاب `PfLiveHub` (`/signalr-hubs/production-feasibility`) و `TableChangeNotifier` (Query Notification روی Service Broker، در نبود آن پایش checksum). کانال‌ها: `body-tracking`، `body-preassign`، `trim-tracking`؛ صفحه‌های ردیابی بدنه‌سازی و مونتاژ عضو می‌شوند. تنظیم: `ProductionFeasibility:LiveUpdates`. |
| ۶ — AllowAnonymous | انجام نشد | نیازمند تأیید صریح (کاهش امنیت)؛ `[Authorize]` فعال مانده است. |
| ۷ — منطق در Backend | رعایت شد | ثبت ترکیب سنکرون، محصولات مکانیکی و حذف با دلیل در سرویس‌های Backend (یک تراکنش). |
| ۸ — رویه‌ها از بک‌اند قبلی | انجام شد | `S_CriticalPartDelete_Web`، `S_CriticalPartsPruductUpdate_Web`، `S_CriticalPartsSet*_Web`، `S_InsertCriticalPartsSet_*_Web`، `S_GetCriticalPartsSet_Saloons`، گزارش‌های SAP و … . `S_CriticalPartsInsert_Web` در SFCDB2 نیست؛ ثبت مکانیکی همچنان `S_CriticalPartsINSERT` است. `S_GetSyncCriticalPartsProperty` هم در SFCDB2 نیست و پیاده نشد. |
| ۹ — SFCDB2.md | انجام شد | امضای رویه‌ها، استخراج مدل و `sp-matrix.md` از SFCDB2 خوانده می‌شوند. |

نکته: `S_CriticalPartsGeneralReport_Web`، `S_RPT_IncommingBodies*` و `S_TrimShopCriticalBody_SelectAll` در SFCDB2 کوتاه شده‌اند؛ ستون‌های مدل آن‌ها باید با اجرای واقعی روی پایگاه داده راستی‌آزمایی شود (ستون‌های اضافه از دست نمی‌روند).
