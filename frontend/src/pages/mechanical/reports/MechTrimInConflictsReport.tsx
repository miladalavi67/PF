import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsNotPossibleProductsToTrimStep } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetNotPossibleProductsToTrimStepInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "carId", label: "شناسه محصول (CarId)", type: "number", required: true },
];

/**
 * دلایل عدم امکان ورود خودرو به مونتاژ.
 * Backend: GET /api/production-feasibility/mechanical/reports/not-possible-to-trim-in (dbo.S_TrimInNotPossibleProductsSelectAll)، دسترسی mechanical.reports.conflicts.
 * فرم سامانه ویندوزی: frmRPTNotPossibleProductsToStepTrimShop.
 */
export default function MechTrimInConflictsReport() {
  return (
    <ReportPage<IMechReportsGetNotPossibleProductsToTrimStepInput>
      title="دلایل عدم امکان ورود خودرو به مونتاژ"
      permission={ApiPermissions.mechReports.conflicts}
      filters={filters}
      run={getMechReportsNotPossibleProductsToTrimStep}
    />
  );
}
