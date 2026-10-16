import { Autocomplete } from "@mui/material";
import type { AutocompleteProps } from "@mui/material";

type IkcoAutocompleteProps<
  T,
  Multiple extends boolean | undefined,
  DisableClearable extends boolean | undefined,
  FreeSolo extends boolean | undefined
> = Omit<
  AutocompleteProps<T, Multiple, DisableClearable, FreeSolo>,
  "sx" | "style"
>;

export default function IkcoAutocomplete<
  T,
  Multiple extends boolean | undefined = false,
  DisableClearable extends boolean | undefined = false,
  FreeSolo extends boolean | undefined = false
>(
  props: IkcoAutocompleteProps<
    T,
    Multiple,
    DisableClearable,
    FreeSolo
  >
) {
  return <Autocomplete {...props} />;
}