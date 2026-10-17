import { IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import BodyCriticalSearchPanel from "../BodyCriticalSearchPanel";

/**
 * گزارش اقلام بحرانی (frmRPTBodyCriticalSearch).
 * Backend: POST /api/production-feasibility/body-shop/reports/critical-parts/search (PF.S_BodyCriticalSearch_Web)، دسترسی bodyshop.reports.search.
 * «ردیابی پارامترهای بحرانی» در modal باز می‌شود.
 */
export default function BodyCriticalPartsReport() {
  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">گزارش اقلام بحرانی</IkcoTypography>
      <BodyCriticalSearchPanel mode="report" />
    </IkcoStack>
  );
}
