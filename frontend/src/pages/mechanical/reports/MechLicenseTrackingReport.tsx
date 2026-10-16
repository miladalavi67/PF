import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { searchMechLicensesTracking } from "../../../server/services/mechLicensesService";
import { ApiPermissions } from "../../../types/permissions";
import { mechLicensesSearchTrackingInputRules, type IMechLicensesSearchTrackingInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date", required: true },
  { name: "toDate", label: "تا تاریخ", type: "date", required: true },
  { name: "partNo", label: "شماره فنی", type: "text" },
  { name: "setCode", label: "کد ست", type: "text" },
];

/**
 * ردیابی مجوزهای تولید با کسری.
 * Backend: POST /api/production-feasibility/mechanical/licenses/tracking/search (dbo.S_ShortageLicenseTrackingSearch)، دسترسی mechanical.licenses.report.
 * فرم سامانه ویندوزی: frmShortageLicenseTracking.
 */
export default function MechLicenseTrackingReport() {
  return (
    <ReportPage<IMechLicensesSearchTrackingInput>
      title="ردیابی مجوزهای تولید با کسری"
      permission={ApiPermissions.mechLicenses.report}
      filters={filters}
      run={searchMechLicensesTracking}
      rules={mechLicensesSearchTrackingInputRules}
    />
  );
}
