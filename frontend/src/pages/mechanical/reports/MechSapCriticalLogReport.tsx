import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsSapCriticalLog } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetSapCriticalLogInputRules, type IMechReportsGetSapCriticalLogInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "partNo", label: "شماره فنی", type: "text" },
];

/**
 * لاگ انتقال اقلام بحرانی از SAP.
 * Backend: GET /api/production-feasibility/mechanical/reports/sap-critical-log (dbo.S_SapCriticalLog)، دسترسی mechanical.reports.sap.
 * فرم سامانه ویندوزی: frm_Rpt_SapCriticalLog.
 */
export default function MechSapCriticalLogReport() {
  return (
    <ReportPage<IMechReportsGetSapCriticalLogInput>
      title="لاگ انتقال اقلام بحرانی از SAP"
      permission={ApiPermissions.mechReports.sap}
      filters={filters}
      run={getMechReportsSapCriticalLog}
      rules={mechReportsGetSapCriticalLogInputRules}
    />
  );
}
