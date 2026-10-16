import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsPreAssignCriticalBodies } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { bodyReportsGetPreAssignCriticalBodiesInputRules, type IBodyReportsGetPreAssignCriticalBodiesInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date" },
  { name: "toDate", label: "تا تاریخ", type: "date" },
  { name: "partNo", label: "شماره فنی", type: "text" },
];

/**
 * بدنه‌های پیش‌تخصیص بحرانی.
 * Backend: GET /api/production-feasibility/body-shop/reports/pre-assign-critical-bodies (PF.S_PreAssignCriticalBody_SelectAll)، دسترسی bodyshop.reports.preassigncritical.
 * فرم سامانه ویندوزی: frmRPTPreAssignCriticalBody.
 */
export default function BodyPreAssignCriticalReport() {
  return (
    <ReportPage<IBodyReportsGetPreAssignCriticalBodiesInput>
      title="بدنه‌های پیش‌تخصیص بحرانی"
      permission={ApiPermissions.bodyReports.preassigncritical}
      filters={filters}
      run={getBodyReportsPreAssignCriticalBodies}
      rules={bodyReportsGetPreAssignCriticalBodiesInputRules}
    />
  );
}
