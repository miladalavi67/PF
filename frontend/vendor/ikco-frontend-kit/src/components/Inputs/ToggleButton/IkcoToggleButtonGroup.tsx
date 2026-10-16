import { ToggleButtonGroup } from "@mui/material";
import type { ToggleButtonGroupProps } from "@mui/material";

type IkcoToggleButtonGroupProps = Omit<ToggleButtonGroupProps, "sx" | "style">;

export default function IkcoToggleButtonGroup(
  props: IkcoToggleButtonGroupProps,
) {
  return <ToggleButtonGroup {...props} />;
}

