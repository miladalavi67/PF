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
import { Add, Delete, Edit } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import { createMechReasons, deleteMechReasons, getMechReasonsList, updateMechReasons } from "../../server/services/mechReasonsService";
import type { ITabularResult } from "../../types/Common";
import { mechReasonsCreateInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";

interface FormValues {
  code: string;
  descF: string;
  descE: string;
  active: boolean;
}

const emptyForm: FormValues = { code: "", descF: "", descE: "", active: true };

const raw = (row: IGridRow, ...keys: string[]) => {
  const r = row as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return r[k];
  return "";
};

/**
 * دلایل عدم امکان تولید (frmProgNotPossibilityReason): فهرست فعال/غیرفعال و ثبت، ویرایش و حذف.
 * شناسه ردیف همان ستون Id رویه S_ProgNotPossibilityReasonSelectAll است.
 */
export default function MechNotPossibleReasons() {
  const canCreate = useCan(ApiPermissions.mechReasons.create);
  const canUpdate = useCan(ApiPermissions.mechReasons.update);
  const canDelete = useCan(ApiPermissions.mechReasons.delete);
  const [selectType, setSelectType] = useState<number | null>(1);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [editing, setEditing] = useState<{ id: number | null } | null>(null);
  const [removing, setRemoving] = useState<number | null>(null);
  const [saving, setSaving] = useState(false);
  const { control, register, handleSubmit, reset, formState: { errors } } = useForm<FormValues>({ defaultValues: emptyForm });

  const load = useCallback(async () => {
    setLoading(true);
    try {
      setResult(await getMechReasonsList({ selectType: selectType ?? 1 }));
    } finally {
      setLoading(false);
    }
  }, [selectType]);

  useEffect(() => {
    void load();
  }, [load]);

  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const openEdit = useCallback(
    (row: IGridRow) => {
      const status = String(raw(row, "Status")).toLowerCase();
      reset({
        code: String(raw(row, "Code")),
        descF: String(raw(row, "DescF")),
        descE: String(raw(row, "DescE")),
        active: status !== "0" && status !== "false",
      });
      setEditing({ id: Number(raw(row, "Id", "id")) });
    },
    [reset],
  );

  const columns: IkcoGridColDef[] = useMemo(
    () => [
      {
        field: "__actions",
        type: "actions",
        headerName: "عملیات",
        width: 110,
        getActions: (params) => {
          const row = params.row as IGridRow;
          return [
            ...(canUpdate ? [<IkcoGridActionsCellItem key="e" icon={<IkcoTooltip title="ویرایش"><IkcoIconButton><Edit color="primary" /></IkcoIconButton></IkcoTooltip>} label="ویرایش" onClick={() => openEdit(row)} />] : []),
            ...(canDelete ? [<IkcoGridActionsCellItem key="d" icon={<IkcoTooltip title="حذف"><IkcoIconButton><Delete color="error" /></IkcoIconButton></IkcoTooltip>} label="حذف" onClick={() => setRemoving(Number(raw(row, "Id", "id")))} />] : []),
          ];
        },
      },
      ...baseColumns,
    ],
    [baseColumns, canUpdate, canDelete, openEdit],
  );

  const onSubmit = async (v: FormValues) => {
    if (!editing) return;
    setSaving(true);
    try {
      const body = { code: v.code.trim(), descF: v.descF.trim(), descE: v.descE.trim(), status: v.active ? 1 : 0 };
      if (editing.id === null) await createMechReasons(body);
      else await updateMechReasons(editing.id, body);
      toast.success("دلیل عدم امکان تولید ذخیره شد");
      setEditing(null);
      await load();
    } finally {
      setSaving(false);
    }
  };

  const confirmDelete = async () => {
    if (removing === null) return;
    setSaving(true);
    try {
      await deleteMechReasons(removing);
      toast.success("حذف انجام شد");
      setRemoving(null);
      await load();
    } finally {
      setSaving(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">دلایل عدم امکان تولید</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xl: 3, lg: 3, md: 4, sm: 6, xs: 12 }}>
              <LookupAutocomplete lookupKey="active-states" label="نمایش" value={selectType} onChange={setSelectType} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.mechReasons.create}>
            <IkcoButton variant="contained" startIcon={<Add />} onClick={() => { reset(emptyForm); setEditing({ id: null }); }}>
              دلیل جدید
            </IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="دلایل عدم امکان تولید" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
          {!canCreate && !canUpdate && !canDelete && <IkcoTypography variant="caption">فقط مشاهده</IkcoTypography>}
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} />

      <IkcoDialog open={editing !== null} maxWidth="sm" fullWidth onClose={() => setEditing(null)}>
        <IkcoDialogTitle>{editing?.id === null ? "ثبت دلیل جدید" : "ویرایش دلیل"}</IkcoDialogTitle>
        <DialogContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xs: 12, sm: 4 }}>
              <IkcoTextField label="کد" disabled={editing?.id !== null} {...register("code", mechReasonsCreateInputRules.code)} error={!!errors.code} helperText={errors.code?.message} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 8 }}>
              <Controller name="active" control={control} render={({ field }) => <IkcoCheckbox label="فعال" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
            <IkcoGrid size={12}>
              <IkcoTextField label="شرح فارسی" {...register("descF", mechReasonsCreateInputRules.descF)} error={!!errors.descF} helperText={errors.descF?.message} />
            </IkcoGrid>
            <IkcoGrid size={12}>
              <IkcoTextField label="شرح انگلیسی" {...register("descE", mechReasonsCreateInputRules.descE)} error={!!errors.descE} helperText={errors.descE?.message} />
            </IkcoGrid>
          </IkcoGrid>
        </DialogContent>
        <IkcoDialogActions>
          <IkcoButton variant="contained" loading={saving} onClick={handleSubmit(onSubmit)}>ذخیره</IkcoButton>
          <IkcoButton onClick={() => setEditing(null)}>انصراف</IkcoButton>
        </IkcoDialogActions>
      </IkcoDialog>

      <ConfirmDialog open={removing !== null} close={() => setRemoving(null)} title="حذف دلیل" message="این دلیل حذف شود؟" confirmText="حذف" loading={saving} onConfirm={confirmDelete} />
    </IkcoStack>
  );
}
