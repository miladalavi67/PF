import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
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
import { Delete, Edit, History, Inventory2, PlaylistAddCheck, Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import LookupMultiSelect from "../../components/LookupMultiSelect";
import TabularDialog from "../../components/TabularDialog";
import { getMechLicensesTrackingList } from "../../server/services/mechLicensesService";
import { deleteMechParts, getMechPartsProducts } from "../../server/services/mechPartsService";
import { searchMechSearchCriticalParts } from "../../server/services/mechSearchService";
import { deleteSyncSets, getSyncSetsProductDetails } from "../../server/services/syncSetsService";
import type { ITabularResult } from "../../types/Common";
import { mechSearchSearchCriticalPartsInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";

interface Filters {
  partNo: string;
  setCode: string;
  fromDate: string | null;
  toDate: string | null;
  trimShopIds: number[];
  status: number | null;
  kind: number | null;
}

const defaults: Filters = { partNo: "", setCode: "", fromDate: null, toDate: null, trimShopIds: [], status: 1, kind: -1 };
const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

type RowAction = { type: "products" | "history" | "delete"; id: number; kind: number };

const field = (row: IGridRow, ...keys: string[]) => {
  const r = row as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return Number(r[k]);
  return 0;
};

/**
 * جستجوی اقلام بحرانی مکانیکی و سنکرون (frmCriticalPartSearch، رویه S_CriticalPartsSearchByConditions). از نتیجه می‌توان
 * محصولات را دید، تاریخچه مجوزها را مشاهده کرد، بحران را ویرایش یا حذف کرد و شرایط تولید را تغییر داد.
 */
export default function MechCriticalSearch() {
  const navigate = useMFENavigate();
  const canUpdate = useCan(ApiPermissions.mechParts.update);
  const canDelete = useCan(ApiPermissions.mechParts.delete);
  const canChange = useCan(ApiPermissions.mechLicenses.change);
  const { control, register, handleSubmit, reset, formState: { errors } } = useForm<Filters>({ defaultValues: defaults });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [action, setAction] = useState<RowAction | null>(null);
  const [deleting, setDeleting] = useState(false);
  const [last, setLast] = useState<Filters | null>(null);

  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const columns: IkcoGridColDef[] = useMemo(
    () => [
      {
        field: "__actions",
        type: "actions",
        headerName: "عملیات",
        width: 230,
        getActions: (params) => {
          const row = params.row as IGridRow;
          const id = field(row, "id", "Id");
          const kind = field(row, "CriticalPartTypeId");
          const editPath = kind === 1 ? `synchron/sets?id=${id}` : `mechanical/register?id=${id}`;
          return [
            <IkcoGridActionsCellItem key="p" icon={<IkcoTooltip title="محصولات"><IkcoIconButton><Inventory2 color="primary" /></IkcoIconButton></IkcoTooltip>} label="محصولات" onClick={() => setAction({ type: "products", id, kind })} />,
            <IkcoGridActionsCellItem key="h" icon={<IkcoTooltip title="تاریخچه مجوزها"><IkcoIconButton><History color="primary" /></IkcoIconButton></IkcoTooltip>} label="تاریخچه" onClick={() => setAction({ type: "history", id, kind })} />,
            ...(canChange ? [<IkcoGridActionsCellItem key="l" icon={<IkcoTooltip title="تغییر شرایط تولید"><IkcoIconButton><PlaylistAddCheck color="primary" /></IkcoIconButton></IkcoTooltip>} label="شرایط تولید" onClick={() => navigate(`mechanical/license-change?type=${kind}&id=${id}`)} />] : []),
            ...(canUpdate ? [<IkcoGridActionsCellItem key="e" icon={<IkcoTooltip title="ویرایش"><IkcoIconButton><Edit color="primary" /></IkcoIconButton></IkcoTooltip>} label="ویرایش" onClick={() => navigate(editPath)} />] : []),
            ...(canDelete ? [<IkcoGridActionsCellItem key="d" icon={<IkcoTooltip title="حذف"><IkcoIconButton><Delete color="error" /></IkcoIconButton></IkcoTooltip>} label="حذف" onClick={() => setAction({ type: "delete", id, kind })} />] : []),
          ];
        },
      },
      ...baseColumns,
    ],
    [baseColumns, canChange, canUpdate, canDelete, navigate],
  );

  const run = async (f: Filters) => {
    setLoading(true);
    try {
      const data = await searchMechSearchCriticalParts({
        partNo: f.partNo.trim() || null,
        setCode: f.setCode.trim() || null,
        fromDate: f.fromDate,
        toDate: f.toDate,
        trimShopIds: f.trimShopIds.length > 0 ? f.trimShopIds.join(",") : "-1",
        status: f.status ?? -1,
        criticalPartTypeId: f.kind ?? -1,
      });
      setLast(f);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const confirmDelete = async () => {
    if (!action) return;
    setDeleting(true);
    try {
      if (action.kind === 1) await deleteSyncSets(action.id);
      else await deleteMechParts(action.id);
      toast.success("حذف انجام شد");
      setAction(null);
      if (last) await run(last);
    } finally {
      setDeleting(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">جستجوی اقلام بحرانی (مکانیکی و سنکرون)</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره فنی" {...register("partNo", mechSearchSearchCriticalPartsInputRules.partNo)} error={!!errors.partNo} helperText={errors.partNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="کد ست (سنکرون)" {...register("setCode", mechSearchSearchCriticalPartsInputRules.setCode)} error={!!errors.setCode} helperText={errors.setCode?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="fromDate" control={control} render={({ field: f }) => <IkcoDatePicker label="از تاریخ ثبت" value={f.value} onChange={f.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="toDate" control={control} render={({ field: f }) => <IkcoDatePicker label="تا تاریخ ثبت" value={f.value} onChange={f.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="trimShopIds" control={control} render={({ field: f }) => <LookupMultiSelect lookupKey="trim-shops" label="سالن‌های مونتاژ" value={f.value} onChange={f.onChange} helperText="بدون انتخاب = همه" />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="status" control={control} render={({ field: f }) => <LookupAutocomplete lookupKey="critical-statuses" label="وضعیت" value={f.value} onChange={f.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="kind" control={control} render={({ field: f }) => <LookupAutocomplete lookupKey="critical-kinds" label="نوع بحران" value={f.value} onChange={f.onChange} />} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.mechSearch.search} fallback={<IkcoTypography color="error">شما به جستجو دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={handleSubmit(run)}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton onClick={() => { reset(defaults); setResult(null); }}>پاک کردن</IkcoButton>
          <PermissionGuard can={ApiPermissions.mechParts.create}>
            <IkcoButton onClick={() => navigate("mechanical/register")}>ثبت بحران مکانیکی جدید</IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="جستجوی اقلام بحرانی" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "فیلترها را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} />

      <TabularDialog
        open={action?.type === "products"}
        close={() => setAction(null)}
        title="محصولات بحران"
        load={() => (action?.kind === 1 ? getSyncSetsProductDetails(action.id) : getMechPartsProducts(action?.id ?? 0))}
      />
      <TabularDialog
        open={action?.type === "history"}
        close={() => setAction(null)}
        title="تاریخچه مجوزهای بحران"
        load={() => getMechLicensesTrackingList({ criticalPartTypeId: action?.kind ?? 0, criticalPartId: action?.id ?? 0 })}
      />
      <ConfirmDialog
        open={action?.type === "delete"}
        close={() => setAction(null)}
        title="حذف بحران"
        message="محصولات این بحران حذف شود؟ این عملیات قابل بازگشت نیست."
        confirmText="حذف"
        loading={deleting}
        onConfirm={confirmDelete}
      />
    </IkcoStack>
  );
}
