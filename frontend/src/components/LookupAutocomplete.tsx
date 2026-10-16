import { IkcoAutocomplete, IkcoTextField } from "@ikco/frontend-kit";
import { useLookup } from "../hooks/useLookup";
import type { ILookupRow } from "../types/Lookups";

interface LookupAutocompleteProps {
  /** کلید فهرست مرجع Backend (مثلاً car-groups، trim-shops) */
  lookupKey: string;
  label: string;
  /** شناسه انتخاب‌شده */
  value: number | null | undefined;
  onChange: (value: number | null, row: ILookupRow | null) => void;
  /** شناسه والد برای فهرست‌های وابسته (مدل ← خانواده) */
  parentId?: number | null;
  /** فهرست وابسته است و بدون والد خالی می‌ماند */
  requireParent?: boolean;
  error?: boolean;
  helperText?: string;
  disabled?: boolean;
}

/**
 * انتخاب از فهرست‌های مرجع Backend — جایگزین uctlLookUp / DropDownList سامانه ویندوزی.
 * نمایش «کد - شرح»؛ با react-hook-form از طریق Controller استفاده شود.
 */
export default function LookupAutocomplete({
  lookupKey,
  label,
  value,
  onChange,
  parentId,
  requireParent,
  error,
  helperText,
  disabled,
}: LookupAutocompleteProps) {
  const { rows, loading } = useLookup(lookupKey, parentId, { requireParent });
  const selected = rows.find((r) => r.id === value) ?? null;

  return (
    <IkcoAutocomplete
      options={rows}
      value={selected}
      loading={loading}
      disabled={disabled}
      getOptionLabel={(o) => (o.code && o.code !== String(o.id) ? `${o.code} - ${o.title}` : o.title)}
      isOptionEqualToValue={(a, b) => a.id === b.id}
      onChange={(_, row) => onChange(row ? row.id : null, row)}
      renderInput={(params) => <IkcoTextField {...params} label={label} error={error} helperText={helperText} />}
      noOptionsText="موردی یافت نشد"
      loadingText="در حال بارگذاری..."
    />
  );
}
