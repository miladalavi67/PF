/**
 * عنوان فارسی ستون‌های خروجی رویه‌ها (بر اساس نام ستون). ستون‌هایی که اینجا نیستند با نام اصلی نمایش داده می‌شوند.
 * شروع کار: ستون‌های پرتکرار؛ فایل columnHeaders.generated.ts (از توضیحات ستون‌های پایگاه داده) کامل‌تر است.
 */
import { generatedColumnHeaders } from "./columnHeaders.generated";

export const columnHeaders: Record<string, string> = {
  ...generatedColumnHeaders,
};
