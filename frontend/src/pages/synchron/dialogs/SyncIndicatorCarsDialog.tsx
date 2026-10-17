import {
  IkcoAlert,
  IkcoButton,
  IkcoCardContent as DialogContent,
  IkcoDataGrid,
  IkcoDialog,
  IkcoDialogActions,
  IkcoDialogTitle,
  IkcoGrid,
  IkcoGridActionsCellItem,
  IkcoIconButton,
  IkcoStack,
  IkcoTextField,
  IkcoTooltip,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { useCan } from "@ikco/mfe-core";
import { DirectionsCar, Search } from "@mui/icons-material";
import { useCallback, useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import FormDialog from "../../../components/FormDialog";
import { getMechPartsBodyProperties } from "../../../server/services/mechPartsService";
import { getSyncSetsById, getSyncSetsNotEnteredIndicatorCarsCount, getSyncSetsSaloons, updateSyncSetsSaloonIndicatorCar } from "../../../server/services/syncSetsService";
import type { ITabularResult } from "../../../types/Common";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows, type IGridRow } from "../../../utils/tabularUtility";
import ActionBar from "../../../components/ActionBar";

const PLANT_ID = 1;

const num = (row: IGridRow | null, ...keys: string[]) => {
  const r = (row ?? {}) as Record<string, unknown>;
  for (const k of keys) if (r[k] !== undefined && r[k] !== null) return Number(r[k]);
  return 0;
};

/**
 * تعیین خودروی شاخص سالن‌های یک بحران سنکرون (frmCriticalPartSetIndicatorCars)، modal دکمه «بدنه (های) شاخص» فرم بحران سنکرون؛
 * برای هر سالن شماره بدنه شاخص وارد و پس از یافتن مشخصات بدنه با S_CriticalPartsSetSaloonUpdate ثبت می‌شود
 * (ثبت، بحران را فعال و تعداد بدنه‌های پس از خودروی شاخص را بازمحاسبه می‌کند).
 */
export default function SyncIndicatorCarsDialog({ open, close, setId }: { open: boolean; close: (open: boolean) => void; setId: number }) {
  const canUpdate = useCan(ApiPermissions.syncSets.update);
  const [header, setHeader] = useState<ITabularResult | null>(null);
  const [saloons, setSaloons] = useState<ITabularResult | null>(null);
  const [missing, setMissing] = useState<number | null>(null);
  const [loading, setLoading] = useState(false);
  const [target, setTarget] = useState<IGridRow | null>(null);
  const [bodyNo, setBodyNo] = useState("");
  const [carId, setCarId] = useState(0);
  const [bodyInfo, setBodyInfo] = useState("");
  const [busy, setBusy] = useState(false);

  const load = useCallback(async () => {
    if (!setId) return;
    setLoading(true);
    try {
      setHeader(await getSyncSetsById(setId));
      setSaloons(await getSyncSetsSaloons(setId));
      const count = await getSyncSetsNotEnteredIndicatorCarsCount(setId);
      const first = count.rows[0] as Record<string, unknown> | undefined;
      setMissing(first ? Number(Object.values(first)[0] ?? 0) : null);
    } finally {
      setLoading(false);
    }
  }, [setId]);

  useEffect(() => {
    if (open) void load();
  }, [open, load]);

  const rows = useMemo(() => tabularRows(saloons), [saloons]);
  const baseColumns = useMemo(() => resultColumns(saloons, columnHeaders), [saloons]);
  const headerRow = (header?.rows[0] ?? null) as Record<string, unknown> | null;

  const columns: IkcoGridColDef[] = useMemo(
    () =>
      canUpdate
        ? [
            {
              field: "__actions",
              type: "actions",
              headerName: "تعیین",
              width: 80,
              getActions: (params) => [
                <IkcoGridActionsCellItem key="c" icon={<IkcoTooltip title="تعیین خودروی شاخص"><IkcoIconButton><DirectionsCar color="primary" /></IkcoIconButton></IkcoTooltip>} label="تعیین" onClick={() => { setBodyNo(""); setCarId(0); setBodyInfo(""); setTarget(params.row as IGridRow); }} />,
              ],
            },
            ...baseColumns,
          ]
        : baseColumns,
    [baseColumns, canUpdate],
  );

  const findBody = async () => {
    if (!bodyNo.trim()) return;
    setBusy(true);
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
      setBodyInfo(`${String(row["CarGroupDescf"] ?? "")} - ${String(row["CarModelDesc"] ?? "")} - ${String(row["CarClassDesc"] ?? "")} | رنگ: ${String(row["CarColorDescf"] ?? "")}`);
    } finally {
      setBusy(false);
    }
  };

  const save = async () => {
    if (!target || !carId) return;
    setBusy(true);
    try {
      await updateSyncSetsSaloonIndicatorCar(num(target, "Id", "id"), setId, { saloonId: num(target, "SaloonId"), indicatorCarId: carId });
      toast.success("خودروی شاخص ثبت شد");
      setTarget(null);
      await load();
    } finally {
      setBusy(false);
    }
  };

  return (
    <FormDialog open={open} close={close} title={`تعریف بدنه‌های شاخص موجود در سالن‌های مشترک اقلام بحرانی سنکرون — Id ${setId}`} maxWidth="lg">
    <IkcoStack spacing={2}>
      {headerRow && <IkcoAlert status="info" severity="info">{`ست: ${String(headerRow["SetDescF"] ?? headerRow["DescF"] ?? headerRow["Code"] ?? "-")}`}</IkcoAlert>}
      {missing !== null && missing > 0 && <IkcoAlert status="warning" severity="warning">{`برای ${missing} سالن خودروی شاخص تعیین نشده است؛ تا تعیین آن، محاسبه امکان تولید برای این سالن‌ها انجام نمی‌شود.`}</IkcoAlert>}
      <ActionBar>
          <IkcoButton onClick={() => void load()}>بازخوانی</IkcoButton>
        </ActionBar>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />

      <IkcoDialog open={target !== null} maxWidth="sm" fullWidth onClose={() => setTarget(null)}>
        <IkcoDialogTitle>{`خودروی شاخص سالن ${String((target as Record<string, unknown> | null)?.["SaloonDescF"] ?? "")}`}</IkcoDialogTitle>
        <DialogContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={{ xs: 8 }}>
              <IkcoTextField label="شماره بدنه" value={bodyNo} onChange={(e) => { setBodyNo(e.target.value); setCarId(0); setBodyInfo(""); }} slotProps={{ htmlInput: { maxLength: 8 } }} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 4 }}>
              <IkcoButton startIcon={<Search />} loading={busy} onClick={() => void findBody()}>یافتن</IkcoButton>
            </IkcoGrid>
            {bodyInfo && <IkcoGrid size={12}><IkcoTypography variant="body2">{bodyInfo}</IkcoTypography></IkcoGrid>}
          </IkcoGrid>
        </DialogContent>
        <IkcoDialogActions>
          <IkcoButton variant="contained" disabled={!carId} loading={busy} onClick={() => void save()}>ثبت</IkcoButton>
          <IkcoButton onClick={() => setTarget(null)}>انصراف</IkcoButton>
        </IkcoDialogActions>
      </IkcoDialog>
    </IkcoStack>
    </FormDialog>
  );
}
