import ReportPage, { type IReportFilter } from "../../../components/ReportPage";
import { getMechReportsAsnReport } from "../../../server/services/mechReportsService";
import { ApiPermissions } from "../../../types/permissions";
import { mechReportsGetAsnReportInputRules, type IMechReportsGetAsnReportInput } from "../../../types/Mechanical";

const filters: IReportFilter[] = [
  { name: "partNumber", label: "شماره فنی", type: "text" },
  { name: "fromDate", label: "از تاریخ", type: "date" },
  { name: "toDate", label: "تا تاریخ", type: "date" },
  { name: "type", label: "نوع", type: "number", defaultValue: 1 },
];

/**
 * گزارش ASN.
 * Backend: GET /api/production-feasibility/mechanical/reports/asn (dbo.S_ASNReport)، دسترسی mechanical.reports.sap.
 * فرم سامانه ویندوزی: frm_Rpt_Sap_Asn / frmIncommingOrdersASNDetail.
 */
export default function MechAsnReport() {
  return (
    <ReportPage<IMechReportsGetAsnReportInput>
      title="گزارش ASN"
      permission={ApiPermissions.mechReports.sap}
      filters={filters}
      run={getMechReportsAsnReport}
      rules={mechReportsGetAsnReportInputRules}
    />
  );
}
