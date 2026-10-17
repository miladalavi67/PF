import FormDialog from "../../../components/FormDialog";
import BodyCriticalRegister from "../BodyCriticalRegister";

interface Props {
  open: boolean;
  close: (open: boolean) => void;
  criticalPartId: number;
  onSaved?: () => void;
}

/** «تغییر در مشخصات اقلام بحرانی»: فرم ثبت بحران (frmBodyCriticalParts) در حالت ویرایش به صورت modal */
export default function BodyCriticalEditDialog({ open, close, criticalPartId, onSaved }: Props) {
  return (
    <FormDialog open={open} close={close} title="تغییر در مشخصات اقلام بحرانی" maxWidth="xl" closeText="انصراف">
      {open && (
        <BodyCriticalRegister
          editId={criticalPartId}
          onDone={() => {
            onSaved?.();
            close(false);
          }}
        />
      )}
    </FormDialog>
  );
}
