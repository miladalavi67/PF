import { Stack } from "@mui/material";
import type { StackProps } from "@mui/material";
import type { ReactNode } from "react";

interface IkcoStackProps
  extends Omit<StackProps, "sx" | "style"> {
  children: ReactNode;
}

export default function IkcoStack({
  children,
  ...props
}: IkcoStackProps) {
  return <Stack {...props}>{children}</Stack>;
}