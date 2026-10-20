import { IkcoButton, IkcoCard, IkcoCardContent, IkcoDataGrid, IkcoDatePicker, IkcoGrid, IkcoStack, IkcoSwitch, IkcoTypography } from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import toast from "react-hot-toast";
import PartNumberField from "./PartNumberField";
import type { ITabularResult } from "../types/tabular";
import { columnHeaders } from "../utils/columnHeaders";
import { firstOfMonthJalali, todayJalali } from "../utils/dateUtility";
import { resultColumns, tabularRows } from "../utils/tabularUtility";
import ExcelExportButton from "./ExcelExportButton";
import ActionBar from "./ActionBar";

interface Input {
  fromDate?: string | null;
  toDate?: string | null;
  partNo?: string | null;
}

interface Props {
  title: string;
  /** عنوان بخش جدول (مثلاً «لیست بدنه ها») */
  listTitle: string;
  permission: string;
  run: (input: Input) => Promise<ITabularResult>;
}

/**
 * چیدمان مشترک گزارش‌های «پیش‌تخصیص بدنه‌های بحرانی» و «مغایرت پیش‌تخصیص» (frmRPTPreAssignCriticalBody / frmRPTNotPossibleProductsToStepAssign):
 * کلید «شرایط جستجو: بر اساس تاریخ / بر اساس شماره فنی»، شماره فنی با شرح، بازه تاریخ، جدول لیست بدنه‌ها، جستجو و اکسل.
 */
export default function DateOrPartReport({ title, listTitle, permission, run }: Props) {
  const [byDate, setByDate] = useState(true);
  const [partNo, setPartNo] = useState("");
  const [fromDate, setFromDate] = useState<string | null>(firstOfMonthJalali());
  const [toDate, setToDate] = useState<string | null>(todayJalali());
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);

  const search = async () => {
    if (!byDate && !partNo.trim()) {
      toast.error("شماره فنی را وارد کنید");
      return;
    }
    if (byDate && (!fromDate || !toDate)) {
      toast.error("بازه تاریخ را وارد کنید");
      return;
    }
    setLoading(true);
    try {
      setResult(await run(byDate ? { fromDate, toDate, partNo: null } : { fromDate: null, toDate: null, partNo: partNo.trim() }));
    } finally {
      setLoading(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">{title}</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={1} alignItems="center">
            <IkcoGrid size={{ xs: 12, md: "auto" }}>
              <IkcoSwitch label={`شرایط جستجو: ${byDate ? "بر اساس تاریخ" : "بر اساس شماره فنی"}`} checked={byDate} onChange={(_, c) => { setByDate(c); if (c) { setPartNo(""); } }} />
            </IkcoGrid>
            <PartNumberField source="body" value={partNo} onChange={setPartNo} disabled={byDate} maxLength={30} size={{ xs: 12, md: 2 }} descSize={{ xs: 12, md: 3 }} />
            <IkcoGrid size={{ xs: 6, md: 2 }}>
              <IkcoDatePicker label="از" value={fromDate} disabled={!byDate} onChange={setFromDate} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 6, md: 2 }}>
              <IkcoDatePicker label="تا" value={toDate} disabled={!byDate} onChange={setToDate} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>
      <IkcoTypography variant="subtitle1">{listTitle}</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
      <ActionBar>
          <PermissionGuard can={permission} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={() => void search()}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName={title} columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "شرایط را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>
    </IkcoStack>
  );
}
