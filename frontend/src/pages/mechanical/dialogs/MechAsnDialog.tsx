import FormDialog from "../../../components/FormDialog";
import MechAsnPanel from "../MechAsnPanel";

/** گزارش ASN های در راه (frm_Rpt_Sap_Asn) به صورت modal از «گزارش ردیابی اقلام بحرانی» */
export default function MechAsnDialog({ open, close }: { open: boolean; close: (open: boolean) => void }) {
  return (
    <FormDialog open={open} close={close} title="گزارش ASN های در راه" maxWidth="xl" closeText="بازگشت">
      {open && <MechAsnPanel />}
    </FormDialog>
  );
}
