import { Radio } from "@mui/material";
import type { RadioProps } from "@mui/material";

type IkcoRadioProps = Omit<RadioProps, "sx" | "style">;

export default function IkcoRadio(props: IkcoRadioProps) {
  return <Radio {...props} />;
}
