import DateOrPartReport from "../../../components/DateOrPartReport";
import { getBodyReportsNotPossibleProductsToStepAssign } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";

/**
 * گزارش مغایرت پیش تخصیص بدنه‌های بحرانی (frmRPTNotPossibleProductsToStepAssign).
 * Backend: POST /api/production-feasibility/body-shop/reports/not-possible-products-to-step-assign، دسترسی bodyshop.reports.conflicts.
 */
export default function BodyConflictsReport() {
  return <DateOrPartReport title="گزارش مغایرت پیش تخصیص بدنه‌های بحرانی" listTitle="لیست بدنه ها" permission={ApiPermissions.bodyReports.conflicts} run={getBodyReportsNotPossibleProductsToStepAssign} />;
}
