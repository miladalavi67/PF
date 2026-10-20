import { IkcoButton, IkcoCard, IkcoCardContent, IkcoCheckbox, IkcoChip, IkcoGrid, IkcoRadioGroup, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { CleaningServices, Edit, History, LocalShipping, Print, Refresh, Search, Sync, Visibility } from "@mui/icons-material";
import { useMemo, useRef, useState } from "react";
import toast from "react-hot-toast";
import ExcelExportButton from "../../../components/ExcelExportButton";
import TrackingGrid, { TrackingLegend, type ILegendItem } from "../../../components/TrackingGrid";
import { diffTracking, rowNumber, type ITrackingTone } from "../../../utils/trackingUtility";
import FormDialog from "../../../components/FormDialog";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import LookupMultiSelect from "../../../components/LookupMultiSelect";
import TabularDialog from "../../../components/TabularDialog";
import { getMechLicensesTrackingList } from "../../../server/services/mechLicensesService";
import { getMechReportsFailedSapTransfers, getMechReportsProductionPossibility, getMechReportsSapCriticalLog, getMechReportsStockLog } from "../../../server/services/mechReportsService";
import type { ITabularResult } from "../../../types/tabular";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { firstOfMonthJalali, todayJalali } from "../../../utils/dateUtility";
import { resultColumns, tabularRows, type IGridRow } from "../../../utils/tabularUtility";
import SyncSetManagement from "../../synchron/SyncSetManagement";
import MechContrastDialog from "../dialogs/MechContrastDialog";
import MechAsnDialog from "../dialogs/MechAsnDialog";
import MechCriticalRegister from "../MechCriticalRegister";
import ActionBar from "../../../components/ActionBar";
import useLiveRefresh from "../../../hooks/useLiveRefresh";
import type { IMechReportsGetProductionPossibilityInput } from "../../../types/Mechanical";
import { gridColumns } from "../../../utils/gridColumns";

/** راهنمای رنگ بحران‌ها (رنگ مربع‌های فرم ویندوزی frmRPTProductionPossibility) */
const legend: ILegendItem[] = [
  { label: "بحران جدید", color: "#FFD700" },
  { label: "بلا مانع طی 1 ساعت اخیر", color: "#98FB98" },
  { label: "تغییر در موجودی", color: "#87CEEB" },
  { label: "امکان تولید کمتر از 20", color: "#FF0000" },
];

const rowKey = (row: Record<string, unknown>) => `${row["CriticalPartTypeId"] ?? ""}-${row["Id"] ?? ""}`;

/**
 * رنگ ردیف (فرم SFC فقط resx دارد؛ قاعده از SetColors فرم متناظر PF، frmRPTTrackingTrimCriticalPart، با آستانه‌های راهنمای فرم SFC):
 * بحران جدید زمینه طلایی؛ بحران بلامانع‌شده در یک ساعت اخیر (Status = 2، که رویه فقط تا یک ساعت برمی‌گرداند) زمینه سبز؛
 * امکان تولید (BodyProdPossibilityNo) کمتر از 20 قرمز؛ با تیک «آستانه بحران» امکان تولید 0 تا 40 صورتی؛ در غیر این صورت تغییر موجودی آبی.
 */
const toneOf = (row: Record<string, unknown>, marks: Map<string, "new" | "stock">, threshold: boolean): ITrackingTone => {
  const mark = marks.get(rowKey(row));
  const pos = rowNumber(row, "BodyProdPossibilityNo");
  const tone: ITrackingTone = { background: mark === "new" ? "new" : rowNumber(row, "Status") === 2 ? "resolved" : undefined };
  if (pos !== null && pos < 20) tone.foreground = "critical";
  else if (threshold && pos !== null && pos >= 0 && pos <= 40) tone.foreground = "threshold";
  else if (mark === "stock") tone.foreground = "stockChanged";
  return tone;
};

const value = (row: IGridRow | null, ...keys: string[]) => {
  const r = (row ?? {}) as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return r[k];
  return undefined;
};

type Dialog = "stock" | "license" | "sap" | "sapDiff" | "asn" | "contrast" | "edit" | null;

/**
 * گزارش ردیابی اقلام بحرانی (frmRPTProductionPossibility، رویه S_RptCriticalPartsProduction_Mechanical): فیلتر سالن مونتاژ، وضعیت امکان تولید،
 * دسته‌بندی قطعات و محصول/کلاس/زیرکلاس؛ جدول بحران‌ها (با فیلتر ستونی) و محصولات. دکمه‌های LOG، لاگ مجوزها، لاگ SAP، لاگ مغایرت SAP، گزارش ASN های در راه،
 * گزارش مغایرت زیرکلاس ها و تغییر در مشخصات اقلام بحرانی هر کدام modal باز می‌کنند.
 * Backend: GET /api/production-feasibility/mechanical/reports/production-possibility، دسترسی mechanical.reports.productionpossibility.
 */
export default function MechProductionPossibilityReport() {
  const [allSaloons, setAllSaloons] = useState(true);
  const [saloons, setSaloons] = useState<number[]>([]);
  const [crisis, setCrisis] = useState("100");
  const [kind, setKind] = useState("-1");
  const [modelId, setModelId] = useState<number | null>(null);
  const [classId, setClassId] = useState<number | null>(null);
  const [subClassId, setSubClassId] = useState<number | null>(null);
  const [showFilters, setShowFilters] = useState(true);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [refreshedAt, setRefreshedAt] = useState("");
  const [active, setActive] = useState<IGridRow | null>(null);
  const [dialog, setDialog] = useState<Dialog>(null);
  const [threshold, setThreshold] = useState(false);
  const [marks, setMarks] = useState<Map<string, "new" | "stock">>(new Map());
  const lastRows = useRef<Record<string, unknown>[] | null>(null);

  const columns = useMemo(() => resultColumns(result, columnHeaders, gridColumns.mechProductionPossibility), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);
  const productRows = useMemo(() => (active ? [active] : []), [active]);

  const lastInput = useRef<IMechReportsGetProductionPossibilityInput | null>(null);

  const load = async (input: IMechReportsGetProductionPossibilityInput, silent = false) => {
    if (!silent) setLoading(true);
    try {
      const data = await getMechReportsProductionPossibility(input);
      // بازخوانی زنده: بحران جدید و تغییر موجودی نسبت به نتیجه قبلی رنگی می‌شود
      setMarks(silent ? diffTracking(lastRows.current, data.rows, rowKey, ["Stock", "BodyCount"]) : new Map());
      lastRows.current = data.rows;
      setResult(data);
      lastInput.current = input;
      if (!silent) setActive(null);
      setRefreshedAt(new Date().toLocaleString("fa-IR"));
    } finally {
      if (!silent) setLoading(false);
    }
  };

  const run = () =>
    load({
      saloonId: allSaloons || saloons.length === 0 ? "-1" : saloons.join(","),
      crisisTypeId: Number(crisis),
      criticalPartTypeId: Number(kind),
      subClassId: subClassId ?? 0,
      partNo: null,
      carModelId: modelId ?? 0,
      carClassId: classId ?? 0,
    });

  // تغییر بحران‌ها/مجوزها در پایگاه داده (SignalR) گزارش اجراشده را با همان شرایط تازه می‌کند
  const live = useLiveRefresh("trim-tracking", () => {
    if (lastInput.current) void load(lastInput.current, true);
  });

  const clearFilters = () => {
    setAllSaloons(true);
    setSaloons([]);
    setCrisis("100");
    setKind("-1");
    setModelId(null);
    setClassId(null);
    setSubClassId(null);
  };

  const requireActive = (open: Dialog) => {
    if (!active) {
      toast("ابتدا یک بحران را از جدول انتخاب کنید");
      return;
    }
    setDialog(open);
  };

  const partNo = String(value(active, "PartNo", "Partno", "PartNumber") ?? "").trim();
  const criticalId = Number(value(active, "CriticalPartsId", "Id", "id") ?? 0);
  const criticalType = Number(value(active, "CriticalPartTypeId") ?? 0);

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">گزارش ردیابی اقلام بحرانی</IkcoTypography>
      {showFilters && (
        <IkcoCard>
          <IkcoCardContent>
            <IkcoGrid container spacing={2} alignItems="center">
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoStack direction="row" spacing={1} alignItems="center">
                  <IkcoCheckbox label="همه موارد" checked={allSaloons} onChange={(_, c) => setAllSaloons(c)} />
                  {!allSaloons && <LookupMultiSelect lookupKey="trim-shops" label="سالن مونتاژ" value={saloons} onChange={setSaloons} />}
                </IkcoStack>
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoTypography variant="subtitle2">وضعیت امکان تولید</IkcoTypography>
                <IkcoRadioGroup row value={crisis} onChange={(_, v) => setCrisis(v)} options={[{ value: "100", label: "همه موارد" }, { value: "0", label: "عدم امکان تولید" }, { value: "1", label: "تولید با کسری" }, { value: "2", label: "تولید با قطعه جایگزین" }]} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoTypography variant="subtitle2">دسته بندی قطعات</IkcoTypography>
                <IkcoRadioGroup row value={kind} onChange={(_, v) => setKind(v)} options={[{ value: "-1", label: "همه موارد" }, { value: "1", label: "قطعات سنکرون" }, { value: "0", label: "قطعات مکانیکی" }]} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <LookupAutocomplete lookupKey="car-models" label="محصول" value={modelId} onChange={(v) => { setModelId(v); setClassId(null); setSubClassId(null); }} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <LookupAutocomplete lookupKey="car-classes" label="کلاس" value={classId} parentId={modelId} requireParent onChange={(v) => { setClassId(v); setSubClassId(null); }} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <LookupAutocomplete lookupKey="sub-classes" label="زیرکلاس" value={subClassId} parentId={classId} requireParent onChange={setSubClassId} />
              </IkcoGrid>
              <IkcoGrid size={12}>
                <TrackingLegend
                  items={[
                    ...legend,
                    {
                      label: "آستانه بحران، امکان تولید 0 تا 40",
                      color: "#FF00FF",
                      extra: <IkcoCheckbox label="" checked={threshold} onChange={(_, c) => setThreshold(c)} />,
                    },
                  ]}
                >
                  <IkcoChip size="small" status={live.connected ? "success" : "default"} label={live.connected ? "به‌روزرسانی زنده: متصل" : "به‌روزرسانی زنده: قطع"} />
                  <IkcoTypography variant="caption">{refreshedAt ? `آخرین بروزرسانی: ${refreshedAt}` : ""}</IkcoTypography>
                </TrackingLegend>
              </IkcoGrid>
            </IkcoGrid>
          </IkcoCardContent>
        </IkcoCard>
      )}

      <TrackingGrid tone={(r) => toneOf(r, marks, threshold)} columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} onRowClick={(p) => setActive(p.row as IGridRow)} autoHeight />
      <IkcoTypography variant="subtitle1">محصولات</IkcoTypography>
      <TrackingGrid tone={(r) => toneOf(r, marks, threshold)} columns={columns} rows={productRows} getRowId={(r) => r.__rowId} autoHeight />

      <ActionBar>
          <PermissionGuard can={ApiPermissions.mechReports.productionpossibility} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Refresh />} loading={loading} onClick={() => void run()}>
              بازخوانی
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton startIcon={showFilters ? <Visibility /> : <Search />} onClick={() => setShowFilters((s) => !s)}>
            نمایش/پنهان شرایط جستجو
          </IkcoButton>
          <IkcoButton startIcon={<CleaningServices />} onClick={clearFilters}>
            پاک کردن فیلترها
          </IkcoButton>
          <IkcoButton startIcon={<Print />} onClick={() => window.print()}>
            گزارش چاپی
          </IkcoButton>
          <ExcelExportButton fileName="گزارش ردیابی اقلام بحرانی" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoButton startIcon={<History />} onClick={() => requireActive("stock")}>
            LOG
          </IkcoButton>
          <IkcoButton startIcon={<History />} onClick={() => requireActive("license")}>
            لاگ مجوزها
          </IkcoButton>
          <IkcoButton startIcon={<Sync />} onClick={() => requireActive("sap")}>
            لاگ SAP
          </IkcoButton>
          <IkcoButton startIcon={<Sync />} onClick={() => setDialog("sapDiff")}>
            لاگ مغایرت SAP
          </IkcoButton>
          <IkcoButton startIcon={<LocalShipping />} onClick={() => setDialog("asn")}>
            گزارش ASN های در راه
          </IkcoButton>
          <IkcoButton onClick={() => setDialog("contrast")}>گزارش مغایرت زیرکلاس ها</IkcoButton>
          <PermissionGuard can={ApiPermissions.mechParts.update}>
            <IkcoButton startIcon={<Edit />} onClick={() => requireActive("edit")}>
              تغییر در مشخصات اقلام بحرانی
            </IkcoButton>
          </PermissionGuard>
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "شرایط را وارد و «بازخوانی» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>

      <TabularDialog open={dialog === "stock"} close={() => setDialog(null)} title="LOG" columns={gridColumns.mechStockLog} load={() => getMechReportsStockLog({ partNo })} />
      <TabularDialog open={dialog === "license"} close={() => setDialog(null)} title="لاگ مجوزها" columns={gridColumns.mechLicenseTrackingList} load={() => getMechLicensesTrackingList({ criticalPartTypeId: criticalType, criticalPartId: criticalId })} />
      <TabularDialog open={dialog === "sap"} close={() => setDialog(null)} title="لاگ SAP" columns={gridColumns.mechSapLog} load={() => getMechReportsSapCriticalLog({ partNo })} />
      <TabularDialog open={dialog === "sapDiff"} close={() => setDialog(null)} title="لاگ مغایرت SAP" columns={gridColumns.mechFailedSap} load={() => getMechReportsFailedSapTransfers({ fromDate: firstOfMonthJalali(), toDate: todayJalali() })} />
      <MechAsnDialog open={dialog === "asn"} close={() => setDialog(null)} />
      <MechContrastDialog open={dialog === "contrast"} close={() => setDialog(null)} />
      <FormDialog open={dialog === "edit"} close={() => setDialog(null)} title="تغییر در مشخصات اقلام بحرانی" maxWidth="xl" closeText="انصراف">
        {dialog === "edit" && (criticalType === 1 ? <SyncSetManagement editId={criticalId} onDone={() => setDialog(null)} /> : <MechCriticalRegister editId={criticalId} onDone={() => setDialog(null)} />)}
      </FormDialog>
    </IkcoStack>
  );
}
