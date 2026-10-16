import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsNotPossibleProductsToStepAssign } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { bodyReportsGetNotPossibleProductsToStepAssignInputRules, type IBodyReportsGetNotPossibleProductsToStepAssignInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date" },
  { name: "toDate", label: "تا تاریخ", type: "date" },
  { name: "partNo", label: "شماره فنی", type: "text" },
];

/**
 * مغایرت پیش‌تخصیص بدنه‌های بحرانی.
 * Backend: GET /api/production-feasibility/body-shop/reports/not-possible-to-assign (PF.S_NotPossibleProductsToStepAssign_SelectAll)، دسترسی bodyshop.reports.conflicts.
 * فرم سامانه ویندوزی: frmRPTNotPossibleProductsToStepAssign.
 */
export default function BodyConflictsReport() {
  return (
    <ReportPage<IBodyReportsGetNotPossibleProductsToStepAssignInput>
      title="مغایرت پیش‌تخصیص بدنه‌های بحرانی"
      permission={ApiPermissions.bodyReports.conflicts}
      filters={filters}
      run={getBodyReportsNotPossibleProductsToStepAssign}
      rules={bodyReportsGetNotPossibleProductsToStepAssignInputRules}
    />
  );
}
