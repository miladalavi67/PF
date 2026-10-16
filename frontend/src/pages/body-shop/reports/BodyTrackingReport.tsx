import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsTrackingCriticalParts, getBodyReportsLastChangeOnLicense } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IBodyReportsGetTrackingCriticalPartsInput } from "../../../types/BodyShop";

const filters: IReportFilter[] = [
  { name: "plantId", label: "سایت", type: "lookup", lookupKey: "virtual-plants" },
  { name: "vinBiwSite", label: "خط بدنه‌سازی", type: "lookup", lookupKey: "body-saloons", parent: "plantId" },
  { name: "carGroupId", label: "خانواده محصول", type: "lookup", lookupKey: "car-groups" },
  { name: "carModelId", label: "مدل", type: "lookup", lookupKey: "car-models", parent: "carGroupId" },
  { name: "carClassId", label: "کلاس", type: "lookup", lookupKey: "car-classes", parent: "carModelId" },
  { name: "subClassId", label: "زیرکلاس", type: "lookup", lookupKey: "sub-classes", parent: "carClassId" },
  { name: "productionPossibilityId", label: "شرایط تولید", type: "lookup", lookupKey: "production-possibilities" },
];

/**
 * گزارش ردیابی تغییرات اقلام بحرانی.
 * Backend: POST /api/production-feasibility/body-shop/reports/tracking-critical-parts (PF.GetTrackingCriticalParts_V2)، دسترسی bodyshop.reports.tracking.
 * فرم سامانه ویندوزی: frmRPTTrackingCriticalParts / frmRPTTrackingCriticalParts_withTimer.
 */
export default function BodyTrackingReport() {
  return (
    <ReportPage<IBodyReportsGetTrackingCriticalPartsInput>
      title="گزارش ردیابی تغییرات اقلام بحرانی"
      permission={ApiPermissions.bodyReports.tracking}
      filters={filters}
      run={getBodyReportsTrackingCriticalParts}
      poll={() => getBodyReportsLastChangeOnLicense()}
    />
  );
}
