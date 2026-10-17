import { IkcoButton, IkcoCardContent, IkcoDialog, IkcoDialogActions, IkcoDialogTitle, IkcoStack } from "@ikco/frontend-kit";
import type { ReactNode } from "react";

interface FormDialogProps {
  open: boolean;
  close: (open: boolean) => void;
  title: string;
  children: ReactNode;
  /** دکمه‌های عملیاتی فرم (پیش از «خروج») */
  actions?: ReactNode;
  maxWidth?: "sm" | "md" | "lg" | "xl";
  /** عنوان دکمه بستن (پیش‌فرض «خروج» مانند فرم‌های ویندوزی) */
  closeText?: string;
}

/** قالب مشترک modal برای فرم‌هایی که در سامانه ویندوزی از دکمه‌های فرم دیگر باز می‌شدند (عنوان، محتوا، دکمه‌ها و «خروج») */
export default function FormDialog({ open, close, title, children, actions, maxWidth = "lg", closeText = "خروج" }: FormDialogProps) {
  return (
    <IkcoDialog open={open} maxWidth={maxWidth} fullWidth onClose={() => close(false)}>
      <IkcoDialogTitle>{title}</IkcoDialogTitle>
      <IkcoCardContent>{children}</IkcoCardContent>
      <IkcoDialogActions>
        <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap justifyContent="flex-end">
          {actions}
          <IkcoButton onClick={() => close(false)}>{closeText}</IkcoButton>
        </IkcoStack>
      </IkcoDialogActions>
    </IkcoDialog>
  );
}
