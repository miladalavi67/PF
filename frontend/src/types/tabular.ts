/**
 * قالب جدولی نمایش خروجی رویه‌ها در گریدها (ReportView، TabularDialog، خروجی اکسل). API ردیف‌های نوع‌دار (مدل‌های PfResultRow)
 * برمی‌گرداند و سرویس‌های تولیدشده آن‌ها را با toTabular به این قالب تبدیل می‌کنند؛ نام ستون‌ها همان نام خروجی رویه است.
 */
export interface ITabularResult {
  /** نام ستون‌ها به ترتیب مدل خروجی رویه */
  columns: string[];
  /** ردیف‌ها؛ کلید هر مقدار نام ستون است */
  rows: Record<string, unknown | null>[];
}
