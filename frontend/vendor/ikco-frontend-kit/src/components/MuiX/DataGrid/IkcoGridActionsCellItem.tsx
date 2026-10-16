import type { GridActionsCellItemProps } from "@mui/x-data-grid";
import { GridActionsCellItem } from "@mui/x-data-grid";

// type IkcoGridActionsCellItem = Omit<GridActionsCellItemProps, "sx" | "style">;

export default function IkcoGridActionsCellItem(
  props: GridActionsCellItemProps,
) {
  return <GridActionsCellItem {...props} />;
}
