import { Stack, Toolbar, AppBar, useTheme, useMediaQuery, IconButton, Box } from "@mui/material";
import AccountMenu from "./AccountMenu";
import { collapsedWidth, drawerWidth } from "./constants";
import MenuIcon from '@mui/icons-material/Menu';

interface AppBarComponentProps {
    drawerOpen: boolean;
    handleDrawerToggle: () => void;
}

export default function AppBarComponent({ drawerOpen, handleDrawerToggle }: AppBarComponentProps) {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("md"));

    return (
        <AppBar
            position="fixed"
            sx={{
                width: isMobile ? '100%' : drawerOpen ? `calc(100% - ${drawerWidth}px)` : `calc(100% - ${collapsedWidth}px)`,
                ml: isMobile ? 0 : drawerOpen ? `${drawerWidth}px` : `${collapsedWidth}px`,
                zIndex: theme.zIndex.drawer + 1,
                boxShadow: 0,
            }}
        >
            <Toolbar sx={{ justifyContent: 'space-between' }}>
                {/* بخش راست: دکمه منو (فقط در موبایل) */}
                {isMobile && (
                    <IconButton
                        color="inherit"
                        aria-label="open drawer"
                        onClick={handleDrawerToggle}
                        sx={{ ml: 2 }}  // فاصله از لبه راست
                    >
                        <MenuIcon />
                    </IconButton>
                )}

                {/* اگر در موبایل چیزی سمت چپ بخوای بذاری، اینجا می‌تونی اضافه کنی */}
                {!isMobile && <Box />}  {/* فضای خالی برای بالانس در دسکتاپ */}

                {/* بخش چپ: AccountMenu (همیشه در انتها/چپ قرار می‌گیره) */}
                <Stack direction="row" alignItems="center">
                    <AccountMenu />
                </Stack>
            </Toolbar>
        </AppBar>
    );
}

