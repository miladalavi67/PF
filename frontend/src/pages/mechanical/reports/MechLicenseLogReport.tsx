import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechLicensesTrackingList } from "../../../server/services/mechLicensesService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechLicensesGetTrackingListInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "criticalPartTypeId", label: "نوع بحران", type: "lookup", lookupKey: "critical-part-types", defaultValue: 1 },
  { name: "criticalPartId", label: "شناسه بحران", type: "number", required: true },
];

/**
 * تاریخچه مجوزهای یک بحران.
 * Backend: GET /api/production-feasibility/mechanical/licenses/tracking (dbo.S_ShortageLicenseTrackingSelectAll)، دسترسی mechanical.licenses.view.
 * فرم سامانه ویندوزی: frmLicenseLogs.
 */
export default function MechLicenseLogReport() {
  return (
    <ReportPage<IMechLicensesGetTrackingListInput>
      title="تاریخچه مجوزهای یک بحران"
      permission={ApiPermissions.mechLicenses.view}
      filters={filters}
      run={getMechLicensesTrackingList}
    />
  );
}
