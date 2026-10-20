import { IkcoButton, IkcoCard, IkcoCardContent, IkcoChip, IkcoDataGrid, IkcoGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { History, Search } from "@mui/icons-material";
import { useCallback, useMemo, useRef, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ExcelExportButton from "../../../components/ExcelExportButton";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import TabularDialog from "../../../components/TabularDialog";
import { getBodyCriticalsEventLog } from "../../../server/services/bodyCriticalsService";
import { getBodyReportsTrackingCriticalPartProducts, getBodyReportsTrackingCriticalParts } from "../../../server/services/bodyReportsService";
import type { ITabularResult } from "../../../types/tabular";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../../utils/tabularUtility";
import ActionBar from "../../../components/ActionBar";
import useLiveRefresh from "../../../hooks/useLiveRefresh";

interface Filters {
  plantId: number | null;
  typeId: number | null;
  vinBiwSite: number | null;
  productionPossibilityId: number | null;
  carGroupId: number | null;
  carModelId: number | null;
  carClassId: number | null;
  subClassId: number | null;
}

const defaults: Filters = { plantId: null, typeId: null, vinBiwSite: null, productionPossibilityId: null, carGroupId: null, carModelId: null, carClassId: null, subClassId: null };

const size = { xl: 3, lg: 3, md: 6, sm: 6, xs: 12 };

/** راهنمای رنگ بحران‌ها (مطابق فرم ویندوزی) */
const legend: { label: string; color: "warning" | "success" | "info" | "error" | "default" }[] = [
  { label: "بحران جدید", color: "warning" },
  { label: "بلامانع طی یک ساعت اخیر", color: "success" },
  { label: "تغییر در موجودی", color: "info" },
  { label: "امکان تولید کمتر از 15", color: "error" },
  { label: "آستانه بحران، امکان تولید بین 15 و 25", color: "default" },
];

const criticalId = (row: IGridRow) => Number((row as Record<string, unknown>)["Id"] ?? 0);

/**
 * گزارش ردیابی تغییرات اقلام بحرانی (frmRPTTrackingCriticalParts).
 * Backend: POST /api/production-feasibility/body-shop/reports/tracking-critical-parts (PF.GetTrackingCriticalParts_V2)، دسترسی bodyshop.reports.tracking.
 * با کلیک روی یک بحران، محصولات آن در جدول پایین بارگذاری می‌شود؛ LOG رویدادهای بحران انتخاب‌شده را در modal نشان می‌دهد؛
 * تغییر مجوزها و بحران‌ها در پایگاه داده با SignalR (کانال body-tracking) اعلام و گزارش با همان شرایط دوباره بارگذاری می‌شود.
 */
export default function BodyTrackingReport() {
  const { control, handleSubmit, watch, setValue } = useForm<Filters>({ defaultValues: defaults });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [products, setProducts] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [activeId, setActiveId] = useState(0);
  const [showLog, setShowLog] = useState(false);
  const lastFilters = useRef<Filters | null>(null);

  const columns = useMemo(() => resultColumns(result, columnHeaders).filter((c) => c.field !== "Id"), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);
  const productColumns = useMemo(() => resultColumns(products, columnHeaders), [products]);
  const productRows = useMemo(() => tabularRows(products), [products]);

  const execute = useCallback(async (f: Filters, silent = false) => {
    if (!silent) setLoading(true);
    try {
      const data = await getBodyReportsTrackingCriticalParts({
        plantId: f.plantId,
        vinBiwSite: f.vinBiwSite,
        carGroupId: f.carGroupId,
        carModelId: f.carModelId,
        carClassId: f.carClassId,
        subClassId: f.subClassId,
        productionPossibilityId: f.productionPossibilityId,
      });
      lastFilters.current = f;
      setResult(data);
      if (!silent) {
        setProducts(null);
        setActiveId(0);
      }
    } finally {
      if (!silent) setLoading(false);
    }
  }, []);

  // جایگزین Service Broker فرم ویندوزی (RealTimeDetectChangeOnLicense): تغییر مجوزها/بحران‌ها با SignalR اعلام و گزارش با همان شرایط تازه می‌شود
  const live = useLiveRefresh("body-tracking", () => {
    if (lastFilters.current) void execute(lastFilters.current, true);
  });

  const selectRow = async (id: number) => {
    setActiveId(id);
    setProducts(await getBodyReportsTrackingCriticalPartProducts(id));
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">گزارش ردیابی تغییرات اقلام بحرانی</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoStack spacing={2}>
            <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap alignItems="center">
              <IkcoTypography variant="subtitle2">راهنمای رنگ بحران‌ها:</IkcoTypography>
              {legend.map((l) => (
                <IkcoChip key={l.label} label={l.label} status={l.color} size="small" />
              ))}
              <IkcoChip size="small" status={live.connected ? "success" : "default"} label={live.connected ? "به‌روزرسانی زنده: متصل" : "به‌روزرسانی زنده: قطع"} />
            </IkcoStack>
            <IkcoGrid container spacing={2}>
              <IkcoGrid size={size}>
                <Controller name="plantId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="virtual-plants" label="سایت" value={field.value} onChange={(v) => { field.onChange(v); setValue("vinBiwSite", null); }} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="typeId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-critical-part-types" label="نوع بحران" value={field.value} onChange={field.onChange} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="vinBiwSite" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="body-saloons" label="سالن" value={field.value} parentId={watch("plantId")} requireParent onChange={field.onChange} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="productionPossibilityId" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="production-possibilities" label="وضعیت امکان تولید" value={field.value} onChange={field.onChange} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="carGroupId" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="car-groups" label="گروه" value={field.value} onChange={(v) => { field.onChange(v); setValue("carModelId", null); setValue("carClassId", null); setValue("subClassId", null); }} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="carModelId" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="car-models" label="محصول" value={field.value} parentId={watch("carGroupId")} requireParent onChange={(v) => { field.onChange(v); setValue("carClassId", null); setValue("subClassId", null); }} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="carClassId" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="car-classes" label="کلاس" value={field.value} parentId={watch("carModelId")} requireParent onChange={(v) => { field.onChange(v); setValue("subClassId", null); }} />} />
              </IkcoGrid>
              <IkcoGrid size={size}>
                <Controller name="subClassId" control={control} render={({ field }) => <LookupAutocomplete allLabel="همه موارد" lookupKey="sub-classes" label="زیرکلاس" value={field.value} parentId={watch("carClassId")} requireParent onChange={field.onChange} />} />
              </IkcoGrid>
            </IkcoGrid>
          </IkcoStack>
        </IkcoCardContent>
      </IkcoCard>

      <IkcoTypography variant="subtitle1">لیست بحران ها</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} onRowClick={(p) => void selectRow(criticalId(p.row as IGridRow))} autoHeight />
      <IkcoTypography variant="subtitle1">محصولات</IkcoTypography>
      <IkcoDataGrid columns={productColumns} rows={productRows} getRowId={(r) => r.__rowId} autoHeight />

      <ActionBar>
          <PermissionGuard can={ApiPermissions.bodyReports.tracking} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={handleSubmit((f) => execute(f))}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton
            variant="outlined"
            startIcon={<History />}
            onClick={() => (activeId > 0 ? setShowLog(true) : toast("ابتدا یک بحران را از جدول انتخاب کنید"))}
          >
            LOG
          </IkcoButton>
          <ExcelExportButton fileName="گزارش ردیابی تغییرات اقلام بحرانی" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "فیلترها را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>

      <TabularDialog open={showLog} close={setShowLog} title="گزارش رویدادها" load={() => getBodyCriticalsEventLog({ eventTypeId: watch("typeId") ?? 0, changedRecordId: activeId })} />
    </IkcoStack>
  );
}
