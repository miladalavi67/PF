import { IkcoAutocomplete, IkcoTextField } from "@ikco/frontend-kit";
import { useLookup } from "../hooks/useLookup";
import type { ILookupRow } from "../types/Lookups";

interface LookupMultiSelectProps {
  lookupKey: string;
  label: string;
  /** شناسه‌های انتخاب‌شده */
  value: number[];
  onChange: (value: number[]) => void;
  parentId?: number | null;
  helperText?: string;
  disabled?: boolean;
}

/** انتخاب چندگانه از فهرست مرجع (مثلاً سالن‌ها)؛ رشته ویرگولی رویه‌ها با ids.join(",") ساخته می‌شود. */
export default function LookupMultiSelect({ lookupKey, label, value, onChange, parentId, helperText, disabled }: LookupMultiSelectProps) {
  const { rows, loading } = useLookup(lookupKey, parentId);
  const selected = rows.filter((r) => value.includes(r.id));

  return (
    <IkcoAutocomplete<ILookupRow, true>
      multiple
      options={rows}
      value={selected}
      loading={loading}
      disabled={disabled}
      disableCloseOnSelect
      getOptionLabel={(o) => (o.code ? `${o.code} - ${o.title}` : o.title)}
      isOptionEqualToValue={(a, b) => a.id === b.id}
      onChange={(_, items) => onChange(items.map((i) => i.id))}
      renderInput={(params) => <IkcoTextField {...params} label={label} helperText={helperText} />}
      noOptionsText="موردی یافت نشد"
      loadingText="در حال بارگذاری..."
    />
  );
}
