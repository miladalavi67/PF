import { IkcoButton, IkcoDataGrid } from "@ikco/frontend-kit";
import { Refresh } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState, type ReactNode } from "react";
import type { ITabularResult } from "../types/tabular";
import { columnHeaders } from "../utils/columnHeaders";
import type { IGridColumn } from "../utils/gridColumns";
import { resultColumns, tabularRows } from "../utils/tabularUtility";
import ExcelExportButton from "./ExcelExportButton";
import FormDialog from "./FormDialog";

interface TabularDialogProps {
  open: boolean;
  close: (open: boolean) => void;
  title: string;
  /** بارگذاری داده هنگام باز شدن و با «بازخوانی» */
  load: () => Promise<ITabularResult>;
  /** عنوان فارسی اضافی ستون‌ها */
  headers?: Record<string, string>;
  /** ستون‌های قابل نمایش مطابق تعریف گرید فرم ویندوزی (utils/gridColumns)؛ بدون آن همه ستون‌های خروجی رویه */
  columns?: readonly IGridColumn[];
  /** محتوای اضافه بالای جدول (مثلاً شرح بحران) */
  header?: ReactNode;
  /** نمایش دکمه «بازخوانی» (فرم‌های گزارش رویدادها، خطاها و ردیابی) */
  reload?: boolean;
  /** نمایش خروجی اکسل */
  excel?: boolean;
}

/** modal نمایش جدول خروجی یک رویه (جزئیات محصولات، گزارش رویدادها، گزارش خطاها، ردیابی پارامترها ...) با «بازخوانی»، اکسل و «خروج» */
export default function TabularDialog({ open, close, title, load, headers, columns: spec, header, reload = true, excel = true }: TabularDialogProps) {
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const run = useCallback(() => {
    let active = true;
    setLoading(true);
    load()
      .then((r) => {
        if (active) setResult(r);
      })
      .catch(() => {
        if (active) setResult(null);
      })
      .finally(() => {
        if (active) setLoading(false);
      });
    return () => {
      active = false;
    };
    // load هر بار با باز شدن modal ساخته می‌شود
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!open) return;
    return run();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const columns = useMemo(() => resultColumns(result, { ...columnHeaders, ...headers }, spec), [result, headers, spec]);
  const rows = useMemo(() => tabularRows(result), [result]);

  return (
    <FormDialog
      open={open}
      close={close}
      title={title}
      actions={
        <>
          {reload && (
            <IkcoButton startIcon={<Refresh />} onClick={run} loading={loading}>
              بازخوانی
            </IkcoButton>
          )}
          {excel && <ExcelExportButton fileName={title} columns={columns} rows={rows as unknown as Record<string, unknown>[]} />}
        </>
      }
    >
      {header}
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
    </FormDialog>
  );
}
