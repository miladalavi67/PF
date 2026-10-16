import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsProductionPossibilityByTrimShop } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetProductionPossibilityByTrimShopInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "trimShopId", label: "سالن مونتاژ", type: "lookup", lookupKey: "trim-shops", defaultValue: 1 },
  { name: "prodState", label: "وضعیت تولید", type: "lookup", lookupKey: "prod-states", defaultValue: 1 },
];

/**
 * امکان تولید به تفکیک سالن مونتاژ.
 * Backend: GET /api/production-feasibility/mechanical/reports/production-possibility-by-trim-shop (dbo.S_RPT_DESC_ProdPossibilityByTrimShop)، دسترسی mechanical.reports.productionpossibility.
 * فرم سامانه ویندوزی: frmRPTProdpossibility.
 */
export default function MechProductionPossibilityByTrimShopReport() {
  return (
    <ReportPage<IMechReportsGetProductionPossibilityByTrimShopInput>
      title="امکان تولید به تفکیک سالن مونتاژ"
      permission={ApiPermissions.mechReports.productionpossibility}
      filters={filters}
      run={getMechReportsProductionPossibilityByTrimShop}
    />
  );
}
