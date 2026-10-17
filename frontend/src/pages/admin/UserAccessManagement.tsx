import {
  IkcoAutocomplete,
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoDataGrid,
  IkcoGrid,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan } from "@ikco/mfe-core";
import { useCallback, useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import ConfirmDialog from "../../components/ConfirmDialog";
import ExcelExportButton from "../../components/ExcelExportButton";
import {
  deleteUserAccess,
  getUserAccessAllUsers,
  getUserAccessSaloonsByVirtualPlant,
  getUserAccessSites,
  getUserAccessUsersAccessList,
  insertUserAccess,
} from "../../server/services/userAccessService";
import type { ITabularResult } from "../../types/Common";
import { ApiPermissions } from "../../types/permissions";
import { columnHeaders } from "../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../utils/tabularUtility";
import ActionBar from "../../components/ActionBar";

interface Option {
  id: number;
  label: string;
}

type Row = Record<string, unknown>;

/** مقدار اولین کلیدی که نامش (بدون حساسیت به حروف) در فهرست باشد */
const pick = (row: Row, ...keys: string[]) => {
  const lower = new Map(Object.keys(row).map((k) => [k.toLowerCase(), k]));
  for (const k of keys) {
    const real = lower.get(k.toLowerCase());
    if (real !== undefined && row[real] !== null && row[real] !== undefined) return row[real];
  }
  return undefined;
};

/** نخستین ستونی که نامش با پسوند داده‌شده تمام می‌شود (مثلاً شرح خطوط: ...DescF) */
const endingWith = (row: Row, suffix: string) => {
  const key = Object.keys(row).find((k) => k.toLowerCase().endsWith(suffix.toLowerCase()));
  return key ? row[key] : undefined;
};

const userOption = (r: Row): Option => {
  const name = [pick(r, "FirstName"), pick(r, "LastName")].filter(Boolean).join(" ");
  return { id: Number(pick(r, "Id")), label: `${name || String(pick(r, "FullName") ?? "")} (${String(pick(r, "UserName") ?? "")})` };
};

/**
 * مدیریت دسترسی کاربران به خطوط بدنه‌سازی (frmUserManagement): انتخاب سایت، خطوط و کاربران و ثبت دسترسی (ضرب دکارتی)،
 * فهرست دسترسی‌های ثبت‌شده و حذف گروهی. شناسه کاربر PF.B_User.Id است، نه B_Person.
 */
export default function UserAccessManagement() {
  const canDelete = useCan(ApiPermissions.userAccess.delete);
  const [sites, setSites] = useState<Option[]>([]);
  const [lines, setLines] = useState<Option[]>([]);
  const [users, setUsers] = useState<Option[]>([]);
  const [siteId, setSiteId] = useState<number | null>(null);
  const [lineIds, setLineIds] = useState<Option[]>([]);
  const [userIds, setUserIds] = useState<Option[]>([]);
  const [access, setAccess] = useState<ITabularResult | null>(null);
  const [selection, setSelection] = useState<number[]>([]);
  const [loading, setLoading] = useState(false);
  const [busy, setBusy] = useState(false);
  const [confirm, setConfirm] = useState(false);
  // grpDataEntry.Visible فرم ویندوزی: پنل ثبت فقط پس از «افزودن» نمایان است
  const [adding, setAdding] = useState(false);

  const rows = useMemo(() => tabularRows(access), [access]);
  const columns = useMemo(() => resultColumns(access, columnHeaders), [access]);

  const loadAccess = useCallback(async () => {
    setLoading(true);
    try {
      setAccess(await getUserAccessUsersAccessList());
      setSelection([]);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void (async () => {
      const [s, u] = await Promise.all([getUserAccessSites({}), getUserAccessAllUsers()]);
      setSites((s.rows as Row[]).map((r) => ({ id: Number(pick(r, "VirtualPlantId", "Id")), label: String(pick(r, "VirtualPlantDescF") ?? endingWith(r, "DescF") ?? "") })));
      setUsers((u.rows as Row[]).map(userOption));
    })();
    void loadAccess();
  }, [loadAccess]);

  useEffect(() => {
    setLineIds([]);
    setLines([]);
    if (siteId === null) return;
    void getUserAccessSaloonsByVirtualPlant({ plantId: siteId }).then((r) =>
      setLines((r.rows as Row[]).map((x) => ({ id: Number(pick(x, "BiwSiteId", "Id")), label: String(pick(x, "BIWSiteDescF") ?? endingWith(x, "DescF") ?? "") }))),
    );
  }, [siteId]);

  const save = async () => {
    if (lineIds.length === 0 || userIds.length === 0) {
      toast.error("حداقل یک خط و یک کاربر انتخاب کنید");
      return;
    }
    const items = userIds.flatMap((u) => lineIds.map((l) => ({ userId: u.id, biwSiteId: l.id })));
    if (items.length > 1000) {
      toast.error("حداکثر ۱۰۰۰ دسترسی در هر ثبت مجاز است");
      return;
    }
    setBusy(true);
    try {
      await insertUserAccess({ items });
      toast.success(`${items.length} دسترسی ثبت شد`);
      setUserIds([]);
      setAdding(false);
      await loadAccess();
    } finally {
      setBusy(false);
    }
  };

  const remove = async () => {
    const ids = selection.map((rowId) => Number(pick((rows.find((r) => r.__rowId === rowId) ?? {}) as Row, "Id"))).filter((n) => Number.isFinite(n) && n > 0);
    if (ids.length === 0) return;
    setBusy(true);
    try {
      await deleteUserAccess({ ids });
      toast.success("دسترسی‌های انتخاب‌شده حذف شد");
      setConfirm(false);
      await loadAccess();
    } finally {
      setBusy(false);
    }
  };

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">دسترسی کاربران به خطوط بدنه‌سازی</IkcoTypography>
      {adding && (
      <PermissionGuard can={ApiPermissions.userAccess.create}>
        <IkcoCard>
          <IkcoCardContent>
            <IkcoGrid container spacing={2}>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoAutocomplete<Option>
                  options={sites}
                  value={sites.find((s) => s.id === siteId) ?? null}
                  getOptionLabel={(o) => o.label}
                  isOptionEqualToValue={(a, b) => a.id === b.id}
                  onChange={(_, v) => setSiteId(v?.id ?? null)}
                  renderInput={(params) => <IkcoTextField {...params} label="سایت" />}
                />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoAutocomplete<Option, true>
                  multiple
                  options={lines}
                  value={lineIds}
                  disableCloseOnSelect
                  getOptionLabel={(o) => o.label}
                  isOptionEqualToValue={(a, b) => a.id === b.id}
                  onChange={(_, v) => setLineIds(v)}
                  renderInput={(params) => <IkcoTextField {...params} label="خطوط بدنه‌سازی" />}
                />
              </IkcoGrid>
              <IkcoGrid size={{ xs: 12, md: 4 }}>
                <IkcoAutocomplete<Option, true>
                  multiple
                  options={users}
                  value={userIds}
                  disableCloseOnSelect
                  getOptionLabel={(o) => o.label}
                  isOptionEqualToValue={(a, b) => a.id === b.id}
                  onChange={(_, v) => setUserIds(v)}
                  renderInput={(params) => <IkcoTextField {...params} label="کاربران" />}
                />
              </IkcoGrid>
            </IkcoGrid>
          </IkcoCardContent>
          <IkcoCardActions>
            <IkcoButton variant="contained" loading={busy} onClick={() => void save()}>ثبت دسترسی</IkcoButton>
            <IkcoButton disabled={busy} onClick={() => setAdding(false)}>انصراف</IkcoButton>
          </IkcoCardActions>
        </IkcoCard>
      </PermissionGuard>
      )}

      <ActionBar>
          <PermissionGuard can={ApiPermissions.userAccess.create}>
            <IkcoButton variant="contained" disabled={adding} onClick={() => setAdding(true)}>افزودن</IkcoButton>
          </PermissionGuard>
          <IkcoButton disabled={adding} onClick={() => void loadAccess()}>بازخوانی</IkcoButton>
          {canDelete && (
            <IkcoButton color="error" disabled={adding || selection.length === 0} onClick={() => setConfirm(true)}>
              {`حذف انتخاب‌شده‌ها (${selection.length})`}
            </IkcoButton>
          )}
          <ExcelExportButton fileName="دسترسی کاربران به خطوط" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
        </ActionBar>

      <IkcoDataGrid
        columns={columns}
        rows={rows}
        loading={loading}
        getRowId={(r) => r.__rowId}
        checkboxSelection={canDelete}
        rowSelectionModel={selection}
        onRowSelectionModelChange={(m) => setSelection(m as number[])}
      />

      <ConfirmDialog open={confirm} close={() => setConfirm(false)} title="حذف دسترسی" message={`${selection.length} دسترسی انتخاب‌شده حذف شود؟`} confirmText="حذف" loading={busy} onConfirm={remove} />
    </IkcoStack>
  );
}
