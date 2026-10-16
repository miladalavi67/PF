import { Link } from "@mui/material";
import type { LinkProps } from "@mui/material";

type IkcoLinkProps = Omit<LinkProps, "sx" | "style">;

export default function IkcoLink(props: IkcoLinkProps) {
  return <Link {...props} />;
}
