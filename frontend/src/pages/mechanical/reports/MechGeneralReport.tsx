import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsGeneralReport } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetGeneralReportInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "criticalPartTypeId", label: "نوع قطعه بحران", type: "number", defaultValue: 100 },
  { name: "saloonId", label: "سالن", type: "lookup", lookupKey: "trim-shops", defaultValue: 1 },
  { name: "crisisTypeId", label: "نوع بحران", type: "lookup", lookupKey: "crisis-types" },
  { name: "plantId", label: "سایت", type: "lookup", lookupKey: "virtual-plants" },
];

/**
 * گزارش جامع اقلام بحرانی.
 * Backend: GET /api/production-feasibility/mechanical/reports/general (dbo.S_CriticalPartsGeneralReport)، دسترسی mechanical.reports.general.
 * فرم سامانه ویندوزی: frmRPTProdpossibility (جامع).
 */
export default function MechGeneralReport() {
  return (
    <ReportPage<IMechReportsGetGeneralReportInput>
      title="گزارش جامع اقلام بحرانی"
      permission={ApiPermissions.mechReports.general}
      filters={filters}
      run={getMechReportsGeneralReport}
    />
  );
}
