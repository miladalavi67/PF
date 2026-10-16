
import Button from "@mui/material/Button";
import { type ButtonProps } from "@mui/material/Button";

type IkcoButtonProps = Omit<ButtonProps, "style" | "sx">;

export default function IkcoButton(props: IkcoButtonProps) {
  return <Button {...props} sx={{ height: 40, borderRadius: "4px" }} />;
}
