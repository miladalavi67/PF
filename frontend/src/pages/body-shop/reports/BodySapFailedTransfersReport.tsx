import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsFailedSapTransfers } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { bodyReportsGetFailedSapTransfersInputRules, type IBodyReportsGetFailedSapTransfersInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date" },
  { name: "toDate", label: "تا تاریخ", type: "date" },
];

/**
 * قطعات ناموفق در انتقال از SAP.
 * Backend: GET /api/production-feasibility/body-shop/reports/failed-sap-transfers (PF.S_FailedPartTransferd_Sap_Log)، دسترسی bodyshop.reports.sap.
 * فرم سامانه ویندوزی: BodyShop reports (SAP).
 */
export default function BodySapFailedTransfersReport() {
  return (
    <ReportPage<IBodyReportsGetFailedSapTransfersInput>
      title="قطعات ناموفق در انتقال از SAP"
      permission={ApiPermissions.bodyReports.sap}
      filters={filters}
      run={getBodyReportsFailedSapTransfers}
      rules={bodyReportsGetFailedSapTransfersInputRules}
    />
  );
}
