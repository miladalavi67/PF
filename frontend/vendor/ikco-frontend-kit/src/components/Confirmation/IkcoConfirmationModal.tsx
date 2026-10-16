import {
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Typography,
  Box,
  useTheme,
} from "@mui/material";
import WarningAmberRoundedIcon from "@mui/icons-material/WarningAmberRounded";
import ErrorOutlineRoundedIcon from "@mui/icons-material/ErrorOutlineRounded";
import InfoOutlinedIcon from "@mui/icons-material/InfoOutlined";
import IkcoButton from "../Inputs/IkcoButton";


// تعریف انواع وضعیت‌های مودال
export type ConfirmationType = "danger" | "warning" | "info";

interface IkcoConfirmationModalProps {
  open: boolean;
  onClose: () => void;
  onConfirm: () => void;
  loading?: boolean;
  
  type?: ConfirmationType; // نوع مودال برای تعیین رنگ و آیکون
  title?: string;
  message?: string;
  itemName?: string;

  confirmText?: string;
  cancelText?: string;
  
  icon?: React.ReactNode; // قابلیت پاس دادن آیکون سفارشی
}

export const IkcoConfirmationModal = ({
  open,
  onClose,
  onConfirm,
  loading = false,
  type = "danger",
  title,
  message,
  itemName,
  confirmText,
  cancelText = "لغو و بازگشت",
  icon,
}: IkcoConfirmationModalProps) => {
  const theme = useTheme();

  // تنظیمات بصری بر اساس نوع (Type)
  const config = {
    danger: {
      defaultTitle: "تأیید حذف",
      defaultConfirmText: "تایید و حذف",
      color: theme.palette.error.main,
      icon: <ErrorOutlineRoundedIcon sx={{ fontSize: 64 }} />,
      buttonColor: "error" as const,
    },
    warning: {
      defaultTitle: "هشدار",
      defaultConfirmText: "متوجه شدم",
      color: theme.palette.warning.main,
      icon: <WarningAmberRoundedIcon sx={{ fontSize: 64 }} />,
      buttonColor: "warning" as const,
    },
    info: {
      defaultTitle: "اطلاعیه",
      defaultConfirmText: "تایید",
      color: theme.palette.info.main,
      icon: <InfoOutlinedIcon sx={{ fontSize: 64 }} />,
      buttonColor: "primary" as const,
    },
  }[type];

  const finalTitle = title ?? config.defaultTitle;
  const finalConfirmText = confirmText ?? config.defaultConfirmText;
  const finalMessage =
    message ??
    (type === "danger"
      ? `آیا از حذف ${itemName ? `"${itemName}"` : "این مورد"} مطمئن هستید؟`
      : "");

  return (
    <Dialog
      open={open}
      onClose={loading ? undefined : onClose}
      fullWidth
      maxWidth="xs"
      PaperProps={{
        sx: {
          borderRadius: 3, // بر اساس سیستم طراحی ایکو
          p: 2,
        },
      }}
    >
      <DialogTitle sx={{ textAlign: "center", pb: 1 }}>
        <Box
          sx={{
            display: "flex",
            justifyContent: "center",
            mb: 2,
            color: config.color, // استفاده از رنگ تم
          }}
        >
          {icon ?? config.icon}
        </Box>
        <Typography variant="h6" fontWeight="bold">
          {finalTitle}
        </Typography>
      </DialogTitle>

      <DialogContent sx={{ textAlign: "center", pb: 2 }}>
        <Typography variant="body1" color="text.secondary">
          {finalMessage}
        </Typography>
      </DialogContent>

      <DialogActions
        sx={{
          justifyContent: "center",
          gap: 2,
          pb: 2,
          px: 3,
          flexDirection: { xs: "column-reverse", sm: "row" }, // ریسپانسیو بودن دکمه‌ها در موبایل
        }}
      >
        <IkcoButton
          variant="outlined"
          color="secondary"
          onClick={onClose}
          disabled={loading}
          fullWidth
        >
          {cancelText}
        </IkcoButton>

        <IkcoButton
          variant="contained"
          color={config.buttonColor}
          onClick={onConfirm}
          loading={loading} // اگر IkcoButton از لودینگ پشتیبانی می‌کند
          disabled={loading}
          fullWidth
        >
          {finalConfirmText}
        </IkcoButton>
      </DialogActions>
    </Dialog>
  );
};
