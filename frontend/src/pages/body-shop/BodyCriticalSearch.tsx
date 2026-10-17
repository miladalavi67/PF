import { IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import { useState } from "react";
import BodyCriticalSearchPanel from "./BodyCriticalSearchPanel";
import BodyCriticalEditDialog from "./dialogs/BodyCriticalEditDialog";

/**
 * جستجو و مدیریت اقلام بحرانی بدنه‌سازی (frmBodyCriticalSearch): جستجو با فیلترها، مشاهده محصولات، «تغییر در مشخصات اقلام بحرانی»
 * (modal فرم ثبت در حالت ویرایش)، «تغییر در شرایط تولید»، «ردیابی پارامترهای بحرانی»، LOG و «خروج از لیست اقلام بحرانی».
 */
export default function BodyCriticalSearch() {
  const [editId, setEditId] = useState(0);

  return (
    <IkcoStack spacing={2}>
      <IkcoTypography variant="h6">جستجوی اقلام بحرانی</IkcoTypography>
      <BodyCriticalSearchPanel mode="search" onEdit={setEditId} />
      <BodyCriticalEditDialog open={editId > 0} close={() => setEditId(0)} criticalPartId={editId} />
    </IkcoStack>
  );
}
