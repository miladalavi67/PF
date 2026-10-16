import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechLicensesBodiesRelatedToUsedLicense } from "../../../server/services/mechLicensesService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechLicensesGetBodiesRelatedToUsedLicenseInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "criticalPartTypeId", label: "نوع بحران", type: "lookup", lookupKey: "critical-part-types", defaultValue: 1 },
  { name: "criticalPartId", label: "شناسه بحران", type: "number", required: true },
];

/**
 * بدنه‌های مرتبط با مجوز مصرف‌شده.
 * Backend: GET /api/production-feasibility/mechanical/licenses/used-license-bodies (dbo.S_SelectBodiesRelatedToUsedLicense)، دسترسی mechanical.licenses.report.
 * فرم سامانه ویندوزی: frmBodiesRelatedToUsedLicense.
 */
export default function MechUsedLicenseBodiesReport() {
  return (
    <ReportPage<IMechLicensesGetBodiesRelatedToUsedLicenseInput>
      title="بدنه‌های مرتبط با مجوز مصرف‌شده"
      permission={ApiPermissions.mechLicenses.report}
      filters={filters}
      run={getMechLicensesBodiesRelatedToUsedLicense}
    />
  );
}
