import { ThemeProvider, createTheme } from "@mui/material/styles";
import React from "react";
import { components } from "./customizations";
import getPalette from "./customizations/palette";
import { colorSchemes, typography } from "./customizations/themePrimitives";

interface AppThemeProps {
  children: React.ReactNode;
  mode?: "light" | "dark"; // از بیرون هم می‌تونی پاس بدی
}

export default function AppTheme({ children, mode = "light" }: AppThemeProps) {
  const muiTheme = React.useMemo(
    () =>
      createTheme({
        colorSchemes,

        palette: getPalette(mode),
        direction: "rtl",
        components,
        typography,
      }),
    [mode],
  );

  return <ThemeProvider theme={muiTheme}>{children}</ThemeProvider>;
}
