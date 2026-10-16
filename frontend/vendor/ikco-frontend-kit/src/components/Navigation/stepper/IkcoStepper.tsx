import { Stepper } from "@mui/material";
import type { StepperProps } from "@mui/material";

type IkcoStepperProps = Omit<StepperProps, "sx" | "style">;

export default function IkcoStepper(props: IkcoStepperProps) {
  return <Stepper {...props} />;
}
