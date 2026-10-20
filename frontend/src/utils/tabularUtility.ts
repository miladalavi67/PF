import type { IkcoGridColDef } from "@ikco/frontend-kit";
import type { ITabularResult } from "../types/tabular";
import { toPersianDate } from "./dateUtility";

/** ردیف گرید با شناسه یکتای سمت کلاینت (برای خروجی‌هایی که ستون Id ندارند) */
export type IGridRow<T = object> = T & { __rowId: number };

/** افزودن __rowId به ردیف‌ها؛ در IkcoDataGrid از getRowId={(r) => r.__rowId} استفاده شود */
function withRowIds<T extends object>(items: T[] | null | undefined): IGridRow<T>[] {
  return (items ?? []).map((item, index) => ({ ...item, __rowId: index + 1 }));
}

/**
 * ردیف‌های نوع‌دار خروجی رویه (مدل‌های PfResultRow بک‌اند) به قالب جدول: ستون‌ها به ترتیب مدل و پس از آن ستون‌هایی که رویه
 * برگردانده ولی در مدل تعریف نشده‌اند (ExtraColumns). سرویس‌های تولیدشده از همین تابع استفاده می‌کنند.
 */
function toTabular<T extends object>(items: T[] | null | undefined, columns: readonly string[]): ITabularResult {
  const rows = (items ?? []) as unknown as Record<string, unknown>[];
  const known = new Set(columns);
  const extra: string[] = [];
  for (const row of rows) {
    for (const key of Object.keys(row)) {
      if (!known.has(key)) {
        known.add(key);
        extra.push(key);
      }
    }
  }
  return { columns: [...columns, ...extra], rows };
}

/** تبدیل TabularResult (خروجی DataTable رویه‌ها) به ردیف‌های گرید */
function tabularRows(result: ITabularResult | null | undefined): IGridRow[] {
  return withRowIds(result?.rows ?? []);
}

/**
 * ستون‌های گرید برای TabularResult: ترتیب ستون‌ها مطابق headers (عنوان فارسی) و در صورت نبود در headers، ستون‌های
 * باقی‌مانده خروجی رویه با نام اصلی. ستون‌های hidden نمایش داده نمی‌شوند.
 */
function tabularColumns(
  result: ITabularResult | null | undefined,
  headers: Record<string, string>,
  options: { hidden?: string[]; includeUnknown?: boolean } = {},
): IkcoGridColDef[] {
  const hidden = new Set(options.hidden ?? []);
  const known = Object.keys(headers).filter((c) => !hidden.has(c));
  const extra = options.includeUnknown === false ? [] : (result?.columns ?? []).filter((c) => !(c in headers) && !hidden.has(c));
  return [...known, ...extra].map((field) => ({
    field,
    headerName: headers[field] ?? field,
    minWidth: 120,
    flex: 1,
  }));
}

const isoDate = /^\d{4}-\d{2}-\d{2}T/;

/**
 * ستون‌های گرید دقیقاً بر اساس ستون‌های خروجی رویه (به ترتیب خروجی) با عنوان فارسی از فرهنگ headers؛
 * ستونی که در headers نیست با نام اصلی نمایش داده می‌شود. مقدارهای تاریخ ISO به تاریخ شمسی تبدیل می‌شوند.
 */
function resultColumns(result: ITabularResult | null | undefined, headers: Record<string, string>): IkcoGridColDef[] {
  const rows = result?.rows ?? [];
  return (result?.columns ?? []).map((field) => {
    const sample = rows.find((r) => r[field] !== null && r[field] !== undefined)?.[field];
    const column: IkcoGridColDef = { field, headerName: headers[field] ?? field, minWidth: 120, flex: 1 };
    if (typeof sample === "string" && isoDate.test(sample)) {
      column.valueFormatter = (value: unknown) => {
        if (typeof value !== "string" || !isoDate.test(value)) return (value as string) ?? "";
        const date = new Date(value);
        const time = value.length > 10 ? value.slice(11, 19) : "";
        return `${toPersianDate(date) ?? ""}${time && time !== "00:00:00" ? " " + time : ""}`;
      };
    }
    return column;
  });
}

export { withRowIds, tabularRows, tabularColumns, resultColumns, toTabular };
