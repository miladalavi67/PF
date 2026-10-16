import { FormLabel } from "@mui/material";
import type { FormLabelProps } from "@mui/material";

//type IkcoFormLabelProps = Omit<FormLabelProps,"sx" | "style">;

export default function IkcoFormLabel(props: FormLabelProps) {
  return <FormLabel {...props} />;
}
