import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsPreAssignBodyCounts } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IBodyReportsGetPreAssignBodyCountsInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "carId", label: "شناسه محصول (CarId)", type: "number" },
];

/**
 * تعداد بدنه‌های پیش‌تخصیص‌شده.
 * Backend: GET /api/production-feasibility/body-shop/reports/pre-assign-body-counts (PF.S_PreAssignBodyCoun_List)، دسترسی bodyshop.reports.preassigncount.
 * فرم سامانه ویندوزی: frmRPTPreassignBodyCount.
 */
export default function BodyPreAssignCountReport() {
  return (
    <ReportPage<IBodyReportsGetPreAssignBodyCountsInput>
      title="تعداد بدنه‌های پیش‌تخصیص‌شده"
      permission={ApiPermissions.bodyReports.preassigncount}
      filters={filters}
      run={getBodyReportsPreAssignBodyCounts}
    />
  );
}
