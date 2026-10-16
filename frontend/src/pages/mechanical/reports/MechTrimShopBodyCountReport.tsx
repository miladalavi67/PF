import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsTrimShopBodyCount } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { type IMechReportsGetTrimShopBodyCountInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "carId", label: "شناسه محصول (CarId)", type: "number", required: true },
];

/**
 * تعداد بدنه‌های ورودی به مونتاژ.
 * Backend: GET /api/production-feasibility/mechanical/reports/trim-shop-body-count (dbo.S_TrimShopBodyCount_List)، دسترسی mechanical.reports.conflicts.
 * فرم سامانه ویندوزی: frmTrimShopBodyCount.
 */
export default function MechTrimShopBodyCountReport() {
  return (
    <ReportPage<IMechReportsGetTrimShopBodyCountInput>
      title="تعداد بدنه‌های ورودی به مونتاژ"
      permission={ApiPermissions.mechReports.conflicts}
      filters={filters}
      run={getMechReportsTrimShopBodyCount}
    />
  );
}
