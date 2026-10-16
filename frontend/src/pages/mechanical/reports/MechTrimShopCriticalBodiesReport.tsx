import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsTrimShopCriticalBodies } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetTrimShopCriticalBodiesInputRules, type IMechReportsGetTrimShopCriticalBodiesInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "fromDate", label: "از تاریخ", type: "date", required: true },
  { name: "toDate", label: "تا تاریخ", type: "date", required: true },
  { name: "partNo", label: "شماره فنی", type: "text" },
  { name: "criticalPartTypeId", label: "نوع بحران", type: "lookup", lookupKey: "critical-part-types" },
  { name: "setCode", label: "کد ست", type: "text" },
];

/**
 * بدنه‌های بحرانی ورودی به مونتاژ.
 * Backend: GET /api/production-feasibility/mechanical/reports/trim-shop-critical-bodies (dbo.S_TrimShopCriticalBody_SelectAll)، دسترسی mechanical.reports.conflicts.
 * فرم سامانه ویندوزی: frmRPTProductsToStepTrimShop.
 */
export default function MechTrimShopCriticalBodiesReport() {
  return (
    <ReportPage<IMechReportsGetTrimShopCriticalBodiesInput>
      title="بدنه‌های بحرانی ورودی به مونتاژ"
      permission={ApiPermissions.mechReports.conflicts}
      filters={filters}
      run={getMechReportsTrimShopCriticalBodies}
      rules={mechReportsGetTrimShopCriticalBodiesInputRules}
    />
  );
}
