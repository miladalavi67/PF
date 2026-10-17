import { IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { useState } from "react";
import FormDialog from "../../components/FormDialog";
import SyncSetManagement from "../synchron/SyncSetManagement";
import MechCriticalRegister from "./MechCriticalRegister";
import MechCriticalSearchPanel from "./MechCriticalSearchPanel";

/**
 * فرم جستجوی اقلام بحرانی (frmCriticalPartSearch). «تغییر در مشخصات اقلام بحرانی» فرم تعریف بحران مکانیکی یا سنکرون را در حالت
 * ویرایش به صورت modal باز می‌کند؛ «تغییر در شرایط تولید» و Log نیز modal هستند.
 */
export default function MechCriticalSearch() {
  const [edit, setEdit] = useState<{ id: number; kind: number } | null>(null);

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">فرم جستجوی اقلام بحرانی</IkcoTypography>
      <MechCriticalSearchPanel onEdit={(id, kind) => setEdit({ id, kind })} />
      <FormDialog open={edit !== null} close={() => setEdit(null)} title="تغییر در مشخصات اقلام بحرانی" maxWidth="xl" closeText="انصراف">
        {edit?.kind === 1 ? <SyncSetManagement editId={edit.id} onDone={() => setEdit(null)} /> : edit ? <MechCriticalRegister editId={edit.id} onDone={() => setEdit(null)} /> : null}
      </FormDialog>
    </IkcoStack>
  );
}
