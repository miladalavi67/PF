import { IkcoButton, IkcoCard, IkcoCardContent, IkcoChip, IkcoDataGrid, IkcoGrid, IkcoRadioGroup, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { History, Refresh } from "@mui/icons-material";
import { useMemo, useState } from "react";
import toast from "react-hot-toast";
import ExcelExportButton from "../../../components/ExcelExportButton";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import TabularDialog from "../../../components/TabularDialog";
import { getMechReportsGeneralReport, getMechReportsIncomingBodiesByCriticalPart, getMechReportsStockLog } from "../../../server/services/mechReportsService";
import type { ITabularResult } from "../../../types/Common";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../../utils/tabularUtility";
import ActionBar from "../../../components/ActionBar";

const field = (row: IGridRow | undefined, ...keys: string[]) => {
  const r = (row ?? {}) as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return r[k];
  return undefined;
};

/**
 * گزارش جامع اقلام بحرانی (frm_RPT_ST_IncommingBodiesByCP؛ رویه S_RptCriticalPartsGeneral). فیلتر سالن مونتاژ، وضعیت امکان تولید و دسته‌بندی
 * قطعات؛ با کلیک روی یک بحران، بدنه‌های وارد شده به خط به تفکیک همان بحران در جدول «محصولات» نمایش داده می‌شود. LOG تاریخچه موجودی قطعه را در modal باز می‌کند.
 * Backend: GET /api/production-feasibility/mechanical/reports/general-report، دسترسی mechanical.reports.general.
 */
export default function MechGeneralReport() {
  const [saloonId, setSaloonId] = useState<number | null>(1);
  const [crisis, setCrisis] = useState("100");
  const [kind, setKind] = useState("100");
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [incoming, setIncoming] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [active, setActive] = useState<IGridRow | null>(null);
  const [showLog, setShowLog] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);
  const incomingColumns = useMemo(() => resultColumns(incoming, columnHeaders), [incoming]);
  const incomingRows = useMemo(() => tabularRows(incoming), [incoming]);

  const run = async () => {
    if (!saloonId) {
      toast.error("سالن مونتاژ را انتخاب کنید");
      return;
    }
    setLoading(true);
    try {
      setResult(await getMechReportsGeneralReport({ criticalPartTypeId: Number(kind), saloonId, crisisTypeId: Number(crisis), plantId: 1 }));
      setActive(null);
      setIncoming(null);
    } finally {
      setLoading(false);
    }
  };

  const select = async (row: IGridRow) => {
    setActive(row);
    const id = Number(field(row, "CriticalPartsId", "Id", "id") ?? 0);
    if (!id || !saloonId) return;
    setIncoming(await getMechReportsIncomingBodiesByCriticalPart({ id, criticalPartTypeId: Number(field(row, "CriticalPartTypeId") ?? 0), saloonId, plantId: 1 }));
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">گزارش جامع اقلام بحرانی</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={{ xs: 12, md: 3 }}>
              <LookupAutocomplete lookupKey="trim-shops" label="سالن مونتاژ" value={saloonId} onChange={setSaloonId} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 5 }}>
              <IkcoTypography variant="subtitle2">وضعیت امکان تولید</IkcoTypography>
              <IkcoRadioGroup row value={crisis} onChange={(_, v) => setCrisis(v)} options={[{ value: "100", label: "همه موارد" }, { value: "0", label: "عدم امکان تولید" }, { value: "1", label: "تولید با کسری" }, { value: "2", label: "تولید با قطعه جایگزین" }]} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 4 }}>
              <IkcoTypography variant="subtitle2">دسته بندی قطعات</IkcoTypography>
              <IkcoRadioGroup row value={kind} onChange={(_, v) => setKind(v)} options={[{ value: "100", label: "همه موارد" }, { value: "1", label: "سنکرون" }, { value: "0", label: "مکانیکی" }]} />
            </IkcoGrid>
            <IkcoGrid size={12}>
              <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap alignItems="center">
                <IkcoChip label="امکان تولید کمتر از 15" status="error" size="small" />
                <IkcoTypography variant="caption">ت.ب.و.ش.ب.خ = تعداد بدنه وارد شده به خط | ا.ت. = امکان تولید | م.ف.بدنه = موجودی فرآیند بدنه | م.ف.رنگ = موجودی فرآیند رنگ</IkcoTypography>
              </IkcoStack>
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} onRowClick={(p) => void select(p.row as IGridRow)} autoHeight />
      <IkcoTypography variant="subtitle1">محصولات</IkcoTypography>
      <IkcoDataGrid columns={incomingColumns} rows={incomingRows} getRowId={(r) => r.__rowId} autoHeight />

      <ActionBar>
          <PermissionGuard can={ApiPermissions.mechReports.general} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Refresh />} loading={loading} onClick={() => void run()}>
              بازخوانی
            </IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="گزارش جامع اقلام بحرانی" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoButton startIcon={<History />} onClick={() => (active ? setShowLog(true) : toast("ابتدا یک بحران را از جدول انتخاب کنید"))}>
            LOG
          </IkcoButton>
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "شرایط را وارد و «بازخوانی» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>

      <TabularDialog open={showLog} close={setShowLog} title="LOG" load={() => getMechReportsStockLog({ partNo: String(field(active ?? undefined, "PartNo", "Partno", "PartNumber") ?? "").trim() })} />
    </IkcoStack>
  );
}
