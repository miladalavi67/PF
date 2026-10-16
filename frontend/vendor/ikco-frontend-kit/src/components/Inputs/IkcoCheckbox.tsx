import { Checkbox, type CheckboxProps, FormControlLabel } from "@mui/material";

type IkcoCheckboxProps = Omit<CheckboxProps, "sx" | "style" | "size"> & {
  label?: string;
};

export default function IkcoCheckbox({ label, ...rest }: IkcoCheckboxProps) {
  const checkboxElement = <Checkbox {...rest} />;

  if (!label) return checkboxElement;

  return <FormControlLabel control={checkboxElement} label={label} />;
}
