import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { searchBodyReportsCriticalParts } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { bodyReportsSearchCriticalPartsInputRules, type IBodyReportsSearchCriticalPartsInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "plantId", label: "سایت", type: "lookup", lookupKey: "virtual-plants" },
  { name: "vinBiwSite", label: "خط بدنه‌سازی", type: "lookup", lookupKey: "body-saloons", parent: "plantId" },
  { name: "partNumber", label: "شماره فنی", type: "text" },
  { name: "status", label: "فقط فعال", type: "checkbox" },
  { name: "fromDate", label: "از تاریخ", type: "date" },
  { name: "toDate", label: "تا تاریخ", type: "date" },
  { name: "carGroupId", label: "خانواده محصول", type: "lookup", lookupKey: "car-groups" },
  { name: "carModelId", label: "مدل", type: "lookup", lookupKey: "car-models", parent: "carGroupId" },
  { name: "carClassId", label: "کلاس", type: "lookup", lookupKey: "car-classes", parent: "carModelId" },
  { name: "productionPossibilityId", label: "شرایط تولید", type: "lookup", lookupKey: "production-possibilities" },
];

/**
 * گزارش اقلام بحرانی.
 * Backend: POST /api/production-feasibility/body-shop/reports/critical-parts/search (PF.S_BodyCriticalSearch_Web)، دسترسی bodyshop.reports.search.
 * فرم سامانه ویندوزی: frmBodyCriticalSearch / frmRPTBodyCriticalSearch.
 */
export default function BodyCriticalPartsReport() {
  return (
    <ReportPage<IBodyReportsSearchCriticalPartsInput>
      title="گزارش اقلام بحرانی"
      permission={ApiPermissions.bodyReports.search}
      filters={filters}
      run={searchBodyReportsCriticalParts}
      rules={bodyReportsSearchCriticalPartsInputRules}
    />
  );
}
