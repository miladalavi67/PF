import {
  IkcoAlert,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoGrid,
  IkcoRadioGroup,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard, useMFENavigate } from "@ikco/mfe-core";
import { Add, Remove, Search } from "@mui/icons-material";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import { useQuery } from "../../hooks/useQueryString";
import {
  createMechParts,
  existsMechPartsPartNo,
  getMechPartsBodyProperties,
  getMechPartsForEdit,
  getMechPartsFromBop,
  getMechPartsPartDescription,
  getMechPartsPartProducts,
  getMechPartsProducts,
  saveMechPartsProducts,
  updateMechParts,
} from "../../server/services/mechPartsService";
import type { ITabularResult } from "../../types/Common";
import { mechPartsCreateInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";

/** شناسه سایت پیش‌فرض رویه‌های SFC (PlantId=1) */
const PLANT_ID = 1;

type ProductSource = "bop" | "bom";

interface FormValues {
  trimShopId: number | null;
  bodyNo: string;
  partNo: string;
  partDesc: string;
  partGroup: string;
  alterNativePartNo: string;
  criticalPartNote: string;
  stock: string;
  used: string;
  possibleProductionNo: string;
  usageFactor: string;
  deptId: number | null;
  isOutOfSap: boolean;
  productionStateLetterNo: string;
  productionStateNote: string;
  productionPossibility: string;
}

const empty: FormValues = {
  trimShopId: null,
  bodyNo: "",
  partNo: "",
  partDesc: "",
  partGroup: "",
  alterNativePartNo: "",
  criticalPartNote: "",
  stock: "0",
  used: "0",
  possibleProductionNo: "0",
  usageFactor: "1",
  deptId: null,
  isOutOfSap: false,
  productionStateLetterNo: "",
  productionStateNote: "",
  productionPossibility: "0",
};

type Row = IGridRow & Record<string, unknown>;
const num = (r: Record<string, unknown>, ...keys: string[]) => {
  for (const k of keys) if (r[k] !== undefined && r[k] !== null && r[k] !== "") return Number(r[k]);
  return -1;
};
const str = (r: Record<string, unknown> | undefined, k: string) => (r?.[k] === undefined || r?.[k] === null ? "" : String(r[k]).trim());

/** ردیف خروجی BOP یا BOM به ساختار ورودی ثبت محصول */
const toItem = (r: Record<string, unknown>, quantity: number) => ({
  carGroupId: num(r, "GroupId", "CarGroupId"),
  carModelId: num(r, "ModelId", "CarModelId"),
  carClassId: num(r, "ClassId", "CarClassId"),
  subClassId: num(r, "SubClassId", "CarSubClassId"),
  colorId: num(r, "ColorId"),
  trimId: num(r, "TrimId"),
  optionId: num(r, "OptionId"),
  quantity,
});

/**
 * ثبت و ویرایش بحران مکانیکی (frmCriticalPart). ۱) سالن مونتاژ و شماره بدنه (S_CarPropertiesSelectForCriticalParts: بدنه باید در
 * مرحله ورود به مونتاژ باشد)، ۲) شماره فنی و بررسی تکراری بودن، ۳) انتخاب محصولات از BOP یا BOM، ۴) ثبت بحران
 * (S_CriticalPartsINSERT) و محصولات (S_CriticalPartsPruductUpdate_H_V1 در یک تراکنش). در حالت ویرایش (?id=) مشخصات بحران
 * (S_CriticalPartsUpdate) و افزودن محصول ممکن است.
 */
export default function MechCriticalRegister() {
  const query = useQuery();
  const editId = Number(query.get("id") ?? 0);
  const isEdit = editId > 0;
  const navigate = useMFENavigate();
  const { control, register, handleSubmit, reset, getValues, setValue, watch, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });

  const [carId, setCarId] = useState(0);
  const [bodyInfo, setBodyInfo] = useState("");
  const [source, setSource] = useState<ProductSource>("bop");
  const [candidates, setCandidates] = useState<ITabularResult | null>(null);
  const [chosen, setChosen] = useState<Row[]>([]);
  const [candSel, setCandSel] = useState<number[]>([]);
  const [chosenSel, setChosenSel] = useState<number[]>([]);
  const [existing, setExisting] = useState<ITabularResult | null>(null);
  const [busy, setBusy] = useState<"body" | "part" | "save" | null>(null);

  const trimShopId = watch("trimShopId");
  const candRows = useMemo(() => tabularRows(candidates) as Row[], [candidates]);
  const candColumns = useMemo(() => resultColumns(candidates, columnHeaders), [candidates]);
  const existingRows = useMemo(() => tabularRows(existing), [existing]);
  const existingColumns = useMemo(() => resultColumns(existing, columnHeaders), [existing]);

  useEffect(() => {
    if (!isEdit) return;
    void (async () => {
      const data = await getMechPartsForEdit(editId);
      const row = data.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        toast.error("بحران یافت نشد");
        return;
      }
      reset({
        ...empty,
        bodyNo: str(row, "BodyNO"),
        partNo: str(row, "Partno"),
        partDesc: str(row, "Partdesc"),
        partGroup: str(row, "PartGroup"),
        alterNativePartNo: str(row, "AlterNativePartNo"),
        criticalPartNote: str(row, "CriticalPartNote"),
        stock: str(row, "Stock") || "0",
        used: str(row, "USED") || "0",
        possibleProductionNo: str(row, "PossibleProductionNo") || "0",
        usageFactor: str(row, "UsageFactor") || "1",
        deptId: row["DeptId"] === undefined || row["DeptId"] === null ? null : Number(row["DeptId"]),
        isOutOfSap: Boolean(row["IsOutOfSap"]),
        productionStateLetterNo: str(row, "ProductionStateLetterNo"),
        productionStateNote: str(row, "ProductionStateNote"),
        productionPossibility: str(row, "ProductionPossibility") || "0",
      });
      setExisting(await getMechPartsProducts(editId));
    })();
  }, [editId, isEdit, reset]);

  const loadBody = async () => {
    const { bodyNo } = getValues();
    if (!bodyNo.trim()) return;
    setBusy("body");
    try {
      const data = await getMechPartsBodyProperties({ bodyNo: bodyNo.trim(), plantId: PLANT_ID, mode: 1 });
      const row = data.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        setCarId(0);
        setBodyInfo("");
        toast.error("بدنه یافت نشد");
        return;
      }
      setCarId(Number(row["Id"]));
      setBodyInfo(`${str(row, "CarGroupDescf")} - ${str(row, "CarModelDesc")} - ${str(row, "CarClassDesc")} | رنگ: ${str(row, "CarColorDescf")} | مرحله: ${str(row, "LastStepDescF")} (${str(row, "LastSaloonDescF")}) ${str(row, "LastStepdate")} ${str(row, "LastSteptime")}`);
    } finally {
      setBusy(null);
    }
  };

  const loadPart = async () => {
    const { partNo, trimShopId: shop } = getValues();
    if (!partNo.trim() || !shop) {
      toast.error("سالن مونتاژ و شماره فنی را وارد کنید");
      return;
    }
    setBusy("part");
    try {
      const desc = await getMechPartsPartDescription({ partNo: partNo.trim() });
      if (!desc || typeof desc !== "string") {
        toast.error("شماره فنی وارد شده نادرست است");
        return;
      }
      setValue("partDesc", desc.trim().slice(0, 100));
      if (!isEdit) {
        const exists = await existsMechPartsPartNo({ partNo: partNo.trim(), trimShopId: shop });
        if (exists.rows.length > 0) toast("برای این شماره فنی در این سالن قبلاً بحران ثبت شده است");
      }
      const data = source === "bop" ? await getMechPartsFromBop({ partNo: partNo.trim(), trimShopId: shop }) : await getMechPartsPartProducts({ partNo: partNo.trim() });
      if (data.rows.length === 0) toast("موردی یافت نشد");
      setCandidates(data);
      setChosen([]);
    } finally {
      setBusy(null);
    }
  };

  const addChosen = () => {
    const picked = candRows.filter((r) => candSel.includes(r.__rowId) && !chosen.some((c) => c.__rowId === r.__rowId));
    if (picked.length === 0) return;
    setChosen((c) => [...c, ...picked]);
    setCandSel([]);
  };

  const onSubmit = async (v: FormValues) => {
    setBusy("save");
    try {
      if (isEdit) {
        await updateMechParts(editId, {
          stock: Number(v.stock),
          used: Number(v.used),
          usageFactor: v.usageFactor,
          alterNativePartNo: v.alterNativePartNo || null,
          criticalPartNote: v.criticalPartNote || null,
          productionStateLetterNo: v.productionStateLetterNo || null,
          productionStateNote: v.productionStateNote || null,
          partGroup: v.partGroup,
          productionPossibility: Number(v.productionPossibility),
          isOutOfSap: v.isOutOfSap,
        });
        if (chosen.length > 0 && v.trimShopId) {
          await saveMechPartsProducts(editId, { carId: carId || 1, trimShopId: v.trimShopId, plantId: PLANT_ID, isNew: 1, items: chosen.map((r) => toItem(r, Number(v.usageFactor) || 1)) });
        }
        toast.success("ویرایش با موفقیت انجام شد");
        navigate("mechanical/search");
        return;
      }
      if (!carId || !v.trimShopId || !v.deptId) {
        toast.error("سالن، بدنه و واحد مسئول را مشخص کنید");
        return;
      }
      if (chosen.length === 0) {
        toast.error("محصولی برای ثبت بحران انتخاب نشده است");
        return;
      }
      const newId = await createMechParts({
        carId,
        partNo: v.partNo.trim(),
        partDesc: v.partDesc,
        plantId: PLANT_ID,
        partGroup: v.partGroup,
        alterNativePartNo: v.alterNativePartNo || null,
        criticalPartNote: v.criticalPartNote || null,
        stock: Number(v.stock),
        used: Number(v.used),
        possibleProductionNo: Number(v.possibleProductionNo),
        trimShopId: v.trimShopId,
        usageFactor: Number(v.usageFactor),
        deptId: v.deptId,
        isOutOfSap: v.isOutOfSap,
      });
      await saveMechPartsProducts(Number(newId), { carId, trimShopId: v.trimShopId, plantId: PLANT_ID, isNew: 1, items: chosen.map((r) => toItem(r, Number(v.usageFactor) || 1)) });
      toast.success("بحران مکانیکی با موفقیت ثبت شد");
      reset(empty);
      setCarId(0);
      setBodyInfo("");
      setCandidates(null);
      setChosen([]);
    } finally {
      setBusy(null);
    }
  };

  const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">{isEdit ? "ویرایش بحران مکانیکی" : "ثبت بحران مکانیکی"}</IkcoTypography>

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <Controller name="trimShopId" control={control} rules={{ required: "سالن مونتاژ را انتخاب کنید" }} render={({ field }) => <LookupAutocomplete lookupKey="trim-shops" label="سالن مونتاژ" value={field.value} onChange={field.onChange} error={!!errors.trimShopId} helperText={errors.trimShopId?.message} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره بدنه" disabled={isEdit} {...register("bodyNo", { required: !isEdit ? "شماره بدنه الزامی است" : false, maxLength: { value: 8, message: "حداکثر ۸ کاراکتر" } })} error={!!errors.bodyNo} helperText={errors.bodyNo?.message} />
            </IkcoGrid>
            {!isEdit && (
              <IkcoGrid size={size}>
                <IkcoButton variant="outlined" startIcon={<Search />} loading={busy === "body"} onClick={loadBody}>
                  دریافت اطلاعات بدنه
                </IkcoButton>
              </IkcoGrid>
            )}
            {bodyInfo && (
              <IkcoGrid size={12}>
                <IkcoAlert status="info" severity="info">{bodyInfo}</IkcoAlert>
              </IkcoGrid>
            )}
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره فنی" disabled={isEdit} {...register("partNo", { required: "شماره فنی الزامی است", maxLength: { value: 30, message: "حداکثر ۳۰ کاراکتر" } })} error={!!errors.partNo} helperText={errors.partNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoRadioGroup row value={source} onChange={(_, v) => setSource(v as ProductSource)} options={[{ value: "bop", label: "محصولات از BOP" }, { value: "bom", label: "محصولات از BOM" }]} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoButton variant="outlined" startIcon={<Search />} loading={busy === "part"} disabled={!trimShopId} onClick={loadPart}>
                بررسی شماره فنی و دریافت محصولات
              </IkcoButton>
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 6 }}>
              <IkcoTextField label="شرح قطعه" disabled {...register("partDesc")} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="گروه قطعه" {...register("partGroup", { required: "گروه قطعه الزامی است", maxLength: { value: 1, message: "حداکثر ۱ کاراکتر" } })} error={!!errors.partGroup} helperText={errors.partGroup?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <Controller name="deptId" control={control} rules={{ required: !isEdit ? "واحد مسئول را انتخاب کنید" : false }} render={({ field }) => <LookupAutocomplete lookupKey="depts" label="واحد مسئول کسری" value={field.value} onChange={field.onChange} error={!!errors.deptId} helperText={errors.deptId?.message} disabled={isEdit} />} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="موجودی" type="number" {...register("stock", { required: true, min: 0 })} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="مصرف‌شده تاکنون" type="number" {...register("used", { required: true, min: 0 })} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="تعداد مجاز تولید با کسری" type="number" disabled={isEdit} {...register("possibleProductionNo", { required: true, min: 0 })} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="ضریب مصرف" type="number" {...register("usageFactor", { required: true, min: { value: 1, message: "حداقل ۱" } })} error={!!errors.usageFactor} helperText={errors.usageFactor?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="قطعات آلترنات" {...register("alterNativePartNo", mechPartsCreateInputRules.alterNativePartNo)} error={!!errors.alterNativePartNo} helperText={errors.alterNativePartNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات" {...register("criticalPartNote", mechPartsCreateInputRules.criticalPartNote)} error={!!errors.criticalPartNote} helperText={errors.criticalPartNote?.message} />
            </IkcoGrid>
            {isEdit && (
              <>
                <IkcoGrid size={size}>
                  <IkcoTextField label="شماره نامه" {...register("productionStateLetterNo")} />
                </IkcoGrid>
                <IkcoGrid size={size}>
                  <IkcoTextField label="توضیحات وضعیت تولید" {...register("productionStateNote")} />
                </IkcoGrid>
              </>
            )}
            <IkcoGrid size={size}>
              <Controller name="isOutOfSap" control={control} render={({ field }) => <IkcoCheckbox label="خارج از SAP" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>

      <IkcoGrid container spacing={2}>
        <IkcoGrid size={{ xs: 12, md: 7 }}>
          <IkcoStack spacing={1}>
            <IkcoTypography variant="subtitle1">محصولات مصرف‌کننده شماره فنی</IkcoTypography>
            <IkcoDataGrid
              columns={candColumns}
              rows={candRows}
              getRowId={(r) => r.__rowId}
              checkboxSelection
              disableRowSelectionOnClick
              rowSelectionModel={candSel}
              onRowSelectionModelChange={(m) => setCandSel(m as number[])}
            />
            <IkcoButton variant="outlined" startIcon={<Add />} disabled={candSel.length === 0} onClick={addChosen}>
              افزودن به محصولات بحران
            </IkcoButton>
          </IkcoStack>
        </IkcoGrid>
        <IkcoGrid size={{ xs: 12, md: 5 }}>
          <IkcoStack spacing={1}>
            <IkcoTypography variant="subtitle1">محصولات انتخاب‌شده</IkcoTypography>
            <IkcoDataGrid
              columns={candColumns}
              rows={chosen}
              getRowId={(r) => r.__rowId}
              checkboxSelection
              disableRowSelectionOnClick
              rowSelectionModel={chosenSel}
              onRowSelectionModelChange={(m) => setChosenSel(m as number[])}
            />
            <IkcoButton variant="outlined" startIcon={<Remove />} disabled={chosenSel.length === 0} onClick={() => { setChosen((c) => c.filter((r) => !chosenSel.includes(r.__rowId))); setChosenSel([]); }}>
              حذف از انتخاب‌شده‌ها
            </IkcoButton>
          </IkcoStack>
        </IkcoGrid>
      </IkcoGrid>

      {isEdit && existing && (
        <IkcoStack spacing={1}>
          <IkcoTypography variant="subtitle1">محصولات ثبت‌شده بحران</IkcoTypography>
          <IkcoDataGrid columns={existingColumns} rows={existingRows} getRowId={(r) => r.__rowId} autoHeight />
        </IkcoStack>
      )}

      <IkcoCard>
        <IkcoCardActions>
          <PermissionGuard can={isEdit ? ApiPermissions.mechParts.update : ApiPermissions.mechParts.create} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" loading={busy === "save"} onClick={handleSubmit(onSubmit)}>
              {isEdit ? "ذخیره ویرایش" : "ثبت بحران"}
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton onClick={() => navigate("mechanical/search")}>جستجوی بحران‌ها</IkcoButton>
        </IkcoCardActions>
      </IkcoCard>
    </IkcoStack>
  );
}
