import { CardContent } from "@mui/material";
import type { CardContentProps } from "@mui/material";

type IkcoCardContentProps = Omit<CardContentProps, "sx" | "style">;

export default function IkcoCardContent(props: IkcoCardContentProps) {
  return <CardContent {...props} />;
}
