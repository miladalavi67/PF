# ProductionFeasibility API — React service catalog (generated)

| Service | Function | Verb | Route | Permission | Summary |
|---|---|---|---|---|---|
| bodyCriticalsService | getBodyCriticalsBodyInfo | GET | `api/production-feasibility/body-shop/critical-parts/body-info` | `bodyshop.criticalparts.view` | اطلاعات بدنه |
| bodyCriticalsService | getBodyCriticalsPartDescription | GET | `api/production-feasibility/body-shop/critical-parts/part-description` | `bodyshop.criticalparts.view` | شرح فارسی قطعه بر اساس شماره فنی |
| bodyCriticalsService | getBodyCriticalsPartProducts | GET | `api/production-feasibility/body-shop/critical-parts/part-products` | `bodyshop.criticalparts.view` | محصولات مصرف‌کننده یک شماره فنی در یک خط بدنه‌سازی |
| bodyCriticalsService | getBodyCriticalsCodeClassByPartNo | GET | `api/production-feasibility/body-shop/critical-parts/code-class-by-part-no` | `bodyshop.criticalparts.view` | کدهای کلاس مصرف‌کننده شماره فنی |
| bodyCriticalsService | createBodyCriticalsCriticalPart | POST | `api/production-feasibility/body-shop/critical-parts` | `bodyshop.criticalparts.create` | ثبت بحران بدنه‌سازی |
| bodyCriticalsService | updateBodyCriticalsCriticalPart | PUT | `api/production-feasibility/body-shop/critical-parts/{criticalPartsId:int}` | `bodyshop.criticalparts.update` | ویرایش بحران بدنه‌سازی |
| bodyCriticalsService | disableBodyCriticalsCriticalParts | POST | `api/production-feasibility/body-shop/critical-parts/disable` | `bodyshop.criticalparts.disable` | غیرفعال‌سازی بحران‌های بدنه‌سازی |
| bodyCriticalsService | getBodyCriticalsCriticalPartInfo | GET | `api/production-feasibility/body-shop/critical-parts/{criticalPartsId:int}/info` | `bodyshop.criticalparts.view` | اطلاعات یک بحران بدنه‌سازی برای ویرایش |
| bodyCriticalsService | getBodyCriticalsEventLog | GET | `api/production-feasibility/body-shop/critical-parts/event-log` | `bodyshop.criticalparts.view` | گزارش رویدادهای یک بحران |
| bodyCriticalsService | createBodyCriticalsProducts | POST | `api/production-feasibility/body-shop/critical-parts/{criticalPartsId:int}/products` | `bodyshop.criticalparts.create` | ثبت کلاس‌های مصرف‌کننده یک بحران بدنه‌سازی |
| bodyCriticalsService | registerBodyCriticalsLicense | POST | `api/production-feasibility/body-shop/critical-parts/licenses` | `bodyshop.criticalparts.license` | ثبت مجوز تولید با کسری (تغییر شرایط تولید) |
| bodyCriticalsService | batchBodyCriticalsCreate | POST | `api/production-feasibility/body-shop/critical-parts/batch` | `bodyshop.criticalparts.create` | ثبت گروهی بحران‌ها از اکسل |
| bodyCriticalsService | getBodyCriticalsSapWhereUsed | GET | `api/production-feasibility/body-shop/critical-parts/sap/where-used` | `bodyshop.criticalparts.view` | زیرکلاس‌های مصرف‌کننده شماره فنی از SAP |
| bodyReportsService | searchBodyReportsCriticalParts | POST | `api/production-feasibility/body-shop/reports/critical-parts/search` | `bodyshop.reports.search` | جستجوی اقلام بحرانی بدنه‌سازی |
| bodyReportsService | getBodyReportsCriticalPartProducts | GET | `api/production-feasibility/body-shop/reports/critical-parts/{criticalPartsId:int}/products` | `bodyshop.reports.search` | محصولات یک بحران |
| bodyReportsService | getBodyReportsTrackingCriticalParts | POST | `api/production-feasibility/body-shop/reports/tracking-critical-parts` | `bodyshop.reports.tracking` | ردیابی تغییرات اقلام بحرانی |
| bodyReportsService | getBodyReportsTrackingCriticalPartProducts | GET | `api/production-feasibility/body-shop/reports/tracking-critical-parts/{criticalPartsId:int}/products` | `bodyshop.reports.tracking` | محصولات بحران در ردیابی |
| bodyReportsService | getBodyReportsProductsOfSites | GET | `api/production-feasibility/body-shop/reports/products-of-sites` | `bodyshop.reports.productsofsites` | محصولات به تفکیک خطوط بدنه‌سازی |
| bodyReportsService | getBodyReportsPreAssignBodyCounts | GET | `api/production-feasibility/body-shop/reports/pre-assign-body-counts` | `bodyshop.reports.preassigncount` | تعداد بدنه‌های پیش‌تخصیص‌شده |
| bodyReportsService | getBodyReportsPreAssignCriticalBodies | GET | `api/production-feasibility/body-shop/reports/pre-assign-critical-bodies` | `bodyshop.reports.preassigncritical` | بدنه‌های پیش‌تخصیص بحرانی |
| bodyReportsService | getBodyReportsNotPossibleProductsToStepAssign | GET | `api/production-feasibility/body-shop/reports/not-possible-to-assign` | `bodyshop.reports.conflicts` | مغایرت پیش‌تخصیص بدنه‌های بحرانی |
| bodyReportsService | getBodyReportsShortageLicenseTracking | GET | `api/production-feasibility/body-shop/reports/shortage-license-tracking` | `bodyshop.reports.licensetracking` | ردیابی پارامترهای بحرانی (مجوز تولید با کسری) |
| bodyReportsService | getBodyReportsProductionPossibilityStatus | GET | `api/production-feasibility/body-shop/reports/production-possibility-status/{id:int}` | `bodyshop.reports.licensetracking` | وضعیت مجوز تولید یک بحران |
| bodyReportsService | getBodyReportsFailedSapTransfers | GET | `api/production-feasibility/body-shop/reports/failed-sap-transfers` | `bodyshop.reports.sap` | قطعات ناموفق در انتقال از SAP |
| bodyReportsService | getBodyReportsSapDataUpdateTimes | GET | `api/production-feasibility/body-shop/reports/sap-data-update-times` | `bodyshop.reports.sap` | زمان آخرین به‌روزرسانی داده‌های SAP |
| bodyReportsService | getBodyReportsLastChangeOnLicense | GET | `api/production-feasibility/body-shop/reports/last-change-on-license` | `bodyshop.reports.tracking` | شناسه آخرین تغییر مجوزها (برای به‌روزرسانی خودکار گزارش) |
| lookupService | getLookupCatalog | GET | `api/production-feasibility/lookups` | `lookups.view` | فهرست کلیدهای فهرست‌های مرجع |
| lookupService | getLookup | GET | `api/production-feasibility/lookups/{key}` | `lookups.view` | ردیف‌های یک فهرست مرجع |
| mechExceptionsService | getMechExceptionsExceptionList | POST | `api/production-feasibility/mechanical/exceptions/search` | `mechanical.exceptions.view` | فهرست بدنه‌های استثنا |
| mechExceptionsService | getMechExceptionsLicenses | GET | `api/production-feasibility/mechanical/exceptions/{id:int}/licenses` | `mechanical.exceptions.view` | مجوزهای استثنا |
| mechExceptionsService | getMechExceptionsLicenseBodies | GET | `api/production-feasibility/mechanical/exceptions/{id:int}/license-bodies` | `mechanical.exceptions.view` | بدنه‌های مصرف‌کننده مجوز استثنا |
| mechExceptionsService | getMechExceptionsLicenseLog | GET | `api/production-feasibility/mechanical/exceptions/license-log` | `mechanical.exceptions.view` | تاریخچه مجوزهای استثنا برای شماره فنی |
| mechExceptionsService | createMechExceptionsLicense | POST | `api/production-feasibility/mechanical/exceptions/licenses` | `mechanical.exceptions.create` | ثبت مجوز برای استثنا |
| mechExceptionsService | disableMechExceptionsLicense | POST | `api/production-feasibility/mechanical/exceptions/licenses/{id:int}/disable` | `mechanical.exceptions.disable` | غیرفعال‌سازی مجوز استثنا |
| mechExceptionsService | disableMechExceptionsExceptions | POST | `api/production-feasibility/mechanical/exceptions/disable` | `mechanical.exceptions.disable` | غیرفعال‌سازی استثناها |
| mechExceptionsService | getMechExceptionsPartNumbersInExceptions | POST | `api/production-feasibility/mechanical/exceptions/part-numbers-report` | `mechanical.exceptions.view` | گزارش شماره فنی‌های دارای استثنا |
| mechLicensesService | getMechLicensesTrackingInfo | GET | `api/production-feasibility/mechanical/licenses/tracking-info` | `mechanical.licenses.view` | اطلاعات ردیابی مجوزهای یک بحران |
| mechLicensesService | getMechLicensesTrackingList | GET | `api/production-feasibility/mechanical/licenses/tracking` | `mechanical.licenses.view` | تاریخچه مجوزهای یک بحران |
| mechLicensesService | searchMechLicensesTracking | POST | `api/production-feasibility/mechanical/licenses/tracking/search` | `mechanical.licenses.report` | گزارش پارامترهای تولید با کسری تعریف‌شده |
| mechLicensesService | searchMechLicensesTrackingCriticals | POST | `api/production-feasibility/mechanical/licenses/tracking/search-criticals` | `mechanical.licenses.report` | بحران‌های دارای مجوز تولید با کسری |
| mechLicensesService | getMechLicensesBodiesRelatedToUsedLicense | GET | `api/production-feasibility/mechanical/licenses/used-license-bodies` | `mechanical.licenses.report` | بدنه‌های مصرف‌کننده مجوز تولید |
| mechLicensesService | changeMechLicenses | POST | `api/production-feasibility/mechanical/licenses/change` | `mechanical.licenses.change` | تغییر شرایط تولید / ثبت مجوز تولید با کسری |
| mechLicensesService | getMechLicensesSummary | GET | `api/production-feasibility/mechanical/licenses/summary` | `mechanical.licenses.view` | مجوز مصرف‌شده و باقی‌مانده‌های یک بحران |
| mechPartsService | getMechPartsBodyProperties | GET | `api/production-feasibility/mechanical/critical-parts/body-properties` | `mechanical.criticalparts.view` | مشخصات بدنه برای ثبت بحران |
| mechPartsService | getMechPartsPartDescription | GET | `api/production-feasibility/mechanical/critical-parts/part-description` | `mechanical.criticalparts.view` | شرح فارسی قطعه بر اساس شماره فنی |
| mechPartsService | getMechPartsPartInShortageList | GET | `api/production-feasibility/mechanical/critical-parts/part-in-shortage-list` | `mechanical.criticalparts.view` | اطلاعات قطعه در فهرست مرجع قطعات کسری |
| mechPartsService | getMechPartsPartProducts | GET | `api/production-feasibility/mechanical/critical-parts/part-products` | `mechanical.criticalparts.view` | محصولات مصرف‌کننده یک شماره فنی (BOM) |
| mechPartsService | getMechPartsProductsByClassAndPartType | GET | `api/production-feasibility/mechanical/critical-parts/products-by-class-part-type` | `mechanical.criticalparts.view` | قطعات مصرف‌شده در یک کلاس بر اساس نوع قطعه |
| mechPartsService | existsMechPartsPartNo | GET | `api/production-feasibility/mechanical/critical-parts/exists-part-no` | `mechanical.criticalparts.view` | بررسی وجود بحران فعال برای شماره فنی در سالن |
| mechPartsService | getMechPartsFromBop | GET | `api/production-feasibility/mechanical/critical-parts/from-bop` | `mechanical.criticalparts.view` | محصولات شماره فنی از BOP |
| mechPartsService | getMechPartsFromSap | GET | `api/production-feasibility/mechanical/critical-parts/from-sap` | `mechanical.criticalparts.view` | محصولات شماره فنی از SAP بر اساس زیرکلاس‌ها |
| mechPartsService | getMechPartsForEdit | GET | `api/production-feasibility/mechanical/critical-parts/{id:int}/for-edit` | `mechanical.criticalparts.view` | اطلاعات بحران برای ویرایش |
| mechPartsService | getMechPartsById | GET | `api/production-feasibility/mechanical/critical-parts/{id:int}` | `mechanical.criticalparts.view` | جزئیات بحران |
| mechPartsService | getMechPartsProductsForEdit | GET | `api/production-feasibility/mechanical/critical-parts/{id:int}/products-for-edit` | `mechanical.criticalparts.view` | محصولات بحران برای ویرایش |
| mechPartsService | getMechPartsProducts | GET | `api/production-feasibility/mechanical/critical-parts/{criticalPartId:int}/products` | `mechanical.criticalparts.view` | محصولات ثبت‌شده یک بحران |
| mechPartsService | createMechParts | POST | `api/production-feasibility/mechanical/critical-parts` | `mechanical.criticalparts.create` | ثبت بحران مکانیکی |
| mechPartsService | updateMechParts | PUT | `api/production-feasibility/mechanical/critical-parts/{id:int}` | `mechanical.criticalparts.update` | ویرایش بحران مکانیکی |
| mechPartsService | saveMechPartsProduct | POST | `api/production-feasibility/mechanical/critical-parts/{criticalPartsId:int}/products` | `mechanical.criticalparts.update` | ثبت/به‌روزرسانی یک محصول بحران |
| mechPartsService | addMechPartsSubClass | POST | `api/production-feasibility/mechanical/critical-parts/add-subclass` | `mechanical.criticalparts.compare` | افزودن زیرکلاس مغایر به بحران |
| mechPartsService | deleteMechParts | DELETE | `api/production-feasibility/mechanical/critical-parts/{id:int}` | `mechanical.criticalparts.delete` | حذف محصولات بحران مکانیکی |
| mechPartsService | saveMechPartsProducts | POST | `api/production-feasibility/mechanical/critical-parts/{criticalPartsId:int}/products/batch` | `mechanical.criticalparts.update` | ثبت چند محصول برای یک بحران مکانیکی |
| mechPartsService | compareMechPartsSubClasses | POST | `api/production-feasibility/mechanical/critical-parts/compare-subclasses` | `mechanical.criticalparts.compare` | مقایسه زیرکلاس‌های SAP با SFC |
| mechReasonsService | getMechReasonsList | GET | `api/production-feasibility/mechanical/not-possible-reasons` | `mechanical.notpossiblereasons.view` | فهرست دلایل عدم امکان تولید |
| mechReasonsService | createMechReasons | POST | `api/production-feasibility/mechanical/not-possible-reasons` | `mechanical.notpossiblereasons.create` | ثبت دلیل عدم امکان تولید |
| mechReasonsService | updateMechReasons | PUT | `api/production-feasibility/mechanical/not-possible-reasons/{id:int}` | `mechanical.notpossiblereasons.update` | ویرایش دلیل عدم امکان تولید |
| mechReasonsService | deleteMechReasons | DELETE | `api/production-feasibility/mechanical/not-possible-reasons/{id:int}` | `mechanical.notpossiblereasons.delete` | حذف منطقی دلیل عدم امکان تولید |
| mechReportsService | getMechReportsProductionPossibility | POST | `api/production-feasibility/mechanical/reports/production-possibility` | `mechanical.reports.productionpossibility` | گزارش امکان تولید مکانیکی |
| mechReportsService | getMechReportsProductionPossibilityWithLicense | POST | `api/production-feasibility/mechanical/reports/production-possibility-with-license` | `mechanical.reports.productionpossibility` | گزارش امکان تولید با مجوز مصرف‌شده و باقی‌مانده |
| mechReportsService | getMechReportsProductionPossibilityByTrimShop | GET | `api/production-feasibility/mechanical/reports/production-possibility-by-trim-shop` | `mechanical.reports.productionpossibility` | امکان تولید به تفکیک سالن مونتاژ |
| mechReportsService | getMechReportsGeneralReport | GET | `api/production-feasibility/mechanical/reports/general` | `mechanical.reports.general` | گزارش جامع اقلام بحرانی (مکانیکی و سنکرون) |
| mechReportsService | getMechReportsIncomingBodiesByCriticalPart | GET | `api/production-feasibility/mechanical/reports/incoming-bodies` | `mechanical.reports.incoming` | بدنه‌های در راه / وارد شده به خط برای یک بحران |
| mechReportsService | getMechReportsIncomingBodyList | GET | `api/production-feasibility/mechanical/reports/incoming-body-list` | `mechanical.reports.incoming` | فهرست بدنه‌های وارد شده برای یک بحران |
| mechReportsService | getMechReportsIncomingBodiesSummary | GET | `api/production-feasibility/mechanical/reports/incoming-bodies-summary` | `mechanical.reports.incoming` | خلاصه بدنه‌های وارد شده به خط |
| mechReportsService | getMechReportsStockLog | GET | `api/production-feasibility/mechanical/reports/stock-log` | `mechanical.reports.stocklog` | تاریخچه موجودی شماره فنی |
| mechReportsService | getMechReportsSapCriticalLog | GET | `api/production-feasibility/mechanical/reports/sap-critical-log` | `mechanical.reports.sap` | لاگ انتقال اقلام بحرانی از SAP |
| mechReportsService | getMechReportsFailedSapTransfers | GET | `api/production-feasibility/mechanical/reports/failed-sap-transfers` | `mechanical.reports.sap` | قطعات ناموفق در انتقال از SAP |
| mechReportsService | getMechReportsAsnReport | GET | `api/production-feasibility/mechanical/reports/asn` | `mechanical.reports.sap` | گزارش ASN (اعلان ورود محموله) |
| mechReportsService | getMechReportsNotPossibleProductsToTrimStep | GET | `api/production-feasibility/mechanical/reports/not-possible-to-trim-in` | `mechanical.reports.conflicts` | دلایل عدم امکان ورود خودرو به مونتاژ |
| mechReportsService | getMechReportsTrimShopBodyCount | GET | `api/production-feasibility/mechanical/reports/trim-shop-body-count` | `mechanical.reports.conflicts` | تعداد بدنه‌های ورودی به مونتاژ پس از ثبت |
| mechReportsService | getMechReportsTrimShopCriticalBodies | GET | `api/production-feasibility/mechanical/reports/trim-shop-critical-bodies` | `mechanical.reports.conflicts` | بدنه‌های بحرانی ورودی به مونتاژ |
| mechSearchService | searchMechSearchCriticalParts | POST | `api/production-feasibility/mechanical/search/critical-parts` | `mechanical.search.search` | جستجوی اقلام بحرانی (مکانیکی و سنکرون) |
| mechSearchService | searchMechSearchByTrimShop | GET | `api/production-feasibility/mechanical/search/critical-parts-by-trim-shop` | `mechanical.search.search` | جستجوی بحران‌ها بر اساس سالن و بازه تاریخ |
| mechSearchService | getMechSearchReceivedParts | GET | `api/production-feasibility/mechanical/search/received-parts` | `mechanical.search.arrived` | اقلام بحرانی فعال وارد شده به شرکت |
| pfUserService | getPfUserCurrent | GET | `api/production-feasibility/users/me` | `currentuser.view` | اطلاعات کاربر جاری در PF.B_User |
| pfUserService | getPfUserCurrentPermissions | GET | `api/production-feasibility/users/me/permissions` | `(authenticated)` | کلیدهای دسترسی کاربر جاری |
| syncBasisService | getSyncBasisList | GET | `api/production-feasibility/synchron/basis` | `synchron.basis.view` | فهرست مبناهای ست سنکرون |
| syncBasisService | getSyncBasisByCode | GET | `api/production-feasibility/synchron/basis/by-code` | `synchron.basis.view` | مبنای ست بر اساس کد |
| syncBasisService | getSyncBasisById | GET | `api/production-feasibility/synchron/basis/{id}` | `synchron.basis.view` | مبنای ست بر اساس شناسه |
| syncBasisService | getSyncBasisByStatus | GET | `api/production-feasibility/synchron/basis/by-status` | `synchron.basis.view` | مبناهای ست بر اساس وضعیت |
| syncBasisService | countSyncBasisBySetCode | GET | `api/production-feasibility/synchron/basis/count-by-code` | `synchron.basis.view` | تعداد ست‌های ثبت‌شده با کد ست |
| syncBasisService | createSyncBasis | POST | `api/production-feasibility/synchron/basis` | `synchron.basis.create` | ثبت مبنای ست سنکرون |
| syncBasisService | updateSyncBasis | PUT | `api/production-feasibility/synchron/basis/{id:int}` | `synchron.basis.update` | ویرایش مبنای ست سنکرون |
| syncReportsService | getSyncReportsProductionPossibility | POST | `api/production-feasibility/synchron/reports/production-possibility` | `synchron.reports.productionpossibility` | گزارش امکان تولید سنکرون |
| syncSetsService | getSyncSetsById | GET | `api/production-feasibility/synchron/sets/{id:int}` | `synchron.sets.view` | اطلاعات بحران سنکرون |
| syncSetsService | getSyncSetsParts | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/parts` | `synchron.sets.view` | قطعات بحران سنکرون |
| syncSetsService | getSyncSetsPartsWithShortageList | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/parts-with-shortage-list` | `synchron.sets.view` | قطعات بحران سنکرون به همراه فهرست مرجع کسری |
| syncSetsService | getSyncSetsColors | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/colors` | `synchron.sets.view` | رنگ‌های بحران سنکرون |
| syncSetsService | getSyncSetsSaloons | GET | `api/production-feasibility/synchron/sets/{criticalPartSetId:int}/saloons` | `synchron.sets.view` | سالن‌های بحران سنکرون |
| syncSetsService | getSyncSetsProducts | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/products` | `synchron.sets.view` | محصولات بحران سنکرون |
| syncSetsService | getSyncSetsProductDetails | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/product-details` | `synchron.sets.view` | جزئیات محصولات بحران سنکرون |
| syncSetsService | getSyncSetsNotEnteredIndicatorCarsCount | GET | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/not-entered-indicator-cars-count` | `synchron.sets.view` | تعداد سالن‌های بدون خودروی شاخص |
| syncSetsService | createSyncSetsHeader | POST | `api/production-feasibility/synchron/sets` | `synchron.sets.create` | ثبت سرآیند بحران سنکرون |
| syncSetsService | updateSyncSetsHeader | PUT | `api/production-feasibility/synchron/sets/{id:int}` | `synchron.sets.update` | ویرایش بحران سنکرون |
| syncSetsService | addSyncSetsProduct | POST | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/products` | `synchron.sets.update` | ثبت یک محصول بحران سنکرون |
| syncSetsService | updateSyncSetsSaloonIndicatorCar | PUT | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/saloons/{id:int}` | `synchron.sets.update` | تعیین خودروی شاخص سالن بحران سنکرون |
| syncSetsService | deleteSyncSets | DELETE | `api/production-feasibility/synchron/sets/{id:int}` | `synchron.sets.delete` | حذف منطقی بحران سنکرون |
| syncSetsService | saveSyncSetsComposition | POST | `api/production-feasibility/synchron/sets/{criticalPartsSetId:int}/composition` | `synchron.sets.create` | ثبت قطعات، رنگ‌ها، سالن‌ها و محصولات بحران سنکرون |
| userAccessService | getUserAccessAllUsers | GET | `api/production-feasibility/user-management/user-access/users` | `usermanagement.useraccess.view` | فهرست کاربران امکان تولید |
| userAccessService | getUserAccessUsersAccessList | GET | `api/production-feasibility/user-management/user-access` | `usermanagement.useraccess.view` | فهرست دسترسی کاربران به خطوط |
| userAccessService | getUserAccessSites | GET | `api/production-feasibility/user-management/user-access/sites` | `usermanagement.useraccess.view` | سایت‌های مجازی |
| userAccessService | getUserAccessSaloonsByVirtualPlant | GET | `api/production-feasibility/user-management/user-access/saloons-by-virtual-plant` | `usermanagement.useraccess.view` | خطوط/سالن‌های یک سایت مجازی |
| userAccessService | deleteUserAccess | POST | `api/production-feasibility/user-management/user-access/delete` | `usermanagement.useraccess.delete` | حذف دسترسی کاربران |
| userAccessService | insertUserAccess | POST | `api/production-feasibility/user-management/user-access` | `usermanagement.useraccess.create` | ثبت دسترسی کاربران به خطوط بدنه‌سازی |
