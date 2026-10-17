import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoDataGrid,
  IkcoGrid,
  IkcoGridActionsCellItem,
  IkcoIconButton,
  IkcoStack,
  IkcoTooltip,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard, useCan } from "@ikco/mfe-core";
import { AddCircle, FileUpload } from "@mui/icons-material";
import { useMemo, useRef, useState } from "react";
import toast from "react-hot-toast";
import FormDialog from "../../../components/FormDialog";
import ConfirmDialog from "../../../components/ConfirmDialog";
import ExcelExportButton from "../../../components/ExcelExportButton";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import { addMechPartsSubClass, compareMechPartsSubClasses } from "../../../server/services/mechPartsService";
import type { ITabularResult } from "../../../types/Common";
import type { ISubClassPartRow } from "../../../types/Mechanical";
import { ApiPermissions } from "../../../types/permissions";
import { columnHeaders } from "../../../utils/columnHeaders";
import { downloadSampleExcel, readExcelRows } from "../../../utils/excelUtility";
import { resultColumns, tabularRows, type IGridRow } from "../../../utils/tabularUtility";

const kinds = [
  { id: 1, label: "موجود در SAP و ناموجود در SFC" },
  { id: 2, label: "موجود در SFC و ناموجود در SAP" },
];

/**
 * مغایرت زیرکلاس‌های SAP با SFC (frmRPTContrastOfSubClasses، modal دکمه «گزارش مغایرت زیرکلاس ها»، رویه‌های S_GetContrastOfSubClasses و S_AddContrastOfSubClasses).
 * فایل اکسل SAP با دو ستون «کد زیرکلاس» و «شماره فنی» (بدون سطر عنوان) بارگذاری و با زیرکلاس‌های بحران‌های فعال سالن مقایسه می‌شود؛
 * در نوع ۱ می‌توان زیرکلاس مغایر را به بحران افزود.
 */
export default function MechContrastDialog({ open, close }: { open: boolean; close: (open: boolean) => void }) {
  const canCompare = useCan(ApiPermissions.mechParts.compare);
  const fileInput = useRef<HTMLInputElement>(null);
  const [saloonId, setSaloonId] = useState<number | null>(1);
  const [type, setType] = useState<number | null>(1);
  const [pairs, setPairs] = useState<ISubClassPartRow[]>([]);
  const [fileName, setFileName] = useState("");
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [resultType, setResultType] = useState(1);
  const [loading, setLoading] = useState(false);
  const [adding, setAdding] = useState<IGridRow | null>(null);

  const rows = useMemo(() => tabularRows(result), [result]);
  const baseColumns = useMemo(() => resultColumns(result, columnHeaders), [result]);

  const columns: IkcoGridColDef[] = useMemo(
    () =>
      resultType === 1 && canCompare
        ? [
            {
              field: "__actions",
              type: "actions",
              headerName: "افزودن",
              width: 90,
              getActions: (params) => [
                <IkcoGridActionsCellItem key="a" icon={<IkcoTooltip title="افزودن زیرکلاس به بحران"><IkcoIconButton><AddCircle color="primary" /></IkcoIconButton></IkcoTooltip>} label="افزودن" onClick={() => setAdding(params.row as IGridRow)} />,
              ],
            },
            ...baseColumns,
          ]
        : baseColumns,
    [baseColumns, resultType, canCompare],
  );

  const onFile = async (file: File | undefined) => {
    if (!file) return;
    try {
      const data = await readExcelRows(file);
      const parsed = data
        .filter((r) => r[0] && r[1])
        .map((r) => ({ subClassCode: String(r[0]).trim(), partNo: String(r[1]).trim() }));
      if (parsed.length === 0) {
        toast.error("فایل خالی است یا ستون‌های «کد زیرکلاس» و «شماره فنی» پیدا نشد");
        return;
      }
      if (parsed.length > 5000) {
        toast.error("حداکثر ۵۰۰۰ ردیف مجاز است");
        return;
      }
      setPairs(parsed);
      setFileName(file.name);
      toast.success(`${parsed.length} ردیف خوانده شد`);
    } catch {
      toast.error("خواندن فایل اکسل ممکن نشد");
    } finally {
      if (fileInput.current) fileInput.current.value = "";
    }
  };

  const compare = async () => {
    if (!saloonId || !type) return;
    if (pairs.length === 0) {
      toast.error("ابتدا فایل اکسل SAP را بارگذاری کنید");
      return;
    }
    setLoading(true);
    try {
      const data = await compareMechPartsSubClasses({ saloonId, type, rows: pairs });
      setResultType(type);
      setResult(data);
    } finally {
      setLoading(false);
    }
  };

  const confirmAdd = async () => {
    if (!adding || !saloonId) return;
    const row = adding as Record<string, unknown>;
    await addMechPartsSubClass({
      criticalPartsId: Number(row["CriticalPartsId"]),
      subClassCode: String(row["SubClassCode"]).trim(),
      trimShopId: saloonId,
    });
    toast.success("زیرکلاس به بحران افزوده شد");
    setAdding(null);
    await compare();
  };

  return (
    <FormDialog open={open} close={close} title="گزارش مغایرت زیرکلاس‌ها" maxWidth="xl" closeText="بازگشت">
    <IkcoStack spacing={2}>
      <IkcoCard>
        <IkcoCardContent>
          <IkcoGrid container spacing={2} alignItems="center">
            <IkcoGrid size={{ xl: 3, lg: 3, md: 4, sm: 6, xs: 12 }}>
              <LookupAutocomplete lookupKey="trim-shops" label="سالن مونتاژ" value={saloonId} onChange={setSaloonId} />
            </IkcoGrid>
            <IkcoGrid size={{ xl: 4, lg: 4, md: 4, sm: 6, xs: 12 }}>
              <LookupAutocomplete lookupKey="contrast-kinds" label="نوع مغایرت" value={type} onChange={setType} />
            </IkcoGrid>
            <IkcoGrid size={{ xl: 5, lg: 5, md: 4, sm: 12, xs: 12 }}>
              <IkcoStack direction="row" spacing={1} alignItems="center">
                <input ref={fileInput} type="file" accept=".xlsx" hidden onChange={(e) => void onFile(e.target.files?.[0])} />
                <IkcoButton startIcon={<FileUpload />} onClick={() => fileInput.current?.click()}>بارگذاری فایل SAP</IkcoButton>
                <IkcoButton onClick={() => void downloadSampleExcel("نمونه مغایرت زیرکلاس", ["کد زیرکلاس", "شماره فنی"])}>فایل نمونه</IkcoButton>
                <IkcoTypography variant="caption">{pairs.length > 0 ? `${fileName} (${pairs.length} ردیف)` : "فایل انتخاب نشده"}</IkcoTypography>
              </IkcoStack>
            </IkcoGrid>
          </IkcoGrid>
        </IkcoCardContent>
        <IkcoCardActions>
          <PermissionGuard can={ApiPermissions.mechParts.compare} fallback={<IkcoTypography color="error">شما به این عملیات دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" loading={loading} onClick={() => void compare()}>مقایسه</IkcoButton>
          </PermissionGuard>
          <ExcelExportButton fileName="مغایرت زیرکلاس‌ها" columns={baseColumns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{result ? `تعداد مغایرت: ${rows.length}` : kinds.map((k) => `${k.id}: ${k.label}`).join(" | ")}</IkcoTypography>
        </IkcoCardActions>
      </IkcoCard>

      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} />

      <ConfirmDialog
        open={adding !== null}
        close={() => setAdding(null)}
        title="افزودن زیرکلاس"
        message={adding ? `زیرکلاس ${String((adding as Record<string, unknown>)["SubClassCode"] ?? "")} به بحران شماره فنی ${String((adding as Record<string, unknown>)["PartNo"] ?? "")} افزوده شود؟` : ""}
        confirmText="افزودن"
        onConfirm={confirmAdd}
      />
    </IkcoStack>
    </FormDialog>
  );
}
