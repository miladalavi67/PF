import FormDialog from "../../../components/FormDialog";
import MechCriticalSearchPanel from "../MechCriticalSearchPanel";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  /** بحران انتخاب‌شده («تایید») برای ویرایش در فرم فراخوان */
  onPick: (criticalPartId: number, kind: number) => void;
  /** دسته‌بندی اولیه قطعات (-1 همه، 0 مکانیکی، 1 سنکرون) */
  kind?: number;
}

/** فرم جستجوی اقلام بحرانی (frmCriticalPartSearch) به صورت modal از فرم‌های تعریف بحران مکانیکی/سنکرون */
export default function MechCriticalSearchDialog({ open, close, onPick, kind = 0 }: Props) {
  return (
    <FormDialog open={open} close={close} title="فرم جستجوی اقلام بحرانی" maxWidth="xl">
      {open && <MechCriticalSearchPanel initialKind={kind} onPick={onPick} />}
    </FormDialog>
  );
}
