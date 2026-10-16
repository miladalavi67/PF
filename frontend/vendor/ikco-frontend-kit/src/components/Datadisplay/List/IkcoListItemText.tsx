import { ListItemText } from "@mui/material";
import type { ListItemTextProps } from "@mui/material";

type IkcoListItemTextProps = Omit<ListItemTextProps, "sx" | "style">;

export default function IkcoListItemText(props: IkcoListItemTextProps) {
  return <ListItemText {...props} />;
}
