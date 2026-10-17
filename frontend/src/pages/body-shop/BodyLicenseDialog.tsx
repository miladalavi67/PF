import { IkcoButton, IkcoCheckbox, IkcoDataGrid, IkcoDatePicker, IkcoGrid, IkcoStack, IkcoTextField, IkcoTypography } from "@ikco/frontend-kit";
import { AddCircle, Cancel, Check, History } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import FormDialog from "../../components/FormDialog";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import TabularDialog from "../../components/TabularDialog";
import { getBodyCriticalsEventLog, registerBodyCriticalsLicense } from "../../server/services/bodyCriticalsService";
import { getBodyReportsProductionPossibilityStatus, getBodyReportsShortageLicenseTracking } from "../../server/services/bodyReportsService";
import { bodyCriticalsRegisterLicenseInputRules } from "../../types/BodyShop";
import type { ITabularResult } from "../../types/Common";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../utils/tabularUtility";

interface BodyLicenseDialogProps {
  open: boolean;
  close: (open: boolean) => void;
  /** شناسه بحران */
  criticalPartId: number;
  /** نوع بحران (PF.B_CriticalPartType) */
  criticalPartTypeId: number;
  onSaved?: () => void;
}

interface FormValues {
  productionPossibilityId: number | null;
  unlimited: boolean;
  possibleProductionNo: string;
  toDate: string | null;
  toTime: string;
  note: string;
}

const empty: FormValues = { productionPossibilityId: 0, unlimited: true, possibleProductionNo: "", toDate: null, toTime: "", note: "" };

const size = { xl: 4, lg: 4, md: 6, sm: 6, xs: 12 };

/**
 * تغییر شرایط تولید / ثبت مجوز تولید با کسری بحران بدنه‌سازی (frmCriticalPartsProdPosChange، رویه PF.RegisterShortageLicense_V1).
 * مطابق فرم ویندوزی، فیلدها تا انتخاب «جدید» غیرفعالند؛ «عدم امکان تولید» و «قطعه جایگزین» تعداد/تاریخ/ساعت را غیرفعال می‌کنند و
 * «بدون محدودیت» نیز آنها را غیرفعال می‌کند. تایید/انصراف فقط در حالت ثبت جدید فعالند.
 */
export default function BodyLicenseDialog({ open, close, criticalPartId, criticalPartTypeId, onSaved }: BodyLicenseDialogProps) {
  const { control, register, handleSubmit, reset, watch, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });
  const [partTitle, setPartTitle] = useState("");
  const [history, setHistory] = useState<ITabularResult | null>(null);
  const [saving, setSaving] = useState(false);
  const [editing, setEditing] = useState(false);
  const [showLog, setShowLog] = useState(false);
  const possibility = watch("productionPossibilityId");
  const unlimited = watch("unlimited");

  const columns = useMemo(() => resultColumns(history, columnHeaders), [history]);
  const rows = useMemo(() => tabularRows(history), [history]);

  const refresh = useCallback(async () => {
    const status = await getBodyReportsProductionPossibilityStatus(criticalPartId);
    const row = status.rows[0] as Record<string, unknown> | undefined;
    if (row) {
      setPartTitle(`${row["PartNo"] ?? ""} - ${row["PartDescF"] ?? ""}`);
      const to = (row["ProductionPossibilityToDate"] as string | null) ?? null;
      reset({
        productionPossibilityId: row["ProductionPossibilityId"] === null || row["ProductionPossibilityId"] === undefined ? 0 : Number(row["ProductionPossibilityId"]),
        unlimited: !to,
        possibleProductionNo: row["PossibleProductionNo"] === null || row["PossibleProductionNo"] === undefined ? "" : String(row["PossibleProductionNo"]),
        toDate: to,
        toTime: (row["ProductionPossibilityToTime"] as string | null) ?? "",
        note: (row["Note"] as string | null) ?? "",
      });
    } else {
      reset(empty);
    }
    setHistory(await getBodyReportsShortageLicenseTracking({ criticalPartTypeId, criticalPartId }));
  }, [criticalPartId, criticalPartTypeId, reset]);

  useEffect(() => {
    if (!open) return;
    setEditing(false);
    void refresh();
  }, [open, refresh]);

  const startNew = () => {
    reset({ ...empty, note: "" });
    setEditing(true);
  };

  const onSubmit = async (v: FormValues) => {
    const withShortage = v.productionPossibilityId === 1;
    const limited = withShortage && !v.unlimited;
    if (limited && v.possibleProductionNo === "") {
      toast.error("تعداد مجاز تولید با کسری را وارد کنید");
      return;
    }
    setSaving(true);
    try {
      const result = await registerBodyCriticalsLicense({
        criticalPartId,
        criticalPartTypeId,
        productionPossibilityId: v.productionPossibilityId ?? 0,
        possibleProductionNo: limited ? Number(v.possibleProductionNo) : null,
        productionPossibilityToDate: limited ? v.toDate : null,
        productionPossibilityToTime: limited && v.toTime ? v.toTime : null,
        note: v.note || null,
      });
      toast.success(result.message ?? "مجوز با موفقیت ثبت شد");
      onSaved?.();
      setEditing(false);
      await refresh();
    } finally {
      setSaving(false);
    }
  };

  // قواعد DrpCriticalType_SelectedValueChanged / ChkUnlimit_CheckStateChanged
  const limitable = editing && possibility !== 0 && possibility !== 2;
  const countEnabled = limitable && !unlimited;
  const dateEnabled = countEnabled && possibility !== 3;

  return (
    <>
      <FormDialog
        open={open}
        close={close}
        title="تغییر شرایط تولید"
        maxWidth="lg"
        actions={
          <>
            <IkcoButton startIcon={<AddCircle />} disabled={editing} onClick={startNew}>
              جدید
            </IkcoButton>
            <IkcoButton variant="contained" startIcon={<Check />} loading={saving} disabled={!editing} onClick={handleSubmit(onSubmit)}>
              تایید
            </IkcoButton>
            <IkcoButton startIcon={<Cancel />} disabled={!editing || saving} onClick={() => { setEditing(false); void refresh(); }}>
              انصراف
            </IkcoButton>
            <IkcoButton startIcon={<History />} onClick={() => setShowLog(true)}>
              Log
            </IkcoButton>
          </>
        }
      >
        <IkcoStack spacing={2}>
          <IkcoTypography variant="subtitle1">{partTitle}</IkcoTypography>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <Controller
                name="productionPossibilityId"
                control={control}
                render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="شرایط تولید" value={field.value} disabled={!editing} onChange={(id) => field.onChange(id ?? 0)} />}
              />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="unlimited" control={control} render={({ field }) => <IkcoCheckbox label="بدون محدودیت" disabled={!limitable} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="تعداد مجاز تولید با کسری" type="number" disabled={!countEnabled} {...register("possibleProductionNo", { min: { value: 0, message: "نامعتبر" } })} error={!!errors.possibleProductionNo} helperText={errors.possibleProductionNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller
                name="toDate"
                control={control}
                rules={bodyCriticalsRegisterLicenseInputRules.productionPossibilityToDate}
                render={({ field }) => <IkcoDatePicker label="مجاز تا تاریخ" value={field.value} disabled={!dateEnabled} onChange={field.onChange} helperText={errors.toDate?.message} />}
              />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="مجاز تا ساعت (HH:mm:ss)" placeholder="12:00:00" disabled={!dateEnabled} {...register("toTime", bodyCriticalsRegisterLicenseInputRules.productionPossibilityToTime)} error={!!errors.toTime} helperText={errors.toTime?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات" disabled={!editing} {...register("note", bodyCriticalsRegisterLicenseInputRules.note)} error={!!errors.note} helperText={errors.note?.message} />
            </IkcoGrid>
          </IkcoGrid>
          <IkcoTypography variant="subtitle1">ردیابی پارامترها</IkcoTypography>
          <IkcoDataGrid columns={columns} rows={rows} getRowId={(r) => r.__rowId} autoHeight />
        </IkcoStack>
      </FormDialog>
      <TabularDialog open={showLog} close={setShowLog} title="Log" load={() => getBodyCriticalsEventLog({ eventTypeId: criticalPartTypeId, changedRecordId: criticalPartId })} />
    </>
  );
}
