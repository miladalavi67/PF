import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechExceptionsLicenseLog } from "../../../server/services/mechExceptionsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechExceptionsGetLicenseLogInputRules, type IMechExceptionsGetLicenseLogInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "partNo", label: "شماره فنی", type: "text", required: true },
];

/**
 * تاریخچه مجوزهای استثنا.
 * Backend: GET /api/production-feasibility/mechanical/exceptions/license-log (dbo.S_CriticalPartsProductExceptionsLisenceLog)، دسترسی mechanical.exceptions.view.
 * فرم سامانه ویندوزی: frmLicenseLogs.
 */
export default function MechExceptionLicenseLogReport() {
  return (
    <ReportPage<IMechExceptionsGetLicenseLogInput>
      title="تاریخچه مجوزهای استثنا"
      permission={ApiPermissions.mechExceptions.view}
      filters={filters}
      run={getMechExceptionsLicenseLog}
      rules={mechExceptionsGetLicenseLogInputRules}
    />
  );
}
