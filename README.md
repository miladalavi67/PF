# سیستم جامع امکان تولید (Production Feasibility) — استاندارد پلتفرم IKCO

یک پروژه، دو بخش کنار هم:

| پوشه | محتوا |
|---|---|
| `backend/` | ABP 10.6.0 / .NET 10 — ماژول DDD (`Domain.Shared → Domain → Application.Contracts → Application → EntityFrameworkCore → HttpApi`) و میزبان `hosts/` با IKCO.Infrastructure و IKCO.Auth |
| `frontend/` | React 19 + Vite 7 + MUI 7 با `@ikco/frontend-kit` و `@ikco/mfe-core` (Module Federation برای دریچه) |
| `tools/ReactClientGen/` | مولد کلاینت: از روی کنترلرهای کامپایل‌شده، `types/*.ts` و `server/services/*Service.ts` و `permissions.ts` را می‌سازد |
| `local-packages/` | بسته‌های `IKCO.Auth` و `IKCO.Infrastructure` برای بازیابی NuGet بدون دسترسی به فید داخلی |
| `docs/` | `review-report.md` (بررسی اسکلت قدیمی)، `coverage.md` (فرم ← Endpoint ← صفحه)، `sp-matrix.md` (وابستگی رویه‌ها) |

## دامنه پیاده‌سازی

- **بدنه‌سازی (PF)**: ثبت/جستجوی بحران، مجوز تولید با کسری، Where-Used از SAP، گزارش‌ها.
- **امکان تولید مکانیکی (SFC)**: ثبت و جستجوی بحران، محصولات، تغییر شرایط تولید و مجوز، استثناها (عبور مستقیم)، دلایل عدم امکان تولید، مغایرت زیرکلاس با SAP، گزارش‌ها.
- **امکان تولید سنکرون (SFC)**: مبنای ست، مدیریت ست (سرآیند + قطعات/رنگ/سالن/محصول)، خودروی شاخص سالن‌ها، گزارش‌ها.
- **مدیریت کاربران**: دسترسی کاربران به خطوط بدنه‌سازی.

## اجرا

پیش‌نیاز: .NET 10 SDK و Node 20+.

۱. رازها را **فقط** در user-secrets بگذارید (هیچ رمزی در مخزن نیست):

```bash
cd backend/hosts
dotnet user-secrets set "ConnectionStrings:Default" "<رشته اتصال به پایگاه داده SFC/PF>"
dotnet user-secrets set "Oidc:EmbeddedSigningKey" "<حداقل ۳۲ کاراکتر تصادفی>"
```

اختیاری: `ProductionFeasibility:Development:UserName` (کاربر پیش‌فرض اجرای مستقل) و `ProductionFeasibility:Sap:{WhereUsedUrl,UserName,Password}` برای Where-Used.

۲. اجرای هر دو بخش:

```bash
pwsh ./start-dev.ps1
```

یا جداگانه:

```bash
cd backend/hosts && dotnet run --launch-profile IKCO.ProductionFeasibility.Host   # http://localhost:5250/swagger
cd frontend && npm install && npm run dev                                          # http://localhost:5005
```

در حالت Development مقدار `Oidc:Mode=Embedded` است؛ صفحه ورود فرانت (`DevLogin`) نام کاربری را به `POST /_auth/token` می‌دهد. در TEST/Production مقدار `Idp` است و ورود را دریچه انجام می‌دهد؛ `Oidc:Authority`، `Oidc:Audience` و کدهای زیرسامانه را از واحد زیرساخت بگیرید.

### ساخت

```bash
cd backend && dotnet build IKCO.ProductionFeasibility.sln --configfile nuget.offline.config   # بدون فید داخلی
cd frontend && npm run build && npm run check:manifest
```

### بازتولید کلاینت پس از تغییر کنترلرها

```bash
cd frontend && npm run gen:api
```

## قراردادهای مهم

- کلیدهای مجوز API: `<واحد>.<عملیات>` با حروف کوچک؛ کلیدهای UI: `<بخش>.menu.<صفحه>`. منبع واحد: `PfPermissionTree` (ثبت در IdP، مانیفست و کلاینت).
- دسترسی به داده فقط با رویه‌های ذخیره‌شده (Dapper) از طریق `PfDapperRepository`؛ نام پارامترهای جدولی (TVP) در زمان اجرا از `sys.parameters` خوانده می‌شود.
- خطاهای SQL به `BaseException` با کدهای `PfErrorCodes` ترجمه می‌شوند؛ پیام‌های عددی `RAISERROR` از `dbo.B_SqlMessages` خوانده می‌شود.
- جریان‌های چندمرحله‌ای (ثبت بحران، تغییر شرایط تولید، ترکیب ست) در `[UnitOfWork(isTransactional: true)]` اجرا می‌شوند.
- شناسه کاربر هرگز از کلاینت پذیرفته نمی‌شود: برای رویه‌های SFC شناسه `dbo.B_Person` و برای `PF.` شناسه `PF.B_User` از توکن/کاربر جاری ساخته می‌شود.

## مفروضات و محدودیت‌ها (باید با پایگاه داده واقعی راستی‌آزمایی شوند)

- **تأیید با پایگاه داده زنده انجام نشده است.** ساخت (build)، بررسی نوع (tsc)، lint، بررسی مانیفست و بیلد Federation سبز است؛ اجرای رویه‌ها روی پایگاه داده واقعی در دسترس نبود.
- رویه‌های اسکیمای `PF.` و رویه‌های `_Web` در `sfcDB.md` نیستند (`docs/sp-matrix.md`)؛ امضای آن‌ها از کد قدیمی PFback استخراج شده است.
- قالب `IdString` برای `PF.S_DisableCriticalParts` به‌صورت `1,2,3` (بدون ویرگول پایانی) فرض شده است.
- مقادیر ثابت فهرست‌های `crisis-types`، `prod-states`، `critical-kinds`، `contrast-kinds` از توضیحات داخل رویه‌ها استنباط شده‌اند.
- نام ستون‌های خروجی `PF.GetAllUsers`، `PF.GetSiteByDomainId` و `PF.GetUsersAccessList` در اسنپ‌شات نیست؛ صفحه مدیریت کاربران نام ستون‌ها را بدون حساسیت به حروف و با چند نام جایگزین می‌خواند و با ستون‌های واقعی باید یک‌بار آزموده شود.
- در ثبت ست سنکرون هر شماره فنی باید در `T_ShortagePartList` فعال باشد (سمت سرور بر اساس شماره فنی پیدا می‌شود)؛ در نبود، خطای واضح می‌دهد.
- فرم‌های «شماره فنی در تریم PF» با معادل‌های مکانیکی SFC پوشش داده شده‌اند و جداگانه ساخته نشده‌اند.
- فرم‌های بهینه‌سازی/کاندید لغو (`DA_CancelCandidateShortage`) و `frmCandidateUserAccess` هنوز پیاده‌سازی نشده‌اند.
- هشدار NuGet NU1902/NU1903 برای `System.Security.Cryptography.Pkcs 9.0.4` (وابستگی گذرای ABP) عمداً پنهان نشده است.

## نکته درباره رویه‌های قدیمی

- رویه `S_CriticalPartsSetUpdateProductionPossibility` ستون توضیحات (`Note`) را هرگز به‌روز نمی‌کند. رویه تغییر داده نشد؛ توضیح مجوز در رد ردیابی (`S_InfluenceShortageLicenseTracking`) ثبت می‌شود.
- بررسی اسکلت قدیمی بدنه‌سازی و مغایرت‌ها با استاندارد: `docs/review-report.md`.
