import * as React from "react";
import Box from "@mui/material/Box";
import Avatar from "@mui/material/Avatar";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import Divider from "@mui/material/Divider";
import IconButton from "@mui/material/IconButton";
import Typography from "@mui/material/Typography";
import Tooltip from "@mui/material/Tooltip";
import Settings from "@mui/icons-material/Settings";
import Logout from "@mui/icons-material/Logout";
import { NotificationsNone } from "@mui/icons-material";
import { Badge, Stack } from "@mui/material";
import { useTemplateStore } from "../utilities/templateStore";
import ColorModeSelect from "../theme/ColorModeSelect"; //...............................darkmode

export default function AccountMenu() {
  const [anchorEl, setAnchorEl] = React.useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  const { userInfo } = useTemplateStore();

  const department = userInfo?.department ?? "نام واحد يا اداره";
  const fullName = "نام كاربر";
  const profileSrc = userInfo?.profileImage ?? "";

  const handleSignOut = () => {
    
  };

  return (
    <>
      <Box
        sx={{
          display: "flex",
          justifyContent: "space-around",
          alignItems: "center",
          textAlign: "center",
          gap: 2,
        }}
      >
        <IconButton>
          <Badge badgeContent="0" color="primary">
            <NotificationsNone fontSize="medium" />
          </Badge>
        </IconButton>

        <ColorModeSelect />
        {/* dark light */}

        <Divider orientation="vertical" flexItem />
        <Stack flexDirection={"column"}>
          <Typography sx={{ minWidth: 100 }}>{fullName}</Typography>
          <Typography fontSize={12} color="textDisabled">
            {department}
          </Typography>
        </Stack>
        <Tooltip title="منوی کاربری">
          <IconButton
            onClick={handleClick}
            size="small"
            aria-controls={open ? "account-menu" : undefined}
            aria-haspopup="true"
            aria-expanded={open ? "true" : undefined}
          >
            <Avatar alt="no-profile-image" src={profileSrc} />
          </IconButton>
        </Tooltip>
      </Box>
      <Menu
        anchorEl={anchorEl}
        id="account-menu"
        open={open}
        onClose={handleClose}
        onClick={handleClose}
        slotProps={{
          paper: {
            elevation: 0,
            sx: {
              overflow: "visible",
              filter: "drop-shadow(0px 2px 8px rgba(0,0,0,0.32))",
              mt: 1.5,
              "& .MuiAvatar-root": {
                width: 32,
                height: 32,
                ml: -0.5,
                mr: 1,
              },
              "&::before": {
                content: '""',
                display: "block",
                position: "absolute",
                top: 0,
                right: 14,
                width: 10,
                height: 10,
                bgcolor: "background.paper",
                transform: "translateY(-50%) rotate(45deg)",
                zIndex: 0,
              },
            },
          },
        }}
        transformOrigin={{ horizontal: "right", vertical: "top" }}
        anchorOrigin={{ horizontal: "right", vertical: "bottom" }}
      >
        <MenuItem onClick={handleClose}>پروفايل</MenuItem>
        <MenuItem onClick={handleClose}>حساب كاربری</MenuItem>
        <Divider />
        <MenuItem onClick={handleClose}>
          <ListItemIcon>
            <Settings fontSize="small" />
          </ListItemIcon>
          تنظیمات
        </MenuItem>
        <MenuItem onClick={handleSignOut}>
          <ListItemIcon>
            <Logout fontSize="small" />
          </ListItemIcon>
          خروج از سیستم
        </MenuItem>
      </Menu>
    </>
  );
}
