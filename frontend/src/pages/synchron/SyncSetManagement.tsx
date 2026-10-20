import {
  IkcoAlert,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoDataGrid,
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
import { Add, AddCircle, Cancel, Check, DirectionsCar, Delete, History, PlaylistAddCheck, Search } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import LookupAutocomplete from "../../components/LookupAutocomplete";
import LookupMultiSelect from "../../components/LookupMultiSelect";
import TabularDialog from "../../components/TabularDialog";
import { useQuery } from "../../hooks/useQueryString";
import { getMechLicensesTrackingList } from "../../server/services/mechLicensesService";
import { getMechPartsPartDescription } from "../../server/services/mechPartsService";
import { getSyncBasisByStatus } from "../../server/services/syncBasisService";
import {
  addSyncSetsProduct,
  createSyncSetsHeader,
  deleteSyncSets,
  getSyncSetsById,
  getSyncSetsColors,
  getSyncSetsPartsWithShortageList,
  getSyncSetsProductDetails,
  getSyncSetsSaloons,
  saveSyncSetsComposition,
  updateSyncSetsHeader,
} from "../../server/services/syncSetsService";
import type { ITabularResult } from "../../types/tabular";
import type { ISyncSetPartItem, ISyncSetProductItem } from "../../types/Synchron";
import { syncSetsCreateHeaderInputRules } from "../../types/Synchron";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../utils/tabularUtility";
import MechCriticalSearchDialog from "../mechanical/dialogs/MechCriticalSearchDialog";
import MechLicenseChangeDialog from "../mechanical/dialogs/MechLicenseChangeDialog";
import SyncIndicatorCarsDialog from "./dialogs/SyncIndicatorCarsDialog";
import ActionBar from "../../components/ActionBar";
import { gridColumns, type IGridColumn } from "../../utils/gridColumns";

const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };
const ALL = -1;

interface ProductDraft {
  groupId: number | null;
  modelId: number | null;
  classId: number | null;
  subClassId: number | null;
  trimId: number | null;
  toodooziId: number | null;
  optionId: number | null;
  colorId: number | null;
}

const emptyDraft: ProductDraft = { groupId: null, modelId: null, classId: null, subClassId: null, trimId: null, toodooziId: null, optionId: null, colorId: null };

const flag = (row: Record<string, unknown> | undefined, key: string) => {
  const v = row?.[key];
  return v === true || v === 1 || v === "1" || String(v).toLowerCase() === "true";
};

const Grid = ({ result, title, spec }: { result: ITabularResult | null; title: string; spec: readonly IGridColumn[] }) => {
  const columns = useMemo(() => resultColumns(result, columnHeaders, spec), [result, spec]);
  const rows = useMemo(() => tabularRows(result), [result]);
  return (
    <IkcoStack spacing={1}>
      <IkcoTypography variant="subtitle1">{title}</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} getRowId={(r) => r.__rowId} autoHeight />
    </IkcoStack>
  );
};

/**
 * مدیریت بحران‌های سنکرون (frmCriticalPartSet). ثبت: سرآیند (مبنای ست، واحد، رنگ/تریم/تودوزی/آپشن، موجودی) و سپس ترکیب ست
 * (قطعات، رنگ‌ها، سالن‌ها و محصولات) در یک تراکنش سمت سرور. اگر ثبت ترکیب شکست بخورد، سرآیند تازه‌ساخته حذف منطقی می‌شود.
 * ویرایش (?id=): تغییر واحد/موجودی/توضیحات/وضعیت، افزودن محصول و مشاهده ترکیب ثبت‌شده.
 */
interface Props {
  /** شناسه بحران سنکرون برای ویرایش؛ در غیر این صورت از پارامتر ?id= آدرس خوانده می‌شود */
  editId?: number;
  /** پس از ذخیره/حذف (هنگامی که فرم داخل modal باز شده است) */
  onDone?: () => void;
}

export default function SyncSetManagement({ editId: editIdProp, onDone }: Props = {}) {
  const query = useQuery();
  const [pickedId, setPickedId] = useState<number | null>(null);
  const editId = pickedId ?? editIdProp ?? Number(query.get("id") ?? 0);
  const isEdit = editId > 0;
  const [dialog, setDialog] = useState<"search" | "indicator" | "license" | "log" | null>(null);
  const canDelete = useCan(ApiPermissions.syncSets.delete);
  const canIndicator = useCan(ApiPermissions.syncSets.update);

  // سرآیند
  const [basisRows, setBasisRows] = useState<Record<string, unknown>[]>([]);
  const [basisId, setBasisId] = useState<number | null>(null);
  const [deptId, setDeptId] = useState<number | null>(null);
  const [colorCode, setColorCode] = useState("");
  const [colorId, setColorId] = useState<number | null>(null);
  const [trimId, setTrimId] = useState<number | null>(null);
  const [toodooziId, setToodooziId] = useState<number | null>(null);
  const [optionId, setOptionId] = useState<number | null>(null);
  const [stock, setStock] = useState("0");
  const [note, setNote] = useState("");
  const [status, setStatus] = useState<number | null>(1);

  // ترکیب
  const [parts, setParts] = useState<ISyncSetPartItem[]>([]);
  const [partNo, setPartNo] = useState("");
  const [partDesc, setPartDesc] = useState<Record<string, string>>({});
  const [colorIds, setColorIds] = useState<number[]>([]);
  const [saloonIds, setSaloonIds] = useState<number[]>([]);
  const [draft, setDraft] = useState<ProductDraft>(emptyDraft);
  const [products, setProducts] = useState<ISyncSetProductItem[]>([]);

  // نمایش ثبت‌شده در ویرایش
  const [savedParts, setSavedParts] = useState<ITabularResult | null>(null);
  const [savedColors, setSavedColors] = useState<ITabularResult | null>(null);
  const [savedSaloons, setSavedSaloons] = useState<ITabularResult | null>(null);
  const [savedProducts, setSavedProducts] = useState<ITabularResult | null>(null);

  const [busy, setBusy] = useState(false);
  const [confirmDelete, setConfirmDelete] = useState(false);
  const [showProducts, setShowProducts] = useState(false);

  const basis = basisRows.find((r) => Number(r["Id"] ?? r["id"]) === basisId);
  const needsColor = flag(basis, "IsColor");
  const needsTrim = flag(basis, "IsTrim");
  const needsToodoozi = flag(basis, "IsToodoozi");
  const needsOption = flag(basis, "IsOption");

  useEffect(() => {
    void getSyncBasisByStatus({ status: 1 }).then((r) => setBasisRows(r.rows as Record<string, unknown>[]));
  }, []);

  const loadSaved = useCallback(async () => {
    if (!isEdit) return;
    const header = (await getSyncSetsById(editId)).rows[0] as Record<string, unknown> | undefined;
    if (header) {
      setBasisId(header["SetId"] === undefined ? null : Number(header["SetId"]));
      setDeptId(header["DeptId"] === undefined ? null : Number(header["DeptId"]));
      setStock(String(header["Stock"] ?? "0"));
      setNote(String(header["Note"] ?? ""));
      setStatus(header["Status"] === undefined || header["Status"] === null ? 1 : Number(header["Status"]) || 1);
      setColorCode(String(header["ProductsColorCode"] ?? ""));
    }
    setSavedParts(await getSyncSetsPartsWithShortageList(editId));
    setSavedColors(await getSyncSetsColors(editId));
    setSavedSaloons(await getSyncSetsSaloons(editId));
    setSavedProducts(await getSyncSetsProductDetails(editId));
  }, [editId, isEdit]);

  useEffect(() => {
    void loadSaved();
  }, [loadSaved]);

  const addPart = async () => {
    const no = partNo.trim();
    if (!no) return;
    if (parts.some((p) => p.partNo === no)) {
      toast.error("این شماره فنی قبلاً اضافه شده است");
      return;
    }
    setBusy(true);
    try {
      const desc = await getMechPartsPartDescription({ partNo: no });
      if (!desc) {
        toast.error("شماره فنی در سامانه یافت نشد");
        return;
      }
      setParts((p) => [...p, { shortagePartListId: 0, partNo: no }]);
      setPartDesc((d) => ({ ...d, [no]: String(desc) }));
      setPartNo("");
    } finally {
      setBusy(false);
    }
  };

  const toItem = (d: ProductDraft, saloonId: number): ISyncSetProductItem => ({
    carGroupId: d.groupId ?? 0,
    carModelId: d.modelId ?? 0,
    carClassId: d.classId ?? 0,
    carSubClassId: d.subClassId ?? 0,
    carTrimId: d.trimId ?? ALL,
    carToodooziId: d.toodooziId ?? ALL,
    carOptionId: d.optionId ?? ALL,
    carColorId: d.colorId ?? ALL,
    saloonId,
  });

  const draftValid = !!draft.groupId && !!draft.modelId && !!draft.classId && !!draft.subClassId;

  const addDraftProducts = async () => {
    if (!draftValid) {
      toast.error("خانواده، مدل، کلاس و زیرکلاس را انتخاب کنید");
      return;
    }
    if (saloonIds.length === 0) {
      toast.error("حداقل یک سالن انتخاب کنید");
      return;
    }
    const items = saloonIds.map((s) => toItem(draft, s));
    if (isEdit) {
      setBusy(true);
      try {
        for (const item of items) await addSyncSetsProduct(editId, item);
        toast.success("محصول به بحران افزوده شد");
        setSavedProducts(await getSyncSetsProductDetails(editId));
      } finally {
        setBusy(false);
      }
    } else {
      setProducts((p) => [...p, ...items]);
    }
    setDraft(emptyDraft);
  };

  const productColumns: IkcoGridColDef[] = [
    { field: "carSubClassId", headerName: "زیرکلاس", width: 100 },
    { field: "carClassId", headerName: "کلاس", width: 90 },
    { field: "carModelId", headerName: "مدل", width: 90 },
    { field: "carGroupId", headerName: "خانواده", width: 90 },
    { field: "carTrimId", headerName: "تریم", width: 90 },
    { field: "carToodooziId", headerName: "تودوزی", width: 90 },
    { field: "carOptionId", headerName: "آپشن", width: 90 },
    { field: "carColorId", headerName: "رنگ", width: 90 },
    { field: "saloonId", headerName: "سالن", width: 90 },
    {
      field: "__actions",
      type: "actions",
      headerName: "حذف",
      width: 70,
      getActions: (params) => [
        <IkcoGridActionsCellItem key="d" icon={<IkcoTooltip title="حذف از فهرست"><IkcoIconButton><Delete color="error" /></IkcoIconButton></IkcoTooltip>} label="حذف" onClick={() => setProducts((p) => p.filter((_, i) => i !== Number(params.id)))} />,
      ],
    },
  ];

  const clearForm = () => {
    setPickedId(editIdProp ? null : 0);
    setBasisId(null);
    setDeptId(null);
    setColorCode("");
    setColorId(null);
    setTrimId(null);
    setToodooziId(null);
    setOptionId(null);
    setStock("0");
    setNote("");
    setStatus(1);
    setParts([]);
    setPartNo("");
    setPartDesc({});
    setColorIds([]);
    setSaloonIds([]);
    setDraft(emptyDraft);
    setProducts([]);
    setSavedParts(null);
    setSavedColors(null);
    setSavedSaloons(null);
    setSavedProducts(null);
  };

  const create = async () => {
    if (!basisId || !deptId) {
      toast.error("مبنای ست و واحد مسئول را انتخاب کنید");
      return;
    }
    const rule = syncSetsCreateHeaderInputRules.productsColorCode;
    if (colorCode.trim().length === 0 || colorCode.trim().length > 5) {
      toast.error(typeof rule === "object" ? "کد رنگ محصول (حداکثر ۵ کاراکتر) الزامی است" : "کد رنگ محصول نامعتبر است");
      return;
    }
    if (needsColor && !colorId) return void toast.error("رنگ را انتخاب کنید");
    if (needsTrim && !trimId) return void toast.error("تریم را انتخاب کنید");
    if (needsToodoozi && !toodooziId) return void toast.error("تودوزی را انتخاب کنید");
    if (needsOption && !optionId) return void toast.error("آپشن را انتخاب کنید");
    if (parts.length === 0 || saloonIds.length === 0 || products.length === 0) {
      toast.error("حداقل یک قطعه، یک سالن و یک محصول لازم است");
      return;
    }
    setBusy(true);
    let id = 0;
    try {
      // S_CriticalPartsSetInsert_Web: شناسه بحران سنکرون جدید در Result برمی‌گردد
      const header = await createSyncSetsHeader({
        setId: basisId,
        deptId,
        productsColorCode: colorCode.trim(),
        productsColorId: colorId ?? ALL,
        productsTrimId: trimId ?? ALL,
        productsToodooziId: toodooziId ?? ALL,
        productsOptionId: optionId ?? ALL,
        stock: Number(stock) || 0,
        note: note.trim() || null,
      });
      id = header.result;
      await saveSyncSetsComposition(id, { parts, colorIds, saloonIds, products });
      toast.success("بحران سنکرون ثبت شد");
      setPickedId(id);
      onDone?.();
    } catch (e) {
      if (id > 0) {
        try {
          await deleteSyncSets(id);
        } catch {
          // حذف جبرانی ناموفق؛ بحران ناقص در جستجو با وضعیت «در حال تکمیل» دیده می‌شود
        }
      }
      throw e;
    } finally {
      setBusy(false);
    }
  };

  const update = async () => {
    setBusy(true);
    try {
      await updateSyncSetsHeader(editId, { deptId: deptId ?? 0, stock: Number(stock) || 0, note: note.trim() || null, status: status ?? 1 });
      toast.success("تغییرات ذخیره شد");
      await loadSaved();
    } finally {
      setBusy(false);
    }
  };

  const remove = async () => {
    setBusy(true);
    try {
      await deleteSyncSets(editId);
      toast.success("بحران حذف شد");
      setConfirmDelete(false);
      clearForm();
      onDone?.();
    } finally {
      setBusy(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">{isEdit ? `بحران سنکرون شماره ${editId}` : "ثبت بحران سنکرون جدید"}</IkcoTypography>

      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={size}>
              <LookupAutocomplete lookupKey="depts" label="واحد مسئول" value={deptId} onChange={setDeptId} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField select label="مبنای ست" value={basisId ?? ""} disabled={isEdit} onChange={(e) => setBasisId(Number(e.target.value))} slotProps={{ select: { native: true }, inputLabel: { shrink: true } }}>
                <option value="" />
                {basisRows.map((r) => (
                  <option key={String(r["Id"])} value={String(r["Id"])}>{`${String(r["Code"] ?? "")} - ${String(r["DescF"] ?? "")}`}</option>
                ))}
              </IkcoTextField>
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="کد رنگ محصول" value={colorCode} disabled={isEdit} onChange={(e) => setColorCode(e.target.value)} slotProps={{ htmlInput: { maxLength: 5 } }} />
            </IkcoGrid>
            <IkcoGrid size={size}>
              <IkcoTextField label="موجودی" type="number" value={stock} onChange={(e) => setStock(e.target.value)} />
            </IkcoGrid>
            {!isEdit && needsColor && <IkcoGrid size={size}><LookupAutocomplete lookupKey="colors" label="رنگ" value={colorId} onChange={setColorId} /></IkcoGrid>}
            {!isEdit && needsTrim && <IkcoGrid size={size}><LookupAutocomplete lookupKey="trims" label="تریم" value={trimId} onChange={setTrimId} /></IkcoGrid>}
            {!isEdit && needsToodoozi && <IkcoGrid size={size}><LookupAutocomplete lookupKey="toodoozis" label="تودوزی" value={toodooziId} onChange={setToodooziId} /></IkcoGrid>}
            {!isEdit && needsOption && <IkcoGrid size={size}><LookupAutocomplete lookupKey="options" label="آپشن" value={optionId} onChange={setOptionId} /></IkcoGrid>}
            <IkcoGrid size={{ xs: 12, sm: 12, md: 8 }}>
              <IkcoTextField label="توضیحات" value={note} onChange={(e) => setNote(e.target.value)} slotProps={{ htmlInput: { maxLength: 100 } }} />
            </IkcoGrid>
            {isEdit && (
              <IkcoGrid size={size}>
                <LookupAutocomplete lookupKey="active-states" label="وضعیت" value={status} onChange={setStatus} />
              </IkcoGrid>
            )}
          </IkcoGrid>
        </IkcoCardContent>
        {isEdit && (
          <IkcoCardActions>
            <PermissionGuard can={ApiPermissions.syncSets.update}>
              <IkcoButton variant="contained" loading={busy} onClick={() => void update()}>ذخیره سرآیند</IkcoButton>
            </PermissionGuard>
            {canDelete && <IkcoButton color="error" onClick={() => setConfirmDelete(true)}>حذف بحران</IkcoButton>}
          </IkcoCardActions>
        )}
      </IkcoCard>

      {!isEdit && (
        <IkcoCard>
          <IkcoCardContent>
            <IkcoStack spacing={2}>
              <IkcoTypography variant="subtitle1">قطعات ست</IkcoTypography>
              <IkcoGrid container spacing={1} alignItems="center">
                <IkcoGrid size={{ xs: 12, sm: 6, md: 4 }}>
                  <IkcoTextField label="شماره فنی" value={partNo} onChange={(e) => setPartNo(e.target.value)} slotProps={{ htmlInput: { maxLength: 20 } }} />
                </IkcoGrid>
                <IkcoGrid size="auto">
                  <IkcoButton variant="outlined" startIcon={<Add />} loading={busy} onClick={() => void addPart()}>افزودن قطعه</IkcoButton>
                </IkcoGrid>
              </IkcoGrid>
              {parts.map((p) => (
                <IkcoStack key={p.partNo} direction="row" spacing={1} alignItems="center">
                  <IkcoTypography variant="body2">{`${p.partNo} — ${partDesc[p.partNo] ?? ""}`}</IkcoTypography>
                  <IkcoIconButton onClick={() => setParts((x) => x.filter((i) => i.partNo !== p.partNo))}><Delete color="error" /></IkcoIconButton>
                </IkcoStack>
              ))}
              <IkcoGrid container spacing={2}>
                <IkcoGrid size={{ xs: 12, md: 6 }}>
                  <LookupMultiSelect lookupKey="colors" label="رنگ‌های ست (در صورت تفکیک رنگ)" value={colorIds} onChange={setColorIds} />
                </IkcoGrid>
                <IkcoGrid size={{ xs: 12, md: 6 }}>
                  <LookupMultiSelect lookupKey="trim-shops" label="سالن‌های ست" value={saloonIds} onChange={setSaloonIds} />
                </IkcoGrid>
              </IkcoGrid>
            </IkcoStack>
          </IkcoCardContent>
        </IkcoCard>
      )}

      <PermissionGuard can={isEdit ? ApiPermissions.syncSets.update : ApiPermissions.syncSets.create}>
        <IkcoCard>
          <IkcoCardContent>
            <IkcoStack spacing={2}>
              <IkcoTypography variant="subtitle1">{isEdit ? "افزودن محصول به بحران" : "محصولات ست"}</IkcoTypography>
              {isEdit && (
                <IkcoGrid container spacing={2}>
                  <IkcoGrid size={{ xs: 12, md: 6 }}>
                    <LookupMultiSelect lookupKey="trim-shops" label="سالن‌های محصول" value={saloonIds} onChange={setSaloonIds} />
                  </IkcoGrid>
                </IkcoGrid>
              )}
              <IkcoGrid container spacing={2}>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="car-groups" label="خانواده" value={draft.groupId} onChange={(v) => setDraft({ ...draft, groupId: v, modelId: null, classId: null, subClassId: null })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="car-models" label="مدل" value={draft.modelId} parentId={draft.groupId} requireParent onChange={(v) => setDraft({ ...draft, modelId: v, classId: null, subClassId: null })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="car-classes" label="کلاس" value={draft.classId} parentId={draft.modelId} requireParent onChange={(v) => setDraft({ ...draft, classId: v, subClassId: null })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="sub-classes" label="زیرکلاس" value={draft.subClassId} parentId={draft.classId} requireParent onChange={(v) => setDraft({ ...draft, subClassId: v })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="trims" label="تریم" value={draft.trimId} helperText="خالی = همه موارد" onChange={(v) => setDraft({ ...draft, trimId: v })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="toodoozis" label="تودوزی" value={draft.toodooziId} helperText="خالی = همه موارد" onChange={(v) => setDraft({ ...draft, toodooziId: v })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="options" label="آپشن" value={draft.optionId} helperText="خالی = همه موارد" onChange={(v) => setDraft({ ...draft, optionId: v })} /></IkcoGrid>
                <IkcoGrid size={size}><LookupAutocomplete lookupKey="colors" label="رنگ" value={draft.colorId} helperText="خالی = همه موارد" onChange={(v) => setDraft({ ...draft, colorId: v })} /></IkcoGrid>
              </IkcoGrid>
              <IkcoStack direction="row" spacing={1}>
                <IkcoButton startIcon={<Add />} loading={busy && isEdit} onClick={() => void addDraftProducts()}>{isEdit ? "افزودن به بحران" : "افزودن به فهرست (برای هر سالن)"}</IkcoButton>
              </IkcoStack>
              {!isEdit && <IkcoDataGrid columns={productColumns} rows={products.map((p, i) => ({ ...p, id: i }))} getRowId={(r) => r.id} autoHeight />}
            </IkcoStack>
          </IkcoCardContent>
          {!isEdit && (
            <IkcoCardActions>
              <IkcoButton variant="contained" loading={busy} onClick={() => void create()}>ثبت بحران سنکرون</IkcoButton>
            </IkcoCardActions>
          )}
        </IkcoCard>
      </PermissionGuard>

      {isEdit && (
        <IkcoStack spacing={2}>
          {savedParts && savedParts.rows.length === 0 && <IkcoAlert status="warning" severity="warning">برای این بحران قطعه‌ای ثبت نشده است.</IkcoAlert>}
          <Grid result={savedParts} title="قطعات" spec={gridColumns.syncSetParts} />
          <Grid result={savedColors} title="رنگ‌ها" spec={gridColumns.syncSetColors} />
          <Grid result={savedSaloons} title="سالن‌ها" spec={gridColumns.syncSetSaloons} />
          <Grid result={savedProducts} title="محصولات" spec={gridColumns.syncSetProducts} />
          <IkcoButton onClick={() => setShowProducts(true)}>مشاهده محصولات در پنجره</IkcoButton>
        </IkcoStack>
      )}

      <ActionBar>
          <IkcoButton startIcon={<AddCircle />} onClick={clearForm}>جدید</IkcoButton>
          <PermissionGuard can={isEdit ? ApiPermissions.syncSets.update : ApiPermissions.syncSets.create}>
            <IkcoButton variant="contained" startIcon={<Check />} loading={busy} onClick={() => void (isEdit ? update() : create())}>ثبت</IkcoButton>
          </PermissionGuard>
          <IkcoButton startIcon={<Cancel />} onClick={clearForm}>انصراف</IkcoButton>
          {!editIdProp && <IkcoButton startIcon={<Search />} onClick={() => setDialog("search")}>جستجو</IkcoButton>}
          {canIndicator && <IkcoButton startIcon={<PlaylistAddCheck />} disabled={!isEdit} onClick={() => setDialog("license")}>تغییر در شرایط تولید</IkcoButton>}
          <IkcoButton startIcon={<History />} disabled={!isEdit} onClick={() => setDialog("log")}>Log</IkcoButton>
          {canIndicator && <IkcoButton startIcon={<DirectionsCar />} disabled={!isEdit} onClick={() => setDialog("indicator")}>بدنه (های) شاخص</IkcoButton>}
        </ActionBar>

      <MechCriticalSearchDialog
        open={dialog === "search"}
        close={() => setDialog(null)}
        kind={1}
        onPick={(id) => {
          setPickedId(id);
          setDialog(null);
        }}
      />
      <SyncIndicatorCarsDialog open={dialog === "indicator"} close={() => setDialog(null)} setId={editId} />
      <MechLicenseChangeDialog open={dialog === "license"} close={() => setDialog(null)} criticalPartTypeId={1} criticalPartId={editId} />
      <TabularDialog open={dialog === "log"} close={() => setDialog(null)} title="Log" columns={gridColumns.mechLicenseTrackingList} load={() => getMechLicensesTrackingList({ criticalPartTypeId: 1, criticalPartId: editId })} />
      <TabularDialog open={showProducts} close={() => setShowProducts(false)} title="محصولات بحران سنکرون" columns={gridColumns.syncSetProducts} load={() => getSyncSetsProductDetails(editId)} />
      <ConfirmDialog open={confirmDelete} close={() => setConfirmDelete(false)} title="حذف بحران" message="این بحران سنکرون حذف منطقی شود؟" confirmText="حذف" loading={busy} onConfirm={remove} />
    </IkcoStack>
  );
}
