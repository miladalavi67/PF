import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCardContent as DialogContent,
  IkcoDataGrid,
  IkcoDatePicker,
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
import { History, ListAlt, PostAdd, Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import TabularDialog from "../../components/TabularDialog";
import {
  createMechExceptionsLicense,
  disableMechExceptionsExceptions,
  getMechExceptionsExceptionList,
  getMechExceptionsLicenseBodies,
  getMechExceptionsLicenseLog,
  getMechExceptionsLicenses,
} from "../../server/services/mechExceptionsService";
import type { ITabularResult } from "../../types/tabular";
import { mechExceptionsCreateLicenseInputRules, mechExceptionsGetExceptionListInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";

interface Filters {
  bodyNo: string;
  dateFrom: string | null;
  dateTo: string | null;
  state: number | null;
}

interface LicenseForm {
  possibleLicenseCount: string;
  letterNo: string;
  description: string;
}

const defaults: Filters = { bodyNo: "", dateFrom: null, dateTo: null, state: 1 };
const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

type Panel = { type: "licenses" | "bodies" | "log" | "new-license" | "disable"; id: number; partNo: string };

const num = (row: IGridRow | undefined, ...keys: string[]) => {
  const r = (row ?? {}) as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return Number(r[k]);
  return 0;
};

/**
 * استثناهای محصولات بحران یا «عبور مستقیم» بدنه‌ها (frmCriticalPartsProductExceptions). فهرست بدنه‌های استثنا،
 * ثبت مجوز برای استثنا، مشاهده مجوزها، بدنه‌های عبوری و تاریخچه مجوز شماره فنی، و غیرفعال‌سازی گروهی.
 */
export default function MechExceptions() {
  const canCreate = useCan(ApiPermissions.mechExceptions.create);
  const canDisable = useCan(ApiPermissions.mechExceptions.disable);
  const { control, register, handleSubmit, formState: { errors } } = useForm<Filters>({ defaultValues: defaults });
  const licenseForm = useForm<LicenseForm>({ defaultValues: { possibleLicenseCount: "", letterNo: "", description: "" } });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [selection, setSelection] = useState<number[]>([]);
  const [panel, setPanel] = useState<Panel | null>(null);
  const [busy, setBusy] = useState(false);
  const [last, setLast] = useState<Filters | null>(null);

  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const columns: IkcoGridColDef[] = useMemo(
    () => [
      {
        field: "__actions",
        type: "actions",
        headerName: "عملیات",
        width: 190,
        getActions: (params) => {
          const row = params.row as IGridRow;
          const id = num(row, "Id", "id");
          const partNo = String((row as Record<string, unknown>)["PartNo"] ?? "");
          return [
            <IkcoGridActionsCellItem key="l" icon={<IkcoTooltip title="مجوزها"><IkcoIconButton><ListAlt color="primary" /></IkcoIconButton></IkcoTooltip>} label="مجوزها" onClick={() => setPanel({ type: "licenses", id, partNo })} />,
            <IkcoGridActionsCellItem key="b" icon={<IkcoTooltip title="بدنه‌های عبوری"><IkcoIconButton><ListAlt color="secondary" /></IkcoIconButton></IkcoTooltip>} label="بدنه‌ها" onClick={() => setPanel({ type: "bodies", id, partNo })} />,
            <IkcoGridActionsCellItem key="h" icon={<IkcoTooltip title="تاریخچه مجوز شماره فنی"><IkcoIconButton><History color="primary" /></IkcoIconButton></IkcoTooltip>} label="تاریخچه" onClick={() => setPanel({ type: "log", id, partNo })} />,
            ...(canCreate ? [<IkcoGridActionsCellItem key="n" icon={<IkcoTooltip title="ثبت مجوز"><IkcoIconButton><PostAdd color="primary" /></IkcoIconButton></IkcoTooltip>} label="ثبت مجوز" onClick={() => { licenseForm.reset(); setPanel({ type: "new-license", id, partNo }); }} />] : []),
          ];
        },
      },
      ...baseColumns,
    ],
    [baseColumns, canCreate, licenseForm],
  );

  const run = async (f: Filters) => {
    setLoading(true);
    try {
      const data = await getMechExceptionsExceptionList({
        bodyNo: f.bodyNo.trim() || null,
        dateFrom: f.dateFrom,
        dateEnd: f.dateTo,
        state: f.state ?? 1,
      });
      setLast(f);
      setSelection([]);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const saveLicense = async (v: LicenseForm) => {
    if (!panel) return;
    setBusy(true);
    try {
      await createMechExceptionsLicense({
        criticalPartsProductExceptionsId: panel.id,
        possibleLicenseCount: Number(v.possibleLicenseCount),
        letterNo: v.letterNo.trim(),
        description: v.description.trim() || null,
      });
      toast.success("مجوز استثنا ثبت شد");
      setPanel(null);
      if (last) await run(last);
    } finally {
      setBusy(false);
    }
  };

  const disableSelected = async () => {
    setBusy(true);
    try {
      const ids = selection.map((rowId) => num(rows.find((r) => r.__rowId === rowId), "Id", "id"));
      await disableMechExceptionsExceptions({ ids });
      toast.success("استثناهای انتخاب‌شده غیرفعال شد");
      setPanel(null);
      if (last) await run(last);
    } finally {
      setBusy(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">استثناهای محصولات بحران (عبور مستقیم)</IkcoTypography>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره بدنه" {...register("bodyNo", mechExceptionsGetExceptionListInputRules.bodyNo)} error={!!errors.bodyNo} helperText={errors.bodyNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="dateFrom" control={control} render={({ field }) => <IkcoDatePicker label="از تاریخ" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="dateTo" control={control} render={({ field }) => <IkcoDatePicker label="تا تاریخ" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="state" control={control} render={({ field }) => <LookupAutocomplete lookupKey="active-states" label="وضعیت" value={field.value} onChange={field.onChange} />} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.mechExceptions.view} fallback={<IkcoTypography color="error">شما به این صفحه دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={handleSubmit(run)}>
              جستجو
            </IkcoButton>
          </PermissionGuard>
          {canDisable && (
            <IkcoButton color="error" disabled={selection.length === 0} onClick={() => setPanel({ type: "disable", id: 0, partNo: "" })}>
              {`غیرفعال‌سازی انتخاب‌شده‌ها (${selection.length})`}
            </IkcoButton>
          )}
          <ExcelExportButton fileName="استثناهای محصولات بحران" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid
        columns={columns}
        rows={rows}
        loading={loading}
        getRowId={(r) => r.__rowId}
        checkboxSelection={canDisable}
        rowSelectionModel={selection}
        onRowSelectionModelChange={(m) => setSelection(m as number[])}
      />

      <TabularDialog open={panel?.type === "licenses"} close={() => setPanel(null)} title="مجوزهای استثنا" load={() => getMechExceptionsLicenses(panel?.id ?? 0)} />
      <TabularDialog open={panel?.type === "bodies"} close={() => setPanel(null)} title="بدنه‌های عبوری از مجوز" load={() => getMechExceptionsLicenseBodies(panel?.id ?? 0)} />
      <TabularDialog open={panel?.type === "log"} close={() => setPanel(null)} title={`تاریخچه مجوزهای شماره فنی ${panel?.partNo ?? ""}`} load={() => getMechExceptionsLicenseLog({ partNo: panel?.partNo ?? "" })} />

      <IkcoDialog open={panel?.type === "new-license"} maxWidth="sm" fullWidth onClose={() => setPanel(null)}>
        <IkcoDialogTitle>{`ثبت مجوز برای استثنا — ${panel?.partNo ?? ""}`}</IkcoDialogTitle>
        <DialogContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xs: 12, sm: 6 }}>
              <IkcoTextField label="تعداد مجاز" type="number" {...licenseForm.register("possibleLicenseCount", { required: "«تعداد مجاز» الزامی است", min: { value: 1, message: "حداقل ۱" } })} error={!!licenseForm.formState.errors.possibleLicenseCount} helperText={licenseForm.formState.errors.possibleLicenseCount?.message} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, sm: 6 }}>
              <IkcoTextField label="شماره نامه" {...licenseForm.register("letterNo", mechExceptionsCreateLicenseInputRules.letterNo)} error={!!licenseForm.formState.errors.letterNo} helperText={licenseForm.formState.errors.letterNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={12}>
              <IkcoTextField label="توضیحات" {...licenseForm.register("description", mechExceptionsCreateLicenseInputRules.description)} error={!!licenseForm.formState.errors.description} helperText={licenseForm.formState.errors.description?.message} />
            </IkcoGrid>
          </IkcoGrid>
        </DialogContent>
        <IkcoDialogActions>
          <IkcoButton variant="contained" loading={busy} onClick={licenseForm.handleSubmit(saveLicense)}>ثبت</IkcoButton>
          <IkcoButton onClick={() => setPanel(null)}>انصراف</IkcoButton>
        </IkcoDialogActions>
      </IkcoDialog>

      <ConfirmDialog
        open={panel?.type === "disable"}
        close={() => setPanel(null)}
        title="غیرفعال‌سازی استثنا"
        message={`${selection.length} استثنای انتخاب‌شده غیرفعال شود؟`}
        confirmText="غیرفعال‌سازی"
        loading={busy}
        onConfirm={disableSelected}
      />
    </IkcoStack>
  );
}
