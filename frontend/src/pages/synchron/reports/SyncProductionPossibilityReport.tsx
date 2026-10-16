import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getSyncReportsProductionPossibility } from "../../../server/services/syncReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { syncReportsGetProductionPossibilityInputRules, type ISyncReportsGetProductionPossibilityInput } from "../../../types/Synchron";

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
 * گزارش امکان تولید سنکرون.
 * Backend: POST /api/production-feasibility/synchron/reports/production-possibility (dbo.S_RptCriticalPartsProduction_Synchron)، دسترسی synchron.reports.productionpossibility.
 * فرم سامانه ویندوزی: frmRPTProductionPossibility (سنکرون).
 */
export default function SyncProductionPossibilityReport() {
  return (
    <ReportPage<ISyncReportsGetProductionPossibilityInput>
      title="گزارش امکان تولید سنکرون"
      permission={ApiPermissions.syncReports.productionpossibility}
      filters={filters}
      run={getSyncReportsProductionPossibility}
      rules={syncReportsGetProductionPossibilityInputRules}
    />
  );
}
