import {
  IkcoButton,
  IkcoCard,
  IkcoCardActions,
  IkcoCardContent,
  IkcoDataGrid,
  IkcoGrid,
  IkcoStack,
  IkcoTypography,
  type IkcoGridColDef,
} from "@ikco/frontend-kit";
import { PermissionGuard } from "@ikco/mfe-core";
import { Search } from "@mui/icons-material";
import type { ReactNode } from "react";
import type { IGridRow } from "../utils/tabularUtility";
import ExcelExportButton from "./ExcelExportButton";

interface ReportViewProps {
  /** عنوان برای نام فایل خروجی اکسل */
  title: string;
  /** کلید دسترسی API گزارش؛ دکمه «نمایش» فقط برای دارندگان آن */
  permission: string;
  /** فیلدهای پارامتر — هر فیلد داخل IkcoGrid با size={{ xl: 3, lg: 3, md: 4, sm: 6, xs: 12 }} */
  filters?: ReactNode;
  onRun: () => void;
  onClear?: () => void;
  loading: boolean;
  columns: IkcoGridColDef[];
  rows: IGridRow[];
  /** false تا پیش از اولین اجرا (پیام راهنما به جای «داده‌ای وجود ندارد») */
  executed?: boolean;
}

/**
 * چیدمان استاندارد گزارش (جایگزین frmReportMaster): کارت پارامترها با «نمایش»، «پاک کردن» و «خروجی اکسل» و
 * IkcoDataGrid نتیجه. صفحه گزارش فقط ورودی DTO را می‌سازد و سرویس گزارش را فراخوانی می‌کند.
 */
export default function ReportView({ title, permission, filters, onRun, onClear, loading, columns, rows, executed = true }: ReportViewProps) {
  return (
    <IkcoStack spacing={2}>
      <IkcoCard>
        {filters && (
          <IkcoCardContent>
            <IkcoGrid container spacing={2}>
              {filters}
            </IkcoGrid>
          </IkcoCardContent>
        )}
        <IkcoCardActions>
          <PermissionGuard can={permission} fallback={<IkcoTypography color="error">شما به اجرای این گزارش دسترسی ندارید.</IkcoTypography>}>
            <IkcoButton variant="contained" startIcon={<Search />} loading={loading} onClick={onRun}>
              نمایش
            </IkcoButton>
          </PermissionGuard>
          {onClear && <IkcoButton onClick={onClear}>پاک کردن</IkcoButton>}
          <ExcelExportButton fileName={title} columns={columns} rows={rows as unknown as Record<string, unknown>[]} />
          <IkcoTypography variant="caption">{executed ? `تعداد: ${rows.length}` : "پارامترها را وارد و «نمایش» را انتخاب کنید"}</IkcoTypography>
        </IkcoCardActions>
      </IkcoCard>
      <IkcoDataGrid columns={columns} rows={rows} loading={loading} getRowId={(r) => r.__rowId} />
    </IkcoStack>
  );
}
