import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsShortageLicenseTracking } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IBodyReportsGetShortageLicenseTrackingInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "criticalPartTypeId", label: "نوع بحران", type: "lookup", lookupKey: "body-critical-part-types" },
  { name: "criticalPartId", label: "شناسه بحران", type: "number" },
];

/**
 * ردیابی پارامترهای بحرانی.
 * Backend: GET /api/production-feasibility/body-shop/reports/shortage-license-tracking (PF.GetShortageLicenseTrackingSelectAll)، دسترسی bodyshop.reports.licensetracking.
 * فرم سامانه ویندوزی: frmRPTShortageLicenseTracking.
 */
export default function BodyLicenseTrackingReport() {
  return (
    <ReportPage<IBodyReportsGetShortageLicenseTrackingInput>
      title="ردیابی پارامترهای بحرانی"
      permission={ApiPermissions.bodyReports.licensetracking}
      filters={filters}
      run={getBodyReportsShortageLicenseTracking}
    />
  );
}
