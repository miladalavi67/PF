import * as React from "react";
import { CacheProvider } from "@emotion/react";
import createCache from "@emotion/cache";
import rtlPlugin from "stylis-plugin-rtl";
import { alpha, useTheme } from "@mui/material/styles";
import { useMediaQuery } from "@mui/material"; 
import CssBaseline from "@mui/material/CssBaseline";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import ResponsiveDrawer from "./ResponsiveDrawer";
import IkcoBreadcrumb from "./IkcoBreadcrumb";
import ProtectedRoute from "../utilities/ProtectedRoute";
import { useTemplateStore } from "../utilities/templateStore";
import AppTheme from "../theme/AppTheme";
import { collapsedWidth, drawerWidth } from "./constants";

const cacheRtl = createCache({
  key: "rtl",
  stylisPlugins: [rtlPlugin],
});

export default function Layout() {
  const { userUnit, userMenu, getUserMenu } = useTemplateStore();

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("md"));

  const [drawerOpen, setDrawerOpen] = React.useState(true);
  const [mobileOpen, setMobileOpen] = React.useState(false);

  const handleDrawerToggle = () => {
    if (isMobile) {
      setMobileOpen(!mobileOpen);
    } else {
      setDrawerOpen(!drawerOpen);
    }
  };

  React.useEffect(() => {
    if (userUnit.status === "success" && userMenu.status === "idle") {
      getUserMenu();
    }
  }, [getUserMenu, userMenu.status, userUnit.status]);

  return (
    <CacheProvider value={cacheRtl}>
      <AppTheme>
        <CssBaseline enableColorScheme />
        <Box sx={{ display: "flex" }}>
          <ResponsiveDrawer
            drawerOpen={drawerOpen}
            mobileOpen={mobileOpen}
            handleDrawerToggle={handleDrawerToggle}
          />

          <Box
            component="main"
            sx={{
              flexGrow: 1,
              backgroundColor: (theme) =>
                theme.vars
                  ? `rgba(${theme.vars.palette.background.defaultChannel} / 1)`
                  : alpha(theme.palette.background.default, 1),
              overflow: "auto",
              p: 3,
              mt: "64px",
            
              mr: isMobile
                ? 0
                : drawerOpen
                ? `10px`
                : `10px`,
              ml: 0,
              width: isMobile
                ? "100%"
                : drawerOpen
                ? `calc(100% - ${drawerWidth}px)`
                : `calc(100% - ${collapsedWidth}px)`,
              transition: (theme) =>
                theme.transitions.create(["margin", "width"], {
                  easing: theme.transitions.easing.sharp,
                  duration: drawerOpen
                    ? theme.transitions.duration.enteringScreen
                    : theme.transitions.duration.leavingScreen,
                }),
            }}
          >
            <Stack spacing={2} sx={{ mx: 3, pb: 5 }}>
              <IkcoBreadcrumb />
              <React.Suspense fallback="loading ...">
                <ProtectedRoute />
              </React.Suspense>
            </Stack>
          </Box>
        </Box>
      </AppTheme>
    </CacheProvider>
  );
}
