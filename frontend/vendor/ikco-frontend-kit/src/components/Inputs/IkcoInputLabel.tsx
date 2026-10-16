import { InputLabel } from "@mui/material";
import type { InputLabelProps } from "@mui/material";

type IkcoInputLabelProps = Omit<InputLabelProps, "sx" | "style">;

export default function IkcoInputLabel(props: IkcoInputLabelProps) {
  return <InputLabel {...props} />;
}
