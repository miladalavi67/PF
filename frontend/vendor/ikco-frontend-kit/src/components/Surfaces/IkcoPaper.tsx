import { Paper } from "@mui/material";
import type { PaperProps } from "@mui/material";

type IkcoPaperProps = Omit<PaperProps, "sx" | "style">;

export default function IkcoPaper(props: IkcoPaperProps) {
  return <Paper {...props} />;
}
