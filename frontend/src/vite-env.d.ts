/// <reference types="vite/client" />

interface ImportMetaEnv {
  /** آدرس پایه Backend امکان تولید (مثلاً http://localhost:5250/) */
  readonly VITE_API_BASE_URL: string;
  /** شناسه نرم‌افزار در دریچه (تخصیص‌یافته توسط سازمان) */
  readonly VITE_SOFTWARE_ID?: string;
  /** فقط اجرای مستقل: login = فرم ورود توسعه؛ هر مقدار دیگر یا خالی = بدون ورود با همه دسترسی‌ها */
  readonly VITE_AUTH_MODE?: string;
  /** فقط اجرای مستقل: کاربر پیش‌فرض فرم ورود توسعه / کاربر توکن حالت بدون ورود (پیش‌فرض devtester) */
  readonly VITE_DEV_USER?: string;
  /** فقط اجرای مستقل: شرکت توکن Embedded (پیش‌فرض ikco) */
  readonly VITE_DEV_COMPANY?: string;
}

interface ImportMeta {
  readonly env: ImportMetaEnv;
}
