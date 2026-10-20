import { IkcoBox, IkcoDataGrid, IkcoStack, IkcoTypography } from "@ikco/frontend-kit";
import type { ComponentProps, ReactNode } from "react";
import type { IGridRow } from "../utils/tabularUtility";
import type { ITrackingTone, TrackingBackground, TrackingForeground } from "../utils/trackingUtility";

/**
 * رنگ‌آمیزی ردیف‌های گزارش‌های ردیابی اقلام بحرانی مطابق متد SetColors فرم‌های ویندوزی
 * (frmRPTTrackingCriticalParts، frmRPTTrackingTrimCriticalPart / frmRPTProductionPossibility).
 * رنگ زمینه/متن ردیف همان Color.* فرم ویندوزی است.
 */
const backgrounds: Record<TrackingBackground, string> = {
  new: "#FFD700", // Gold — بحران جدید
  resolved: "#ADFF2F", // بلامانع طی یک ساعت اخیر (Chartreuse راهنمای فرم)
  license: "#FFFF00", // Yellow — مجوز مانده کمتر از 10
};
const foregrounds: Record<TrackingForeground, string> = {
  critical: "#DC143C", // Crimson — امکان تولید کمتر از آستانه
  threshold: "#FF69B4", // HotPink — آستانه بحران
  stockChanged: "#6495ED", // CornflowerBlue — تغییر در موجودی
  license: "#8B4513", // SaddleBrown
};

/** کلاس‌های رنگ ردیف روی خود گرید (IkcoDataGrid فقط style را نمی‌پذیرد) */
const toneClasses = {
  ...Object.fromEntries(
    Object.entries(backgrounds).map(([k, color]) => [
      `& .pf-bg-${k}, & .pf-bg-${k}:hover, & .pf-bg-${k}.Mui-selected, & .pf-bg-${k}.Mui-selected:hover`,
      { backgroundColor: color, color: "#000" },
    ]),
  ),
  ...Object.fromEntries(Object.entries(foregrounds).map(([k, color]) => [`& .pf-fg-${k}`, { color, fontWeight: 600 }])),
};

type GridProps = ComponentProps<typeof IkcoDataGrid>;

interface TrackingGridProps extends Omit<GridProps, "getRowClassName"> {
  /** رنگ هر ردیف */
  tone: (row: IGridRow) => ITrackingTone | undefined;
}

export default function TrackingGrid({ tone, ...props }: TrackingGridProps) {
  return (
    <IkcoDataGrid
      {...props}
      sx={toneClasses}
      getRowClassName={(p) => {
        const t = tone(p.row as IGridRow);
        return [t?.background && `pf-bg-${t.background}`, t?.foreground && `pf-fg-${t.foreground}`].filter(Boolean).join(" ");
      }}
    />
  );
}

export interface ILegendItem {
  label: string;
  /** رنگ مربع راهنما در فرم ویندوزی */
  color: string;
  /** کنترل اضافه کنار راهنما (مثلاً چک‌باکس آستانه بحران) */
  extra?: ReactNode;
}

/** راهنمای رنگ بحران‌ها (مربع‌های رنگی فرم ویندوزی) */
export function TrackingLegend({ title = "راهنمای رنگ بحران ها:", items, children }: { title?: string; items: ILegendItem[]; children?: ReactNode }) {
  return (
    <IkcoStack direction="row" spacing={2} flexWrap="wrap" useFlexGap alignItems="center">
      <IkcoTypography variant="subtitle2">{title}</IkcoTypography>
      {items.map((l) => (
        <IkcoStack key={l.label} direction="row" spacing={0.75} alignItems="center">
          {l.extra}
          <IkcoBox component="span" display="inline-block" flexShrink={0} width={18} height={18} borderRadius={0.5} border={1} borderColor="text.secondary" bgcolor={l.color}>
            {null}
          </IkcoBox>
          <IkcoTypography variant="caption">{l.label}</IkcoTypography>
        </IkcoStack>
      ))}
      {children}
    </IkcoStack>
  );
}
