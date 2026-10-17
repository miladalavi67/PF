import type { MFEManifest, MFERouteItem } from "@ikco/mfe-core";
import packageJson from "../package.json" with { type: "json" };

/**
 * شناسنامه میکروفرانت «سامانه جامع امکان تولید» برای پوسته دریچه (Daricheh).
 * هر permissionKey یک برگ UI در PfPermissionTree.Ui (Backend) است و هر مسیر باید در App.tsx هم تعریف شود
 * (npm run check:manifest). این فایل از روی catalog صفحات تولید شده است؛ تغییر دستی مجاز است.
 */

const group = (id: string, parentId: string | null, title: string, order: number, children: MFERouteItem[]): MFERouteItem => ({
  id,
  parentId,
  title,
  path: null,
  permissionKey: null,
  componentName: null,
  componentPath: null,
  icon: "Folder",
  order,
  children,
});

const leaf = (
  id: string,
  parentId: string,
  title: string,
  path: string,
  permissionKey: string,
  componentName: string,
  folder: string,
  order: number,
): MFERouteItem => ({
  id,
  parentId,
  title,
  path,
  permissionKey,
  componentName,
  componentPath: `./src/pages/${folder}/${componentName}`,
  icon: "Description",
  order,
});

const manifest: MFEManifest = {
  appKey: "productionfeasibility",
  version: packageJson.version,
  routeMenus: [
    group("bodyshop", null, "بدنه سازی", 1, [
      group("bodyshop.operations", "bodyshop", "عملیات", 1, [
        leaf("body-shop.register", "bodyshop.operations", "ثبت بحران", "/body-shop/register", "bodyshop.menu.register", "BodyCriticalRegister", "body-shop", 1),
        leaf("body-shop.search", "bodyshop.operations", "جستجوی اقلام بحرانی", "/body-shop/search", "bodyshop.menu.search", "BodyCriticalSearch", "body-shop", 2),
      ]),
      group("bodyshop.reports", "bodyshop", "گزارشات", 2, [
        leaf("body-shop.reports.tracking", "bodyshop.reports", "گزارش ردیابی تغییرات اقلام بحرانی", "/body-shop/reports/tracking", "bodyshop.menu.tracking", "BodyTrackingReport", "body-shop/reports", 1),
        leaf("body-shop.reports.critical-parts", "bodyshop.reports", "گزارش اقلام بحرانی", "/body-shop/reports/critical-parts", "bodyshop.menu.criticalreport", "BodyCriticalPartsReport", "body-shop/reports", 2),
        leaf("body-shop.reports.pre-assign-critical", "bodyshop.reports", "گزارش پیش تخصیص بدنه‌های بحرانی", "/body-shop/reports/pre-assign-critical", "bodyshop.menu.preassigncritical", "BodyPreAssignCriticalReport", "body-shop/reports", 3),
        leaf("body-shop.reports.conflicts", "bodyshop.reports", "گزارش مغایرت پیش تخصیص بدنه‌های بحرانی", "/body-shop/reports/conflicts", "bodyshop.menu.conflicts", "BodyConflictsReport", "body-shop/reports", 4),
        leaf("body-shop.reports.products-of-sites", "bodyshop.reports", "گزارش محصولات به تفکیک خطوط بدنه‌سازی", "/body-shop/reports/products-of-sites", "bodyshop.menu.productsofsites", "BodyProductsOfSitesReport", "body-shop/reports", 5),
      ]),
    ]),
    group("mechanical", null, "مونتاژ - مکانیکی", 2, [
      group("mechanical.operations", "mechanical", "عملیات", 1, [
        leaf("mechanical.register", "mechanical.operations", "تعریف اقلام بحرانی قطعات مکانیکی", "/mechanical/register", "mechanical.menu.register", "MechCriticalRegister", "mechanical", 1),
        leaf("mechanical.search", "mechanical.operations", "جستجوی اقلام بحرانی", "/mechanical/search", "mechanical.menu.search", "MechCriticalSearch", "mechanical", 2),
      ]),
      group("mechanical.reports", "mechanical", "گزارشات", 2, [
        leaf("mechanical.reports.production-possibility", "mechanical.reports", "گزارش ردیابی اقلام بحرانی", "/mechanical/reports/production-possibility", "mechanical.menu.productionpossibility", "MechProductionPossibilityReport", "mechanical/reports", 1),
        leaf("mechanical.reports.general", "mechanical.reports", "گزارش جامع اقلام بحرانی", "/mechanical/reports/general", "mechanical.menu.general", "MechGeneralReport", "mechanical/reports", 2),
        leaf("mechanical.reports.asn", "mechanical.reports", "گزارش ASN های در راه", "/mechanical/reports/asn", "mechanical.menu.asn", "MechAsnReport", "mechanical/reports", 3),
      ]),
    ]),
    group("synchron", null, "مونتاژ - سنکرون", 3, [
      group("synchron.operations", "synchron", "عملیات", 1, [
        leaf("synchron.sets", "synchron.operations", "تعریف اقلام بحرانی سنکرون", "/synchron/sets", "synchron.menu.sets", "SyncSetManagement", "synchron", 1),
      ]),
    ]),
    group("system", null, "مدیریت سیستم", 4, [
      group("system.admin", "system", "مدیریت کاربران", 1, [
        leaf("user-management", "system.admin", "مدیریت کاربران", "/user-management", "usermanagement.menu.access", "UserAccessManagement", "admin", 1),
      ]),
    ]),
  ],
  internalRoutes: [
  ],
};

export default manifest;
