import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getBodyReportsProductsOfSites } from "../../../server/services/bodyReportsService";
import { ApiPermissions } from "../../../types/permissions";

const filters: IReportFilter[] = [

];

/**
 * محصولات به تفکیک خطوط بدنه‌سازی.
 * Backend: GET /api/production-feasibility/body-shop/reports/products-of-sites (PF.S_VINBIWSite_CarModel_SelectAll)، دسترسی bodyshop.reports.productsofsites.
 * فرم سامانه ویندوزی: frmRPTGetProductsOfBIWSite.
 */
export default function BodyProductsOfSitesReport() {
  return (
    <ReportPage<Record<string, never>>
      title="محصولات به تفکیک خطوط بدنه‌سازی"
      permission={ApiPermissions.bodyReports.productsofsites}
      filters={filters}
      run={() => getBodyReportsProductsOfSites()}
      autoRun
    />
  );
}
