import { IkcoGrid, IkcoTextField } from "@ikco/frontend-kit";
import { useEffect, useRef, useState, type ComponentProps } from "react";
import { getBodyCriticalsPartDescription } from "../server/services/bodyCriticalsService";
import { getMechPartsPartDescription } from "../server/services/mechPartsService";

type CellSize = ComponentProps<typeof IkcoGrid>["size"];

interface PartNumberFieldProps {
  value: string;
  onChange: (value: string) => void;
  /**
   * سرویس شرح فنی بر اساس دسترسی صفحه: body = بدنه‌سازی، mech = مونتاژ (هر دو رویه SIMP_SelectPartDescByPartNo).
   * پیش‌فرض mech.
   */
  source?: "body" | "mech";
  label?: string;
  descLabel?: string;
  disabled?: boolean;
  maxLength?: number;
  error?: boolean;
  helperText?: string;
  /** اندازه سلول شماره فنی و شرح در IkcoGrid والد */
  size?: CellSize;
  descSize?: CellSize;
  /** پس از دریافت شرح از Backend (خالی = شماره فنی در BOM یافت نشد) */
  onResolved?: (description: string, partNo: string) => void;
  onEnter?: () => void;
}

/**
 * فیلد شماره فنی + شرح فنی (PFReview بند ۳): با تایپ/تغییر شماره فنی، شرح از Backend خوانده و در فیلد کناری نمایش داده می‌شود.
 * دو سلول IkcoGrid برمی‌گرداند و داخل یک IkcoGrid container قرار می‌گیرد.
 */
export default function PartNumberField({
  value,
  onChange,
  source = "mech",
  label = "شماره فنی",
  descLabel = "شرح شماره فنی",
  disabled,
  maxLength = 50,
  error,
  helperText,
  size = { xs: 12, sm: 6, md: 3 },
  descSize = { xs: 12, sm: 6, md: 3 },
  onResolved,
  onEnter,
}: PartNumberFieldProps) {
  const [description, setDescription] = useState("");
  const resolved = useRef(onResolved);
  resolved.current = onResolved;

  useEffect(() => {
    const partNo = (value ?? "").trim();
    setDescription("");
    if (!partNo) return;
    let active = true;
    const timer = window.setTimeout(async () => {
      try {
        const load = source === "body" ? getBodyCriticalsPartDescription : getMechPartsPartDescription;
        const desc = await load({ partNo });
        const text = typeof desc === "string" ? desc.trim() : "";
        if (!active) return;
        setDescription(text);
        resolved.current?.(text, partNo);
      } catch {
        // پیام خطا توسط لایه http نمایش داده شده است
      }
    }, 400);
    return () => {
      active = false;
      window.clearTimeout(timer);
    };
  }, [value, source]);

  return (
    <>
      <IkcoGrid size={size}>
        <IkcoTextField
          label={label}
          value={value ?? ""}
          disabled={disabled}
          error={error}
          helperText={helperText}
          onChange={(e) => onChange(e.target.value)}
          onKeyDown={(e) => {
            if (e.key === "Enter") onEnter?.();
          }}
          slotProps={{ htmlInput: { maxLength } }}
        />
      </IkcoGrid>
      <IkcoGrid size={descSize}>
        <IkcoTextField label={descLabel} value={description} disabled />
      </IkcoGrid>
    </>
  );
}
