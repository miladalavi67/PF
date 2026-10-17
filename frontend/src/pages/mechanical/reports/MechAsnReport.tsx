import { IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import MechAsnPanel from "../MechAsnPanel";

/** گزارش ASN های در راه (frm_Rpt_Sap_Asn). همین گزارش از «گزارش ردیابی اقلام بحرانی» نیز به صورت modal باز می‌شود. */
export default function MechAsnReport() {
  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">گزارش ASN های در راه</IkcoTypography>
      <MechAsnPanel />
    </IkcoStack>
  );
}
