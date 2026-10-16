import { Snackbar } from "@mui/material";
import type { SnackbarProps } from "@mui/material";

//type IkcoSnackbarProps = Omit<SnackbarProps,"sx" | "style">;

export default function IkcoSnackbar(props: SnackbarProps) {
  return <Snackbar {...props} />;
}
