import { MenuItem } from "@mui/material";
import type { MenuItemProps } from "@mui/material";

type IkcoMenuItemProps = Omit<MenuItemProps, "sx" | "style">;

export default function IkcoMenuItem(props: IkcoMenuItemProps) {
  return <MenuItem {...props} />;
}
