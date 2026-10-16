import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { searchMechLicensesTrackingCriticals } from "../../../server/services/mechLicensesService";
import { ApiPermissions } from "../../../types/permissions";
import { mechLicensesSearchTrackingCriticalsInputRules, type IMechLicensesSearchTrackingCriticalsInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date", required: true },
  { name: "toDate", label: "تا تاریخ", type: "date", required: true },
  { name: "partNo", label: "شماره فنی", type: "text" },
  { name: "setCode", label: "کد ست", type: "text" },
];

/**
 * بحران‌های دارای مجوز تولید با کسری.
 * Backend: POST /api/production-feasibility/mechanical/licenses/tracking/search-criticals (dbo.S_ShortagelicenseTrackingSearchCriticals)، دسترسی mechanical.licenses.report.
 * فرم سامانه ویندوزی: frmCriticalShortageLicenseTracking.
 */
export default function MechLicenseTrackingCriticalsReport() {
  return (
    <ReportPage<IMechLicensesSearchTrackingCriticalsInput>
      title="بحران‌های دارای مجوز تولید با کسری"
      permission={ApiPermissions.mechLicenses.report}
      filters={filters}
      run={searchMechLicensesTrackingCriticals}
      rules={mechLicensesSearchTrackingCriticalsInputRules}
    />
  );
}
