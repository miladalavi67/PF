import { Tooltip } from "@mui/material";
import type { TooltipProps } from "@mui/material";

type IkcoTooltipProps = Omit<TooltipProps, "sx" | "style">

export default function IkcoTooltip(
  props: IkcoTooltipProps
) {
  return <Tooltip {...props} />;
}