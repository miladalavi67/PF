import type { PaletteMode } from "@mui/material";
import type { PaletteOptions } from "@mui/material";

// ۱. تعریف پالت خاکستری مشترک (بر اساس تصویر دیزاین سیستم)
const greyScale = {
  50: "#F9FAFB",
  100: "#F3F4F6",
  200: "#E5E7EB",
  300: "#D1D5DB",
  400: "#9CA3AF",
  500: "#6B7280",
  600: "#4B5563",
  700: "#374151",
  800: "#1F2937",
  900: "#111827",
};

// ۲. تابع اصلی برای دریافت تنظیمات پالت
export const getPalette = (mode: PaletteMode): PaletteOptions => ({
  mode,
  ...(mode === "light"
    ? {
        // --- تنظیمات پالت LIGHT ---
        primary: {
          main: "#1C75BC", // Shade 500
          light: "#7AB9E2", // Shade 300
          dark: "#124F7F", // Shade 700
          contrastText: "#ffffff",
        },
        secondary: {
          main: "#EA7724", // Shade 500
          light: "#F1A46E", // Shade 300
          dark: "#A44E10", // Shade 700
          contrastText: "#ffffff",
        },
        text: {
          primary: greyScale[900],
          secondary: greyScale[600],
          disabled: greyScale[400],
        },
        background: {
          default: greyScale[50],
          paper: "#FFFFFF",
        },
        divider: greyScale[200],
        action: {
          active: greyScale[500],
          hover: "#1c75bc0a",
          selected: "#1c75bc14",
        },
      }
    : {
        // --- تنظیمات پالت DARK ---
        primary: {
          main: "#7AB9E2", // Shade 300 (روشن‌تر برای حالت تاریک)
          light: "#CFE6F6", // Shade 100
          dark: "#1C75BC", // Shade 500
          contrastText: "#000000de",
        },
        secondary: {
          main: "#F1A46E", // Shade 300
          light: "#F8D2B6", // Shade 100
          dark: "#EA7724", // Shade 500
          contrastText: "#000000de",
        },
        text: {
          primary: greyScale[50],
          secondary: greyScale[300],
          disabled: greyScale[500],
        },
        background: {
          default: greyScale[900],
          paper: greyScale[800],
        },
        divider: "#ffffff1f",
        action: {
          active: greyScale[400],
          hover: "#ffffff14",
          selected: "#ffffff29",
        },
      }),

  // ۳. رنگ‌های مشترک (Semantic Colors) که در هر دو مود ثابت یا مشابه هستند
  success: {
    main: "#10B961",
    light: "#ECFDF5",
    dark: "#047857",
    contrastText: "#ffffff",
  },
  error: {
    main: "#EF4444",
    light: "#FEF2F2",
    dark: "#B91C1C",
    contrastText: "#ffffff",
  },
  warning: {
    main: "#F59E0B",
    light: "#FFFBEB",
    dark: "#B45309",
    contrastText: "#ffffff",
  },
  info: {
    main: "#1C75BC",
    light: "#7AB9E2",
    dark: "#124F7F",
    contrastText: "#ffffff",
  },
  grey: greyScale,
});

export default getPalette;
