import {
  IkcoAlert,
  IkcoButton,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoDatePicker,
  IkcoGrid,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { AddCircle, Cancel, Check, History } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import FormDialog from "../../../components/FormDialog";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import TabularDialog from "../../../components/TabularDialog";
import { changeMechLicenses, getMechLicensesSummary, getMechLicensesTrackingInfo, getMechLicensesTrackingList } from "../../../server/services/mechLicensesService";
import type { ITabularResult } from "../../../types/tabular";
import type { ILicenseSummaryDto } from "../../../types/Mechanical";
import { mechLicensesChangeInputRules } from "../../../types/Mechanical";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../../utils/tabularUtility";
import { gridColumns } from "../../../utils/gridColumns";

interface FormValues {
  productionPossibility: number | null;
  unlimited: boolean;
  possibleProductionNo: string;
  toDate: string | null;
  toTime: string;
  letterNo: string;
  note: string;
  productionFallId: string;
  editLast: boolean;
}

const empty: FormValues = { productionPossibility: 0, unlimited: true, possibleProductionNo: "", toDate: null, toTime: "", letterNo: "", note: "", productionFallId: "", editLast: false };

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** 0 = بحران مکانیکی، 1 = بحران سنکرون */
  criticalPartTypeId: number;
  criticalPartId: number;
  onSaved?: () => void;
}

/**
 * ایجاد شرایط جدید تولید / اصلاح مجوز اقلام بحرانی (frmCriticalPartsProdPosChange) به صورت modal.
 * بالای فرم مجوز مصرف‌شده و باقی‌مانده‌ها و پایین آن اطلاعات ردیابی مجوز نمایش داده می‌شود؛ «Log» تاریخچه مجوزها را در modal باز می‌کند.
 */
export default function MechLicenseChangeDialog({ open, close, criticalPartTypeId, criticalPartId, onSaved }: Props) {
  const { control, register, handleSubmit, watch, reset, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });
  const [summary, setSummary] = useState<ILicenseSummaryDto | null>(null);
  const [history, setHistory] = useState<ITabularResult | null>(null);
  const [saving, setSaving] = useState(false);
  const [showLog, setShowLog] = useState(false);
  // _isChange فرم ویندوزی: فقط پس از «جدید» فیلدها فعال می‌شوند
  const [editing, setEditing] = useState(false);

  const possibility = watch("productionPossibility");
  const unlimited = watch("unlimited");

  const refresh = useCallback(async () => {
    if (!criticalPartId) return;
    setSummary(await getMechLicensesSummary({ criticalPartTypeId, criticalPartId }));
    setHistory(await getMechLicensesTrackingInfo({ criticalPartTypeId, criticalPartId }));
  }, [criticalPartId, criticalPartTypeId]);

  useEffect(() => {
    if (!open) return;
    reset(empty);
    setEditing(false);
    void refresh();
  }, [open, refresh, reset]);

  const columns = useMemo(() => resultColumns(history, columnHeaders, gridColumns.licenseTracking), [history]);
  const rows = useMemo(() => tabularRows(history), [history]);

  const startNew = () => {
    reset(empty);
    setEditing(true);
  };

  const cancelNew = () => {
    reset(empty);
    setEditing(false);
  };

  const onSubmit = async (v: FormValues) => {
    const limited = v.productionPossibility === 1 && !v.unlimited;
    if (limited && v.possibleProductionNo === "") {
      toast.error("تعداد مجاز تولید با کسری را وارد کنید");
      return;
    }
    setSaving(true);
    try {
      await changeMechLicenses({
        criticalPartTypeId,
        criticalPartId,
        productionPossibility: v.productionPossibility ?? 0,
        possibleProductionNo: limited && v.possibleProductionNo !== "" ? Number(v.possibleProductionNo) : null,
        toDate: limited ? v.toDate : null,
        toTime: limited && v.toTime ? v.toTime : null,
        letterNo: v.letterNo || null,
        note: v.note || null,
        productionFallId: v.productionFallId !== "" ? Number(v.productionFallId) : null,
        editLast: v.editLast,
      });
      toast.success("شرایط تولید با موفقیت ثبت شد");
      onSaved?.();
      setEditing(false);
      await refresh();
    } finally {
      setSaving(false);
    }
  };

  // قواعد DrpCriticalType_SelectedValueChanged / ChkUnlimit_CheckStateChanged: عدم امکان تولید (۰) و قطعه جایگزین (۲) همه را غیرفعال می‌کنند؛
  // نوع ۳ (مجوز برنامه‌ریزی) فقط تعداد؛ سایر موارد (مجوز) تعداد، تاریخ و ساعت — مگر «بدون محدودیت» انتخاب شده باشد.
  const limitable = editing && possibility !== 0 && possibility !== 2;
  const countEnabled = limitable && !unlimited;
  const dateEnabled = countEnabled && possibility !== 3;

  const size = { xl: 4, lg: 4, md: 6, sm: 6, xs: 12 };

  return (
    <>
      <FormDialog
        open={open}
        close={close}
        title={`ایجاد شرایط جدید تولید / اصلاح مجوز اقلام بحرانی — ${criticalPartTypeId === 1 ? "سنکرون" : "مکانیکی"} شماره ${criticalPartId}`}
        maxWidth="lg"
        actions={
          <>
            <PermissionGuard can={ApiPermissions.mechLicenses.change} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
              <IkcoButton startIcon={<AddCircle />} disabled={editing} onClick={startNew}>
                جدید
              </IkcoButton>
              <IkcoButton variant="contained" startIcon={<Check />} loading={saving} disabled={!editing} onClick={handleSubmit(onSubmit)}>
                تایید
              </IkcoButton>
              <IkcoButton startIcon={<Cancel />} disabled={!editing || saving} onClick={cancelNew}>
                انصراف
              </IkcoButton>
            </PermissionGuard>
            <IkcoButton startIcon={<History />} onClick={() => setShowLog(true)}>
              Log
            </IkcoButton>
          </>
        }
      >
        <IkcoStack spacing={2}>
          {summary && (
            <IkcoAlert status="info" severity="info">
              {`مجوز مصرف‌شده: ${summary.usedLicense ?? "-"} | باقی‌مانده معتبر مجوزهای قبلی: ${summary.validRemainingOfAllShouldPreviousShortageLicenses ?? "-"} | باقی‌مانده آخرین مجوز محدود: ${summary.validRemainingOfLastLimitedShortageLicense ?? "-"}`}
            </IkcoAlert>
          )}
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <Controller name="productionPossibility" control={control} render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="پارامترهای شرایط تولید" value={field.value} disabled={!editing} onChange={(v) => field.onChange(v ?? 0)} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="unlimited" control={control} render={({ field }) => <IkcoCheckbox label="بدون محدودیت" disabled={!limitable} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="تعداد" type="number" disabled={!countEnabled} {...register("possibleProductionNo", { min: { value: 0, message: "نامعتبر" }, max: { value: 32767, message: "حداکثر ۳۲۷۶۷" } })} error={!!errors.possibleProductionNo} helperText={errors.possibleProductionNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="toDate" control={control} rules={mechLicensesChangeInputRules.toDate} render={({ field }) => <IkcoDatePicker label="تا تاریخ" value={field.value} disabled={!dateEnabled} onChange={field.onChange} helperText={errors.toDate?.message} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="تا ساعت (HH:mm:ss)" placeholder="12:00:00" disabled={!dateEnabled} {...register("toTime", mechLicensesChangeInputRules.toTime)} error={!!errors.toTime} helperText={errors.toTime?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره نامه" disabled={!editing} {...register("letterNo", mechLicensesChangeInputRules.letterNo)} error={!!errors.letterNo} helperText={errors.letterNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات" disabled={!editing} {...register("note", mechLicensesChangeInputRules.note)} error={!!errors.note} helperText={errors.note?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="علت افت تولید (شناسه)" type="number" disabled={!editing} {...register("productionFallId", { min: 0, max: 255 })} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="editLast" control={control} render={({ field }) => <IkcoCheckbox label="فقط اصلاح آخرین مجوز تولید با مجوز" disabled={!editing} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
          </IkcoGrid>
          <IkcoTypography variant="subtitle1">ردیابی پارامترها</IkcoTypography>
          <IkcoDataGrid columns={columns} rows={rows} getRowId={(r) => r.__rowId} autoHeight />
        </IkcoStack>
      </FormDialog>
      <TabularDialog open={showLog} close={setShowLog} title="Log" columns={gridColumns.mechLicenseTrackingList} load={() => getMechLicensesTrackingList({ criticalPartTypeId, criticalPartId })} />
    </>
  );
}
