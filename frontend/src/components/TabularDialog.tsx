import { IkcoButton, IkcoDataGrid, IkcoDialog, IkcoDialogActions, IkcoDialogTitle, IkcoCardContent } from "@ikco/frontend-kit";
import { useEffect, useMemo, useState } from "react";
import type { ITabularResult } from "../types/Common";
import { columnHeaders } from "../utils/columnHeaders";
import { resultColumns, tabularRows } from "../utils/tabularUtility";
import ExcelExportButton from "./ExcelExportButton";

interface TabularDialogProps {
  open: boolean;
  close: (open: boolean) => void;
  title: string;
  /** بارگذاری داده هنگام باز شدن */
  load: () => Promise<ITabularResult>;
  /** عنوان فارسی اضافی ستون‌ها */
  headers?: Record<string, string>;
}

/** دیالوگ نمایش جدول خروجی یک رویه (جزئیات محصولات، تاریخچه رویدادها، تاریخچه مجوزها ...) با خروجی اکسل */
export default function TabularDialog({ open, close, title, load, headers }: TabularDialogProps) {
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (!open) return;
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
    // load هر بار با باز شدن دیالوگ ساخته می‌شود
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const columns = useMemo(() => resultColumns(result, { ...columnHeaders, ...headers }), [result, headers]);
  const rows = useMemo(() => tabularRows(result), [result]);

  return (
    <IkcoDialog open={open} maxWidth="lg" fullWidth onClose={() => close(false)}>
      <IkcoDialogTitle>{title}</IkcoDialogTitle>
      <IkcoCardContent>
        <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
      </IkcoCardContent>
      <IkcoDialogActions>
        <ExcelExportButton fileName={title} columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
        <IkcoButton onClick={() => close(false)}>بستن</IkcoButton>
      </IkcoDialogActions>
    </IkcoDialog>
  );
}
