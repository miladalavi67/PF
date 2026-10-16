import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsStockLog } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetStockLogInputRules, type IMechReportsGetStockLogInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "partNo", label: "شماره فنی", type: "text", required: true },
];

/**
 * تاریخچه موجودی اقلام بحرانی.
 * Backend: GET /api/production-feasibility/mechanical/reports/stock-log (dbo.SEXT_CriticalPartsStockLog)، دسترسی mechanical.reports.stocklog.
 * فرم سامانه ویندوزی: frm_Rpt_CriticalPartsStockLog.
 */
export default function MechStockLogReport() {
  return (
    <ReportPage<IMechReportsGetStockLogInput>
      title="تاریخچه موجودی اقلام بحرانی"
      permission={ApiPermissions.mechReports.stocklog}
      filters={filters}
      run={getMechReportsStockLog}
      rules={mechReportsGetStockLogInputRules}
    />
  );
}
