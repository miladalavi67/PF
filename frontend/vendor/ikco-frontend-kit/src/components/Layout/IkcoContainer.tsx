import { Container } from "@mui/material";
import type { ContainerProps } from "@mui/material";
import type { ReactNode } from "react";

interface IkcoContainerProps
  extends Omit<ContainerProps, "sx" | "style"> {
  children: ReactNode;
}

export default function IkcoContainer({
  children,
  ...props
}: IkcoContainerProps) {
  return (
    <Container {...props}>
      {children}
    </Container>
  );
}