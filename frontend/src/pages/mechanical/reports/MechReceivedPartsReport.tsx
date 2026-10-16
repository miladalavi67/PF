import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechSearchReceivedParts } from "../../../server/services/mechSearchService";
import { ApiPermissions } from "../../../types/permissions";
import { mechSearchGetReceivedPartsInputRules, type IMechSearchGetReceivedPartsInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date", required: true },
  { name: "toDate", label: "تا تاریخ", type: "date", required: true },
];

/**
 * اقلام بحرانی وارد شده به شرکت.
 * Backend: GET /api/production-feasibility/mechanical/search/received-parts (dbo.S_CriticalPart_RecievedParts)، دسترسی mechanical.search.arrived.
 * فرم سامانه ویندوزی: frmCriticalPartsArrived.
 */
export default function MechReceivedPartsReport() {
  return (
    <ReportPage<IMechSearchGetReceivedPartsInput>
      title="اقلام بحرانی وارد شده به شرکت"
      permission={ApiPermissions.mechSearch.arrived}
      filters={filters}
      run={getMechSearchReceivedParts}
      rules={mechSearchGetReceivedPartsInputRules}
    />
  );
}
