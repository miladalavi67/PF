import { useEffect, useState } from "react";
import { getLookup } from "../server/services/lookupService";
import type { ILookupRow } from "../types/Lookups";

const cache = new Map<string, Promise<ILookupRow[]>>();

/**
 * فهرست مرجع Backend (GET lookups/{key}) با کش درون‌حافظه‌ای؛ کلیدهای وابسته با parentId فیلتر می‌شوند.
 * وقتی فهرست وابسته است و parentId هنوز انتخاب نشده، فهرست خالی برمی‌گردد.
 */
export function useLookup(key: string, parentId?: number | null, options: { requireParent?: boolean } = {}) {
  const [rows, setRows] = useState<ILookupRow[]>([]);
  const [loading, setLoading] = useState(false);
  const waitForParent = options.requireParent && (parentId === null || parentId === undefined);

  useEffect(() => {
    if (waitForParent) {
      setRows([]);
      return;
    }
    let active = true;
    const cacheKey = `${key}|${parentId ?? ""}`;
    let promise = cache.get(cacheKey);
    if (!promise) {
      promise = getLookup(key, { parentId: parentId ?? null }).then((r) => r.items);
      cache.set(cacheKey, promise);
      promise.catch(() => cache.delete(cacheKey));
    }
    setLoading(true);
    promise
      .then((items) => {
        if (active) setRows(items);
      })
      .catch(() => {
        if (active) setRows([]);
      })
      .finally(() => {
        if (active) setLoading(false);
      });
    return () => {
      active = false;
    };
  }, [key, parentId, waitForParent]);

  return { rows, loading };
}
