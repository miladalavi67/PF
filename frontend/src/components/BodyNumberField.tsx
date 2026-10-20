import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import { getBodyCriticalsBodyInfo } from "../server/services/bodyCriticalsService";
import type { IBodyInfoRow } from "../types/BodyShop";
import BodyInfoStrip, { bodyInfoCells } from "./BodyInfoStrip";

interface BodyNumberFieldProps {
  bodyNumber: string;
  onBodyNumberChange: (value: string) => void;
  /** پس از جستجو: اطلاعات بدنه یا null اگر بدنه یافت نشد / کاربر به خط آن دسترسی ندارد */
  onLoaded?: (info: IBodyInfoRow | null) => void | Promise<void>;
  disabled?: boolean;
  /** جستجوی خودکار شماره بدنه اولیه هنگام نمایش (مثلاً modal باز شده از فرم دیگر) */
  autoSearch?: boolean;
}

/**
 * شماره بدنه + نمایش مشخصات بدنه (PFReview بند ۳): با Enter یا دکمه جستجو، اطلاعات بدنه از Backend
 * (GET body-shop/critical-parts/body-info، رویه SEXT_BodyInfo_V2) خوانده و کنار شماره بدنه نمایش داده می‌شود.
 */
export default function BodyNumberField({ bodyNumber, onBodyNumberChange, onLoaded, disabled, autoSearch }: BodyNumberFieldProps) {
  const [info, setInfo] = useState<IBodyInfoRow | null>(null);
  const [loading, setLoading] = useState(false);

  const search = async () => {
    const value = bodyNumber.trim();
    if (!value) return;
    setLoading(true);
    try {
      const body = await getBodyCriticalsBodyInfo({ bodyNumber: value });
      const found = body && typeof body === "object" ? body : null;
      if (!found) toast.error("بدنه یافت نشد");
      setInfo(found);
      await onLoaded?.(found);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (autoSearch && bodyNumber.trim()) void search();
    // فقط هنگام نمایش اولیه
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <BodyInfoStrip
      bodyNumber={bodyNumber}
      onBodyNumberChange={(v) => {
        onBodyNumberChange(v);
        if (info) setInfo(null);
      }}
      onSearch={() => void search()}
      cells={bodyInfoCells(info)}
      disabled={disabled}
      loading={loading}
    />
  );
}
