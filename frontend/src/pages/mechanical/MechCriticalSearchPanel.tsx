import {
  IkcoButton,
  IkcoCheckbox,
  IkcoDataGrid,
  IkcoGrid,
  IkcoRadioGroup,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan } from "@ikco/mfe-core";
import { Check, Edit, ExitToApp, History, Logout, PlaylistAddCheck, Search } from "@mui/icons-material";
import { useMemo, useState } from "react";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import LookupMultiSelect from "../../components/LookupMultiSelect";
import SectionPanel from "../../components/SectionPanel";
import TabularDialog from "../../components/TabularDialog";
import { getMechLicensesTrackingList } from "../../server/services/mechLicensesService";
import { deleteMechParts, getMechPartsProducts } from "../../server/services/mechPartsService";
import { searchMechSearchCriticalParts } from "../../server/services/mechSearchService";
import { deleteSyncSets, getSyncSetsProductDetails } from "../../server/services/syncSetsService";
import type { ITabularResult } from "../../types/tabular";
import { mechSearchSearchCriticalPartsInputRules } from "../../types/Mechanical";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../utils/tabularUtility";
import MechLicenseChangeDialog from "./dialogs/MechLicenseChangeDialog";
import PartNumberField from "../../components/PartNumberField";
import ActionBar from "../../components/ActionBar";
import { gridColumns } from "../../utils/gridColumns";

interface Props {
  /** دسته‌بندی اولیه قطعات: -1 همه موارد، 0 مکانیکی، 1 سنکرون */
  initialKind?: number;
  /** تغییر در مشخصات بحران انتخاب‌شده */
  onEdit?: (id: number, kind: number) => void;
  /** تأیید: بازگرداندن بحران انتخاب‌شده به فرم فراخوان */
  onPick?: (id: number, kind: number) => void;
  /** خروج (بستن modal) */
  onExit?: () => void;
}

const num = (row: IGridRow | undefined, ...keys: string[]) => {
  const r = (row ?? {}) as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return Number(r[k]);
  return 0;
};

/**
 * فرم جستجوی اقلام بحرانی (frmCriticalPartSearch، رویه S_CriticalPartsSearchByConditions): فیلتر شماره فنی، کد ست، سالن، دسته‌بندی قطعات
 * (مکانیکی/سنکرون) و وضعیت؛ جدول مشخصات اقلام بحرانی، جدول محصولات بحران انتخاب‌شده و دکمه‌های «خروج از لیست اقلام بحرانی»،
 * «تغییر در مشخصات اقلام بحرانی»، «تغییر در شرایط تولید» (modal) و Log (modal).
 */
export default function MechCriticalSearchPanel({ initialKind = -1, onEdit, onPick, onExit }: Props) {
  const canUpdate = useCan(ApiPermissions.mechParts.update);
  const canDelete = useCan(ApiPermissions.mechParts.delete);
  const canChange = useCan(ApiPermissions.mechLicenses.change);

  const [partNo, setPartNo] = useState("");
  const [setCode, setSetCode] = useState("");
  const [allSaloons, setAllSaloons] = useState(true);
  const [trimShopIds, setTrimShopIds] = useState<number[]>([]);
  const [kind, setKind] = useState(String(initialKind));
  const [status, setStatus] = useState("1");

  const [result, setResult] = useState<ITabularResult | null>(null);
  const [products, setProducts] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);
  const [active, setActive] = useState<{ id: number; kind: number } | null>(null);
  const [dialog, setDialog] = useState<"license" | "log" | "delete" | null>(null);
  const [deleting, setDeleting] = useState(false);

  const rows = useMemo(() => tabularRows(result), [result]);
  const columns = useMemo(() => resultColumns(result, columnHeaders, gridColumns.mechCriticalSearch), [result]);
  const productColumns = useMemo(() => resultColumns(products, columnHeaders, active?.kind === 1 ? gridColumns.syncSetProducts : gridColumns.mechCriticalProducts), [products, active]);
  const productRows = useMemo(() => tabularRows(products), [products]);

  // GrvParts_RowsChanged: دکمه‌های تغییر/حذف/ردیابی فقط وقتی جدول ردیف دارد فعالند
  const hasRows = rows.length > 0;

  const run = async () => {
    if (partNo.length > 30 || setCode.length > 30) {
      toast.error(mechSearchSearchCriticalPartsInputRules.partNo?.maxLength?.message ?? "طول مقدار نامعتبر است");
      return;
    }
    setLoading(true);
    try {
      const data = await searchMechSearchCriticalParts({
        partNo: partNo.trim() || null,
        setCode: setCode.trim() || null,
        fromDate: null,
        toDate: null,
        trimShopIds: !allSaloons && trimShopIds.length > 0 ? trimShopIds.join(",") : "-1",
        status: Number(status),
        criticalPartTypeId: setCode.trim() ? 1 : Number(kind),
      });
      setActive(null);
      setProducts(null);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const selectRow = async (row: IGridRow) => {
    const id = num(row, "id", "Id");
    const rowKind = num(row, "CriticalPartTypeId");
    setActive({ id, kind: rowKind });
    setProducts(rowKind === 1 ? await getSyncSetsProductDetails(id) : await getMechPartsProducts(id));
  };

  const requireActive = (action: (a: { id: number; kind: number }) => void) => {
    if (!active) {
      toast("ابتدا یک بحران را از جدول انتخاب کنید");
      return;
    }
    action(active);
  };

  const confirmDelete = async () => {
    if (!active) return;
    setDeleting(true);
    try {
      if (active.kind === 1) await deleteSyncSets(active.id);
      else await deleteMechParts(active.id);
      toast.success("حذف انجام شد");
      setDialog(null);
      await run();
    } finally {
      setDeleting(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <SectionPanel title="شرایط جستجو">
          <IkcoGrid container spacing={2} alignItems="center">
            <PartNumberField value={partNo} onChange={setPartNo} descLabel="شرح" maxLength={30} size={{ xs: 12, md: 3 }} descSize={{ xs: 12, md: 3 }} />
            <IkcoGrid size={{ xs: 12, md: 3 }}>
              <IkcoTextField label="کد ست" value={setCode} onChange={(e) => setSetCode(e.target.value)} slotProps={{ htmlInput: { maxLength: 30 } }} placeholder="فقط سنکرون" />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 3 }}>
              <IkcoStack direction="row" spacing={1} alignItems="center">
                <IkcoCheckbox label="همه موارد" checked={allSaloons} onChange={(_, c) => setAllSaloons(c)} />
                {!allSaloons && <LookupMultiSelect lookupKey="trim-shops" label="سالن" value={trimShopIds} onChange={setTrimShopIds} />}
              </IkcoStack>
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 4 }}>
              <IkcoTypography variant="subtitle2">دسته بندی قطعات</IkcoTypography>
              <IkcoRadioGroup row value={kind} onChange={(_, v) => setKind(v)} options={[{ value: "-1", label: "همه موارد" }, { value: "0", label: "مکانیکی" }, { value: "1", label: "سنکرون" }]} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 5 }}>
              <IkcoTypography variant="subtitle2">درحال تکمیل / فعال / غیرفعال</IkcoTypography>
              <IkcoRadioGroup row value={status} onChange={(_, v) => setStatus(v)} options={[{ value: "0", label: "درحال تکمیل (فقط سنکرون)" }, { value: "1", label: "فعال" }, { value: "2", label: "غیرفعال" }]} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 3 }}>
              <PermissionGuard can={ApiPermissions.mechSearch.search} fallback={<IkcoTypography color="error">شما به جستجو دسترسی ندارید.</IkcoTypography>}>
                <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={() => void run()}>
                  جستجو
                </IkcoButton>
              </PermissionGuard>
            </IkcoGrid>
          </IkcoGrid>
      </SectionPanel>

      <IkcoTypography variant="subtitle1">مشخصات اقلام بحرانی</IkcoTypography>
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} onRowClick={(p) => void selectRow(p.row as IGridRow)} autoHeight />
      <IkcoTypography variant="subtitle1">محصولات</IkcoTypography>
      <IkcoDataGrid columns={productColumns} rows={productRows} getRowId={(r) => r.__rowId} autoHeight />

      <ActionBar>
          {canDelete && (
            <IkcoButton color="error" variant="outlined" startIcon={<Logout />} disabled={!hasRows} onClick={() => requireActive(() => setDialog("delete"))}>
              خروج از لیست اقلام بحرانی
            </IkcoButton>
          )}
          {canUpdate && onEdit && (
            <IkcoButton variant="outlined" startIcon={<Edit />} disabled={!hasRows} onClick={() => requireActive((a) => onEdit(a.id, a.kind))}>
              تغییر در مشخصات اقلام بحرانی
            </IkcoButton>
          )}
          {canChange && (
            <IkcoButton variant="outlined" startIcon={<PlaylistAddCheck />} disabled={!hasRows} onClick={() => requireActive(() => setDialog("license"))}>
              تغییر در شرایط تولید
            </IkcoButton>
          )}
          <IkcoButton variant="outlined" startIcon={<History />} disabled={!hasRows} onClick={() => requireActive(() => setDialog("log"))}>
            Log
          </IkcoButton>
          {onPick && (
            <IkcoButton variant="contained" startIcon={<Check />} disabled={!hasRows} onClick={() => requireActive((a) => onPick(a.id, a.kind))}>
              تایید
            </IkcoButton>
          )}
          {onExit && (
            <IkcoButton startIcon={<ExitToApp />} onClick={onExit}>
              خروج
            </IkcoButton>
          )}
          <IkcoTypography variant="caption">{result ? `تعداد: ${rows.length}` : "فیلترها را وارد و «جستجو» را انتخاب کنید"}</IkcoTypography>
        </ActionBar>

      {dialog === "license" && active && <MechLicenseChangeDialog open close={() => setDialog(null)} criticalPartTypeId={active.kind} criticalPartId={active.id} onSaved={() => void run()} />}
      <TabularDialog open={dialog === "log"} close={() => setDialog(null)} title="Log" columns={gridColumns.mechLicenseTrackingList} load={() => getMechLicensesTrackingList({ criticalPartTypeId: active?.kind ?? 0, criticalPartId: active?.id ?? 0 })} />
      <ConfirmDialog
        open={dialog === "delete"}
        close={() => setDialog(null)}
        title="خروج از لیست اقلام بحرانی"
        message="بحران انتخاب‌شده حذف شود؟ این عملیات قابل بازگشت نیست."
        confirmText="حذف"
        loading={deleting}
        onConfirm={confirmDelete}
      />
    </IkcoStack>
  );
}
