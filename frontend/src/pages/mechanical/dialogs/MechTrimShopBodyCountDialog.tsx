import { IkcoDataGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import BodyInfoStrip from "../../../components/BodyInfoStrip";
import ExcelExportButton from "../../../components/ExcelExportButton";
import FormDialog from "../../../components/FormDialog";
import { getMechPartsBodyProperties } from "../../../server/services/mechPartsService";
import { getMechReportsTrimShopBodyCount } from "../../../server/services/mechReportsService";
import type { ITabularResult } from "../../../types/tabular";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../../utils/tabularUtility";

/** شناسه سایت پیش‌فرض رویه‌های SFC (PlantId=1) */
const PLANT_ID = 1;

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  initialBodyNumber?: string;
}

const text = (row: Record<string, unknown> | null, ...keys: string[]) => {
  for (const k of keys) if (row && row[k] !== undefined && row[k] !== null && String(row[k]).trim() !== "") return String(row[k]).trim();
  return "";
};

/** بدنه‌های ورود به مونتاژ شده (frmTrimShopBodyCount): با شماره بدنه، لیست بدنه‌های وارد شده هم‌محصول و مجموع آن‌ها نمایش داده می‌شود */
export default function MechTrimShopBodyCountDialog({ open, close, initialBodyNumber = "" }: Props) {
  const [bodyNumber, setBodyNumber] = useState(initialBodyNumber);
  const [info, setInfo] = useState<Record<string, unknown> | null>(null);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);

  const search = async (value = bodyNumber) => {
    const trimmed = value.trim();
    if (!trimmed) return;
    setLoading(true);
    try {
      const data = await getMechPartsBodyProperties({ bodyNo: trimmed, plantId: PLANT_ID, mode: 1 });
      const row = data.rows[0] as Record<string, unknown> | undefined;
      if (!row) {
        setInfo(null);
        setResult(null);
        toast.error("بدنه یافت نشد");
        return;
      }
      setInfo(row);
      setResult(await getMechReportsTrimShopBodyCount({ carId: Number(row["Id"]) }));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!open) return;
    setBodyNumber(initialBodyNumber);
    setInfo(null);
    setResult(null);
    if (initialBodyNumber.trim()) void search(initialBodyNumber);
    // فقط با باز شدن modal
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [open]);

  const cells = [
    { label: "محصول", value: [text(info, "CarGroupDescf"), text(info, "CarModelDesc"), text(info, "CarClassDesc")].filter(Boolean).join(" - ") },
    { label: "رنگ", value: text(info, "CarColorDescf") },
    { label: "زیرکلاس", value: text(info, "SubClassCode", "SubClassDesc") },
    { label: "شماره موتور", value: text(info, "EngineNO", "EngineNo") },
    { label: "VIN", value: text(info, "VIN", "Vin") },
    { label: "مرحله جاری", value: text(info, "LastStepDescF") },
    { label: "سالن", value: text(info, "LastSaloonDescF") },
  ];

  return (
    <FormDialog
      open={open}
      close={close}
      title="بدنه‌های ورود به مونتاژ شده"
      closeText="خروج"
      actions={<ExcelExportButton fileName="بدنه‌های ورود به مونتاژ شده" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />}
    >
      <IkcoStack spacing={2}>
        <BodyInfoStrip bodyNumber={bodyNumber} onBodyNumberChange={setBodyNumber} onSearch={() => void search()} cells={cells} loading={loading} />
        <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
        <IkcoTypography>{`مجموع: ${rows.length}`}</IkcoTypography>
      </IkcoStack>
    </FormDialog>
  );
}
