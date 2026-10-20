import { IkcoAutocomplete, IkcoTextField } from "@ikco/frontend-kit";
import { useEffect, useMemo, useRef } from "react";
import { useLookup } from "../hooks/useLookup";
import type { ILookupRow } from "../types/Lookups";

/** شناسه گزینه «همه» (allLabel)؛ به بیرون null داده می‌شود */
const ALL = Number.MIN_SAFE_INTEGER;

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
  /**
   * پیش‌فرض true: مانند DropDownList فرم‌های ویندوزی وقتی چیزی انتخاب نشده اولین گزینه فهرست انتخاب می‌شود
   * (هنگام بارگذاری، تغییر والد و پاک‌سازی فرم) و فیلد خالی نمی‌ماند. فقط جایی که «بدون انتخاب» معنای مستقل دارد false شود.
   */
  autoSelectFirst?: boolean;
  /**
   * برای فیلترهای گزارش/جستجو که «بدون انتخاب» یعنی همه: اولین گزینه فهرست این عنوان (مثلاً «همه موارد») است و مقدار null دارد؛
   * پیش‌فرض فیلد همین گزینه اول است.
   */
  allLabel?: string;
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
  autoSelectFirst = true,
  allLabel,
  error,
  helperText,
  disabled,
}: LookupAutocompleteProps) {
  const { rows, loading } = useLookup(lookupKey, parentId, { requireParent });
  const options = useMemo(() => (allLabel ? [{ id: ALL, code: null, title: allLabel }, ...rows] : rows), [allLabel, rows]);
  const selected = rows.find((r) => r.id === value) ?? (allLabel ? options[0] : null);
  const change = useRef(onChange);
  change.current = onChange;

  // اولین گزینه وقتی مقداری انتخاب نشده یا مقدار فعلی در فهرست جدید (مثلاً پس از تغییر والد) وجود ندارد؛
  // با allLabel گزینه اول «همه» با مقدار null است و نیازی به انتخاب خودکار نیست
  useEffect(() => {
    if (!autoSelectFirst || allLabel || disabled || loading || rows.length === 0) return;
    if (value === null || value === undefined || !rows.some((r) => r.id === value)) change.current(rows[0].id, rows[0]);
  }, [autoSelectFirst, allLabel, disabled, loading, rows, value]);

  return (
    <IkcoAutocomplete
      options={options}
      value={selected}
      loading={loading}
      disabled={disabled}
      disableClearable={(autoSelectFirst || !!allLabel) && options.length > 0}
      getOptionLabel={(o) => (o.code && o.code !== String(o.id) ? `${o.code} - ${o.title}` : o.title)}
      isOptionEqualToValue={(a, b) => a.id === b.id}
      onChange={(_, row) => onChange(row && row.id !== ALL ? row.id : null, row && row.id !== ALL ? row : null)}
      renderInput={(params) => <IkcoTextField {...params} label={label} error={error} helperText={helperText} />}
      noOptionsText="موردی یافت نشد"
      loadingText="در حال بارگذاری..."
    />
  );
}
