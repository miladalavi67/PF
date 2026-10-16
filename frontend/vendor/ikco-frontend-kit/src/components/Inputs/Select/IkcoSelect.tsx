
import { Select } from "@mui/material";
import type { SelectProps } from "@mui/material";

type IkcoSelectProps<T = unknown> = Omit<
  SelectProps<T>,
  "sx" | "style"
>;

export default function IkcoSelect<T = unknown>(
  props: IkcoSelectProps<T>
) {
  return <Select<T> {...props} />;
}