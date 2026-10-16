import { Backdrop } from "@mui/material";
import type { BackdropProps } from "@mui/material";

type IkcoBackdropProps = Omit<BackdropProps, "sx" | "style">;

export default function IkcoBackdrop(props: IkcoBackdropProps) {
  return <Backdrop {...props} />;
}
