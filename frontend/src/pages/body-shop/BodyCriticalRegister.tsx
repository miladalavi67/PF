import {
  IkcoAlert,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoGrid,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard, useMFENavigate } from "@ikco/mfe-core";
import { Add, Remove, Search } from "@mui/icons-material";
import { useEffect, useMemo, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import { useQuery } from "../../hooks/useQueryString";
import {
  createBodyCriticalsCriticalPart,
  createBodyCriticalsProducts,
  getBodyCriticalsBodyInfo,
  getBodyCriticalsCriticalPartInfo,
  getBodyCriticalsPartDescription,
  getBodyCriticalsPartProducts,
  updateBodyCriticalsCriticalPart,
} from "../../server/services/bodyCriticalsService";
import { getBodyReportsCriticalPartProducts } from "../../server/services/bodyReportsService";
import { ApiPermissions } from "../../types/permissions";
import type { IBodyInfoRow, IPartProductRow } from "../../types/BodyShop";
import { bodyCriticalsCreateCriticalPartInputRules, bodyCriticalsUpdateCriticalPartInputRules } from "../../types/BodyShop";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import { columnHeaders } from "../../utils/columnHeaders";
import type { ITabularResult } from "../../types/Common";

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

/**
 * ثبت و ویرایش بحران بدنه‌سازی (frmBodyCriticalParts). جریان: شماره بدنه ← اطلاعات و خط بدنه‌سازی؛ شماره فنی ← شرح و محصولات BOM
 * همان خط؛ انتخاب کلاس‌ها؛ ثبت (S_CreateCriticalParts_V3 و سپس S_CreateCritocalPartsProduct). در حالت ویرایش (?id=) فقط
 * مشخصات بحران قابل ویرایش است (S_Web_UpdateCriticalParts).
 */
export default function BodyCriticalRegister() {
  const query = useQuery();
  const editId = Number(query.get("id") ?? 0);
  const isEdit = editId > 0;
  const navigate = useMFENavigate();
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

  const loadBody = async () => {
    const bodyNumber = getValues("bodyNumber").trim();
    if (!bodyNumber) return;
    setLoadingBody(true);
    try {
      const info = await getBodyCriticalsBodyInfo({ bodyNumber });
      if (!info || typeof info !== "object") {
        setBody(null);
        toast.error("بدنه یافت نشد یا به خط بدنه‌سازی آن دسترسی ندارید");
        return;
      }
      setBody(info);
      setBom([]);
      setChosen([]);
      setSaloonCode(info.saloonDescF ?? info.saloon ?? "");
    } finally {
      setLoadingBody(false);
    }
  };

  const loadPart = async () => {
    const partNo = getValues("partNo").trim();
    if (!partNo || !body?.b_VIN_BIWSiteID) {
      toast.error("ابتدا بدنه را مشخص کنید");
      return;
    }
    setLoadingPart(true);
    try {
      const desc = await getBodyCriticalsPartDescription({ partNo });
      if (!desc || typeof desc !== "string") {
        toast.error("شماره فنی وارد شده نادرست است");
        return;
      }
      setValue("partDesc", desc.slice(0, 100));
      const products = await getBodyCriticalsPartProducts({ partNo, biwSiteId: body.b_VIN_BIWSiteID });
      if (products.items.length === 0) toast("موردی یافت نشد");
      setBom(products.items);
      setChosen([]);
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
        navigate("body-shop/search");
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
      reset(emptyForm);
      setBody(null);
      setBom([]);
      setChosen([]);
    } finally {
      setSaving(false);
    }
  };

  const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };
  const rules = isEdit ? bodyCriticalsUpdateCriticalPartInputRules : bodyCriticalsCreateCriticalPartInputRules;

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">{isEdit ? "ویرایش بحران بدنه‌سازی" : "ثبت بحران بدنه‌سازی"}</IkcoTypography>

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره بدنه" disabled={isEdit} {...register("bodyNumber", { required: "شماره بدنه الزامی است", maxLength: { value: 8, message: "حداکثر ۸ کاراکتر" } })} error={!!errors.bodyNumber} helperText={errors.bodyNumber?.message} />
            </IkcoGrid>
            {!isEdit && (
              <IkcoGrid size={size}>
                <IkcoButton variant="outlined" startIcon={<Search />} loading={loadingBody} onClick={loadBody}>
                  دریافت اطلاعات بدنه
                </IkcoButton>
              </IkcoGrid>
            )}
            {(body || isEdit) && (
              <IkcoGrid size={{ xs: 12, md: 8 }}>
                <IkcoAlert status="info" severity="info">
                  {body
                    ? `سالن: ${body.saloonDescF ?? body.saloon ?? "-"} | مرحله: ${body.stepDescF ?? "-"} | ${body.stepDate ?? ""} ${body.stepTime ?? ""} | ${body.carAttribute ?? ""} | رنگ: ${body.color ?? "-"}`
                    : `سالن: ${saloonCode || "-"}`}
                </IkcoAlert>
              </IkcoGrid>
            )}
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره فنی" disabled={isEdit || (!body && !isEdit)} {...register("partNo", { ...bodyCriticalsCreateCriticalPartInputRules.partNo, required: "شماره فنی الزامی است" })} error={!!errors.partNo} helperText={errors.partNo?.message} />
            </IkcoGrid>
            {!isEdit && (
              <IkcoGrid size={size}>
                <IkcoButton variant="outlined" startIcon={<Search />} loading={loadingPart} disabled={!body} onClick={loadPart}>
                  بررسی شماره فنی و دریافت محصولات
                </IkcoButton>
              </IkcoGrid>
            )}
            <IkcoGrid size={{ xs: 12, md: 6 }}>
              <IkcoTextField label="شرح قطعه" disabled {...register("partDesc")} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="گروه قطعه" {...register("partGroup", { ...rules.partGroup, required: "گروه قطعه الزامی است" })} error={!!errors.partGroup} helperText={errors.partGroup?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="موجودی گروهی" type="number" disabled={isEdit} {...register("totalStock", { required: !isEdit ? "موجودی را وارد کنید" : false, min: { value: 0, message: "نامعتبر" } })} error={!!errors.totalStock} helperText={errors.totalStock?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="موجودی خام" type="number" {...register("rawStock", { required: "موجودی را وارد کنید", min: { value: 0, message: "نامعتبر" } })} error={!!errors.rawStock} helperText={errors.rawStock?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره فنی جایگزین" {...register("alterNativePartNo", rules.alterNativePartNo)} error={!!errors.alterNativePartNo} helperText={errors.alterNativePartNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات قطعه" {...register("criticalPartNote", rules.criticalPartNote)} error={!!errors.criticalPartNote} helperText={errors.criticalPartNote?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="شماره نامه" {...register("letterNo", rules.letterNo)} error={!!errors.letterNo} helperText={errors.letterNo?.message} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="توضیحات وضعیت تولید" {...register("productionStateNote", rules.productionStateNote)} error={!!errors.productionStateNote} helperText={errors.productionStateNote?.message} />
            </IkcoGrid>
            {!isEdit && (
              <IkcoGrid size={size}>
                <Controller name="isOutOfSAP" control={control} render={({ field }) => <IkcoCheckbox label="خارج از SAP" checked={field.value} onChange={(_, c) => field.onChange(c)} />} />
              </IkcoGrid>
            )}
          </IkcoGrid>
        </IkcoCardContent>
      </IkcoCard>

      {!isEdit && (
        <IkcoGrid container spacing={2}>
          <IkcoGrid size={{ xs: 12, md: 6 }}>
            <IkcoStack spacing={1}>
              <IkcoTypography variant="subtitle1">محصولات مصرف‌کننده شماره فنی (BOM)</IkcoTypography>
              <IkcoDataGrid
                columns={productColumns}
                rows={bom}
                getRowId={(r) => r.rowNo}
                checkboxSelection
                disableRowSelectionOnClick
                rowSelectionModel={bomSel}
                onRowSelectionModelChange={(m) => setBomSel(m as number[])}
                autoHeight
              />
              <IkcoButton variant="outlined" startIcon={<Add />} disabled={bomSel.length === 0} onClick={addProducts}>
                افزودن محصولات انتخاب‌شده
              </IkcoButton>
            </IkcoStack>
          </IkcoGrid>
          <IkcoGrid size={{ xs: 12, md: 6 }}>
            <IkcoStack spacing={1}>
              <IkcoTypography variant="subtitle1">محصولات انتخاب‌شده برای بحران</IkcoTypography>
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
              <IkcoButton variant="outlined" startIcon={<Remove />} disabled={chosenSel.length === 0} onClick={removeProducts}>
                حذف از لیست انتخاب‌شده‌ها
              </IkcoButton>
            </IkcoStack>
          </IkcoGrid>
        </IkcoGrid>
      )}

      {isEdit && existing && (
        <IkcoStack spacing={1}>
          <IkcoTypography variant="subtitle1">محصولات ثبت‌شده بحران</IkcoTypography>
          <IkcoDataGrid columns={existingColumns} rows={existingRows} getRowId={(r) => r.__rowId} autoHeight />
        </IkcoStack>
      )}

      <IkcoCard>
        <IkcoCardActions>
          <PermissionGuard can={isEdit ? ApiPermissions.bodyCriticals.update : ApiPermissions.bodyCriticals.create} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" loading={saving} onClick={handleSubmit(onSubmit)}>
              {isEdit ? "ذخیره ویرایش" : "ثبت بحران"}
            </IkcoButton>
          </PermissionGuard>
          <IkcoButton onClick={() => navigate("body-shop/search")}>جستجوی اقلام بحرانی</IkcoButton>
        </IkcoCardActions>
      </IkcoCard>
    </IkcoStack>
  );
}
