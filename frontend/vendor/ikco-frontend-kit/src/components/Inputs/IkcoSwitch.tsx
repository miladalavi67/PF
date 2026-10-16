import { Switch, FormControlLabel } from "@mui/material";
import type { SwitchProps } from "@mui/material";
import type { ReactNode } from "react";

interface IkcoSwitchProps extends Omit<SwitchProps, "sx"> {
  label?: ReactNode;
}

export default function IkcoSwitch({
  label,
  ...props
}: IkcoSwitchProps) {
  return (
    <FormControlLabel
      control={<Switch {...props} />}
      label={label}
    />
  );
}