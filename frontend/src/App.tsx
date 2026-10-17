import type { RouteObject } from "react-router";
import { IkcoTemplateProvider } from "@ikco/frontend-kit";
import {
  createMFEAuthContext,
  MFEProvider,
  setAccessTokenProvider,
  type MFERouteItem,
} from "@ikco/mfe-core";
import { useEffect, useMemo } from "react";
import manifest from "./mfe-manifest";
import { devAccessToken, openAccessMode, useDevSessionStore } from "./store/devSessionStore";
import DevLogin from "./components/DevLogin";
import BodyCriticalRegister from "./pages/body-shop/BodyCriticalRegister";
import BodyCriticalSearch from "./pages/body-shop/BodyCriticalSearch";
import BodyTrackingReport from "./pages/body-shop/reports/BodyTrackingReport";
import BodyCriticalPartsReport from "./pages/body-shop/reports/BodyCriticalPartsReport";
import BodyPreAssignCriticalReport from "./pages/body-shop/reports/BodyPreAssignCriticalReport";
import BodyConflictsReport from "./pages/body-shop/reports/BodyConflictsReport";
import BodyProductsOfSitesReport from "./pages/body-shop/reports/BodyProductsOfSitesReport";
import MechCriticalRegister from "./pages/mechanical/MechCriticalRegister";
import MechCriticalSearch from "./pages/mechanical/MechCriticalSearch";
import MechProductionPossibilityReport from "./pages/mechanical/reports/MechProductionPossibilityReport";
import MechGeneralReport from "./pages/mechanical/reports/MechGeneralReport";
import MechAsnReport from "./pages/mechanical/reports/MechAsnReport";
import SyncSetManagement from "./pages/synchron/SyncSetManagement";
import UserAccessManagement from "./pages/admin/UserAccessManagement";

/**
 * اجرای مستقل میکروفرانت (npm run dev). در دریچه، پوسته منوها را از mfe-manifest.ts و صفحات را از componentPath
 * بارگذاری می‌کند و توکن/دسترسی‌ها را خودش فراهم می‌کند. هر مسیر اینجا باید در mfe-manifest.ts هم باشد
 * (npm run check:manifest). فایل از روی catalog صفحات تولید شده است.
 */
const routeObject: RouteObject[] = [
  { path: "/body-shop/register", element: <BodyCriticalRegister /> },
  { path: "/body-shop/search", element: <BodyCriticalSearch /> },
  { path: "/body-shop/reports/tracking", element: <BodyTrackingReport /> },
  { path: "/body-shop/reports/critical-parts", element: <BodyCriticalPartsReport /> },
  { path: "/body-shop/reports/pre-assign-critical", element: <BodyPreAssignCriticalReport /> },
  { path: "/body-shop/reports/conflicts", element: <BodyConflictsReport /> },
  { path: "/body-shop/reports/products-of-sites", element: <BodyProductsOfSitesReport /> },
  { path: "/mechanical/register", element: <MechCriticalRegister /> },
  { path: "/mechanical/search", element: <MechCriticalSearch /> },
  { path: "/mechanical/reports/production-possibility", element: <MechProductionPossibilityReport /> },
  { path: "/mechanical/reports/general", element: <MechGeneralReport /> },
  { path: "/mechanical/reports/asn", element: <MechAsnReport /> },
  { path: "/synchron/sets", element: <SyncSetManagement /> },
  { path: "/user-management", element: <UserAccessManagement /> },
];

/** همانند دریچه: فقط منوهایی که کاربر permissionKey آن‌ها را دارد (گروه‌های خالی حذف می‌شوند) */
const filterMenus = (items: MFERouteItem[], can: (key: string) => boolean): MFERouteItem[] =>
  items
    .map((item) => ({ ...item, children: item.children ? filterMenus(item.children, can) : undefined }))
    .filter((item) => (item.path ? !item.permissionKey || can(item.permissionKey) : (item.children?.length ?? 0) > 0));

function App() {
  const { token, userName, permissions, ready, restore, startOpenAccess } = useDevSessionStore();

  useEffect(() => {
    if (openAccessMode) startOpenAccess();
    else restore();
  }, [restore, startOpenAccess]);

  const roles = useMemo(() => new Set<string>(), []);
  const mfeAuth = useMemo(
    () => createMFEAuthContext(permissions, roles, { getAccessToken: devAccessToken }),
    [permissions, roles],
  );

  useEffect(() => {
    setAccessTokenProvider(mfeAuth.user.getAccessToken);
  }, [mfeAuth]);

  const menus = useMemo(() => filterMenus(manifest.routeMenus, (key) => permissions.has(key)), [permissions]);
  const userInfo = useMemo(() => ({ fullName: userName ?? "", department: "سامانه جامع امکان تولید" }), [userName]);

  // پوسته منو را یک‌بار می‌سازد؛ تا پیش از دریافت دسترسی‌ها نباید رندر شود
  if (!ready) return openAccessMode ? null : <DevLogin />;
  if (!openAccessMode && !token) return <DevLogin />;

  return (
    <MFEProvider auth={mfeAuth} appKey={manifest.appKey} manifest={manifest} basePath="">
      <IkcoTemplateProvider
        config={{
          applicationName: "سیستم جامع امکان تولید",
          mainPage: "/body-shop/register",
          prefixApplicationTitlePage: "امکان تولید",
          softwareId: Number(import.meta.env.VITE_SOFTWARE_ID ?? 0),
          apiBaseUrl: import.meta.env.VITE_API_BASE_URL,
        }}
        routes={routeObject}
        myMenus={menus}
        userInfo={userInfo}
        headerLogo="/images/ikco_logo.png"
        appLogo="/images/AvaLogo.png"
      />
    </MFEProvider>
  );
}

export default App;
