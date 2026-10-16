import { StepLabel } from "@mui/material";
import type { StepLabelProps } from "@mui/material";

type IkcoStepLabelProps = Omit<StepLabelProps, "sx" | "style">;

export default function IkcoStepLabel(props: IkcoStepLabelProps) {
  return <StepLabel {...props} />;
}
