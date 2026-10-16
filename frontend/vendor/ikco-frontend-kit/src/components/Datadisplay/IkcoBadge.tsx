import { Badge } from "@mui/material";
import type { BadgeProps } from "@mui/material";

type IkcoBadgeProps = Omit<BadgeProps, "sx" | "style">

export default function IkcoBadge(
  props: IkcoBadgeProps
) {
  return <Badge {...props} />;
}