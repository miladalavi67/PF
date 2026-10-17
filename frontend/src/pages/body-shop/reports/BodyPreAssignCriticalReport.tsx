import DateOrPartReport from "../../../components/DateOrPartReport";
import { getBodyReportsPreAssignCriticalBodies } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";

/**
 * گزارش پیش تخصیص بدنه‌های بحرانی (frmRPTPreAssignCriticalBody).
 * Backend: POST /api/production-feasibility/body-shop/reports/pre-assign-critical-bodies، دسترسی bodyshop.reports.preassigncritical.
 */
export default function BodyPreAssignCriticalReport() {
  return <DateOrPartReport title="گزارش پیش تخصیص بدنه‌های بحرانی" listTitle="لیست بدنه ها" permission={ApiPermissions.bodyReports.preassigncritical} run={getBodyReportsPreAssignCriticalBodies} />;
}
