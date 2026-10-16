import { Chip } from "@mui/material";
import type { ChipProps } from "@mui/material";

type IkcoChipVariant = "filled" | "outlined";

type IkcoChipStatus = "default" | "success" | "warning" | "error" | "info";

interface IkcoChipProps extends Omit<ChipProps, "color" | "variant" | "sx"> {
  status?: IkcoChipStatus;
  variant?: IkcoChipVariant;
}

const statusColorMap: Record<IkcoChipStatus, ChipProps["color"]> = {
  default: "default",
  success: "success",
  warning: "warning",
  error: "error",
  info: "info",
};

export default function IkcoChip({
  status = "default",
  variant = "filled",
  ...props
}: IkcoChipProps) {
  return <Chip variant={variant} color={statusColorMap[status]} {...props} />;
}
