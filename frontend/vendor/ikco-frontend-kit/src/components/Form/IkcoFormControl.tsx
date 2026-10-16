import { FormControl } from "@mui/material";
import type { FormControlProps } from "@mui/material";

//type IkcoFormControlProps = Omit<FormControlProps,"sx" | "style">;

export default function IkcoFormControl(props: FormControlProps) {
  return <FormControl {...props} />;
}
