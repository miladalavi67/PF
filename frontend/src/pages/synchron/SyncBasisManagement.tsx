import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCardContent as DialogContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoDialog,
  IkcoDialogActions,
  IkcoDialogTitle,
  IkcoGrid,
  IkcoGridActionsCellItem,
  IkcoIconButton,
  IkcoStack,
  IkcoTextField,
  IkcoTooltip,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan } from "@ikco/mfe-core";
import { Add, Edit } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ExcelExportButton from "../../components/ExcelExportButton";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import { createSyncBasis, getSyncBasisByStatus, updateSyncBasis } from "../../server/services/syncBasisService";
import type { ITabularResult } from "../../types/Common";
import { syncBasisCreateInputRules } from "../../types/Synchron";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";

interface FormValues {
  code: string;
  descF: string;
  carClassId: number | null;
  partTypeId: string;
  levelType: string;
  isTrim: boolean;
  isOption: boolean;
  isToodoozi: boolean;
  isColor: boolean;
  active: boolean;
}

const emptyForm: FormValues = { code: "", descF: "", carClassId: null, partTypeId: "", levelType: "2", isTrim: false, isOption: false, isToodoozi: false, isColor: false, active: true };

const raw = (row: IGridRow, ...keys: string[]) => {
  const r = row as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return r[k];
  return undefined;
};
const flag = (v: unknown) => v === true || v === 1 || v === "1" || String(v).toLowerCase() === "true";

/**
 * تعریف مبنای ست‌های سنکرون (frmCriticalPartsSetBasis): کد و شرح ست، کلاس مبنا، نوع قطعه (شناسه و سطح درخت BOM)
 * و تفکیک بر اساس تریم/آپشن/تودوزی/رنگ. ثبت با S_InsertCriticalPartSet و ویرایش با S_UpdateCriticalPartsSet.
 */
export default function SyncBasisManagement() {
  const canUpdate = useCan(ApiPermissions.syncBasis.update);
  const [statusFilter, setStatusFilter] = useState<number | null>(1);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [editing, setEditing] = useState<{ id: number | null } | null>(null);
  const [saving, setSaving] = useState(false);
  const { control, register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>({ defaultValues: emptyForm });

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setResult(await getSyncBasisByStatus({ status: statusFilter ?? 1 }));
    } finally {
      setLoading(false);
    }
  }, [statusFilter]);

  useEffect(() => {
    void load();
  }, [load]);

  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const openEdit = useCallback(
    (row: IGridRow) => {
      const status = raw(row, "Status");
      reset({
        code: String(raw(row, "Code") ?? ""),
        descF: String(raw(row, "DescF") ?? ""),
        carClassId: raw(row, "BasisCarClassId") === undefined ? null : Number(raw(row, "BasisCarClassId")),
        partTypeId: String(raw(row, "PartTypeId") ?? ""),
        levelType: String(raw(row, "LevelType") ?? "2"),
        isTrim: flag(raw(row, "IsTrim")),
        isOption: flag(raw(row, "IsOption")),
        isToodoozi: flag(raw(row, "IsToodoozi")),
        isColor: flag(raw(row, "IsColor")),
        active: status === undefined ? true : !(status === 0 || status === "0" || status === false || String(status).includes("غير") || String(status).includes("غیر")),
      });
      setEditing({ id: Number(raw(row, "Id", "id")) });
    },
    [reset],
  );

  const columns: IkcoGridColDef[] = useMemo(
    () =>
      canUpdate
        ? [
            {
              field: "__actions",
              type: "actions",
              headerName: "ویرایش",
              width: 80,
              getActions: (params) => [
                <IkcoGridActionsCellItem key="e" icon={<IkcoTooltip title="ویرایش"><IkcoIconButton><Edit color="primary" /></IkcoIconButton></IkcoTooltip>} label="ویرایش" onClick={() => openEdit(params.row as IGridRow)} />,
              ],
            },
            ...baseColumns,
          ]
        : baseColumns,
    [baseColumns, canUpdate, openEdit],
  );

  const onSubmit = async (v: FormValues) => {
    if (!editing || v.carClassId === null) return;
    setSaving(true);
    try {
      const common = { code: v.code.trim(), descF: v.descF.trim(), partTypeId: Number(v.partTypeId), levelType: Number(v.levelType), isTrim: v.isTrim, isOption: v.isOption, isToodoozi: v.isToodoozi, isColor: v.isColor };
      if (editing.id === null) await createSyncBasis({ ...common, carClass: v.carClassId });
      else await updateSyncBasis(editing.id, { ...common, carClassId: v.carClassId, status: v.active });
      toast.success("مبنای ست ذخیره شد");
      setEditing(null);
      await load();
    } finally {
      setSaving(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">تعریف مبنای ست‌ها</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xl: 3, lg: 3, md: 4, sm: 6, xs: 12 }}>
              <LookupAutocomplete lookupKey="active-states" label="نمایش" value={statusFilter} onChange={setStatusFilter} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.syncBasis.create}>
            <IkcoButton variant="contained" startIcon={<Add />} onClick={() => { reset(emptyForm); setEditing({ id: null }); }}>
              مبنای جدید
            </IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="مبنای ست‌های سنکرون" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} />

      <IkcoDialog open={editing !== null} maxWidth="md" fullWidth onClose={() => setEditing(null)}>
        <IkcoDialogTitle>{editing?.id === null ? "ثبت مبنای ست جدید" : "ویرایش مبنای ست"}</IkcoDialogTitle>
        <DialogContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xs: 12, sm: 4 }}>
              <IkcoTextField label="کد ست" {...register("code", syncBasisCreateInputRules.code)} error={!!errors.code} helperText={errors.code?.message} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 8 }}>
              <IkcoTextField label="شرح فارسی" {...register("descF", syncBasisCreateInputRules.descF)} error={!!errors.descF} helperText={errors.descF?.message} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 4 }}>
              <Controller name="carClassId" control={control} rules={{ required: "کلاس مبنا را انتخاب کنید" }} render={({ field }) => <LookupAutocomplete lookupKey="car-classes" label="کلاس مبنا" value={field.value} onChange={field.onChange} error={!!errors.carClassId} helperText={errors.carClassId?.message} />} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 4 }}>
              <IkcoTextField label="شناسه نوع قطعه (BOM)" type="number" {...register("partTypeId", { required: "شناسه نوع قطعه الزامی است", min: { value: 1, message: "نامعتبر" } })} error={!!errors.partTypeId} helperText={errors.partTypeId?.message} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 4 }}>
              <IkcoTextField label="سطح نوع قطعه (۱ تا ۳)" type="number" {...register("levelType", { required: "سطح الزامی است", min: { value: 1, message: "۱ تا ۳" }, max: { value: 3, message: "۱ تا ۳" } })} error={!!errors.levelType} helperText={errors.levelType?.message} />
            </IkcoGrid>
            {(["isTrim", "isOption", "isToodoozi", "isColor"] as const).map((name, i) => (
              <IkcoGrid key={name} size={{ xs: 6, sm: 3 }}>
                <Controller name={name} control={control} render={({ field }) => <IkcoCheckbox label={["بر اساس تریم", "بر اساس آپشن", "بر اساس تودوزی", "بر اساس رنگ"][i]} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
              </IkcoGrid>
            ))}
            {editing?.id !== null && (
              <IkcoGrid size={12}>
                <Controller name="active" control={control} render={({ field }) => <IkcoCheckbox label="فعال" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
              </IkcoGrid>
            )}
          </IkcoGrid>
        </DialogContent>
        <IkcoDialogActions>
          <IkcoButton variant="contained" loading={saving} onClick={handleSubmit(onSubmit)}>ذخیره</IkcoButton>
          <IkcoButton onClick={() => setEditing(null)}>انصراف</IkcoButton>
        </IkcoDialogActions>
      </IkcoDialog>
    </IkcoStack>
  );
}
