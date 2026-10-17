import {
  IkcoButton,
  IkcoCard,
  IkcoCardContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoDatePicker,
  IkcoGrid,
  IkcoStack,
  IkcoSwitch,
  IkcoTextArea,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan } from "@ikco/mfe-core";
import { Cancel, Check, Edit, ExitToApp, History, Logout, PlaylistAddCheck, Radar, Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import SectionPanel from "../../components/SectionPanel";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import TabularDialog from "../../components/TabularDialog";
import { disableBodyCriticalsCriticalParts, getBodyCriticalsEventLog, getBodyCriticalsPartDescription } from "../../server/services/bodyCriticalsService";
import { getBodyReportsCriticalPartProducts, getBodyReportsShortageLicenseTracking, searchBodyReportsCriticalParts } from "../../server/services/bodyReportsService";
import { bodyReportsSearchCriticalPartsInputRules } from "../../types/BodyShop";
import type { ITabularResult } from "../../types/Common";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import BodyLicenseDialog from "./BodyLicenseDialog";
import ActionBar from "../../components/ActionBar";

interface Filters {
  typeId: number | null;
  productionPossibilityId: number | null;
  plantId: number | null;
  vinBiwSite: number | null;
  partNumber: string;
  activeOnly: boolean;
  byDate: boolean;
  fromDate: string | null;
  toDate: string | null;
  byProduct: boolean;
  carGroupId: number | null;
  carModelId: number | null;
  carClassId: number | null;
}

const defaults: Filters = {
  typeId: null,
  productionPossibilityId: null,
  plantId: null,
  vinBiwSite: null,
  partNumber: "",
  activeOnly: true,
  byDate: false,
  fromDate: null,
  toDate: null,
  byProduct: false,
  carGroupId: null,
  carModelId: null,
  carClassId: null,
};

const size = { xl: 3, lg: 4, md: 6, sm: 6, xs: 12 };

const rowCriticalId = (row: IGridRow) => Number((row as Record<string, unknown>)["Id"] ?? 0);

interface Props {
  /** search: فرم frmBodyCriticalSearch با عملیات؛ report: گزارش frmRPTBodyCriticalSearch فقط خواندنی */
  mode: "search" | "report";
  /** تغییر در مشخصات بحران انتخاب‌شده (فرم ثبت در حالت ویرایش) */
  onEdit?: (criticalPartId: number) => void;
  /** تأیید: بازگرداندن بحران انتخاب‌شده به فرم فراخوان (هنگامی که فرم به صورت modal باز شده است) */
  onPick?: (criticalPartId: number) => void;
  /** خروج (بستن modal) */
  onExit?: () => void;
}

/**
 * چیدمان مشترک «جستجوی اقلام بحرانی بدنه‌سازی» (frmBodyCriticalSearch) و «گزارش اقلام بحرانی» (frmRPTBodyCriticalSearch):
 * فیلترها، جدول مشخصات اقلام بحرانی، جدول محصولات بحران انتخاب‌شده و دکمه‌های فرم. دکمه‌هایی که در فرم ویندوزی فرم دیگری
 * باز می‌کردند اینجا modal باز می‌کنند.
 */
export default function BodyCriticalSearchPanel({ mode, onEdit, onPick, onExit }: Props) {
  const canUpdate = useCan(ApiPermissions.bodyCriticals.update);
  const canLicense = useCan(ApiPermissions.bodyCriticals.license);
  const { control, register, handleSubmit, reset, watch, setValue, getValues, formState: { errors } } = useForm<Filters>({ defaultValues: defaults });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [products, setProducts] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [selection, setSelection] = useState<number[]>([]);
  const [activeId, setActiveId] = useState(0);
  const [partDesc, setPartDesc] = useState("");
  const [confirm, setConfirm] = useState(false);
  const [disabling, setDisabling] = useState(false);
  // حالت «حذف با ذکر دلیل» (txtReasonOfDelete / btnAcceptDelete / btnCancelDelete فرم ویندوزی)
  const [deleting, setDeleting] = useState(false);
  const [reason, setReason] = useState("");
  const [dialog, setDialog] = useState<"events" | "tracking" | "license" | null>(null);
  const [lastFilters, setLastFilters] = useState<Filters | null>(null);

  const rows = useMemo(() => tabularRows(result), [result]);
  const columns = useMemo(() => resultColumns(result, columnHeaders).filter((c) => c.field !== "Id"), [result]);
  const productColumns = useMemo(() => resultColumns(products, columnHeaders), [products]);
  const productRows = useMemo(() => tabularRows(products), [products]);
  const typeId = watch("typeId") ?? 0;
  const isSearch = mode === "search";

  const run = async (f: Filters) => {
    setLoading(true);
    try {
      const data = await searchBodyReportsCriticalParts({
        plantId: f.plantId,
        vinBiwSite: f.vinBiwSite,
        partNumber: f.partNumber.trim() || null,
        status: f.activeOnly,
        fromDate: f.byDate ? f.fromDate : null,
        toDate: f.byDate ? f.toDate : null,
        carGroupId: f.byProduct ? f.carGroupId : null,
        carModelId: f.byProduct ? f.carModelId : null,
        carClassId: f.byProduct ? f.carClassId : null,
        productionPossibilityId: f.productionPossibilityId,
      });
      setLastFilters(f);
      setSelection([]);
      setActiveId(0);
      setProducts(null);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const showProducts = async (id: number) => {
    setActiveId(id);
    setProducts(await getBodyReportsCriticalPartProducts(id));
  };

  const loadPartDesc = async () => {
    const partNo = getValues("partNumber").trim();
    if (!partNo) {
      setPartDesc("");
      return;
    }
    const desc = await getBodyCriticalsPartDescription({ partNo });
    setPartDesc(typeof desc === "string" ? desc : "");
  };

  const requireActive = (action: () => void) => {
    if (activeId <= 0) {
      toast("ابتدا یک بحران را از جدول انتخاب کنید");
      return;
    }
    action();
  };

  const startDelete = () => {
    if (selection.length === 0) {
      toast("ابتدا بحران‌های مورد نظر را از جدول انتخاب کنید");
      return;
    }
    setConfirm(true);
  };

  const cancelDelete = () => {
    setDeleting(false);
    setReason("");
  };

  const acceptDelete = async () => {
    if (!reason.trim()) {
      toast.error("لطفا توضیح دلیل حذف بحران را وارد كنید");
      return;
    }
    const ids = rows.filter((r) => selection.includes(r.__rowId)).map(rowCriticalId);
    if (ids.length === 0) return;
    setDisabling(true);
    try {
      const response = await disableBodyCriticalsCriticalParts({ ids, eventTypeId: typeId, reason: reason.trim() });
      toast.success(response.message ?? "غیرفعال‌سازی انجام شد");
      cancelDelete();
      if (lastFilters) await run(lastFilters);
    } finally {
      setDisabling(false);
    }
  };

  const clearAll = () => {
    reset(defaults);
    setResult(null);
    setProducts(null);
    setSelection([]);
    setActiveId(0);
    setPartDesc("");
    cancelDelete();
  };

  const hasRows = rows.length > 0;
  const byDate = watch("byDate");
  const byProduct = watch("byProduct");

  return (
    <IkcoStack spacing={2}>
      <SectionPanel title="شرایط جستجو">
        <IkcoGrid container spacing={2} alignItems="center">
          <IkcoGrid size={size}>
            <Controller name="typeId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-critical-part-types" label="نوع بحران" value={field.value} onChange={field.onChange} />} />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <Controller name="productionPossibilityId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="وضعیت امکان تولید" value={field.value} onChange={field.onChange} />} />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <Controller name="plantId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="virtual-plants" label="سایت" value={field.value} onChange={(v) => { field.onChange(v); setValue("vinBiwSite", null); }} />} />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <Controller name="vinBiwSite" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-saloons" label="سالن" value={field.value} parentId={watch("plantId")} onChange={field.onChange} />} />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <IkcoTextField label="شماره فنی" {...register("partNumber", { ...bodyReportsSearchCriticalPartsInputRules.partNumber, onBlur: () => void loadPartDesc() })} error={!!errors.partNumber} helperText={errors.partNumber?.message} />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <IkcoTextField label="شرح شماره فنی" value={partDesc} disabled />
          </IkcoGrid>
          <IkcoGrid size={size}>
            <Controller name="activeOnly" control={control} render={({ field }) => <IkcoSwitch label={`وضعیت اقلام بحرانی: ${field.value ? "فعال" : "غیرفعال"}`} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
          </IkcoGrid>
        </IkcoGrid>
        <IkcoGrid container spacing={2} alignItems="center">
          <IkcoGrid size={{ xs: 12, sm: 4, md: 2 }}>
            <Controller name="byDate" control={control} render={({ field }) => <IkcoCheckbox label="انتخاب تاریخ" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <Controller name="fromDate" control={control} render={({ field }) => <IkcoDatePicker label="از" value={field.value} disabled={!byDate} onChange={field.onChange} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <Controller name="toDate" control={control} render={({ field }) => <IkcoDatePicker label="تا" value={field.value} disabled={!byDate} onChange={field.onChange} />} />
          </IkcoGrid>
        </IkcoGrid>
        <IkcoGrid container spacing={2} alignItems="center">
          <IkcoGrid size={{ xs: 12, sm: 3, md: 2 }}>
            <Controller name="byProduct" control={control} render={({ field }) => <IkcoCheckbox label="انتخاب محصول" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 3, md: 3 }}>
            <Controller name="carGroupId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-groups" label="محصول" value={field.value} disabled={!byProduct} onChange={(v) => { field.onChange(v); setValue("carModelId", null); setValue("carClassId", null); }} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 3, md: 3 }}>
            <Controller name="carModelId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-models" label="مدل" value={field.value} disabled={!byProduct} parentId={watch("carGroupId")} requireParent onChange={(v) => { field.onChange(v); setValue("carClassId", null); }} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 3, md: 3 }}>
            <Controller name="carClassId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-classes" label="کلاس" value={field.value} disabled={!byProduct} parentId={watch("carModelId")} requireParent onChange={field.onChange} />} />
          </IkcoGrid>
        </IkcoGrid>
      </SectionPanel>

      <IkcoTypography variant="subtitle1">مشخصات اقلام بحرانی</IkcoTypography>
      <IkcoDataGrid
        columns={columns}
        rows={rows}
        loading={loading}
        getRowId={(r) => r.__rowId}
        checkboxSelection={isSearch}
        disableRowSelectionOnClick={isSearch}
        rowSelectionModel={isSearch ? selection : undefined}
        onRowSelectionModelChange={isSearch ? (m) => setSelection(m as number[]) : undefined}
        onRowClick={(p) => void showProducts(rowCriticalId(p.row as IGridRow))}
        autoHeight
      />
      <IkcoTypography variant="subtitle1">{deleting ? "توضیح دلیل حذف قطعه بحرانی" : "محصولات"}</IkcoTypography>
      {deleting ? (
        <IkcoCard>
          <IkcoCardContent>
            <IkcoStack spacing={2}>
              <IkcoTextArea label={`دلیل حذف (${selection.length} بحران انتخاب‌شده)`} rows={3} value={reason} onChange={(e) => setReason(e.target.value)} />
              <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
                <IkcoButton variant="contained" color="error" startIcon={<Check />} loading={disabling} onClick={() => void acceptDelete()}>
                  تایید حذف
                </IkcoButton>
                <IkcoButton startIcon={<Cancel />} disabled={disabling} onClick={cancelDelete}>
                  انصراف از حذف
                </IkcoButton>
              </IkcoStack>
            </IkcoStack>
          </IkcoCardContent>
        </IkcoCard>
      ) : (
        <IkcoDataGrid columns={productColumns} rows={productRows} getRowId={(r) => r.__rowId} autoHeight />
      )}

      <ActionBar>
          <PermissionGuard can={ApiPermissions.bodyReports.search} fallback={<IkcoTypography color="error">شما به جستجو دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} disabled={deleting} onClick={handleSubmit(run)}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          {isSearch ? (
            <>
              <PermissionGuard can={ApiPermissions.bodyCriticals.disable}>
                <IkcoButton color="error" variant="outlined" startIcon={<Logout />} disabled={deleting || !hasRows} onClick={startDelete}>
                  خروج از لیست اقلام بحرانی ({selection.length})
                </IkcoButton>
              </PermissionGuard>
              {canUpdate && onEdit && (
                <IkcoButton variant="outlined" startIcon={<Edit />} disabled={deleting || !hasRows} onClick={() => requireActive(() => onEdit(activeId))}>
                  تغییر در مشخصات اقلام بحرانی
                </IkcoButton>
              )}
              {canLicense && (
                <IkcoButton variant="outlined" startIcon={<PlaylistAddCheck />} disabled={deleting || !hasRows} onClick={() => requireActive(() => setDialog("license"))}>
                  تغییر در شرایط تولید
                </IkcoButton>
              )}
            </>
          ) : (
            <ExcelExportButton fileName="گزارش اقلام بحرانی" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          )}
          <IkcoButton variant="outlined" startIcon={<Radar />} disabled={deleting || !hasRows} onClick={() => requireActive(() => setDialog("tracking"))}>
            ردیابی پارامترهای بحرانی
          </IkcoButton>
          {isSearch && (
            <IkcoButton variant="outlined" startIcon={<History />} disabled={deleting || !hasRows} onClick={() => requireActive(() => setDialog("events"))}>
              LOG
            </IkcoButton>
          )}
          {isSearch && (
            <IkcoButton startIcon={<Cancel />} disabled={deleting} onClick={clearAll}>
              انصراف
            </IkcoButton>
          )}
          {isSearch && onPick && (
            <IkcoButton variant="contained" startIcon={<Check />} disabled={deleting || !hasRows} onClick={() => requireActive(() => onPick(activeId))}>
              تایید
            </IkcoButton>
          )}
          {onExit && (
            <IkcoButton startIcon={<ExitToApp />} onClick={onExit}>
              خروج
            </IkcoButton>
          )}
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "فیلترها را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>

      <ConfirmDialog
        open={confirm}
        close={setConfirm}
        title="هشدار"
        message="آیا از حذف اقلام بحرانی مطمئن هستید؟"
        confirmText="بله"
        onConfirm={() => {
          setConfirm(false);
          setDeleting(true);
        }}
      />
      <TabularDialog open={dialog === "events"} close={() => setDialog(null)} title="گزارش رویدادها" load={() => getBodyCriticalsEventLog({ eventTypeId: typeId, changedRecordId: activeId })} />
      <TabularDialog open={dialog === "tracking"} close={() => setDialog(null)} title="ردیابی پارامترهای بحرانی" load={() => getBodyReportsShortageLicenseTracking({ criticalPartTypeId: typeId, criticalPartId: activeId })} />
      {dialog === "license" && <BodyLicenseDialog open close={() => setDialog(null)} criticalPartId={activeId} criticalPartTypeId={typeId} onSaved={() => lastFilters && void run(lastFilters)} />}
    </IkcoStack>
  );
}
