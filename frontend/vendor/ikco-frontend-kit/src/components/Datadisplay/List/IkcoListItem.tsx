import { ListItem } from "@mui/material";
import type { ListItemProps } from "@mui/material";

type IkcoListItemProps = Omit<ListItemProps, "sx" | "style">;

export default function IkcoListItem(props: IkcoListItemProps) {
  return <ListItem {...props} />;
}
