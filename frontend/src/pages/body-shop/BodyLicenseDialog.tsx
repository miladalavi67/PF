import { IkcoButton, IkcoCardContent, IkcoDatePicker, IkcoDialog, IkcoDialogActions, IkcoDialogTitle, IkcoGrid, IkcoTextField, IkcoTypography } from "@ikco/frontend-kit";
import { useEffect, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import { registerBodyCriticalsLicense } from "../../server/services/bodyCriticalsService";
import { getBodyReportsProductionPossibilityStatus } from "../../server/services/bodyReportsService";
import { bodyCriticalsRegisterLicenseInputRules } from "../../types/BodyShop";

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
  possibleProductionNo: string;
  toDate: string | null;
  toTime: string;
  note: string;
}

const empty: FormValues = { productionPossibilityId: 0, possibleProductionNo: "", toDate: null, toTime: "", note: "" };

/** تغییر شرایط تولید / ثبت مجوز تولید با کسری بحران بدنه‌سازی (frmCriticalPartsProdPosChange، رویه PF.RegisterShortageLicense_V1) */
export default function BodyLicenseDialog({ open, close, criticalPartId, criticalPartTypeId, onSaved }: BodyLicenseDialogProps) {
  const { control, register, handleSubmit, reset, watch, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });
  const [partTitle, setPartTitle] = useState("");
  const [saving, setSaving] = useState(false);
  const possibility = watch("productionPossibilityId");

  useEffect(() => {
    if (!open) return;
    void (async () => {
      const status = await getBodyReportsProductionPossibilityStatus(criticalPartId);
      const row = status.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        reset(empty);
        return;
      }
      setPartTitle(`${row["PartNo"] ?? ""} - ${row["PartDescF"] ?? ""}`);
      reset({
        productionPossibilityId: row["ProductionPossibilityId"] === null || row["ProductionPossibilityId"] === undefined ? 0 : Number(row["ProductionPossibilityId"]),
        possibleProductionNo: row["PossibleProductionNo"] === null || row["PossibleProductionNo"] === undefined ? "" : String(row["PossibleProductionNo"]),
        toDate: (row["ProductionPossibilityToDate"] as string | null) ?? null,
        toTime: (row["ProductionPossibilityToTime"] as string | null) ?? "",
        note: (row["Note"] as string | null) ?? "",
      });
    })();
  }, [open, criticalPartId, reset]);

  const onSubmit = async (v: FormValues) => {
    setSaving(true);
    try {
      const withShortage = v.productionPossibilityId === 1;
      const result = await registerBodyCriticalsLicense({
        criticalPartId,
        criticalPartTypeId,
        productionPossibilityId: v.productionPossibilityId ?? 0,
        possibleProductionNo: withShortage && v.possibleProductionNo !== "" ? Number(v.possibleProductionNo) : null,
        productionPossibilityToDate: withShortage ? v.toDate : null,
        productionPossibilityToTime: withShortage && v.toTime ? v.toTime : null,
        note: v.note || null,
      });
      toast.success(result.message ?? "مجوز با موفقیت ثبت شد");
      onSaved?.();
      close(false);
    } finally {
      setSaving(false);
    }
  };

  return (
    <IkcoDialog open={open} maxWidth="md" fullWidth onClose={() => close(false)}>
      <IkcoDialogTitle>تغییر شرایط تولید</IkcoDialogTitle>
      <IkcoCardContent>
        <IkcoGrid container spacing={2}>
          <IkcoGrid size={12}>
            <IkcoTypography>{partTitle}</IkcoTypography>
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 6 }}>
            <Controller
              name="productionPossibilityId"
              control={control}
              render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="شرایط تولید" value={field.value} onChange={(id) => field.onChange(id ?? 0)} />}
            />
          </IkcoGrid>
          {possibility === 1 && (
            <>
              <IkcoGrid size={{ xs: 12, md: 6 }}>
                <IkcoTextField label="تعداد مجاز تولید با کسری (خالی = نامحدود)" type="number" {...register("possibleProductionNo", { min: { value: 0, message: "نامعتبر" } })} error={!!errors.possibleProductionNo} helperText={errors.possibleProductionNo?.message} />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 6 }}>
                <Controller
                  name="toDate"
                  control={control}
                  rules={bodyCriticalsRegisterLicenseInputRules.productionPossibilityToDate}
                  render={({ field }) => <IkcoDatePicker label="مجاز تا تاریخ (خالی = نامحدود)" value={field.value} onChange={field.onChange} helperText={errors.toDate?.message} />}
                />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 6 }}>
                <IkcoTextField label="مجاز تا ساعت (HH:mm:ss)" placeholder="12:00:00" {...register("toTime", bodyCriticalsRegisterLicenseInputRules.productionPossibilityToTime)} error={!!errors.toTime} helperText={errors.toTime?.message} />
              </IkcoGrid>
            </>
          )}
          <IkcoGrid size={12}>
            <IkcoTextField label="توضیحات" {...register("note", bodyCriticalsRegisterLicenseInputRules.note)} error={!!errors.note} helperText={errors.note?.message} />
          </IkcoGrid>
        </IkcoGrid>
      </IkcoCardContent>
      <IkcoDialogActions>
        <IkcoButton onClick={() => close(false)}>انصراف</IkcoButton>
        <IkcoButton variant="contained" loading={saving} onClick={handleSubmit(onSubmit)}>
          ثبت
        </IkcoButton>
      </IkcoDialogActions>
    </IkcoDialog>
  );
}
