/* eslint-disable @typescript-eslint/no-explicit-any */
import { useEffect, useRef, useState } from "react";
import {
  DateTimePicker,
  LocalizationProvider,
  type DateTimePickerProps,
} from "@mui/x-date-pickers";
import { AdapterMomentJalaali } from "@mui/x-date-pickers/AdapterMomentJalaali";
import moment, { type Moment } from "moment-jalaali";
import { FormHelperText, Button, Box } from "@mui/material";

type IkcoDateTimePickerProps = Omit<
  DateTimePickerProps,
  "value" | "onChange" | "sx" | "style" | "slotProps" | "slots" | "renderInput"
> & {
  value?: string | null;
  onChange?: (dateTime: string | null) => void;
  helperText?: string;
  size?: "small" | "medium";
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

function parseJalaliDateTimeToMoment(val?: string | null): Moment | null {
  if (!val) return null;
  const input = normalizePersianDigitsToEnglish(val.trim());

  const formats = [
    "jYYYY/jM/jD HH:mm",
    "jYYYY/jMM/jDD HH:mm",
    "jYYYY-jM-jD HH:mm",
    "jYYYY-jMM-jDD HH:mm",
    "jYYYY/jM/jD HH:mm:ss",
    "jYYYY/jMM/jDD HH:mm:ss",
    "jYYYY-jM-jD HH:mm:ss",
    "jYYYY-jMM-jDD HH:mm:ss",
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

function formatMomentToJalaliDateTimeString(m: Moment | null): string | null {
  if (!m) return null;
  const formatted = m.format("jYYYY/jMM/jDD HH:mm:ss");
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
    </Box>
  );
}

export default function IkcoDateTimePicker({
  value,
  onChange,
  helperText,
  size = "medium",
  ...rest
}: IkcoDateTimePickerProps) {
  const [committed, setCommitted] = useState<Moment | null>(() =>
    parseJalaliDateTimeToMoment(value),
  );
  const [draft, setDraft] = useState<Moment | null>(() =>
    parseJalaliDateTimeToMoment(value),
  );
  const [open, setOpen] = useState(false);

  const allowCloseRef = useRef(false);

  useEffect(() => {
    const m = parseJalaliDateTimeToMoment(value);
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

  const handleClear = () => {
    allowCloseRef.current = true;
    setDraft(null);
    setCommitted(null);
    setOpen(false);
    onChange?.(null);
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  const handleAccept = (v?: Moment | null) => {
    allowCloseRef.current = true;
    const finalValue = (v === undefined ? draft : v) ?? null;
    setCommitted(finalValue);
    setOpen(false);
    onChange?.(formatMomentToJalaliDateTimeString(finalValue));
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  const handleCancel = () => {
    allowCloseRef.current = true;
    setDraft(committed);
    setOpen(false);
    setTimeout(() => {
      allowCloseRef.current = false;
    }, 0);
  };

  const handleCloseRequested = () => {
    if (allowCloseRef.current) {
      setOpen(false);
      setDraft(committed);
    } else {
      setOpen(true);
    }
  };

  const pickerValue = open ? draft : committed;

  return (
    <LocalizationProvider dateAdapter={AdapterMomentJalaali}>
      <DateTimePicker
        {...rest}
        ampm={false}
        closeOnSelect={false}
        localeText={{
          okButtonLabel: "تایید",
          cancelButtonLabel: "انصراف",
          clearButtonLabel: "پاک کردن",
        }}
        slots={{
          actionBar: CustomActionBar,
        }}
        slotProps={{
          textField: { size: size, fullWidth: true },
          actionBar: {
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
        onClose={handleCloseRequested}
        onAccept={(v) => handleAccept(v as Moment | null)}
      />
      <FormHelperText>{helperText}</FormHelperText>
    </LocalizationProvider>
  );
}
