import { Tab } from "@mui/material";
import type { TabProps } from "@mui/material";

type IkcoTabProps = Omit<TabProps, "sx" | "style">;

export default function IkcoTab(props: IkcoTabProps) {
  return <Tab {...props} />;
}
