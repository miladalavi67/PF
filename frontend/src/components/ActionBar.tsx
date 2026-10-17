import { IkcoCard, IkcoCardContent, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import type { ReactNode } from "react";

interface ActionBarProps {
  children: ReactNode;
  /** متن توضیحی کنار دکمه‌ها (مثلاً «تعداد: ۱۲») که در انتهای ردیف قرار می‌گیرد */
  info?: ReactNode;
}

/**
 * نوار دکمه‌های فرم. برخلاف IkcoCardActions دکمه‌ها در صورت کمبود جا به خط بعد می‌روند (flex-wrap) و متن دکمه‌ها فشرده
 * یا شکسته نمی‌شود؛ فاصله‌ی بین دکمه‌ها با gap تامین می‌شود.
 */
export default function ActionBar({ children, info }: ActionBarProps) {
  return (
    <IkcoCard>
      <IkcoCardContent>
        <IkcoStack direction="row" spacing={1} flexWrap="wrap" useFlexGap alignItems="center">
          {children}
          {info !== undefined && info !== null && info !== false && (
            <IkcoTypography variant="caption" color="text.secondary">
              {info}
            </IkcoTypography>
          )}
        </IkcoStack>
      </IkcoCardContent>
    </IkcoCard>
  );
}
