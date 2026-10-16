import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsIncomingBodiesByCriticalPart } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetIncomingBodiesByCriticalPartInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "id", label: "شناسه بحران", type: "number", required: true },
  { name: "criticalPartTypeId", label: "نوع بحران", type: "lookup", lookupKey: "critical-part-types", defaultValue: 1 },
  { name: "saloonId", label: "سالن", type: "lookup", lookupKey: "trim-shops", defaultValue: 1 },
  { name: "plantId", label: "سایت", type: "lookup", lookupKey: "virtual-plants", defaultValue: 1 },
];

/**
 * بدنه‌های وارد شده به خط به تفکیک بحران.
 * Backend: GET /api/production-feasibility/mechanical/reports/incoming-bodies (dbo.S_RPT_IncommingBodiesByCriticalPartId)، دسترسی mechanical.reports.incoming.
 * فرم سامانه ویندوزی: frm_RPT_Des_IncommingBodiesByCPId.
 */
export default function MechIncomingBodiesReport() {
  return (
    <ReportPage<IMechReportsGetIncomingBodiesByCriticalPartInput>
      title="بدنه‌های وارد شده به خط به تفکیک بحران"
      permission={ApiPermissions.mechReports.incoming}
      filters={filters}
      run={getMechReportsIncomingBodiesByCriticalPart}
    />
  );
}
