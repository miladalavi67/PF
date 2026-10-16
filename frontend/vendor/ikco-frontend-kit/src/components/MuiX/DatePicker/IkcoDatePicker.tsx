/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useRef, useState } from "react";
import {
  DatePicker,
  LocalizationProvider,
  type DatePickerProps,
} from "@mui/x-date-pickers";
import { AdapterMomentJalaali } from "@mui/x-date-pickers/AdapterMomentJalaali";
import moment, { type Moment } from "moment-jalaali";
import { FormHelperText, Button, Box } from "@mui/material";

type IkcoDatePickerProps = Omit<
  DatePickerProps,
  "value" | "onChange" | "sx" | "style" | "slotProps" | "slots" | "renderInput"
> & {
  value?: string | null;
  onChange?: (date: string | null) => void;
  helperText?: string;
  size?: "small" | "medium"; //minajam
};
moment.loadPersian({ dialect: "persian-modern", usePersianDigits: true });

const PERSIAN_TO_EN: Record<string, string> = {
  "۰": "0",
  "۱": "1",
  "۲": "2",
  "۳": "3",
  "۴": "4",
  "۵": "5",
  "۶": "6",
  "۷": "7",
  "۸": "8",
  "۹": "9",
};

function normalizePersianDigitsToEnglish(s: string) {
  return s.replace(/[۰-۹]/g, (ch) => PERSIAN_TO_EN[ch] ?? ch);
}

function parseJalaliStringToMoment(val?: string | null): Moment | null {
  if (!val) return null;
  const input = normalizePersianDigitsToEnglish(val.trim());
  const formats = [
    "jYYYY/jM/jD",
    "jYYYY/jMM/jDD",
    "jYYYY-jM-jD",
    "jYYYY-jMM-jDD",
  ];
  for (const f of formats) {
    const m = moment(input, f, true);
    if (m.isValid()) return m;
  }
  const m2 = moment(input);
  return m2.isValid() ? m2 : null;
}

function formatMomentToJalaliString(m: Moment | null): string | null {
  if (!m) return null;
  const formatted = m.format("jYYYY/jMM/jDD");
  return normalizePersianDigitsToEnglish(formatted);
}

function CustomActionBar(props: any) {
  const { onAccept, onClear, onCancel } = props;

  return (
    <Box
      sx={{
        width: "100%",
        maxWidth: "100%",
        boxSizing: "border-box",
        display: "flex",
        flexDirection: "row",
        flexWrap: "nowrap",
        gap: 1,
        justifyContent: "flex-start",
        alignItems: "center",
        p: 1,
        overflowX: "auto",
        WebkitOverflowScrolling: "touch",
        alignSelf: "stretch",
      }}
    >
      <>
        <Button
          variant="outlined"
          onClick={() => onClear?.()}
          sx={{ borderColor: "#e57373", color: "#b71c1c" }}
        >
          پاک کردن
        </Button>

        <Button
          variant="outlined"
          onClick={() => onCancel?.()}
          sx={{ borderColor: "#9e9e9e", color: "#424242" }}
        >
          انصراف
        </Button>

        <Button
          variant="contained"
          onClick={() => onAccept?.()}
          sx={{ backgroundColor: "#1976d2", color: "#fff" }}
        >
          تایید
        </Button>
      </>
    </Box>
  );
}

export default function IkcoDatePicker({
  value,
  onChange,
  helperText,
  size = "medium", //minajam
  ...rest
}: IkcoDatePickerProps) {
  const [committed, setCommitted] = useState<Moment | null>(() =>
    parseJalaliStringToMoment(value),
  );
  const [draft, setDraft] = useState<Moment | null>(() =>
    parseJalaliStringToMoment(value),
  );
  const [open, setOpen] = useState(false);

  // ref برای کنترل اجازه‌ی بستن؛ وقتی true باشه اجازه میدیم بسته بشه
  const allowCloseRef = useRef(false);

  useEffect(() => {
    const m = parseJalaliStringToMoment(value);
    setCommitted(m);
    setDraft(m);
  }, [value]);

  const handleOpen = () => {
    setDraft(committed);
    setOpen(true);
  };

  const handleChange = (v: Moment | null) => {
    setDraft(v);
  };

  // پاک کردن: reset هر دو state committed و draft به null
  const handleClear = () => {
    // اجازه بستن رو میدیم، تا عملیات بسته شدن واقعی انجام بشه
    allowCloseRef.current = true;
    setDraft(null);
    setCommitted(null);
    setOpen(false);
    onChange?.(null);
    // بعد از یک فریم اجازه رو برمیگردونیم (محافظت)
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  // وقتی کاربر تایید زد => مقدار نهایی ذخیره و prop onChange صدا زده میشه
  // تابع onAccept ممکنه مقدار ورودی هم بهش پاس داده شه در بعضی ورژن‌ها؛ ما هر دو حالت را پوشش میدیم
  const handleAccept = (v?: Moment | null) => {
    allowCloseRef.current = true;
    const finalValue = (v === undefined ? draft : v) ?? null;
    setCommitted(finalValue);
    setOpen(false);
    onChange?.(formatMomentToJalaliString(finalValue));
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  // انصراف: discard تغییرات (draft -> committed) و بستن
  const handleCancel = () => {
    allowCloseRef.current = true;
    setDraft(committed);
    setOpen(false);
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  // اگر پیکر بدون تایید بسته شد (مثلاً کلیک بیرون یا escape)، ما مانع بسته شدن می‌شویم
  const handleCloseRequested = () => {
    if (allowCloseRef.current) {
      // اجازه داده شده — بسته شدن را انجام بده (همان handleClose معمول)
      // از آنجایی که خودمان open را کنترل می‌کنیم کافی است setOpen(false) بزنیم.
      setOpen(false);
      // و draft را به committed برگردانیم (یا بسته به خواسته شما می‌توان حفظ کرد)
      setDraft(committed);
    } else {
      // بستن مجاز نیست: عملاً هیچ کار نکنیم و پیکر باز بماند
      // (ممکن است لازم باشد setOpen(true) بزنیم تا اگر MUI تلاش کرده false کند، ما دوباره بازش کنیم)
      setOpen(true);
    }
  };

  const pickerValue = open ? draft : committed;

  return (
    <LocalizationProvider dateAdapter={AdapterMomentJalaali}>
      <DatePicker
        {...rest}
        // جلوگیری از بسته شدن خودکار بعد از انتخاب
        closeOnSelect={false}
        // اگر ورژن شما prop دیگری برای غیرفعال کردن بستن وجود داشته باشه (مثل disableCloseOnSelect)
        // میشه اضافه‌اش کرد: disableCloseOnSelect
        localeText={{
          okButtonLabel: "تایید",
          cancelButtonLabel: "انصراف",
          clearButtonLabel: "پاک کردن",
        }}
        // جایگزینی ActionBar با کامپوننت دلخواه
        slots={{
          actionBar: CustomActionBar,
        }}
        // به CustomActionBar prop های موردنیاز رو میدیم
        slotProps={{
          textField: { size: size, fullWidth: true }, //minajam
          actionBar: {
            // توابعی که کامپوننت سفارشی (CustomActionBar) آنها را صدا می‌زند
            onAccept: (v?: Moment | null) => handleAccept(v),
            onClear: () => handleClear(),
            onCancel: () => handleCancel(),
          } as any,
          layout: {
            sx: {
              display: "flex",
              flexDirection: "column",
              justifyContent: "flex-end",
            },
          },
        }}
        onChange={(v) => handleChange(v as Moment | null)}
        value={pickerValue}
        open={open}
        onOpen={handleOpen}
        // وقتی پیکر درخواست بستن داشت، این تابع فراخوانده میشه — ما تصمیم می‌گیریم ببندیم یا نه
        onClose={handleCloseRequested}
        // onAccept رو هم میدیم برای تضمین (بعضی ورژن‌ها اینجا مقدار میفرستن)
        onAccept={(v) => handleAccept(v as Moment | null)}
      />
      <FormHelperText>{helperText}</FormHelperText>
    </LocalizationProvider>
  );
}
