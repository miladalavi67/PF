import {
  IkcoButton,
  IkcoCardContent,
  IkcoDialog,
  IkcoDialogActions,
  IkcoDialogTitle,
  IkcoTypography,
} from "@ikco/frontend-kit";

interface ConfirmDialogProps {
  open: boolean;
  close: (open: boolean) => void;
  onConfirm: () => void | Promise<void>;
  title?: string;
  message: string;
  confirmText?: string;
  loading?: boolean;
}

/** تأیید عملیات مخرب (غیرفعال‌سازی، حذف، تعویض) — الگوی IkcoDialog استاندارد */
export default function ConfirmDialog({
  open,
  close,
  onConfirm,
  title = "تأیید عملیات",
  message,
  confirmText = "تأیید",
  loading = false,
}: ConfirmDialogProps) {
  return (
    <IkcoDialog open={open} maxWidth="xs" fullWidth onClose={() => close(false)}>
      <IkcoDialogTitle>{title}</IkcoDialogTitle>
      <IkcoCardContent>
        <IkcoTypography>{message}</IkcoTypography>
      </IkcoCardContent>
      <IkcoDialogActions>
        <IkcoButton onClick={() => close(false)}>انصراف</IkcoButton>
        <IkcoButton variant="contained" color="error" loading={loading} onClick={() => onConfirm()}>
          {confirmText}
        </IkcoButton>
      </IkcoDialogActions>
    </IkcoDialog>
  );
}
