import { IkcoCard, IkcoCardContent, IkcoDivider, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import type { ReactNode } from "react";

interface SectionPanelProps {
  title: string;
  children: ReactNode;
  /** عناصر سمت مقابل عنوان (مثلاً Chip وضعیت یا شمارنده) */
  aside?: ReactNode;
  /** غیرفعال‌شدن کل گروه مانند GroupBox.Enabled = false در فرم ویندوزی (نمایش کم‌رنگ و بدون دسترسی) */
  disabled?: boolean;
}

/**
 * معادل GroupBox فرم ویندوزی: کارت با عنوان و خط جداکننده. اگر disabled باشد، فیلدهای داخل آن توسط صفحه غیرفعال می‌شوند
 * و عنوان به صورت ثانویه نمایش داده می‌شود (جلوگیری از شلوغی بدون استایل سفارشی).
 */
export default function SectionPanel({ title, children, aside, disabled }: SectionPanelProps) {
  return (
    <IkcoCard>
      <IkcoCardContent>
        <IkcoStack spacing={2}>
          <IkcoStack direction="row" spacing={1} alignItems="center" justifyContent="space-between" flexWrap="wrap" useFlexGap>
            <IkcoTypography variant="subtitle1" color={disabled ? "text.disabled" : "text.primary"}>
              {title}
            </IkcoTypography>
            {aside}
          </IkcoStack>
          <IkcoDivider />
          {children}
        </IkcoStack>
      </IkcoCardContent>
    </IkcoCard>
  );
}
