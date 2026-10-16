import { Divider } from "@mui/material";
import type { DividerProps } from "@mui/material";

type IkcoDividerProps = Omit<DividerProps, "sx" | "style">

export default function IkcoDivider(
  props: IkcoDividerProps
) {
  return <Divider {...props} />;
}