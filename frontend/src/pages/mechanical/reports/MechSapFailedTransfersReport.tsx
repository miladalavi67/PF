import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsFailedSapTransfers } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetFailedSapTransfersInputRules, type IMechReportsGetFailedSapTransfersInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date", required: true },
  { name: "toDate", label: "تا تاریخ", type: "date", required: true },
];

/**
 * قطعات ناموفق در انتقال از SAP.
 * Backend: GET /api/production-feasibility/mechanical/reports/failed-sap-transfers (dbo.S_FailedPartTransferd_Sap_Log)، دسترسی mechanical.reports.sap.
 * فرم سامانه ویندوزی: frm_Rpt_FailedTransfer_Sap.
 */
export default function MechSapFailedTransfersReport() {
  return (
    <ReportPage<IMechReportsGetFailedSapTransfersInput>
      title="قطعات ناموفق در انتقال از SAP"
      permission={ApiPermissions.mechReports.sap}
      filters={filters}
      run={getMechReportsFailedSapTransfers}
      rules={mechReportsGetFailedSapTransfersInputRules}
    />
  );
}
