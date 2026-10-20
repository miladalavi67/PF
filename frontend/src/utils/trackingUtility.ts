/** ابزار گزارش‌های ردیابی اقلام بحرانی (رنگ‌آمیزی ردیف‌ها مطابق SetColors فرم‌های ویندوزی) */
export type TrackingBackground = "new" | "resolved" | "license";
export type TrackingForeground = "critical" | "threshold" | "stockChanged" | "license";

export interface ITrackingTone {
  background?: TrackingBackground;
  foreground?: TrackingForeground;
}

/**
 * تشخیص بحران جدید و تغییر موجودی پس از بازخوانی زنده (معادل SqlNotificationInfo.Insert / Update و IsChangeTotal فرم ویندوزی):
 * ردیفی که در نتیجه قبلی نبوده «new» و ردیفی که یکی از ستون‌های موجودی‌اش تغییر کرده «stock».
 */
export function diffTracking(
  prev: Record<string, unknown>[] | null | undefined,
  next: Record<string, unknown>[],
  key: (row: Record<string, unknown>) => string,
  stockFields: string[],
): Map<string, "new" | "stock"> {
  const marks = new Map<string, "new" | "stock">();
  if (!prev) return marks;
  const old = new Map(prev.map((r) => [key(r), r]));
  for (const row of next) {
    const k = key(row);
    const before = old.get(k);
    if (!before) marks.set(k, "new");
    else if (stockFields.some((f) => String(before[f] ?? "") !== String(row[f] ?? ""))) marks.set(k, "stock");
  }
  return marks;
}

/** عدد یک ستون ردیف (null در نبود مقدار) */
export function rowNumber(row: Record<string, unknown>, field: string): number | null {
  const v = row[field];
  if (v === null || v === undefined || v === "") return null;
  const n = Number(v);
  return Number.isFinite(n) ? n : null;
}
