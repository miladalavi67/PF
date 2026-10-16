import { IconButton } from "@mui/material";
import { type IconButtonProps } from "@mui/material/IconButton";
// type IkcoButtonProps = Omit<IconButtonProps, "style" | "sx">;
export default function IkcoIconButton(props: IconButtonProps) {
  return <IconButton {...props} />;
}
