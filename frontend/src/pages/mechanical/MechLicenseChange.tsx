import {
  IkcoAlert,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoDatePicker,
  IkcoGrid,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard, useMFENavigate } from "@ikco/mfe-core";
import { useCallback, useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import { useQuery } from "../../hooks/useQueryString";
import { changeMechLicenses, getMechLicensesSummary, getMechLicensesTrackingInfo } from "../../server/services/mechLicensesService";
import type { ITabularResult } from "../../types/Common";
import type { ILicenseSummaryDto } from "../../types/Mechanical";
import { mechLicensesChangeInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../utils/tabularUtility";

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

/**
 * تغییر شرایط تولید / اصلاح مجوز تولید با کسری (frmCriticalPartsProdPosChange). مسیر داخلی با پارامترهای ?type=0|1&id=.
 * نوع 0 بحران مکانیکی و 1 بحران سنکرون است؛ بالای صفحه مجوز مصرف‌شده و باقی‌مانده‌ها و تاریخچه ردیابی مجوز نمایش داده می‌شود.
 */
export default function MechLicenseChange() {
  const query = useQuery();
  const typeId = Number(query.get("type") ?? 0);
  const id = Number(query.get("id") ?? 0);
  const navigate = useMFENavigate();
  const { control, register, handleSubmit, watch, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });
  const [summary, setSummary] = useState<ILicenseSummaryDto | null>(null);
  const [history, setHistory] = useState<ITabularResult | null>(null);
  const [saving, setSaving] = useState(false);

  const possibility = watch("productionPossibility");
  const unlimited = watch("unlimited");

  const refresh = useCallback(async () => {
    if (!id) return;
    setSummary(await getMechLicensesSummary({ criticalPartTypeId: typeId, criticalPartId: id }));
    setHistory(await getMechLicensesTrackingInfo({ criticalPartTypeId: typeId, criticalPartId: id }));
  }, [id, typeId]);

  useEffect(() => {
    void refresh();
  }, [refresh]);

  const columns = useMemo(() => resultColumns(history, columnHeaders), [history]);
  const rows = useMemo(() => tabularRows(history), [history]);

  const onSubmit = async (v: FormValues) => {
    setSaving(true);
    try {
      const limited = v.productionPossibility === 1 && !v.unlimited;
      await changeMechLicenses({
        criticalPartTypeId: typeId,
        criticalPartId: id,
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
      await refresh();
    } finally {
      setSaving(false);
    }
  };

  if (!id) return <IkcoAlert status="warning" severity="warning">شناسه بحران مشخص نشده است؛ این صفحه از «جستجوی اقلام بحرانی» باز می‌شود.</IkcoAlert>;

  const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">{`تغییر شرایط تولید — ${typeId === 1 ? "بحران سنکرون" : "بحران مکانیکی"} شماره ${id}`}</IkcoTypography>

      {summary && (
        <IkcoAlert status="info" severity="info">
          {`مجوز مصرف‌شده: ${summary.usedLicense ?? "-"} | باقی‌مانده معتبر مجوزهای قبلی: ${summary.validRemainingOfAllShouldPreviousShortageLicenses ?? "-"} | باقی‌مانده آخرین مجوز محدود: ${summary.validRemainingOfLastLimitedShortageLicense ?? "-"}`}
        </IkcoAlert>
      )}

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <Controller name="productionPossibility" control={control} render={({ field }) => <LookupAutocomplete lookupKey="production-possibilities" label="شرایط تولید" value={field.value} onChange={(v) => field.onChange(v ?? 0)} />} />
            </IkcoGrid>
            {possibility === 1 && (
              <>
                <IkcoGrid size={size}>
                  <Controller name="unlimited" control={control} render={({ field }) => <IkcoCheckbox label="نامحدود (بدون تعداد و تاریخ)" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
                </IkcoGrid>
                {!unlimited && (
                  <>
                    <IkcoGrid size={size}>
                      <IkcoTextField label="تعداد مجاز تولید با کسری" type="number" {...register("possibleProductionNo", { min: { value: 0, message: "نامعتبر" }, max: { value: 32767, message: "حداکثر ۳۲۷۶۷" } })} error={!!errors.possibleProductionNo} helperText={errors.possibleProductionNo?.message} />
                    </IkcoGrid>
                    <IkcoGrid size={size}>
                      <Controller name="toDate" control={control} rules={mechLicensesChangeInputRules.toDate} render={({ field }) => <IkcoDatePicker label="مجاز تا تاریخ" value={field.value} onChange={field.onChange} helperText={errors.toDate?.message} />} />
                    </IkcoGrid>
                    <IkcoGrid size={size}>
                      <IkcoTextField label="مجاز تا ساعت (HH:mm:ss)" placeholder="12:00:00" {...register("toTime", mechLicensesChangeInputRules.toTime)} error={!!errors.toTime} helperText={errors.toTime?.message} />
                    </IkcoGrid>
                  </>
                )}
              </>
            )}
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره نامه مجوز" {...register("letterNo", mechLicensesChangeInputRules.letterNo)} error={!!errors.letterNo} helperText={errors.letterNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات" {...register("note", mechLicensesChangeInputRules.note)} error={!!errors.note} helperText={errors.note?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="علت افت تولید (شناسه)" type="number" {...register("productionFallId", { min: 0, max: 255 })} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="editLast" control={control} render={({ field }) => <IkcoCheckbox label="ویرایش آخرین مجوز فعال (به‌جای ثبت مجوز جدید)" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.mechLicenses.change} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" loading={saving} onClick={handleSubmit(onSubmit)}>
              ثبت
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton onClick={() => navigate("mechanical/search")}>بازگشت به جستجو</IkcoButton>
        </IkcoCardActions>
      </IkcoCard>

      <IkcoTypography variant="subtitle1">اطلاعات ردیابی مجوزهای این بحران</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} getRowId={(r) => r.__rowId} autoHeight />
    </IkcoStack>
  );
}
