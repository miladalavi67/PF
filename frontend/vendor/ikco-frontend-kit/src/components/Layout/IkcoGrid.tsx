import { Grid } from "@mui/material";
import type { GridProps } from "@mui/material";
import type { ReactNode } from "react";

interface IkcoGridProps
  extends Omit<GridProps, "sx" | "style"> {
  children: ReactNode;
}

export default function IkcoGrid({
  children,
  ...props
}: IkcoGridProps) {
  return <Grid {...props}>{children}</Grid>;
}