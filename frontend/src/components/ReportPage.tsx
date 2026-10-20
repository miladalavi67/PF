import { IkcoCheckbox, IkcoDatePicker, IkcoGrid, IkcoTextField, type IkcoGridColDef } from "@ikco/frontend-kit";
import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { Controller, useForm } from "react-hook-form";
import type { IFieldRules } from "../types/Common";
import type { ITabularResult } from "../types/tabular";
import { columnHeaders } from "../utils/columnHeaders";
import { firstOfMonthJalali, todayJalali } from "../utils/dateUtility";
import { resultColumns, tabularRows, type IGridRow } from "../utils/tabularUtility";
import LookupAutocomplete from "./LookupAutocomplete";
import LookupMultiSelect from "./LookupMultiSelect";
import ReportView from "./ReportView";

/** نوع فیلد پارامتر گزارش */
export type ReportFilterType = "text" | "number" | "date" | "checkbox" | "lookup" | "multilookup";

export interface IReportFilter {
  /** نام خصوصیت ورودی DTO (camelCase) */
  name: string;
  label: string;
  type: ReportFilterType;
  /** کلید فهرست مرجع برای lookup / multilookup */
  lookupKey?: string;
  /** نام فیلد والد برای فهرست‌های وابسته */
  parent?: string;
  required?: boolean;
  /** مقدار پیش‌فرض (برای date: "today" یا "monthStart") */
  defaultValue?: string | number | boolean;
}

interface ReportPageProps<TInput extends object> {
  title: string;
  /** کلید دسترسی API گزارش (فقط دارندگان آن دکمه «نمایش» را می‌بینند) */
  permission: string;
  filters: IReportFilter[];
  run: (input: TInput) => Promise<ITabularResult>;
  /** قواعد اعتبارسنجی تولیدشده از DTO Backend */
  rules?: Partial<Record<string, IFieldRules>>;
  /** عنوان فارسی ستون‌ها (بر اساس نام ستون خروجی رویه) */
  headers?: Record<string, string>;
  /** اجرای خودکار هنگام باز شدن صفحه (برای گزارش‌های بدون پارامتر) */
  autoRun?: boolean;
  /** تشخیص تغییر داده: تابعی که رشته/عدد آخرین تغییر را برمی‌گرداند؛ با تغییر مقدار، گزارش دوباره اجرا می‌شود */
  poll?: () => Promise<string | number | null | undefined>;
  pollSeconds?: number;
}

const size = { xl: 3, lg: 3, md: 4, sm: 6, xs: 12 };

const initialValue = (f: IReportFilter): unknown => {
  if (f.type === "date") {
    if (f.defaultValue === "today") return todayJalali();
    if (f.defaultValue === "monthStart") return firstOfMonthJalali();
    return f.required ? (f.name.toLowerCase().startsWith("from") ? firstOfMonthJalali() : todayJalali()) : null;
  }
  if (f.type === "checkbox") return Boolean(f.defaultValue ?? false);
  if (f.type === "multilookup") return [];
  if (f.type === "lookup") return typeof f.defaultValue === "number" ? f.defaultValue : null;
  return f.defaultValue ?? "";
};

/**
 * صفحه گزارش پارامتری (جایگزین frmReportMaster و فرم‌های RPT سامانه ویندوزی): فیلترها بر اساس تعریف، فراخوانی سرویس تولیدشده
 * و نمایش نتیجه در IkcoDataGrid با خروجی اکسل. ستون‌ها از خروجی رویه (TabularResult) ساخته می‌شوند.
 */
export default function ReportPage<TInput extends object>({
  title,
  permission,
  filters,
  run,
  rules,
  headers,
  autoRun = false,
  poll,
  pollSeconds = 30,
}: ReportPageProps<TInput>) {
  const defaults = useMemo(() => Object.fromEntries(filters.map((f) => [f.name, initialValue(f)])), [filters]);
  const { control, register, handleSubmit, reset, watch, setValue, formState: { errors } } = useForm<Record<string, unknown>>({ defaultValues: defaults });
  const [result, setResult] = useState<ITabularResult | null>(null);
  const [rows, setRows] = useState<IGridRow[]>([]);
  const [loading, setLoading] = useState(false);
  const [executed, setExecuted] = useState(false);
  const lastInput = useRef<TInput | null>(null);
  const lastStamp = useRef<string | number | null>(null);

  const columns: IkcoGridColDef[] = useMemo(() => resultColumns(result, { ...columnHeaders, ...headers }), [result, headers]);

  const toInput = useCallback(
    (values: Record<string, unknown>): TInput => {
      const out: Record<string, unknown> = {};
      for (const f of filters) {
        const v = values[f.name];
        if (f.type === "number" || f.type === "lookup") out[f.name] = v === "" || v === null || v === undefined ? null : Number(v);
        else if (f.type === "checkbox") out[f.name] = Boolean(v);
        else if (f.type === "multilookup") out[f.name] = Array.isArray(v) && v.length > 0 ? (v as number[]).join(",") : "-1";
        else out[f.name] = v === "" || v === undefined ? null : typeof v === "string" ? v.trim() : v;
      }
      return out as TInput;
    },
    [filters],
  );

  const execute = useCallback(
    async (input: TInput, silent = false) => {
      if (!silent) setLoading(true);
      try {
        const data = await run(input);
        lastInput.current = input;
        setResult(data);
        setRows(tabularRows(data));
        setExecuted(true);
      } finally {
        if (!silent) setLoading(false);
      }
    },
    [run],
  );

  useEffect(() => {
    if (autoRun) void execute(toInput(defaults));
    // فقط هنگام باز شدن صفحه
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    if (!poll) return;
    const timer = window.setInterval(async () => {
      try {
        const stamp = (await poll()) ?? null;
        if (lastStamp.current !== null && stamp !== null && stamp !== lastStamp.current && lastInput.current) await execute(lastInput.current, true);
        if (stamp !== null) lastStamp.current = stamp;
      } catch {
        // خطای شبکه قبلاً توسط axios اعلام می‌شود
      }
    }, Math.max(10, pollSeconds) * 1000);
    return () => window.clearInterval(timer);
  }, [poll, pollSeconds, execute]);

  const fieldRules = (f: IReportFilter) => ({
    ...(rules?.[f.name] ?? {}),
    ...(f.required ? { required: `«${f.label}» الزامی است` } : {}),
  });

  const renderFilter = (f: IReportFilter) => {
    const err = errors[f.name]?.message as string | undefined;
    const parentValue = f.parent ? (watch(f.parent) as number | null | undefined) : undefined;

    switch (f.type) {
      case "text":
        return <IkcoTextField label={f.label} {...register(f.name, fieldRules(f))} error={!!err} helperText={err} />;
      case "number":
        return <IkcoTextField label={f.label} type="number" {...register(f.name, fieldRules(f))} error={!!err} helperText={err} />;
      case "date":
        return (
          <Controller
            name={f.name}
            control={control}
            rules={fieldRules(f)}
            render={({ field }) => <IkcoDatePicker label={f.label} value={(field.value as string | null) ?? null} onChange={field.onChange} helperText={err} />}
          />
        );
      case "checkbox":
        return <Controller name={f.name} control={control} render={({ field }) => <IkcoCheckbox label={f.label} checked={Boolean(field.value)} onChange={(_, c) => field.onChange(c)} />} />;
      case "lookup":
        return (
          <Controller
            name={f.name}
            control={control}
            rules={fieldRules(f)}
            render={({ field }) => (
              <LookupAutocomplete
                lookupKey={f.lookupKey!}
                label={f.label}
                value={field.value as number | null}
                parentId={parentValue ?? null}
                requireParent={Boolean(f.parent)}
                onChange={(id) => {
                  field.onChange(id);
                  // فیلدهای وابسته به این فیلد پاک می‌شوند
                  filters.filter((x) => x.parent === f.name).forEach((child) => setValue(child.name, null));
                }}
                error={!!err}
                helperText={err}
              />
            )}
          />
        );
      case "multilookup":
        return (
          <Controller
            name={f.name}
            control={control}
            render={({ field }) => <LookupMultiSelect lookupKey={f.lookupKey!} label={f.label} value={(field.value as number[]) ?? []} onChange={field.onChange} helperText="بدون انتخاب = همه" />}
          />
        );
      default:
        return null;
    }
  };

  return (
    <ReportView
      title={title}
      permission={permission}
      onRun={handleSubmit((values) => execute(toInput(values)))}
      onClear={() => {
        reset(defaults);
        setResult(null);
        setRows([]);
        setExecuted(false);
      }}
      loading={loading}
      columns={columns}
      rows={rows}
      executed={executed}
      filters={
        filters.length === 0
          ? undefined
          : filters.map((f) => (
              <IkcoGrid key={f.name} size={size}>
                {renderFilter(f)}
              </IkcoGrid>
            ))
      }
    />
  );
}
