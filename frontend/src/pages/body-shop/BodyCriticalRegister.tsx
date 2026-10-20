import {
  IkcoButton,
  IkcoCheckbox,
  IkcoChip,
  IkcoDataGrid,
  IkcoGrid,
  IkcoIconButton,
  IkcoStack,
  IkcoTextArea,
  IkcoTextField,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { AddCircle, ArrowDownward, ArrowUpward, Cancel, FileUpload, History, Save, Search, Tag } from "@mui/icons-material";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import BodyInfoStrip, { bodyInfoCells } from "../../components/BodyInfoStrip";
import SectionPanel from "../../components/SectionPanel";
import TabularDialog from "../../components/TabularDialog";
import { useQuery } from "../../hooks/useQueryString";
import {
  createBodyCriticalsCriticalPart,
  createBodyCriticalsProducts,
  getBodyCriticalsBodyInfo,
  getBodyCriticalsCriticalPartInfo,
  getBodyCriticalsEventLog,
  getBodyCriticalsPartDescription,
  getBodyCriticalsPartProducts,
  updateBodyCriticalsCriticalPart,
} from "../../server/services/bodyCriticalsService";
import { getBodyReportsCriticalPartProducts } from "../../server/services/bodyReportsService";
import type { IBodyInfoRow, IPartProductRow } from "../../types/BodyShop";
import { bodyCriticalsCreateCriticalPartInputRules, bodyCriticalsUpdateCriticalPartInputRules } from "../../types/BodyShop";
import type { ITabularResult } from "../../types/tabular";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import BodyCriticalSearchDialog from "./dialogs/BodyCriticalSearchDialog";
import BodyExcelImportDialog from "./dialogs/BodyExcelImportDialog";
import BodyPreAssignCountDialog from "./dialogs/BodyPreAssignCountDialog";
import ActionBar from "../../components/ActionBar";

interface FormValues {
  bodyNumber: string;
  partNo: string;
  partDesc: string;
  partGroup: string;
  alterNativePartNo: string;
  criticalPartNote: string;
  totalStock: string;
  rawStock: string;
  letterNo: string;
  productionStateNote: string;
  isOutOfSAP: boolean;
}

const emptyForm: FormValues = {
  bodyNumber: "",
  partNo: "",
  partDesc: "",
  partGroup: "",
  alterNativePartNo: "",
  criticalPartNote: "",
  totalStock: "",
  rawStock: "",
  letterNo: "",
  productionStateNote: "",
  isOutOfSAP: false,
};

const text = (row: Record<string, unknown> | undefined, key: string) => (row?.[key] === null || row?.[key] === undefined ? "" : String(row[key]));

const productColumns: IkcoGridColDef<IPartProductRow>[] = [
  { field: "rowNo", headerName: "ردیف", width: 80 },
  { field: "carGroupDescF", headerName: "خانواده محصول", flex: 1, minWidth: 140 },
  { field: "carModelDescF", headerName: "مدل", flex: 1, minWidth: 140 },
  { field: "carClassCode", headerName: "کد کلاس", width: 120 },
  { field: "subclass", headerName: "زیرکلاس", width: 120 },
  { field: "quantity", headerName: "ضریب مصرف", width: 110, type: "number" },
];

/** وضعیت‌های فرم ویندوزی (formState): عادی ← جدید ← بدنه معتبر ← قطعه معتبر */
type Flow = "normal" | "create" | "body" | "part";

const flowLabels: Record<Flow, string> = {
  normal: "حالت عادی (برای شروع «جدید» را انتخاب کنید)",
  create: "ثبت جدید: شماره بدنه را وارد کنید",
  body: "بدنه تایید شد: شماره فنی را وارد کنید",
  part: "قطعه تایید شد: محصولات و مشخصات را ثبت کنید",
};

interface Props {
  /** شناسه بحران برای ویرایش؛ در غیر این صورت از پارامتر ?id= آدرس خوانده می‌شود */
  editId?: number;
  /** پس از ذخیره ویرایش (هنگامی که فرم داخل modal باز شده است) */
  onDone?: () => void;
}

/**
 * ثبت بحران بدنه‌سازی (frmBodyCriticalParts). جریان: شماره بدنه ← مشخصات بدنه شاخص؛ شماره فنی ← شرح و محصولات BOM همان خط؛
 * انتقال کلاس‌ها به «محصولات انتخاب‌شده» با فلش‌ها؛ ذخیره (S_CreateCriticalParts_V3 و سپس S_CreateCritocalPartsProduct).
 * در حالت ویرایش فقط مشخصات بحران قابل ویرایش است (S_Web_UpdateCriticalParts). «ایجاد با اکسل»، «جستجوی اقلام بحرانی»، «لاگ» و
 * «تعداد بدنه‌های پیش‌تخصیص‌شده» به صورت modal باز می‌شوند.
 */
export default function BodyCriticalRegister({ editId: editIdProp, onDone }: Props = {}) {
  const query = useQuery();
  const [pickedId, setPickedId] = useState(0);
  const editId = pickedId || editIdProp || Number(query.get("id") ?? 0);
  const isEdit = editId > 0;
  const { control, register, handleSubmit, reset, getValues, setValue, formState: { errors } } = useForm<FormValues>({ defaultValues: emptyForm });

  const [body, setBody] = useState<IBodyInfoRow | null>(null);
  const [bom, setBom] = useState<IPartProductRow[]>([]);
  const [chosen, setChosen] = useState<IPartProductRow[]>([]);
  const [bomSel, setBomSel] = useState<number[]>([]);
  const [chosenSel, setChosenSel] = useState<number[]>([]);
  const [existing, setExisting] = useState<ITabularResult | null>(null);
  const [saving, setSaving] = useState(false);
  const [loadingBody, setLoadingBody] = useState(false);
  const [loadingPart, setLoadingPart] = useState(false);
  const [saloonCode, setSaloonCode] = useState("");
  const [dialog, setDialog] = useState<"excel" | "search" | "log" | "count" | null>(null);
  const [flow, setFlow] = useState<Flow>("normal");
  const [loadedPart, setLoadedPart] = useState("");
  // در حالت ویرایش، فرم مستقیماً در وضعیت «قطعه معتبر» است (PartNoValidate)
  const stage: Flow = isEdit ? "part" : flow;
  const bodyOpen = stage === "create";
  const partOpen = stage === "body" || stage === "part";
  const productsOpen = stage === "part";

  useEffect(() => {
    if (!isEdit) return;
    void (async () => {
      const info = await getBodyCriticalsCriticalPartInfo(editId);
      const row = info.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        toast.error("بحران یافت نشد");
        return;
      }
      reset({
        bodyNumber: text(row, "BodyNO"),
        partNo: text(row, "PartNo"),
        partDesc: text(row, "PartDescF"),
        partGroup: text(row, "PartGroup"),
        alterNativePartNo: text(row, "AlternativePartNo"),
        criticalPartNote: text(row, "CriticalPartNote").trim(),
        totalStock: text(row, "TotalStock"),
        rawStock: text(row, "RawStock"),
        letterNo: text(row, "ProductionStateLetterNo"),
        productionStateNote: text(row, "ProductionStateNote"),
        isOutOfSAP: false,
      });
      setSaloonCode(text(row, "SaloonDescF"));
      setExisting(await getBodyReportsCriticalPartProducts(editId));
    })();
  }, [editId, isEdit, reset]);

  const existingColumns = useMemo(() => resultColumns(existing, columnHeaders), [existing]);
  const existingRows: IGridRow[] = useMemo(() => tabularRows(existing), [existing]);

  const clearForm = () => {
    reset(emptyForm);
    setBody(null);
    setBom([]);
    setChosen([]);
    setExisting(null);
    setSaloonCode("");
    setPickedId(0);
    setLoadedPart("");
    setFlow("normal");
  };

  const loadBody = async () => {
    const bodyNumber = getValues("bodyNumber").trim();
    if (!bodyNumber) return;
    setLoadingBody(true);
    try {
      const info = await getBodyCriticalsBodyInfo({ bodyNumber });
      if (!info || typeof info !== "object") {
        // مطابق uctBodyInfo_CompeleteBodyNo: بدنه نامعتبر = وضعیت Cancel (پاک‌سازی فرم)
        clearForm();
        toast.error("بدنه یافت نشد یا به خط بدنه‌سازی آن دسترسی ندارید");
        return;
      }
      setBody(info);
      setBom([]);
      setChosen([]);
      setSaloonCode(info.saloonDescF ?? info.saloon ?? "");
      setFlow("body");
    } finally {
      setLoadingBody(false);
    }
  };

  const loadPart = async () => {
    const partNo = getValues("partNo").trim();
    if (!partNo || !body?.b_VIN_BIWSiteID || !partOpen || partNo === loadedPart) return;
    setLoadingPart(true);
    try {
      const desc = await getBodyCriticalsPartDescription({ partNo });
      if (!desc || typeof desc !== "string") {
        toast.error("شماره فنی وارد شده نادرست است");
        return;
      }
      setValue("partDesc", desc.slice(0, 100));
      const products = await getBodyCriticalsPartProducts({ partNo, biwSiteId: body.b_VIN_BIWSiteID });
      if (products.items.length === 0) {
        // مطابق uctPartNo_txtPartNoKeyDown: بدون محصول، شماره فنی پاک و فرم به حالت «بدنه معتبر» برمی‌گردد
        toast("موردی یافت نشد");
        setValue("partNo", "");
        setValue("partDesc", "");
        setLoadedPart("");
        setBom([]);
        setChosen([]);
        setFlow("body");
        return;
      }
      setBom(products.items);
      setChosen([]);
      setLoadedPart(partNo);
      setFlow("part");
    } finally {
      setLoadingPart(false);
    }
  };

  const addProducts = () => {
    const picked = bom.filter((r) => bomSel.includes(r.rowNo));
    if (picked.length === 0) {
      toast("محصولی انتخاب نشده است");
      return;
    }
    setChosen((c) => [...c, ...picked]);
    setBom((b) => b.filter((r) => !bomSel.includes(r.rowNo)));
    setBomSel([]);
  };

  const removeProducts = () => {
    const picked = chosen.filter((r) => chosenSel.includes(r.rowNo));
    setBom((b) => [...b, ...picked].sort((x, y) => x.rowNo - y.rowNo));
    setChosen((c) => c.filter((r) => !chosenSel.includes(r.rowNo)));
    setChosenSel([]);
  };

  const onSubmit = async (v: FormValues) => {
    setSaving(true);
    try {
      if (isEdit) {
        await updateBodyCriticalsCriticalPart(editId, {
          partGroup: v.partGroup.trim(),
          alterNativePartNo: v.alterNativePartNo || null,
          criticalPartNote: v.criticalPartNote || null,
          rawStock: Number(v.rawStock || 0),
          productionStateNote: v.productionStateNote || null,
          letterNo: v.letterNo || null,
        });
        toast.success("ویرایش با موفقیت انجام شد");
        onDone?.();
        if (!editIdProp) clearForm();
        return;
      }
      if (!body || !body.b_VIN_BIWSiteID) {
        toast.error("اطلاعات بدنه کامل نیست");
        return;
      }
      if (chosen.length === 0) {
        toast.error("محصولی جهت ثبت بحران انتخاب نشده است");
        return;
      }
      const created = await createBodyCriticalsCriticalPart({
        carId: body.carId,
        partNo: v.partNo.trim(),
        partDesc: v.partDesc,
        partGroup: v.partGroup.trim(),
        alterNativePartNo: v.alterNativePartNo || null,
        criticalPartNote: v.criticalPartNote || null,
        biwSiteId: body.b_VIN_BIWSiteID,
        totalStock: Number(v.totalStock || 0),
        rawStock: Number(v.rawStock || 0),
        productionStateNote: v.productionStateNote || null,
        letterNo: v.letterNo || null,
        isOutOfSAP: v.isOutOfSAP,
      });
      await createBodyCriticalsProducts(created.result, {
        carId: body.carId,
        items: chosen.map((r) => ({ carClassId: r.carClassId, usageFactor: r.quantity })),
      });
      toast.success(created.message ?? "ثبت با موفقیت انجام شد");
      clearForm();
    } finally {
      setSaving(false);
    }
  };

  const rules = isEdit ? bodyCriticalsUpdateCriticalPartInputRules : bodyCriticalsCreateCriticalPartInputRules;
  const cells = bodyInfoCells(body ? body : saloonCode ? { saloonDescF: saloonCode } : null);

  return (
    <IkcoStack spacing={2}>
      <IkcoStack direction="row" spacing={1} alignItems="center" flexWrap="wrap" useFlexGap>
        <IkcoTypography variant="h6">{isEdit ? "ویرایش بحران بدنه‌سازی" : "ثبت بحران"}</IkcoTypography>
        <IkcoChip label={flowLabels[stage]} status={stage === "normal" ? "default" : stage === "part" ? "success" : "info"} size="small" />
      </IkcoStack>

      <SectionPanel title="مشخصات بدنه شاخص" disabled={!bodyOpen}>
        <Controller
          name="bodyNumber"
          control={control}
          rules={{ required: !isEdit ? "شماره بدنه الزامی است" : false, maxLength: { value: 8, message: "حداکثر ۸ کاراکتر" } }}
          render={({ field }) => <BodyInfoStrip bodyNumber={field.value} onBodyNumberChange={field.onChange} onSearch={() => void loadBody()} cells={cells} disabled={!bodyOpen} loading={loadingBody} />}
        />
        {errors.bodyNumber && <IkcoTypography color="error">{errors.bodyNumber.message}</IkcoTypography>}
        {body && (
          <IkcoGrid container spacing={1}>
            <IkcoGrid size={{ xs: 6, md: 3 }}>
              <IkcoTextField label="تاریخ ثبت پیش تخصیص" value={body.stepDate ?? ""} disabled />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 6, md: 3 }}>
              <IkcoTextField label="سال ساخت بدنه" value={(body.carAttribute ?? "").slice(0, 4)} disabled />
            </IkcoGrid>
          </IkcoGrid>
        )}
      </SectionPanel>

      <SectionPanel title="قطعه کسری" disabled={!partOpen}>
        <IkcoGrid container spacing={2} alignItems="flex-start">
          <IkcoGrid size={{ xs: 12, sm: 6, md: 4 }}>
            <IkcoStack direction="row" spacing={1} alignItems="flex-start">
              <IkcoTextField
                label="شماره فنی"
                disabled={isEdit || !partOpen}
                {...register("partNo", { ...bodyCriticalsCreateCriticalPartInputRules.partNo, required: "شماره فنی الزامی است" })}
                error={!!errors.partNo}
                helperText={errors.partNo?.message}
                onBlur={() => !isEdit && void loadPart()}
                onKeyDown={(e) => {
                  if (e.key === "Enter" && !isEdit) void loadPart();
                }}
              />
              {!isEdit && (
                <IkcoIconButton disabled={!partOpen} onClick={() => void loadPart()} aria-label="بررسی شماره فنی">
                  <Search />
                </IkcoIconButton>
              )}
            </IkcoStack>
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 6, md: 4 }}>
            <IkcoTextField label="شرح شماره فنی" disabled {...register("partDesc")} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 4, md: 4 }}>
            <IkcoTextField label="گروه قطعه" disabled={!partOpen} {...register("partGroup", { ...rules.partGroup, required: "گروه قطعه را وارد نمایید" })} error={!!errors.partGroup} helperText={errors.partGroup?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <IkcoTextField label="موجودی قطعه" type="number" disabled={!partOpen} {...register("rawStock", { required: "موجودی قطعه را وارد نمایید", min: { value: 0, message: "نامعتبر" } })} error={!!errors.rawStock} helperText={errors.rawStock?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 6, sm: 4, md: 3 }}>
            <IkcoTextField label="موجودی مجموعه" type="number" disabled={isEdit || !partOpen} {...register("totalStock", { required: !isEdit ? "موجودی قطعه را وارد نمایید" : false, min: { value: 0, message: "نامعتبر" } })} error={!!errors.totalStock} helperText={errors.totalStock?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, sm: 6, md: 3 }}>
            <IkcoTextField label="شماره نامه" disabled={!productsOpen} {...register("letterNo", rules.letterNo)} error={!!errors.letterNo} helperText={errors.letterNo?.message} />
          </IkcoGrid>
          {!isEdit && (
            <IkcoGrid size={{ xs: 12, sm: 6, md: 3 }}>
              <Controller name="isOutOfSAP" control={control} render={({ field }) => <IkcoCheckbox label="خارج از SAP" disabled={!partOpen} checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
            </IkcoGrid>
          )}
          <IkcoGrid size={{ xs: 12, md: 4 }}>
            <IkcoTextArea label="آلترناتیوها" rows={2} disabled={!partOpen} {...register("alterNativePartNo", rules.alterNativePartNo)} error={!!errors.alterNativePartNo} helperText={errors.alterNativePartNo?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 4 }}>
            <IkcoTextArea label="توضیحات شماره فنی" rows={2} disabled={!partOpen} {...register("criticalPartNote", rules.criticalPartNote)} error={!!errors.criticalPartNote} helperText={errors.criticalPartNote?.message} />
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 4 }}>
            <IkcoTextArea label="توضیحات بحران" rows={2} disabled={!productsOpen} {...register("productionStateNote", rules.productionStateNote)} error={!!errors.productionStateNote} helperText={errors.productionStateNote?.message} />
          </IkcoGrid>
        </IkcoGrid>
      </SectionPanel>

      {!isEdit && (
        <SectionPanel
          title="محصولات"
          disabled={!productsOpen}
          aside={
            <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap>
              <IkcoChip label={`BOM: ${bom.length}`} size="small" />
              <IkcoChip label={`انتخاب‌شده: ${chosen.length}`} status={chosen.length > 0 ? "success" : "default"} size="small" />
            </IkcoStack>
          }
        >
          <IkcoTypography variant="subtitle2">محصولات مرتبط در BOM</IkcoTypography>
          <IkcoDataGrid
            columns={productColumns}
            rows={bom}
            loading={loadingPart}
            getRowId={(r) => r.rowNo}
            checkboxSelection
            disableRowSelectionOnClick
            rowSelectionModel={bomSel}
            onRowSelectionModelChange={(m) => setBomSel(m as number[])}
            autoHeight
          />
          <IkcoStack direction="row" spacing={1} justifyContent="center" flexWrap="wrap" useFlexGap>
            <IkcoButton variant="contained" startIcon={<ArrowDownward />} disabled={!productsOpen || bomSel.length === 0} onClick={addProducts}>
              افزودن به انتخاب‌شده‌ها
            </IkcoButton>
            <IkcoButton variant="outlined" color="error" startIcon={<ArrowUpward />} disabled={!productsOpen || chosenSel.length === 0} onClick={removeProducts}>
              بازگرداندن به BOM
            </IkcoButton>
          </IkcoStack>
          <IkcoTypography variant="subtitle2">محصولات انتخاب شده</IkcoTypography>
          <IkcoDataGrid
            columns={productColumns}
            rows={chosen}
            getRowId={(r) => r.rowNo}
            checkboxSelection
            disableRowSelectionOnClick
            rowSelectionModel={chosenSel}
            onRowSelectionModelChange={(m) => setChosenSel(m as number[])}
            autoHeight
          />
        </SectionPanel>
      )}

      {isEdit && existing && (
        <SectionPanel title="محصولات ثبت‌شده بحران">
          <IkcoDataGrid columns={existingColumns} rows={existingRows} getRowId={(r) => r.__rowId} autoHeight />
        </SectionPanel>
      )}

      <ActionBar>
        <IkcoButton startIcon={<AddCircle />} disabled={stage !== "normal"} onClick={() => setFlow("create")}>
          جدید
        </IkcoButton>
        {!isEdit && (
          <PermissionGuard can={ApiPermissions.bodyCriticals.create}>
            <IkcoButton startIcon={<FileUpload />} disabled={stage !== "normal"} onClick={() => setDialog("excel")}>
              ایجاد با اکسل
            </IkcoButton>
          </PermissionGuard>
        )}
        <PermissionGuard can={isEdit ? ApiPermissions.bodyCriticals.update : ApiPermissions.bodyCriticals.create} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
          <IkcoButton variant="contained" startIcon={<Save />} loading={saving} disabled={stage !== "part"} onClick={handleSubmit(onSubmit)}>
            ذخیره
          </IkcoButton>
        </PermissionGuard>
        {!editIdProp && (
          <IkcoButton startIcon={<Search />} disabled={stage !== "normal"} onClick={() => setDialog("search")}>
            جستجوی اقلام بحرانی
          </IkcoButton>
        )}
        <IkcoButton startIcon={<History />} disabled={!isEdit} onClick={() => setDialog("log")}>
          لاگ
        </IkcoButton>
        <IkcoButton startIcon={<Tag />} onClick={() => setDialog("count")}>
          تعداد بدنه‌های پیش تخصیص شده
        </IkcoButton>
        <IkcoButton startIcon={<Cancel />} disabled={stage === "normal"} onClick={() => (editIdProp ? onDone?.() : clearForm())}>
          انصراف
        </IkcoButton>
      </ActionBar>

      <BodyExcelImportDialog open={dialog === "excel"} close={() => setDialog(null)} defaultBiwSiteId={body?.b_VIN_BIWSiteID} />
      <BodyCriticalSearchDialog
        open={dialog === "search"}
        close={() => setDialog(null)}
        onPick={(id) => {
          setPickedId(id);
          setDialog(null);
        }}
      />
      <BodyPreAssignCountDialog open={dialog === "count"} close={() => setDialog(null)} initialBodyNumber={getValues("bodyNumber")} />
      <TabularDialog open={dialog === "log"} close={() => setDialog(null)} title="گزارش رویدادها" load={() => getBodyCriticalsEventLog({ eventTypeId: 0, changedRecordId: editId })} />
    </IkcoStack>
  );
}
