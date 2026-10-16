import { Box } from "@mui/material";
import type { BoxProps } from "@mui/material";
import type { ReactNode } from "react";

interface IkcoBoxProps
  extends Omit<BoxProps, "sx" | "style"> {
  children: ReactNode;
}

export default function IkcoBox({
  children,
  ...props
}: IkcoBoxProps) {
  return <Box {...props}>{children}</Box>;
}