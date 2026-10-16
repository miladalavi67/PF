import type { Components, Theme } from "@mui/material";

export const appBarCustomization: Components<Theme> = {
  MuiAppBar: {
    styleOverrides: {
      root: ({ theme }) => ({
        background: theme.palette.background.paper,
        color: theme.palette.common.black,
        borderBottom: `1px solid ${theme.palette.divider}`,
        transition: theme.transitions.create(["width", "margin"], {
          easing: theme.transitions.easing.sharp,
          duration: theme.transitions.duration.leavingScreen,
        }),
      }),
    },
  },
};
