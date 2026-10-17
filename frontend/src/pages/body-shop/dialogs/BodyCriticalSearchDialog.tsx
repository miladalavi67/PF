import FormDialog from "../../../components/FormDialog";
import BodyCriticalSearchPanel from "../BodyCriticalSearchPanel";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** بحران انتخاب‌شده («تایید» یا «تغییر در مشخصات») برای ویرایش در فرم ثبت */
  onPick: (criticalPartId: number) => void;
}

/** جستجوی اقلام بحرانی (frmBodyCriticalSearch) به صورت modal از فرم ثبت بحران */
export default function BodyCriticalSearchDialog({ open, close, onPick }: Props) {
  return (
    <FormDialog open={open} close={close} title="جستجوی اقلام بحرانی" maxWidth="xl" closeText="خروج">
      {open && <BodyCriticalSearchPanel mode="search" onPick={onPick} onEdit={onPick} />}
    </FormDialog>
  );
}
