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
import BodyLicenseTrackingReport from "./pages/body-shop/reports/BodyLicenseTrackingReport";
import BodyProductsOfSitesReport from "./pages/body-shop/reports/BodyProductsOfSitesReport";
import BodyPreAssignCountReport from "./pages/body-shop/reports/BodyPreAssignCountReport";
import BodyPreAssignCriticalReport from "./pages/body-shop/reports/BodyPreAssignCriticalReport";
import BodyConflictsReport from "./pages/body-shop/reports/BodyConflictsReport";
import BodyEventLogReport from "./pages/body-shop/reports/BodyEventLogReport";
import BodySapFailedTransfersReport from "./pages/body-shop/reports/BodySapFailedTransfersReport";
import MechCriticalRegister from "./pages/mechanical/MechCriticalRegister";
import MechCriticalSearch from "./pages/mechanical/MechCriticalSearch";
import MechExceptions from "./pages/mechanical/MechExceptions";
import MechNotPossibleReasons from "./pages/mechanical/MechNotPossibleReasons";
import MechContrastOfSubClasses from "./pages/mechanical/MechContrastOfSubClasses";
import MechReceivedPartsReport from "./pages/mechanical/reports/MechReceivedPartsReport";
import MechLicenseChange from "./pages/mechanical/MechLicenseChange";
import MechProductionPossibilityReport from "./pages/mechanical/reports/MechProductionPossibilityReport";
import MechProductionPossibilityWithLicenseReport from "./pages/mechanical/reports/MechProductionPossibilityWithLicenseReport";
import MechProductionPossibilityByTrimShopReport from "./pages/mechanical/reports/MechProductionPossibilityByTrimShopReport";
import MechGeneralReport from "./pages/mechanical/reports/MechGeneralReport";
import MechIncomingBodiesReport from "./pages/mechanical/reports/MechIncomingBodiesReport";
import MechIncomingBodiesSummaryReport from "./pages/mechanical/reports/MechIncomingBodiesSummaryReport";
import MechLicenseTrackingReport from "./pages/mechanical/reports/MechLicenseTrackingReport";
import MechLicenseTrackingCriticalsReport from "./pages/mechanical/reports/MechLicenseTrackingCriticalsReport";
import MechUsedLicenseBodiesReport from "./pages/mechanical/reports/MechUsedLicenseBodiesReport";
import MechLicenseLogReport from "./pages/mechanical/reports/MechLicenseLogReport";
import MechExceptionPartNumbersReport from "./pages/mechanical/reports/MechExceptionPartNumbersReport";
import MechExceptionLicenseLogReport from "./pages/mechanical/reports/MechExceptionLicenseLogReport";
import MechStockLogReport from "./pages/mechanical/reports/MechStockLogReport";
import MechSapCriticalLogReport from "./pages/mechanical/reports/MechSapCriticalLogReport";
import MechSapFailedTransfersReport from "./pages/mechanical/reports/MechSapFailedTransfersReport";
import MechAsnReport from "./pages/mechanical/reports/MechAsnReport";
import MechTrimInConflictsReport from "./pages/mechanical/reports/MechTrimInConflictsReport";
import MechTrimShopBodyCountReport from "./pages/mechanical/reports/MechTrimShopBodyCountReport";
import MechTrimShopCriticalBodiesReport from "./pages/mechanical/reports/MechTrimShopCriticalBodiesReport";
import SyncBasisManagement from "./pages/synchron/SyncBasisManagement";
import SyncSetManagement from "./pages/synchron/SyncSetManagement";
import SyncIndicatorCars from "./pages/synchron/SyncIndicatorCars";
import SyncProductionPossibilityReport from "./pages/synchron/reports/SyncProductionPossibilityReport";
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
  { path: "/body-shop/reports/license-tracking", element: <BodyLicenseTrackingReport /> },
  { path: "/body-shop/reports/products-of-sites", element: <BodyProductsOfSitesReport /> },
  { path: "/body-shop/reports/pre-assign-count", element: <BodyPreAssignCountReport /> },
  { path: "/body-shop/reports/pre-assign-critical", element: <BodyPreAssignCriticalReport /> },
  { path: "/body-shop/reports/conflicts", element: <BodyConflictsReport /> },
  { path: "/body-shop/reports/event-log", element: <BodyEventLogReport /> },
  { path: "/body-shop/reports/sap-failed-transfers", element: <BodySapFailedTransfersReport /> },
  { path: "/mechanical/register", element: <MechCriticalRegister /> },
  { path: "/mechanical/search", element: <MechCriticalSearch /> },
  { path: "/mechanical/exceptions", element: <MechExceptions /> },
  { path: "/mechanical/not-possible-reasons", element: <MechNotPossibleReasons /> },
  { path: "/mechanical/contrast-of-subclasses", element: <MechContrastOfSubClasses /> },
  { path: "/mechanical/received-parts", element: <MechReceivedPartsReport /> },
  { path: "/mechanical/license-change", element: <MechLicenseChange /> },
  { path: "/mechanical/reports/production-possibility", element: <MechProductionPossibilityReport /> },
  { path: "/mechanical/reports/production-possibility-with-license", element: <MechProductionPossibilityWithLicenseReport /> },
  { path: "/mechanical/reports/production-possibility-by-trim-shop", element: <MechProductionPossibilityByTrimShopReport /> },
  { path: "/mechanical/reports/general", element: <MechGeneralReport /> },
  { path: "/mechanical/reports/incoming-bodies", element: <MechIncomingBodiesReport /> },
  { path: "/mechanical/reports/incoming-bodies-summary", element: <MechIncomingBodiesSummaryReport /> },
  { path: "/mechanical/reports/license-tracking", element: <MechLicenseTrackingReport /> },
  { path: "/mechanical/reports/license-tracking-criticals", element: <MechLicenseTrackingCriticalsReport /> },
  { path: "/mechanical/reports/used-license-bodies", element: <MechUsedLicenseBodiesReport /> },
  { path: "/mechanical/reports/license-log", element: <MechLicenseLogReport /> },
  { path: "/mechanical/reports/exception-part-numbers", element: <MechExceptionPartNumbersReport /> },
  { path: "/mechanical/reports/exception-license-log", element: <MechExceptionLicenseLogReport /> },
  { path: "/mechanical/reports/stock-log", element: <MechStockLogReport /> },
  { path: "/mechanical/reports/sap-critical-log", element: <MechSapCriticalLogReport /> },
  { path: "/mechanical/reports/sap-failed-transfers", element: <MechSapFailedTransfersReport /> },
  { path: "/mechanical/reports/asn", element: <MechAsnReport /> },
  { path: "/mechanical/reports/trim-in-conflicts", element: <MechTrimInConflictsReport /> },
  { path: "/mechanical/reports/trim-shop-body-count", element: <MechTrimShopBodyCountReport /> },
  { path: "/mechanical/reports/trim-shop-critical-bodies", element: <MechTrimShopCriticalBodiesReport /> },
  { path: "/synchron/basis", element: <SyncBasisManagement /> },
  { path: "/synchron/sets", element: <SyncSetManagement /> },
  { path: "/synchron/indicator-cars", element: <SyncIndicatorCars /> },
  { path: "/synchron/reports/production-possibility", element: <SyncProductionPossibilityReport /> },
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
