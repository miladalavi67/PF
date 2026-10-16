import type {
  GridActionsColDef,
  GridSingleSelectColDef,
  GridValidRowModel,
} from "@mui/x-data-grid";
import type { GridBaseColDef } from "@mui/x-data-grid/internals";

// eslint-disable-next-line @typescript-eslint/no-explicit-any
export type IkcoGridColDef<R extends GridValidRowModel = any, V = any, F = V> =
  | GridBaseColDef<R, V, F>
  | GridActionsColDef<R, V, F>
  | GridSingleSelectColDef<R, V, F>;
