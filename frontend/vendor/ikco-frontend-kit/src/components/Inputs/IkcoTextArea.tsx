import { TextField } from "@mui/material";
import type { TextFieldProps } from "@mui/material";

type ResizeMode = "none" | "vertical" | "horizontal" | "both";

interface IkcoTextAreaProps
  extends Omit<TextFieldProps, "multiline" | "sx"> {
  rows?: number;
  resize?: ResizeMode;
}

export default function IkcoTextArea({
  rows = 4,
  resize = "vertical",
  size = "small",
  variant = "outlined",
  InputProps,
  ...props
}: IkcoTextAreaProps) {
  return (
    <TextField
      multiline
      rows={rows}
      size={size}
      variant={variant}
      fullWidth
      InputProps={{
        ...InputProps,
        sx: {
          "& textarea": {
            resize,
          },
        },
      }}
      {...props}
    />
  );
}