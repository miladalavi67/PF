import { styled, useTheme } from '@mui/material/styles';
import MuiDrawer from '@mui/material/Drawer';
import {
  Box,
  IconButton,
  Stack,
  Tooltip,
  Typography,
  TextField,
  InputAdornment
} from '@mui/material';
import MenuOpenIcon from '@mui/icons-material/MenuOpen';
import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search';
import { Logout } from '@mui/icons-material';
import MenuContent from './MenuContent';
import SmallLogo from '../images/ikco_logo.png';
import Logo from '../images/logo2.png';
import { appBarHeigth, collapsedWidth, drawerWidth } from './constants';
import { useTemplateStore } from '../utilities/templateStore';
import React from 'react';

interface UserInfo {
  fullName: string;
  profileImage?: string;
  employeeCode?: string;
}

interface SideMenuProps {
  open: boolean;
  onToggle: () => void;
  userInfo?: UserInfo | null;
}

const Drawer = styled(MuiDrawer, {
  shouldForwardProp: (prop) => prop !== 'open'
})<{ open: boolean }>(({ theme, open }) => ({
  width: open ? drawerWidth : collapsedWidth,
  flexShrink: 0,
  whiteSpace: 'nowrap',
  boxSizing: 'border-box',
  transition: theme.transitions.create('width', {
    easing: theme.transitions.easing.sharp,
    duration: open ? theme.transitions.duration.enteringScreen : theme.transitions.duration.leavingScreen
  }),
  overflowX: 'hidden',
  '& .MuiDrawer-paper': {
    width: open ? drawerWidth : collapsedWidth,
    overflowX: 'hidden',
    transition: theme.transitions.create('width', {
      easing: theme.transitions.easing.sharp,
      duration: open ? theme.transitions.duration.enteringScreen : theme.transitions.duration.leavingScreen
    }),
    borderRight: 'none',
    boxShadow: theme.shadows[1],
    height: '100vh',
    display: 'flex',
    flexDirection: 'column'
  }
}));

export default function SideMenu({ open, onToggle, userInfo }: SideMenuProps) {
  const theme = useTheme();
  const { layoutConfig, headerLogo, collapsedLogo, appLogo } = useTemplateStore();

  const [searchQuery, setSearchQuery] = React.useState('');



  const displayName = userInfo?.fullName || 'کاربر';
  const appName = layoutConfig?.applicationName || 'نام نرم‌افزار';

  return (
    <Drawer variant="permanent" open={open}>
      {/* هدر اصلی */}
      <Box
        sx={{
          display: 'flex',
          alignItems: 'center',
          justifyContent: open ? 'space-between' : 'center',
          borderBottom: `1px solid ${theme.palette.divider}`,
          bgcolor: 'background.paper',
          height: appBarHeigth,
          px: open ? 3 : 1,
          position: 'relative',
        }}
      >

        <IconButton
          onClick={onToggle}
          size="large"
          sx={{ color: theme.palette.text.primary }}

        >
          {open ? (
            <MenuOpenIcon sx={{ fontSize: "35px" }} />
          ) : (
            <MenuIcon sx={{ fontSize: "28px" }} />
          )}
        </IconButton>

        {open && (
          <Box sx={{ display: 'flex', alignItems: 'center' }}>
            <img src={open ? (headerLogo || Logo) : (collapsedLogo || SmallLogo)}
              alt="Logo"
              style={{ height: 50 }} />
          </Box>
        )}

      </Box>

      {/* بخش لوگو نرم‌افزار + نام + جستجو */}
      <Box
        sx={{
          px: 3,
          py: 2,
          display: 'flex',
          flexDirection: 'column',
          alignItems: 'center',
          gap: 2,
          borderBottom: open ? `1px solid ${theme.palette.divider}` : 'none',
          bgcolor: 'background.paper',
        }}
      >
        {/* لوگوی نرم‌افزار - همیشه */}
        <img
          src={appLogo || SmallLogo}
          alt="App Logo"
          style={{
            width: open ? 60 : 40,
            height: open ? 60 : 40,
            borderRadius: 12,
            objectFit: 'contain'
          }}
        />

        {/* نام نرم‌افزار و جستجو  */}
        {open && (
          <>
            <Typography
              variant="subtitle1"
              fontWeight={600}
              sx={{ color: 'text.primary', fontSize: '0.95rem' }}
            >
              {appName}
            </Typography>

            {/* فیلد جستجو */}
            <TextField
              placeholder="جستجو در منو..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              size="small"
              fullWidth
              variant="outlined"
              InputProps={{
                startAdornment: (
                  <InputAdornment position="start">
                    <SearchIcon fontSize="small" />
                  </InputAdornment>
                ),
              }}
              sx={{
                '& .MuiOutlinedInput-root': {
                  borderRadius: 3,
                  fontSize: '0.875rem',
                },
              }}
            />
          </>
        )}
      </Box>

      {/* محتوای منو - با قابلیت فیلتر بر اساس جستجو */}
      <Box sx={{ flexGrow: 1, overflowY: 'auto', overflowX: 'hidden', bgcolor: 'background.paper', }}>
        <MenuContent
          open={open}
          onToggle={onToggle}
          searchQuery={searchQuery}
        />
      </Box>

      {/* فوتر */}
      <Stack
        sx={{
          p: open ? 2 : 1,
          gap: 2,
          alignItems: 'center',
          borderTop: `1px solid ${theme.palette.divider}`,
          justifyContent: open ? 'space-between' : 'center',
          flexDirection: open ? 'row' : 'column',
          bgcolor: 'background.paper',
        }}
      >
        {open && (
          <>
            <Box sx={{ textAlign: 'left' }}>
              <Typography variant="body2" fontWeight={500}>
                {displayName}
              </Typography>
              {userInfo?.employeeCode && (
                <Typography variant="caption" color="text.disabled">
                  کد پرسنلی: {userInfo.employeeCode}
                </Typography>
              )}
            </Box>

            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <Typography color='text.disabled' fontSize={12}>version 1.2.0</Typography>
              <Tooltip title="خروج">
                <IconButton >
                  <Logout color="action" />
                </IconButton>
              </Tooltip>
            </Box>
          </>
        )}
      </Stack>
    </Drawer>
  );
}
