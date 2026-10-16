import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsProductionPossibilityWithLicense } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetProductionPossibilityWithLicenseInputRules, type IMechReportsGetProductionPossibilityWithLicenseInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "saloonId", label: "سالن‌ها", type: "multilookup", lookupKey: "trim-shops" },
  { name: "crisisTypeId", label: "شرایط تولید", type: "lookup", lookupKey: "crisis-types" },
  { name: "criticalPartTypeId", label: "نوع قطعه بحران", type: "number", defaultValue: -1 },
  { name: "subClassId", label: "زیرکلاس", type: "number", defaultValue: 0 },
  { name: "carModelId", label: "مدل", type: "number", defaultValue: 0 },
  { name: "carClassId", label: "کلاس", type: "number", defaultValue: 0 },
];

/**
 * گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده.
 * Backend: POST /api/production-feasibility/mechanical/reports/production-possibility-with-license (dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5)، دسترسی mechanical.reports.productionpossibility.
 * فرم سامانه ویندوزی: frmRPTProductionPossibility (با مجوز).
 */
export default function MechProductionPossibilityWithLicenseReport() {
  return (
    <ReportPage<IMechReportsGetProductionPossibilityWithLicenseInput>
      title="گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده"
      permission={ApiPermissions.mechReports.productionpossibility}
      filters={filters}
      run={getMechReportsProductionPossibilityWithLicense}
      rules={mechReportsGetProductionPossibilityWithLicenseInputRules}
    />
  );
}
