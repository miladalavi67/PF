import { List } from "@mui/material";
import type {ListProps } from "@mui/material";

type IkcoListProps = Omit<ListProps,"sx" | "style">;

export default function IkcoList(props: IkcoListProps) {
  return <List {...props} />;
}
