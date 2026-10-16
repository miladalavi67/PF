import { ToggleButton } from "@mui/material";
import type { ToggleButtonProps } from "@mui/material";

type IkcoToggleButtonProps = Omit<ToggleButtonProps, "sx" | "style">;

export default function IkcoToggleButton(props: IkcoToggleButtonProps) {
  return <ToggleButton {...props} />;
}
