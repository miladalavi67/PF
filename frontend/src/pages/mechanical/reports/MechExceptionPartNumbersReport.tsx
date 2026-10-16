import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechExceptionsPartNumbersInExceptions } from "../../../server/services/mechExceptionsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechExceptionsGetPartNumbersInExceptionsInputRules, type IMechExceptionsGetPartNumbersInExceptionsInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "filterValues", label: "مقادیر فیلتر (جداشده با ویرگول)", type: "text", required: true },
  { name: "trimId", label: "سالن مونتاژ", type: "lookup", lookupKey: "trim-shops", defaultValue: 1 },
  { name: "filterType", label: "نوع فیلتر", type: "lookup", lookupKey: "exception-filter-types", defaultValue: 1 },
  { name: "partNumber", label: "شماره فنی", type: "text" },
];

/**
 * شماره فنی‌های دارای استثنا.
 * Backend: POST /api/production-feasibility/mechanical/exceptions/part-numbers-report (dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException)، دسترسی mechanical.exceptions.view.
 * فرم سامانه ویندوزی: frmRPT_CriticalPartsProductExceptions.
 */
export default function MechExceptionPartNumbersReport() {
  return (
    <ReportPage<IMechExceptionsGetPartNumbersInExceptionsInput>
      title="شماره فنی‌های دارای استثنا"
      permission={ApiPermissions.mechExceptions.view}
      filters={filters}
      run={getMechExceptionsPartNumbersInExceptions}
      rules={mechExceptionsGetPartNumbersInExceptionsInputRules}
    />
  );
}
