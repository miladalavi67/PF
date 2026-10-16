import { FormControlLabel } from "@mui/material";
import type { FormControlLabelProps } from "@mui/material";

//type IkcoFormControlLabelProps = Omit<FormControlLabelProps,"sx" | "style">;

export default function IkcoFormControlLabel(props: FormControlLabelProps) {
  return <FormControlLabel {...props} />;
}
