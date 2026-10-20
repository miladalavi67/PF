import { IkcoDataGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { useEffect, useMemo, useState } from "react";
import BodyNumberField from "../../../components/BodyNumberField";
import ExcelExportButton from "../../../components/ExcelExportButton";
import FormDialog from "../../../components/FormDialog";
import { getBodyReportsPreAssignBodyCounts } from "../../../server/services/bodyReportsService";
import type { IBodyInfoRow } from "../../../types/BodyShop";
import type { ITabularResult } from "../../../types/tabular";
import { columnHeaders } from "../../../utils/columnHeaders";
import { resultColumns, tabularRows } from "../../../utils/tabularUtility";
import { gridColumns } from "../../../utils/gridColumns";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** شماره بدنه‌ای که فرم مبدأ در اختیار دارد؛ با باز شدن modal جستجو می‌شود */
  initialBodyNumber?: string;
}

/** تعداد بدنه‌های پیش‌تخصیص‌شده (frmRPTPreassignBodyCount): با شماره بدنه، تعداد بدنه‌های پیش‌تخصیص هر محصول نمایش داده می‌شود */
export default function BodyPreAssignCountDialog({ open, close, initialBodyNumber = "" }: Props) {
  const [bodyNumber, setBodyNumber] = useState(initialBodyNumber);
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [loading, setLoading] = useState(false);

  const columns = useMemo(() => resultColumns(result, columnHeaders, gridColumns.bodyPreAssignCount), [result]);
  const rows = useMemo(() => tabularRows(result), [result]);

  // اطلاعات بدنه توسط BodyNumberField خوانده و نمایش داده می‌شود؛ اینجا فقط تعداد پیش‌تخصیص محصول آن
  const loadCounts = async (body: IBodyInfoRow | null) => {
    if (!body) {
      setResult(null);
      return;
    }
    setLoading(true);
    try {
      setResult(await getBodyReportsPreAssignBodyCounts({ carId: body.carId }));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (!open) return;
    setBodyNumber(initialBodyNumber);
    setResult(null);
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
        <BodyNumberField bodyNumber={bodyNumber} onBodyNumberChange={setBodyNumber} onLoaded={loadCounts} autoSearch />
        <IkcoTypography variant="subtitle1">نمایش</IkcoTypography>
        <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} autoHeight />
      </IkcoStack>
    </FormDialog>
  );
}
