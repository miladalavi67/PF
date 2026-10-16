import { IkcoButton, type IkcoGridColDef } from "@ikco/frontend-kit";
import { FileDownload } from "@mui/icons-material";
import { useState } from "react";
import toast from "react-hot-toast";
import { exportToExcel } from "../utils/excelUtility";
import { todayJalali } from "../utils/dateUtility";

interface ExcelExportButtonProps {
  fileName: string;
  columns: IkcoGridColDef[];
  rows: Record<string, unknown>[];
}

/** خروجی اکسل ستون‌های داده‌ای گرید (بدون ستون‌های عملیات) — جایگزین ExportToExcel سامانه قدیم */
export default function ExcelExportButton({ fileName, columns, rows }: ExcelExportButtonProps) {
  const [loading, setLoading] = useState(false);

  const handleExport = async () => {
    if (rows.length === 0) {
      toast("داده‌ای برای خروجی گرفتن وجود ندارد");
      return;
    }
    setLoading(true);
    try {
      const dataColumns = columns
        .filter((c) => c.type !== "actions" && c.field !== "__rowId")
        .map((c) => ({ field: c.field, headerName: c.headerName ?? c.field }));
      await exportToExcel(`${fileName} ${todayJalali().replaceAll("/", "-")}`, dataColumns, rows);
    } finally {
      setLoading(false);
    }
  };

  return (
    <IkcoButton variant="outlined" startIcon={<FileDownload />} loading={loading} onClick={handleExport}>
      خروجی اکسل
    </IkcoButton>
  );
}
