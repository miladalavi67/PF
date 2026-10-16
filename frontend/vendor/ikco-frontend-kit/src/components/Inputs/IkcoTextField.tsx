import { TextField, type TextFieldProps } from "@mui/material";

type IkcoTextFieldProps = Omit<
  TextFieldProps,
  "variant" | "size" | "sx" | "style"
>;

export default function IkcoTextField({
  fullWidth = true,
  InputLabelProps,
  ...rest
}: IkcoTextFieldProps) {
  return (
    <TextField
      {...rest}
      fullWidth={fullWidth}
      size="small"
      variant="outlined"
      InputLabelProps={{
        shrink: true,
        ...InputLabelProps,
      }}
    />
  );
}