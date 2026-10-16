import { Step } from "@mui/material";
import type { StepProps } from "@mui/material";

type IkcoStepProps = Omit<StepProps, "sx" | "style">;

export default function IkcoStep(props: IkcoStepProps) {
  return <Step {...props} />;
}
