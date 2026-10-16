import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsProductionPossibility } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetProductionPossibilityInputRules, type IMechReportsGetProductionPossibilityInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "saloonId", label: "سالن‌ها", type: "multilookup", lookupKey: "trim-shops" },
  { name: "crisisTypeId", label: "شرایط تولید", type: "lookup", lookupKey: "crisis-types" },
  { name: "criticalPartTypeId", label: "نوع قطعه بحران", type: "number", defaultValue: -1 },
  { name: "subClassId", label: "زیرکلاس", type: "number", defaultValue: 0 },
  { name: "partNo", label: "شماره فنی", type: "text" },
  { name: "carModelId", label: "مدل", type: "number", defaultValue: 0 },
  { name: "carClassId", label: "کلاس", type: "number", defaultValue: 0 },
];

/**
 * گزارش امکان تولید.
 * Backend: POST /api/production-feasibility/mechanical/reports/production-possibility (dbo.S_RptCriticalPartsProduction_Mechanical)، دسترسی mechanical.reports.productionpossibility.
 * فرم سامانه ویندوزی: frmRPTProductionPossibility / frmRPTProdpossibility.
 */
export default function MechProductionPossibilityReport() {
  return (
    <ReportPage<IMechReportsGetProductionPossibilityInput>
      title="گزارش امکان تولید"
      permission={ApiPermissions.mechReports.productionpossibility}
      filters={filters}
      run={getMechReportsProductionPossibility}
      rules={mechReportsGetProductionPossibilityInputRules}
    />
  );
}
