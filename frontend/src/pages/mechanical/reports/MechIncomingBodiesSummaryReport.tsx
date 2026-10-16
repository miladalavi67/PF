import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsIncomingBodiesSummary } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetIncomingBodiesSummaryInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "saloonId", label: "سالن", type: "lookup", lookupKey: "trim-shops", defaultValue: 1 },
  { name: "crisisTypeId", label: "نوع بحران", type: "lookup", lookupKey: "crisis-types", defaultValue: 1 },
  { name: "productionStepGroupId", label: "گروه مرحله تولید", type: "number", defaultValue: 16 },
  { name: "plantId", label: "سایت", type: "lookup", lookupKey: "virtual-plants", defaultValue: 1 },
];

/**
 * خلاصه بدنه‌های وارد شده به خط.
 * Backend: GET /api/production-feasibility/mechanical/reports/incoming-bodies-summary (dbo.S_RPT_IncommingBodiesByCriticalParts)، دسترسی mechanical.reports.incoming.
 * فرم سامانه ویندوزی: frm_RPT_ST_IncommingBodiesByCP.
 */
export default function MechIncomingBodiesSummaryReport() {
  return (
    <ReportPage<IMechReportsGetIncomingBodiesSummaryInput>
      title="خلاصه بدنه‌های وارد شده به خط"
      permission={ApiPermissions.mechReports.incoming}
      filters={filters}
      run={getMechReportsIncomingBodiesSummary}
    />
  );
}
