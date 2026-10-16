import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoDatePicker,
  IkcoGrid,
  IkcoGridActionsCellItem,
  IkcoIconButton,
  IkcoStack,
  IkcoTextField,
  IkcoTooltip,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan, useMFENavigate } from "@ikco/mfe-core";
import { Edit, History, Inventory2, PlaylistAddCheck, Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import TabularDialog from "../../components/TabularDialog";
import { disableBodyCriticalsCriticalParts, getBodyCriticalsEventLog } from "../../server/services/bodyCriticalsService";
import { getBodyReportsCriticalPartProducts, searchBodyReportsCriticalParts } from "../../server/services/bodyReportsService";
import { ApiPermissions } from "../../types/permissions";
import { bodyReportsSearchCriticalPartsInputRules } from "../../types/BodyShop";
import type { ITabularResult } from "../../types/Common";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import BodyLicenseDialog from "./BodyLicenseDialog";

interface Filters {
  typeId: number | null;
  plantId: number | null;
  vinBiwSite: number | null;
  partNumber: string;
  activeOnly: boolean;
  fromDate: string | null;
  toDate: string | null;
  carGroupId: number | null;
  carModelId: number | null;
  carClassId: number | null;
  productionPossibilityId: number | null;
}

const defaults: Filters = {
  typeId: null,
  plantId: null,
  vinBiwSite: null,
  partNumber: "",
  activeOnly: true,
  fromDate: null,
  toDate: null,
  carGroupId: null,
  carModelId: null,
  carClassId: null,
  productionPossibilityId: null,
};

const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

/** شناسه ردیف نتیجه (ستون Id رویه) */
const rowCriticalId = (row: IGridRow) => Number((row as Record<string, unknown>)["Id"] ?? 0);

/**
 * جستجو و مدیریت اقلام بحرانی بدنه‌سازی (frmBodyCriticalSearch): جستجو با فیلترها، مشاهده محصولات، ویرایش، تغییر شرایط تولید،
 * رویدادها و غیرفعال‌سازی گروهی بحران‌های انتخاب‌شده.
 */
export default function BodyCriticalSearch() {
  const navigate = useMFENavigate();
  const canUpdate = useCan(ApiPermissions.bodyCriticals.update);
  const canLicense = useCan(ApiPermissions.bodyCriticals.license);
  const { control, register, handleSubmit, reset, watch, setValue, formState: { errors } } = useForm<Filters>({ defaultValues: defaults });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [selection, setSelection] = useState<number[]>([]);
  const [confirm, setConfirm] = useState(false);
  const [disabling, setDisabling] = useState(false);
  const [dialog, setDialog] = useState<{ kind: "products" | "events" | "license"; id: number } | null>(null);
  const [lastFilters, setLastFilters] = useState<Filters | null>(null);

  const typeId = watch("typeId") ?? 0;
  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const columns: IkcoGridColDef[] = useMemo(
    () => [
      ...baseColumns.filter((c) => c.field !== "Id"),
      {
        field: "__actions",
        type: "actions",
        headerName: "عملیات",
        width: 190,
        getActions: (params) => {
          const id = rowCriticalId(params.row as IGridRow);
          return [
            <IkcoGridActionsCellItem key="products" icon={<IkcoTooltip title="محصولات"><IkcoIconButton><Inventory2 color="primary" /></IkcoIconButton></IkcoTooltip>} label="محصولات" onClick={() => setDialog({ kind: "products", id })} />,
            <IkcoGridActionsCellItem key="events" icon={<IkcoTooltip title="رویدادها"><IkcoIconButton><History color="primary" /></IkcoIconButton></IkcoTooltip>} label="رویدادها" onClick={() => setDialog({ kind: "events", id })} />,
            ...(canUpdate ? [<IkcoGridActionsCellItem key="edit" icon={<IkcoTooltip title="ویرایش"><IkcoIconButton><Edit color="primary" /></IkcoIconButton></IkcoTooltip>} label="ویرایش" onClick={() => navigate(`body-shop/register?id=${id}`)} />] : []),
            ...(canLicense ? [<IkcoGridActionsCellItem key="license" icon={<IkcoTooltip title="تغییر شرایط تولید"><IkcoIconButton><PlaylistAddCheck color="primary" /></IkcoIconButton></IkcoTooltip>} label="شرایط تولید" onClick={() => setDialog({ kind: "license", id })} />] : []),
          ];
        },
      },
    ],
    [baseColumns, canUpdate, canLicense, navigate],
  );

  const run = async (f: Filters) => {
    setLoading(true);
    try {
      const data = await searchBodyReportsCriticalParts({
        plantId: f.plantId,
        vinBiwSite: f.vinBiwSite,
        partNumber: f.partNumber.trim() || null,
        status: f.activeOnly,
        fromDate: f.fromDate,
        toDate: f.toDate,
        carGroupId: f.carGroupId,
        carModelId: f.carModelId,
        carClassId: f.carClassId,
        productionPossibilityId: f.productionPossibilityId,
      });
      setLastFilters(f);
      setSelection([]);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const disableSelected = async () => {
    const ids = rows.filter((r) => selection.includes(r.__rowId)).map(rowCriticalId);
    if (ids.length === 0) return;
    setDisabling(true);
    try {
      const response = await disableBodyCriticalsCriticalParts({ ids });
      toast.success(response.message ?? "غیرفعال‌سازی انجام شد");
      setConfirm(false);
      if (lastFilters) await run(lastFilters);
    } finally {
      setDisabling(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">جستجوی اقلام بحرانی بدنه‌سازی</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={size}>
              <Controller name="typeId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-critical-part-types" label="نوع بحران" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="plantId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="virtual-plants" label="سایت" value={field.value} onChange={(v) => { field.onChange(v); setValue("vinBiwSite", null); }} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="vinBiwSite" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-saloons" label="خط بدنه‌سازی" value={field.value} parentId={watch("plantId")} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره فنی" {...register("partNumber", bodyReportsSearchCriticalPartsInputRules.partNumber)} error={!!errors.partNumber} helperText={errors.partNumber?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="fromDate" control={control} render={({ field }) => <IkcoDatePicker label="از تاریخ" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="toDate" control={control} render={({ field }) => <IkcoDatePicker label="تا تاریخ" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="carGroupId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-groups" label="خانواده محصول" value={field.value} onChange={(v) => { field.onChange(v); setValue("carModelId", null); setValue("carClassId", null); }} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="carModelId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-models" label="مدل" value={field.value} parentId={watch("carGroupId")} requireParent onChange={(v) => { field.onChange(v); setValue("carClassId", null); }} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="carClassId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="car-classes" label="کلاس" value={field.value} parentId={watch("carModelId")} requireParent onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="productionPossibilityId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="شرایط تولید" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="activeOnly" control={control} render={({ field }) => <IkcoCheckbox label="فقط فعال" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.bodyReports.search} fallback={<IkcoTypography color="error">شما به جستجو دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={handleSubmit(run)}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton onClick={() => { reset(defaults); setResult(null); setSelection([]); }}>پاک کردن</IkcoButton>
          <PermissionGuard can={ApiPermissions.bodyCriticals.disable}>
            <IkcoButton color="error" variant="outlined" disabled={selection.length === 0} onClick={() => setConfirm(true)}>
              غیرفعال‌سازی انتخاب‌شده‌ها ({selection.length})
            </IkcoButton>
          </PermissionGuard>
          <PermissionGuard can={ApiPermissions.bodyCriticals.create}>
            <IkcoButton onClick={() => navigate("body-shop/register")}>ثبت بحران جدید</IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="اقلام بحرانی بدنه‌سازی" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "فیلترها را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid
        columns={columns}
        rows={rows}
        loading={loading}
        getRowId={(r) => r.__rowId}
        checkboxSelection
        disableRowSelectionOnClick
        rowSelectionModel={selection}
        onRowSelectionModelChange={(m) => setSelection(m as number[])}
      />

      <ConfirmDialog
        open={confirm}
        close={setConfirm}
        title="غیرفعال‌سازی بحران"
        message={`بحران‌های انتخاب‌شده (${selection.length} مورد) غیرفعال شوند؟`}
        confirmText="غیرفعال‌سازی"
        loading={disabling}
        onConfirm={disableSelected}
      />
      <TabularDialog
        open={dialog?.kind === "products"}
        close={() => setDialog(null)}
        title="محصولات بحران"
        load={() => getBodyReportsCriticalPartProducts(dialog?.id ?? 0)}
      />
      <TabularDialog
        open={dialog?.kind === "events"}
        close={() => setDialog(null)}
        title="رویدادهای بحران"
        load={() => getBodyCriticalsEventLog({ eventTypeId: typeId, changedRecordId: dialog?.id ?? 0 })}
      />
      {dialog?.kind === "license" && (
        <BodyLicenseDialog
          open
          close={() => setDialog(null)}
          criticalPartId={dialog.id}
          criticalPartTypeId={typeId}
          onSaved={() => lastFilters && void run(lastFilters)}
        />
      )}
    </IkcoStack>
  );
}
