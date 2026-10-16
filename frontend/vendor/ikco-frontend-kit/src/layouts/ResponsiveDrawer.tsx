import {
    Box,
    CssBaseline,
    useTheme,
    useMediaQuery,
    Divider,
    IconButton,
} from "@mui/material";
import MuiDrawer from "@mui/material/Drawer";
import SideMenu from "./SideMenu";
import MenuContent from "./MenuContent";
import Logo from "../images/logo2.png";
import MenuIcon from '@mui/icons-material/Menu';
import AppBarComponent from "./AppBarComponent";
import { drawerWidth } from "./constants";

interface ResponsiveDrawerProps {
    drawerOpen: boolean;
    mobileOpen: boolean;
    handleDrawerToggle: () => void;
}

export default function ResponsiveDrawer({
    drawerOpen,
    mobileOpen,
    handleDrawerToggle,
}: ResponsiveDrawerProps) {
    const theme = useTheme();
    const isMobile = useMediaQuery(theme.breakpoints.down("md"));

    return (
        <Box sx={{ display: "flex" }}>
            <CssBaseline />

            <AppBarComponent
                drawerOpen={drawerOpen}
                handleDrawerToggle={handleDrawerToggle}
            />

            {!isMobile && (
                <SideMenu
                    open={drawerOpen}
                    onToggle={handleDrawerToggle}
                    userInfo={{
                        fullName: "-",
                        
                    }}
                />
            )}

            {isMobile && (
                <MuiDrawer
                    variant="temporary"
                    open={mobileOpen}
                    onClose={handleDrawerToggle}
                    ModalProps={{ keepMounted: true }}

                    sx={{
                        display: { xs: "block", md: "none" },
                        "& .MuiDrawer-paper": {
                            boxSizing: "border-box",
                            width: drawerWidth,
                            top: 56,
                        },
                    }}
                >
                    <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", p: 2 }}>

                        <img
                            src={Logo}
                            alt="Logo"
                            style={{ width: "100px" }}
                        />

                        <IconButton onClick={handleDrawerToggle}>
                            <MenuIcon />
                        </IconButton>
                    </Box>

                    <Divider />

                    <MenuContent
                        open={true}
                        onToggle={handleDrawerToggle}
                    />

                </MuiDrawer>
            )}
        </Box>
    );
}