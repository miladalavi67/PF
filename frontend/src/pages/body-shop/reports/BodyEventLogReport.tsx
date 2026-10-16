import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyCriticalsEventLog } from "../../../server/services/bodyCriticalsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IBodyCriticalsGetEventLogInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "eventTypeId", label: "نوع بحران", type: "lookup", lookupKey: "body-critical-part-types" },
  { name: "changedRecordId", label: "شناسه بحران", type: "number" },
];

/**
 * گزارش رویدادها.
 * Backend: GET /api/production-feasibility/body-shop/critical-parts/event-log (PF.S_EventLogList)، دسترسی bodyshop.criticalparts.view.
 * فرم سامانه ویندوزی: frmEventLog.
 */
export default function BodyEventLogReport() {
  return (
    <ReportPage<IBodyCriticalsGetEventLogInput>
      title="گزارش رویدادها"
      permission={ApiPermissions.bodyCriticals.view}
      filters={filters}
      run={getBodyCriticalsEventLog}
    />
  );
}
