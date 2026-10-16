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
    group("bodyshop", null, "بدنه‌سازی", 1, [
      group("bodyshop.operations", "bodyshop", "عملیات", 1, [
        leaf("body-shop.register", "bodyshop.operations", "ثبت بحران بدنه‌سازی", "/body-shop/register", "bodyshop.menu.register", "BodyCriticalRegister", "body-shop", 1),
        leaf("body-shop.search", "bodyshop.operations", "جستجوی اقلام بحرانی", "/body-shop/search", "bodyshop.menu.search", "BodyCriticalSearch", "body-shop", 2),
      ]),
      group("bodyshop.reports", "bodyshop", "گزارش‌ها", 2, [
        leaf("body-shop.reports.tracking", "bodyshop.reports", "گزارش ردیابی تغییرات اقلام بحرانی", "/body-shop/reports/tracking", "bodyshop.menu.tracking", "BodyTrackingReport", "body-shop/reports", 1),
        leaf("body-shop.reports.critical-parts", "bodyshop.reports", "گزارش اقلام بحرانی", "/body-shop/reports/critical-parts", "bodyshop.menu.criticalreport", "BodyCriticalPartsReport", "body-shop/reports", 2),
        leaf("body-shop.reports.license-tracking", "bodyshop.reports", "ردیابی پارامترهای بحرانی", "/body-shop/reports/license-tracking", "bodyshop.menu.licensetracking", "BodyLicenseTrackingReport", "body-shop/reports", 3),
        leaf("body-shop.reports.products-of-sites", "bodyshop.reports", "محصولات به تفکیک خطوط بدنه‌سازی", "/body-shop/reports/products-of-sites", "bodyshop.menu.productsofsites", "BodyProductsOfSitesReport", "body-shop/reports", 4),
        leaf("body-shop.reports.pre-assign-count", "bodyshop.reports", "تعداد بدنه‌های پیش‌تخصیص‌شده", "/body-shop/reports/pre-assign-count", "bodyshop.menu.preassigncount", "BodyPreAssignCountReport", "body-shop/reports", 5),
        leaf("body-shop.reports.pre-assign-critical", "bodyshop.reports", "بدنه‌های پیش‌تخصیص بحرانی", "/body-shop/reports/pre-assign-critical", "bodyshop.menu.preassigncritical", "BodyPreAssignCriticalReport", "body-shop/reports", 6),
        leaf("body-shop.reports.conflicts", "bodyshop.reports", "مغایرت پیش‌تخصیص بدنه‌های بحرانی", "/body-shop/reports/conflicts", "bodyshop.menu.conflicts", "BodyConflictsReport", "body-shop/reports", 7),
        leaf("body-shop.reports.event-log", "bodyshop.reports", "گزارش رویدادها", "/body-shop/reports/event-log", "bodyshop.menu.eventlog", "BodyEventLogReport", "body-shop/reports", 8),
        leaf("body-shop.reports.sap-failed-transfers", "bodyshop.reports", "قطعات ناموفق در انتقال از SAP", "/body-shop/reports/sap-failed-transfers", "bodyshop.menu.sap", "BodySapFailedTransfersReport", "body-shop/reports", 9),
      ]),
    ]),
    group("mechanical", null, "مونتاژ - مکانیکی", 2, [
      group("mechanical.operations", "mechanical", "عملیات", 1, [
        leaf("mechanical.register", "mechanical.operations", "ثبت بحران مکانیکی", "/mechanical/register", "mechanical.menu.register", "MechCriticalRegister", "mechanical", 1),
        leaf("mechanical.search", "mechanical.operations", "جستجوی اقلام بحرانی (مکانیکی و سنکرون)", "/mechanical/search", "mechanical.menu.search", "MechCriticalSearch", "mechanical", 2),
        leaf("mechanical.exceptions", "mechanical.operations", "استثناهای محصولات بحران", "/mechanical/exceptions", "mechanical.menu.exceptions", "MechExceptions", "mechanical", 3),
        leaf("mechanical.not-possible-reasons", "mechanical.operations", "دلایل عدم امکان تولید", "/mechanical/not-possible-reasons", "mechanical.menu.reasons", "MechNotPossibleReasons", "mechanical", 4),
        leaf("mechanical.contrast-of-subclasses", "mechanical.operations", "مغایرت زیرکلاس‌ها با SAP", "/mechanical/contrast-of-subclasses", "mechanical.menu.contrast", "MechContrastOfSubClasses", "mechanical", 5),
        leaf("mechanical.received-parts", "mechanical.operations", "اقلام بحرانی وارد شده به شرکت", "/mechanical/received-parts", "mechanical.menu.arrived", "MechReceivedPartsReport", "mechanical/reports", 6),
      ]),
      group("mechanical.reports", "mechanical", "گزارش‌ها", 2, [
        leaf("mechanical.reports.production-possibility", "mechanical.reports", "گزارش امکان تولید", "/mechanical/reports/production-possibility", "mechanical.menu.productionpossibility", "MechProductionPossibilityReport", "mechanical/reports", 1),
        leaf("mechanical.reports.production-possibility-with-license", "mechanical.reports", "گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده", "/mechanical/reports/production-possibility-with-license", "mechanical.menu.productionpossibilitylicense", "MechProductionPossibilityWithLicenseReport", "mechanical/reports", 2),
        leaf("mechanical.reports.production-possibility-by-trim-shop", "mechanical.reports", "امکان تولید به تفکیک سالن مونتاژ", "/mechanical/reports/production-possibility-by-trim-shop", "mechanical.menu.productionpossibilitytrimshop", "MechProductionPossibilityByTrimShopReport", "mechanical/reports", 3),
        leaf("mechanical.reports.general", "mechanical.reports", "گزارش جامع اقلام بحرانی", "/mechanical/reports/general", "mechanical.menu.general", "MechGeneralReport", "mechanical/reports", 4),
        leaf("mechanical.reports.incoming-bodies", "mechanical.reports", "بدنه‌های وارد شده به خط به تفکیک بحران", "/mechanical/reports/incoming-bodies", "mechanical.menu.incoming", "MechIncomingBodiesReport", "mechanical/reports", 5),
        leaf("mechanical.reports.incoming-bodies-summary", "mechanical.reports", "خلاصه بدنه‌های وارد شده به خط", "/mechanical/reports/incoming-bodies-summary", "mechanical.menu.incomingsummary", "MechIncomingBodiesSummaryReport", "mechanical/reports", 6),
        leaf("mechanical.reports.license-tracking", "mechanical.reports", "ردیابی مجوزهای تولید با کسری", "/mechanical/reports/license-tracking", "mechanical.menu.licensetracking", "MechLicenseTrackingReport", "mechanical/reports", 7),
        leaf("mechanical.reports.license-tracking-criticals", "mechanical.reports", "بحران‌های دارای مجوز تولید با کسری", "/mechanical/reports/license-tracking-criticals", "mechanical.menu.licensecriticals", "MechLicenseTrackingCriticalsReport", "mechanical/reports", 8),
        leaf("mechanical.reports.used-license-bodies", "mechanical.reports", "بدنه‌های مرتبط با مجوز مصرف‌شده", "/mechanical/reports/used-license-bodies", "mechanical.menu.usedlicensebodies", "MechUsedLicenseBodiesReport", "mechanical/reports", 9),
        leaf("mechanical.reports.license-log", "mechanical.reports", "تاریخچه مجوزهای یک بحران", "/mechanical/reports/license-log", "mechanical.menu.licenselog", "MechLicenseLogReport", "mechanical/reports", 10),
        leaf("mechanical.reports.exception-part-numbers", "mechanical.reports", "شماره فنی‌های دارای استثنا", "/mechanical/reports/exception-part-numbers", "mechanical.menu.exceptionparts", "MechExceptionPartNumbersReport", "mechanical/reports", 11),
        leaf("mechanical.reports.exception-license-log", "mechanical.reports", "تاریخچه مجوزهای استثنا", "/mechanical/reports/exception-license-log", "mechanical.menu.exceptionlog", "MechExceptionLicenseLogReport", "mechanical/reports", 12),
        leaf("mechanical.reports.stock-log", "mechanical.reports", "تاریخچه موجودی اقلام بحرانی", "/mechanical/reports/stock-log", "mechanical.menu.stocklog", "MechStockLogReport", "mechanical/reports", 13),
        leaf("mechanical.reports.sap-critical-log", "mechanical.reports", "لاگ انتقال اقلام بحرانی از SAP", "/mechanical/reports/sap-critical-log", "mechanical.menu.sapcritical", "MechSapCriticalLogReport", "mechanical/reports", 14),
        leaf("mechanical.reports.sap-failed-transfers", "mechanical.reports", "قطعات ناموفق در انتقال از SAP", "/mechanical/reports/sap-failed-transfers", "mechanical.menu.sapfailed", "MechSapFailedTransfersReport", "mechanical/reports", 15),
        leaf("mechanical.reports.asn", "mechanical.reports", "گزارش ASN", "/mechanical/reports/asn", "mechanical.menu.asn", "MechAsnReport", "mechanical/reports", 16),
        leaf("mechanical.reports.trim-in-conflicts", "mechanical.reports", "دلایل عدم امکان ورود خودرو به مونتاژ", "/mechanical/reports/trim-in-conflicts", "mechanical.menu.trimconflicts", "MechTrimInConflictsReport", "mechanical/reports", 17),
        leaf("mechanical.reports.trim-shop-body-count", "mechanical.reports", "تعداد بدنه‌های ورودی به مونتاژ", "/mechanical/reports/trim-shop-body-count", "mechanical.menu.trimbodycount", "MechTrimShopBodyCountReport", "mechanical/reports", 18),
        leaf("mechanical.reports.trim-shop-critical-bodies", "mechanical.reports", "بدنه‌های بحرانی ورودی به مونتاژ", "/mechanical/reports/trim-shop-critical-bodies", "mechanical.menu.trimcriticalbodies", "MechTrimShopCriticalBodiesReport", "mechanical/reports", 19),
      ]),
    ]),
    group("synchron", null, "مونتاژ - سنکرون", 3, [
      group("synchron.operations", "synchron", "عملیات", 1, [
        leaf("synchron.basis", "synchron.operations", "تعریف مبنای ست‌ها", "/synchron/basis", "synchron.menu.basis", "SyncBasisManagement", "synchron", 1),
        leaf("synchron.sets", "synchron.operations", "ثبت و ویرایش بحران سنکرون", "/synchron/sets", "synchron.menu.sets", "SyncSetManagement", "synchron", 2),
        leaf("synchron.indicator-cars", "synchron.operations", "تعیین خودروی شاخص سالن‌ها", "/synchron/indicator-cars", "synchron.menu.indicatorcars", "SyncIndicatorCars", "synchron", 3),
      ]),
      group("synchron.reports", "synchron", "گزارش‌ها", 2, [
        leaf("synchron.reports.production-possibility", "synchron.reports", "گزارش امکان تولید سنکرون", "/synchron/reports/production-possibility", "synchron.menu.productionpossibility", "SyncProductionPossibilityReport", "synchron/reports", 1),
      ]),
    ]),
    group("system", null, "مدیریت سیستم", 4, [
      group("system.admin", "system", "مدیریت کاربران", 1, [
        leaf("user-management", "system.admin", "مدیریت کاربران", "/user-management", "usermanagement.menu.access", "UserAccessManagement", "admin", 1),
      ]),
    ]),
  ],
  internalRoutes: [
    {
      path: "/mechanical/license-change",
      componentName: "MechLicenseChange",
      componentPath: "./src/pages/mechanical/MechLicenseChange",
    },
  ],
};

export default manifest;
