import {
  IkcoButton,
  IkcoCheckbox,
  IkcoChip,
  IkcoDataGrid,
  IkcoGrid,
  IkcoRadioGroup,
  IkcoStack,
  IkcoTextArea,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { AddCircle, Cancel, Check, Download, History, PersonSearch, Remove, Search, Tag } from "@mui/icons-material";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import BodyInfoStrip from "../../components/BodyInfoStrip";
import FormDialog from "../../components/FormDialog";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import SectionPanel from "../../components/SectionPanel";
import TabularDialog from "../../components/TabularDialog";
import { useQuery } from "../../hooks/useQueryString";
import { getMechReportsStockLog } from "../../server/services/mechReportsService";
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
import type { ITabularResult } from "../../types/tabular";
import { mechPartsCreateInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import MechCriticalSearchDialog from "./dialogs/MechCriticalSearchDialog";
import MechTrimShopBodyCountDialog from "./dialogs/MechTrimShopBodyCountDialog";
import ActionBar from "../../components/ActionBar";
import { gridColumns } from "../../utils/gridColumns";

/** شناسه سایت پیش‌فرض رویه‌های SFC (PlantId=1) */
const PLANT_ID = 1;

type ProductSource = "bop" | "bom";

/** وضعیت‌های فرم ویندوزی (formState): عادی ← جدید ← بدنه معتبر ← قطعه معتبر */
type Flow = "normal" | "create" | "body" | "part";

const flowLabels: Record<Flow, string> = {
  normal: "حالت عادی (برای شروع «جدید» را انتخاب کنید)",
  create: "ثبت جدید: سالن و شماره بدنه را وارد کنید",
  body: "بدنه تایید شد: شماره فنی را وارد کنید",
  part: "قطعه تایید شد: محصولات را مشخص و تایید کنید",
};

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
interface Props {
  /** شناسه بحران برای ویرایش؛ در غیر این صورت از پارامتر ?id= آدرس خوانده می‌شود */
  editId?: number;
  /** پس از ذخیره ویرایش (هنگامی که فرم داخل modal باز شده است) */
  onDone?: () => void;
}

export default function MechCriticalRegister({ editId: editIdProp, onDone }: Props = {}) {
  const query = useQuery();
  const [pickedId, setPickedId] = useState(0);
  const editId = pickedId || editIdProp || Number(query.get("id") ?? 0);
  const isEdit = editId > 0;
  const { control, register, handleSubmit, reset, getValues, setValue, watch, formState: { errors } } = useForm<FormValues>({ defaultValues: empty });

  const [carId, setCarId] = useState(0);
  const [bodyRow, setBodyRow] = useState<Record<string, unknown> | null>(null);
  const [dialog, setDialog] = useState<"search" | "count" | "log" | "manual" | null>(null);
  const [source, setSource] = useState<ProductSource>("bop");
  const [candidates, setCandidates] = useState<ITabularResult | null>(null);
  const [chosen, setChosen] = useState<Row[]>([]);
  const [candSel, setCandSel] = useState<number[]>([]);
  const [chosenSel, setChosenSel] = useState<number[]>([]);
  const [existing, setExisting] = useState<ITabularResult | null>(null);
  const [busy, setBusy] = useState<"body" | "part" | "save" | null>(null);
  const [flow, setFlow] = useState<Flow>("normal");
  // در حالت ویرایش، فرم مستقیماً در وضعیت «قطعه معتبر» است (PartNoValidate)
  const stage: Flow = isEdit ? "part" : flow;
  const bodyOpen = stage === "create";
  const partOpen = stage === "body" || stage === "part";
  const productsOpen = stage === "part";

  const trimShopId = watch("trimShopId");
  const candRows = useMemo(() => tabularRows(candidates) as Row[], [candidates]);
  const candColumns = useMemo(() => resultColumns(candidates, columnHeaders, gridColumns.mechProductCandidates), [candidates]);
  const existingRows = useMemo(() => tabularRows(existing), [existing]);
  const existingColumns = useMemo(() => resultColumns(existing, columnHeaders, gridColumns.mechCriticalProducts), [existing]);

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

  const clearForm = () => {
    reset(empty);
    setCarId(0);
    setBodyRow(null);
    setCandidates(null);
    setChosen([]);
    setExisting(null);
    setPickedId(0);
    setFlow("normal");
  };

  const loadBody = async () => {
    const { bodyNo } = getValues();
    if (!bodyNo.trim()) return;
    setBusy("body");
    try {
      const data = await getMechPartsBodyProperties({ bodyNo: bodyNo.trim(), plantId: PLANT_ID, mode: 1 });
      const row = data.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        // بدنه نامعتبر = وضعیت Cancel در فرم ویندوزی (پاک‌سازی کامل)
        clearForm();
        toast.error("بدنه یافت نشد");
        return;
      }
      setCarId(Number(row["Id"]));
      setBodyRow(row);
      setFlow("body");
    } finally {
      setBusy(null);
    }
  };

  const loadPart = async (src: ProductSource = source) => {
    const { partNo, trimShopId: shop } = getValues();
    if (!partNo.trim() || !shop || !partOpen) return;
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
      const data = src === "bop" ? await getMechPartsFromBop({ partNo: partNo.trim(), trimShopId: shop }) : await getMechPartsPartProducts({ partNo: partNo.trim() });
      if (data.rows.length === 0) toast("موردی یافت نشد");
      setCandidates(data);
      setChosen([]);
      setFlow("part");
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
        onDone?.();
        if (!editIdProp) clearForm();
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
      clearForm();
    } finally {
      setBusy(null);
    }
  };

  const rowText = (k: string, ...alt: string[]) => [k, ...alt].map((x) => str(bodyRow ?? undefined, x)).find((x) => x) ?? "";
  const cells = [
    { label: "محصول", value: [rowText("CarGroupDescf"), rowText("CarModelDesc"), rowText("CarClassDesc")].filter(Boolean).join(" - ") },
    { label: "رنگ", value: rowText("CarColorDescf") },
    { label: "زیرکلاس", value: rowText("SubClassCode", "SubClassDesc", "CarSubClassDesc") },
    { label: "شماره موتور", value: rowText("EngineNO", "EngineNo") },
    { label: "VIN", value: rowText("VIN", "Vin") },
    { label: "مرحله جاری", value: rowText("LastStepDescF") },
    { label: "سالن", value: rowText("LastSaloonDescF") },
  ];
  const shownRows = isEdit ? existingRows : chosen;
  const shownColumns = isEdit ? existingColumns : candColumns;

  return (
    <IkcoStack spacing={2}>
      <IkcoStack direction="row" spacing={1} alignItems="center" flexWrap="wrap" useFlexGap>
        <IkcoTypography variant="h6">{isEdit ? "ویرایش اقلام بحرانی قطعات مکانیکی" : "فرم تعریف اقلام بحرانی قطعات مکانیکی"}</IkcoTypography>
        <IkcoChip label={flowLabels[stage]} status={stage === "normal" ? "default" : stage === "part" ? "success" : "info"} size="small" />
      </IkcoStack>

      <SectionPanel title="مشخصات بدنه شاخص" disabled={!bodyOpen}>
        <IkcoGrid container spacing={2} alignItems="center">
          <IkcoGrid size={{ xs: 12, sm: 6, md: 3 }}>
            <Controller name="trimShopId" control={control} rules={{ required: "سالن مونتاژ را انتخاب کنید" }} render={({ field }) => <LookupAutocomplete lookupKey="trim-shops" label="سالن مونتاژ" value={field.value} disabled={!bodyOpen} onChange={field.onChange} error={!!errors.trimShopId} helperText={errors.trimShopId?.message} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 3, md: 2 }}>
            <IkcoTextField label="تاریخ ورود" value={rowText("LastStepdate")} disabled />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 3, md: 2 }}>
            <IkcoTextField label="ساعت ورود" value={rowText("LastSteptime")} disabled />
          </IkcoGrid>
        </IkcoGrid>
        <Controller
          name="bodyNo"
          control={control}
          rules={{ required: !isEdit ? "شماره بدنه الزامی است" : false, maxLength: { value: 8, message: "حداکثر ۸ کاراکتر" } }}
          render={({ field }) => <BodyInfoStrip bodyNumber={field.value} onBodyNumberChange={field.onChange} onSearch={() => void loadBody()} cells={cells} disabled={!bodyOpen} loading={busy === "body"} />}
        />
        {errors.bodyNo && <IkcoTypography color="error">{errors.bodyNo.message}</IkcoTypography>}
      </SectionPanel>

      <SectionPanel title="قطعه کسری" disabled={!partOpen}>
        <IkcoGrid container spacing={2} alignItems="flex-start">
          <IkcoGrid size={{ xs: 12, sm: 6, md: 3 }}>
            <IkcoTextField
              label="شماره فنی"
              disabled={isEdit || !partOpen}
              {...register("partNo", { required: "شماره فنی الزامی است", maxLength: { value: 30, message: "حداکثر ۳۰ کاراکتر" } })}
              error={!!errors.partNo}
              helperText={errors.partNo?.message}
              onBlur={() => !isEdit && !!trimShopId && void loadPart()}
              onKeyDown={(e) => {
                if (e.key === "Enter" && !isEdit) void loadPart();
              }}
            />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 6, md: 4 }}>
            <IkcoTextField label="شرح فنی" disabled {...register("partDesc")} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 2 }}>
            <IkcoTextField label="گروه قطعه" disabled={!partOpen} {...register("partGroup", { required: "گروه قطعه را وارد نمایید", maxLength: { value: 1, message: "حداکثر ۱ کاراکتر" } })} error={!!errors.partGroup} helperText={errors.partGroup?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <IkcoTextField label="موجودی قطعه" type="number" disabled={!partOpen} {...register("stock", { required: true, min: 0 })} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: "auto" }}>
            <Controller name="isOutOfSap" control={control} render={({ field }) => <IkcoCheckbox label="عدم اعمال تغییرات توسط SAP" disabled={!partOpen} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: "grow" }}>
            <IkcoTextArea label="آلترناتیوها" rows={2} disabled={!partOpen} {...register("alterNativePartNo", mechPartsCreateInputRules.alterNativePartNo)} error={!!errors.alterNativePartNo} helperText={errors.alterNativePartNo?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 4 }}>
            <IkcoTextArea label="توضیحات" rows={2} disabled={!partOpen} {...register("criticalPartNote", mechPartsCreateInputRules.criticalPartNote)} error={!!errors.criticalPartNote} helperText={errors.criticalPartNote?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 2 }}>
            <IkcoTextField label="مصرف‌شده تاکنون" type="number" disabled={!partOpen} {...register("used", { required: true, min: 0 })} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 2 }}>
            <IkcoTextField label="ضریب مصرف" type="number" disabled={!partOpen} {...register("usageFactor", { required: true, min: { value: 1, message: "حداقل ۱" } })} error={!!errors.usageFactor} helperText={errors.usageFactor?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <IkcoTextField label="شماره نامه" disabled={!productsOpen} {...register("productionStateLetterNo")} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 5 }}>
            <IkcoTextField label="توضیحات (وضعیت تولید)" disabled={!productsOpen} {...register("productionStateNote")} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <IkcoTextField label="تعداد مجاز تولید با کسری" type="number" disabled={isEdit || !partOpen} {...register("possibleProductionNo", { required: true, min: 0 })} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <Controller name="deptId" control={control} rules={{ required: !isEdit ? "واحد مسئول را انتخاب کنید" : false }} render={({ field }) => <LookupAutocomplete lookupKey="depts" label="واحد مسئول کسری" value={field.value} onChange={field.onChange} error={!!errors.deptId} helperText={errors.deptId?.message} disabled={isEdit || !partOpen} />} />
          </IkcoGrid>
        </IkcoGrid>
      </SectionPanel>

      <SectionPanel title={isEdit ? "محصولات ثبت‌شده بحران" : "محصولات بحران"} disabled={!productsOpen} aside={<IkcoChip label={`تعداد: ${shownRows.length}`} size="small" />}>
        <IkcoDataGrid
          columns={shownColumns}
          rows={shownRows}
          getRowId={(r) => r.__rowId}
          checkboxSelection={!isEdit}
          disableRowSelectionOnClick
          rowSelectionModel={isEdit ? undefined : chosenSel}
          onRowSelectionModelChange={isEdit ? undefined : (m) => setChosenSel(m as number[])}
          autoHeight
        />
      </SectionPanel>

      <ActionBar>
        <IkcoButton startIcon={<AddCircle />} disabled={stage !== "normal"} onClick={() => setFlow("create")}>
          جدید
        </IkcoButton>
        <PermissionGuard can={isEdit ? ApiPermissions.mechParts.update : ApiPermissions.mechParts.create} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
          <IkcoButton variant="contained" startIcon={<Check />} loading={busy === "save"} disabled={stage !== "part"} onClick={handleSubmit(onSubmit)}>
            تایید
          </IkcoButton>
        </PermissionGuard>
        <IkcoButton startIcon={<Cancel />} disabled={stage === "normal"} onClick={() => (editIdProp ? onDone?.() : clearForm())}>
          انصراف
        </IkcoButton>
        <IkcoButton startIcon={<PersonSearch />} disabled={!productsOpen || !trimShopId} onClick={() => setDialog("manual")}>
          افزودن دستی محصول
        </IkcoButton>
        <IkcoButton color="error" startIcon={<Remove />} disabled={isEdit || !productsOpen || chosenSel.length === 0} onClick={() => { setChosen((c) => c.filter((r) => !chosenSel.includes(r.__rowId))); setChosenSel([]); }}>
          حذف محصول
        </IkcoButton>
        {!editIdProp && (
          <IkcoButton startIcon={<Search />} disabled={stage !== "normal"} onClick={() => setDialog("search")}>
            جستجو
          </IkcoButton>
        )}
        <IkcoButton startIcon={<Download />} disabled={isEdit || !productsOpen || !trimShopId} loading={busy === "part"} onClick={() => { setSource("bop"); void loadPart("bop"); }}>
          بازیابی از BOP
        </IkcoButton>
        <IkcoButton startIcon={<History />} disabled={!isEdit} onClick={() => setDialog("log")}>
          LOG
        </IkcoButton>
        <IkcoButton startIcon={<Tag />} onClick={() => setDialog("count")}>
          تعداد بدنه‌های ورود به مونتاژ شده
        </IkcoButton>
      </ActionBar>

      <FormDialog
        open={dialog === "manual"}
        close={() => setDialog(null)}
        title="افزودن دستی محصول"
        actions={
          <IkcoButton variant="contained" startIcon={<AddCircle />} disabled={candSel.length === 0} onClick={() => { addChosen(); setDialog(null); }}>
            افزودن به محصولات بحران
          </IkcoButton>
        }
      >
        <IkcoStack spacing={2}>
          <IkcoStack direction="row" spacing={2} alignItems="center">
            <IkcoRadioGroup row value={source} onChange={(_, v) => setSource(v as ProductSource)} options={[{ value: "bop", label: "محصولات از BOP" }, { value: "bom", label: "محصولات از BOM" }]} />
            <IkcoButton variant="outlined" startIcon={<Search />} loading={busy === "part"} disabled={!trimShopId} onClick={() => void loadPart()}>
              دریافت محصولات
            </IkcoButton>
          </IkcoStack>
          <IkcoDataGrid
            columns={candColumns}
            rows={candRows}
            getRowId={(r) => r.__rowId}
            checkboxSelection
            disableRowSelectionOnClick
            rowSelectionModel={candSel}
            onRowSelectionModelChange={(m) => setCandSel(m as number[])}
            autoHeight
          />
        </IkcoStack>
      </FormDialog>
      <MechCriticalSearchDialog
        open={dialog === "search"}
        close={() => setDialog(null)}
        onPick={(id) => {
          setPickedId(id);
          setDialog(null);
        }}
      />
      <MechTrimShopBodyCountDialog open={dialog === "count"} close={() => setDialog(null)} initialBodyNumber={getValues("bodyNo")} />
      <TabularDialog open={dialog === "log"} close={() => setDialog(null)} title="LOG" columns={gridColumns.mechStockLog} load={() => getMechReportsStockLog({ partNo: getValues("partNo").trim() })} />
    </IkcoStack>
  );
}
