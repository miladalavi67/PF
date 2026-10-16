import { Table } from "@mui/material";
import type {TableProps } from "@mui/material";

//type IkcoTableProps = Omit<TableProps,"sx" | "style">;

export default function IkcoTabel(props: TableProps) {
  return <Table {...props} />;
}
