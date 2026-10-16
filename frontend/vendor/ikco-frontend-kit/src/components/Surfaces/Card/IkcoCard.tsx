import { Card } from "@mui/material";
import type { CardProps } from "@mui/material";

type IkcoCardProps = Omit<CardProps, "sx" | "style">;

export default function IkcoCard(props: IkcoCardProps) {
  return <Card {...props} />;
}
