import { IkcoDataGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import BodyInfoStrip, { bodyInfoCells } from "../../../components/BodyInfoStrip";
import ExcelExportButton from "../../../components/ExcelExportButton";
import FormDialog from "../../../components/FormDialog";
import { getBodyCriticalsBodyInfo } from "../../../server/services/bodyCriticalsService";
import { getBodyReportsPreAssignBodyCounts } from "../../../server/services/bodyReportsService";
import type { IBodyInfoRow } from "../../../types/BodyShop";
import type { ITabularResult } from "../../../types/Common";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../../utils/tabularUtility";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** شماره بدنه‌ای که فرم مبدأ در اختیار دارد؛ با باز شدن modal جستجو می‌شود */
  initialBodyNumber?: string;
}

/** تعداد بدنه‌های پیش‌تخصیص‌شده (frmRPTPreassignBodyCount): با شماره بدنه، تعداد بدنه‌های پیش‌تخصیص هر محصول نمایش داده می‌شود */
export default function BodyPreAssignCountDialog({ open, close, initialBodyNumber = "" }: Props) {
  const [bodyNumber, setBodyNumber] = useState(initialBodyNumber);
  const [info, setInfo] = useState<IBodyInfoRow | null>(null);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);

  const search = async (value = bodyNumber) => {
    const trimmed = value.trim();
    if (!trimmed) return;
    setLoading(true);
    try {
      const body = await getBodyCriticalsBodyInfo({ bodyNumber: trimmed });
      if (!body || typeof body !== "object") {
        setInfo(null);
        setResult(null);
        toast.error("بدنه یافت نشد");
        return;
      }
      setInfo(body);
      setResult(await getBodyReportsPreAssignBodyCounts({ carId: body.carId }));
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

  return (
    <FormDialog
      open={open}
      close={close}
      title="تعداد بدنه‌های پیش‌تخصیص‌شده"
      maxWidth="lg"
      closeText="بازگشت"
      actions={<ExcelExportButton fileName="تعداد بدنه‌های پیش‌تخصیص‌شده" columns={columns} rows={rows as unknown as Record<string, unknown>[]} />}
    >
      <IkcoStack spacing={2}>
        <BodyInfoStrip bodyNumber={bodyNumber} onBodyNumberChange={setBodyNumber} onSearch={() => void search()} cells={bodyInfoCells(info)} loading={loading} />
        <IkcoTypography variant="subtitle1">نمایش</IkcoTypography>
        <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
      </IkcoStack>
    </FormDialog>
  );
}
