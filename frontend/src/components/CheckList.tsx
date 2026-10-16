import {
  IkcoCheckbox,
  IkcoList,
  IkcoListItem,
  IkcoPaper,
  IkcoStack,
  IkcoTextField,
  IkcoTypography,
} from "@ikco/frontend-kit";
import { useMemo, useState } from "react";

export interface ICheckListItem<V extends string | number = string | number> {
  value: V;
  label: string;
}

interface CheckListProps<V extends string | number> {
  title: string;
  items: ICheckListItem<V>[];
  selected: V[];
  onChange: (selected: V[]) => void;
  loading?: boolean;
  height?: number;
}

/**
 * لیست چندانتخابی با جستجو و «انتخاب همه» (جایگزین CheckListBox سامانه قدیم).
 * جستجو فقط نمایش را فیلتر می‌کند؛ انتخاب‌های قبلی حفظ می‌شوند.
 */
export default function CheckList<V extends string | number>({
  title,
  items,
  selected,
  onChange,
  loading = false,
  height = 260,
}: CheckListProps<V>) {
  const [filter, setFilter] = useState("");
  const visible = useMemo(
    () => (filter ? items.filter((i) => i.label.toLowerCase().includes(filter.toLowerCase())) : items),
    [items, filter],
  );
  const selectedSet = new Set(selected);
  const allVisibleChecked = visible.length > 0 && visible.every((i) => selectedSet.has(i.value));

  const toggle = (value: V) =>
    onChange(selectedSet.has(value) ? selected.filter((v) => v !== value) : [...selected, value]);

  const toggleAll = () => {
    const visibleValues = new Set(visible.map((i) => i.value));
    onChange(allVisibleChecked ? selected.filter((v) => !visibleValues.has(v)) : [...new Set([...selected, ...visible.map((i) => i.value)])]);
  };

  return (
    <IkcoPaper variant="outlined">
      <IkcoStack spacing={1} padding={1}>
        <IkcoStack direction="row" justifyContent="space-between" alignItems="center">
          <IkcoTypography variant="subtitle2">{title}</IkcoTypography>
          <IkcoTypography variant="caption">
            {loading ? "در حال بارگذاری..." : `${selected.length} از ${items.length}`}
          </IkcoTypography>
        </IkcoStack>
        <IkcoTextField placeholder="جستجو..." value={filter} onChange={(e) => setFilter(e.target.value)} />
        <IkcoCheckbox label="انتخاب همه" checked={allVisibleChecked} onChange={toggleAll} disabled={visible.length === 0} />
        <IkcoList dense disablePadding>
          <IkcoStack height={height} overflow="auto">
            {visible.map((item) => (
              <IkcoListItem key={String(item.value)} disablePadding>
                <IkcoCheckbox label={item.label} checked={selectedSet.has(item.value)} onChange={() => toggle(item.value)} />
              </IkcoListItem>
            ))}
            {!loading && visible.length === 0 && <IkcoTypography variant="caption">موردی وجود ندارد</IkcoTypography>}
          </IkcoStack>
        </IkcoList>
      </IkcoStack>
    </IkcoPaper>
  );
}
