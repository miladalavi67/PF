import * as React from "react";
import {
  List,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  Collapse,
  Tooltip,
} from "@mui/material";
import { useNavigate, useLocation } from "react-router-dom";
import type { IUserMenu } from "../utilities/types";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { useTemplateStore } from "../utilities/templateStore";

interface MenuContentProps {
  open?: boolean;
  onToggle: () => void;
  searchQuery?: string;
}

export default function MenuContent({
  open = true,
  onToggle,
  searchQuery = "",
}: MenuContentProps) {
  const navigate = useNavigate();
  const location = useLocation();
  const [openSubMenus, setOpenSubMenus] = React.useState<
    Record<string, boolean>
  >({});
  const { userMenu } = useTemplateStore();

  const toggleSubMenu = (label: string) => {
    if (!open) onToggle();
    setOpenSubMenus((prev) => ({
      ...prev,
      [label]: !prev[label],
    }));
  };

  const isActive = (path?: string): boolean => {
    if (!path) return false;
    return location.pathname === path;
  };

  // فیلتر  جستجو
  const filteredMenu = React.useMemo(() => {
    if (!searchQuery.trim()) return userMenu.menuData;

    const query = searchQuery.toLowerCase().trim();

    const filterItems = (items: IUserMenu[]): IUserMenu[] => {
      const result: IUserMenu[] = [];

      for (const item of items) {
        const nameMatches = item.title.toLowerCase().includes(query);

        let filteredChildren: IUserMenu[] = [];
        if (item.children?.length) {
          filteredChildren = filterItems(item.children);
        }

        const hasMatchingChildren = filteredChildren.length > 0;

        if (nameMatches || hasMatchingChildren) {
          result.push({
            ...item,
            children: hasMatchingChildren ? filteredChildren : item.children,
          });
        }
      }

      return result;
    };

    return filterItems(userMenu.menuData);
  }, [userMenu.menuData, searchQuery]);

  const renderMenuItem = (item: IUserMenu, level = 0) => {

    const hasChildren = !!item.children?.length;
    const isSubMenuOpen = openSubMenus[item.title] ?? false;
    // const IconComponent = item.icon;

    // حالت بسته
    if (!open && level === 0) {
      return (
        <Tooltip
          key={item.id}
          title={item.title}
          placement="right"
          arrow
        >
          <ListItemButton
            onClick={() => {
              if (hasChildren) {
                toggleSubMenu(item.title);
              } else if (item.path) {
                navigate(item.path);
              }
            }}
            sx={{
              minHeight: 48,
              justifyContent: "center",
              alignItems: "flex-start",
              py: 1,
            }}
            selected={item.path ? isActive(item.path) : isActive(undefined)}
          >
            <ListItemIcon
              sx={{
                minWidth: 0,
                justifyContent: "center",
              }}
            >
              {/* {IconComponent ? (
                <IconComponent fontSize="small" />
              ) : (
                <Circle fontSize="small" />
              )} */}
            </ListItemIcon>
          </ListItemButton>
        </Tooltip>
      );
    }

    // حالت باز
    return (
      <React.Fragment key={item.id}>
        <ListItemButton
          onClick={() => {
            if (hasChildren) {
              toggleSubMenu(item.title);
            } else if (item.path) {
              navigate(item.path);
            }
          }}
          sx={{
            minHeight: 48,
            justifyContent: "initial",
            //px: 2.5 + level * 2,
            //pl: 2.5 + level * 3,           
          }}
          selected={item.path ? isActive(item.path) : isActive(undefined)}

        >
          <ListItemIcon sx={{ minWidth: 0, mr: 3, justifyContent: "center" }}>
            {/* {IconComponent ? (
              <IconComponent fontSize="small" />
            ) : (
              <Circle fontSize="small" />
            )} */}
          </ListItemIcon>
          <ListItemText
            primary={item.title}
            primaryTypographyProps={{
              sx: {
                whiteSpace: "normal",
                wordBreak: "break-word",
                lineHeight: 1.5,
              },
            }}
          />
          {hasChildren && (isSubMenuOpen ? <ExpandLess /> : <ExpandMore />)}
        </ListItemButton>

        {hasChildren && (
          <Collapse in={isSubMenuOpen} timeout="auto" unmountOnExit>
            <List component="div" disablePadding>
              {item.children!.map((child) => renderMenuItem(child, level + 1))}
            </List>
          </Collapse>
        )}
      </React.Fragment>
    );
  };

  return <List>{filteredMenu.map((item) => renderMenuItem(item))}</List>;
}
