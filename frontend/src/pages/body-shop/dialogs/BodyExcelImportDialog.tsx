import { IkcoAlert, IkcoButton, IkcoGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { FileUpload } from "@mui/icons-material";
import { useRef, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import toast from "react-hot-toast";
import FormDialog from "../../../components/FormDialog";
import LookupAutocomplete from "../../../components/LookupAutocomplete";
import TabularDialog from "../../../components/TabularDialog";
import { batchBodyCriticalsCreate } from "../../../server/services/bodyCriticalsService";
import type { IBatchCriticalPartRow } from "../../../types/BodyShop";
import type { ITabularResult } from "../../../types/tabular";
import { downloadSampleExcel, readExcelRows } from "../../../utils/excelUtility";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** خط بدنه‌سازی پیش‌فرض (از بدنه جاری فرم ثبت) */
  defaultBiwSiteId?: number | null;
  onImported?: () => void;
}

interface FormValues {
  plantId: number | null;
  biwSiteId: number | null;
}

const sampleHeaders = ["شماره بدنه", "شماره فنی", "شرح قطعه", "گروه قطعه", "شماره فنی جایگزین", "توضیحات قطعه", "موجودی گروهی", "موجودی خام", "توضیحات وضعیت تولید", "شماره نامه", "کد کلاس", "ضریب مصرف"];

const num = (v: string | null) => (v === null || v === "" ? undefined : Number(v));

/**
 * ایجاد بحران از روی فایل اکسل (دکمه «ایجاد با اکسل» frmBodyCriticalParts). ستون‌ها به ترتیب نمونه‌ی قابل دانلود هستند؛
 * پس از ثبت گروهی، خطاهای رویه در modal «گزارش خطاها» (frmCriticalExcelLogError) نمایش داده می‌شود.
 */
export default function BodyExcelImportDialog({ open, close, defaultBiwSiteId, onImported }: Props) {
  const { control, watch, setValue, getValues } = useForm<FormValues>({ defaultValues: { plantId: null, biwSiteId: defaultBiwSiteId ?? null } });
  const fileRef = useRef<HTMLInputElement>(null);
  const [fileName, setFileName] = useState("");
  const [rows, setRows] = useState<IBatchCriticalPartRow[]>([]);
  const [saving, setSaving] = useState(false);
  const [errors, setErrors] = useState<ITabularResult | null>(null);
  const [showErrors, setShowErrors] = useState(false);

  const onFile = async (file: File | undefined) => {
    if (!file) return;
    const raw = await readExcelRows(file);
    // سطر عنوان ستون‌ها (در صورت وجود) حذف می‌شود
    const data = raw.length > 0 && raw[0][0] === sampleHeaders[0] ? raw.slice(1) : raw;
    setRows(
      data.map((r) => ({
        bodyNo: r[0] ?? "",
        partNo: r[1] ?? "",
        partDescF: r[2],
        partGroup: r[3] ?? "",
        alternative: r[4],
        partDescription: r[5],
        groupStock: num(r[6]),
        rawStock: num(r[7]),
        description: r[8],
        letterNo: r[9],
        carClassCode: num(r[10]),
        quantity: num(r[11]),
      })),
    );
    setFileName(file.name);
  };

  const submit = async () => {
    const biwSiteId = getValues("biwSiteId");
    if (!biwSiteId) {
      toast.error("خط بدنه‌سازی را انتخاب کنید");
      return;
    }
    if (rows.length === 0) {
      toast.error("فایل اکسل را انتخاب کنید");
      return;
    }
    setSaving(true);
    try {
      const result = await batchBodyCriticalsCreate({ biwSiteId, rows });
      if (result.rows.length > 0) {
        setErrors(result);
        setShowErrors(true);
      } else {
        toast.success("ثبت گروهی با موفقیت انجام شد");
        onImported?.();
        close(false);
      }
    } finally {
      setSaving(false);
    }
  };

  return (
    <>
      <FormDialog
        open={open}
        close={close}
        title="ایجاد با اکسل"
        maxWidth="md"
        closeText="انصراف"
        actions={
          <IkcoButton variant="contained" loading={saving} onClick={submit}>
            ثبت
          </IkcoButton>
        }
      >
        <IkcoStack spacing={2}>
          <IkcoAlert status="info" severity="info">
            ستون‌های فایل به ترتیب: {sampleHeaders.join("، ")}. هر ردیف یک شماره فنی و یک کلاس مصرف‌کننده است.
          </IkcoAlert>
          <IkcoGrid container spacing={2}>
            <IkcoGrid size={{ xs: 12, md: 6 }}>
              <Controller name="plantId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="virtual-plants" label="سایت" value={field.value} onChange={(v) => { field.onChange(v); setValue("biwSiteId", null); }} />} />
            </IkcoGrid>
            <IkcoGrid size={{ xs: 12, md: 6 }}>
              <Controller name="biwSiteId" control={control} render={({ field }) => <LookupAutocomplete lookupKey="body-saloons" label="خط بدنه‌سازی" value={field.value} parentId={watch("plantId")} onChange={field.onChange} />} />
            </IkcoGrid>
          </IkcoGrid>
          <IkcoStack direction="row" spacing={1} alignItems="center">
            <input ref={fileRef} type="file" accept=".xlsx" hidden onChange={(e) => void onFile(e.target.files?.[0])} />
            <IkcoButton variant="outlined" startIcon={<FileUpload />} onClick={() => fileRef.current?.click()}>
              انتخاب فایل اکسل
            </IkcoButton>
            <IkcoButton onClick={() => void downloadSampleExcel("نمونه ثبت گروهی بحران بدنه‌سازی", sampleHeaders)}>دریافت فایل نمونه</IkcoButton>
            <IkcoTypography variant="caption">{fileName ? `${fileName} — ${rows.length} ردیف` : "فایلی انتخاب نشده است"}</IkcoTypography>
          </IkcoStack>
        </IkcoStack>
      </FormDialog>
      <TabularDialog open={showErrors} close={setShowErrors} title="گزارش خطاها" load={() => Promise.resolve(errors ?? { columns: [], rows: [] })} />
    </>
  );
}
