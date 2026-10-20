# ماتریس وابستگی به رویه‌های ذخیره‌شده

این فایل با `_src/gen/gen_docs.py` تولید می‌شود. ستون «در اسنپ‌شات» نشان می‌دهد رویه در `SFCDB2.md` (ساختار جدید پایگاه داده) هست یا نه؛ 
رویه‌های اسکیمای `PF.` در اسنپ‌شات نیستند و قبل از اجرا باید روی پایگاه داده مقصد وجود داشته باشند. رویه‌های مکانیکی/سنکرون
مطابق بک‌اند قبلی (PFback.txt — SFC.Data) هستند؛ مدل خروجی هر رویه در `_src/gen/spec_rows.py` است.

| رویه | در اسنپ‌شات | Endpointهای مصرف‌کننده |
|---|---|---|
| `PF.GetAllUsers` | **خیر** | UserAccess.GetAllUsers |
| `PF.GetProductionPssibilityStatus_V1` | **خیر** | BodyReports.GetProductionPossibilityStatus |
| `PF.GetSaloonByVirtualPlantId` | **خیر** | UserAccess.GetSaloonsByVirtualPlant |
| `PF.GetShortageLicenseTrackingSelectAll` | **خیر** | BodyReports.GetShortageLicenseTracking |
| `PF.GetSiteByDomainId` | **خیر** | UserAccess.GetSites |
| `PF.GetTrackingCriticalParts_Product` | **خیر** | BodyReports.GetTrackingCriticalPartProducts |
| `PF.GetTrackingCriticalParts_V2` | **خیر** | BodyReports.GetTrackingCriticalParts |
| `PF.GetUsersAccessList` | **خیر** | UserAccess.GetUsersAccessList |
| `PF.Get_BodyCriticalSearch_Product_Web` | **خیر** | BodyReports.GetCriticalPartProducts |
| `PF.S_BodyCriticalSearch_Web` | **خیر** | BodyReports.SearchCriticalParts |
| `PF.S_CreateCriticalParts_V3` | **خیر** | BodyCriticals.CreateCriticalPart |
| `PF.S_CriticalPartsInfo` | **خیر** | BodyCriticals.GetCriticalPartInfo |
| `PF.S_DeleteUserAccess` | **خیر** | UserAccess.DeleteUserAccess |
| `PF.S_EventLogList` | **خیر** | BodyCriticals.GetEventLog |
| `PF.S_FailedPartTransferd_Sap_Log` | **خیر** | BodyReports.GetFailedSapTransfers |
| `PF.S_GetCodeClassByPartNo` | **خیر** | BodyCriticals.GetCodeClassByPartNo |
| `PF.S_GetLastChangeOnShortageLicense` | **خیر** | BodyReports.GetLastChangeOnLicense |
| `PF.S_GetSapDataUpdateTimes` | **خیر** | BodyReports.GetSapDataUpdateTimes |
| `PF.S_NotPossibleProductsToStepAssign_SelectAll` | **خیر** | BodyReports.GetNotPossibleProductsToStepAssign |
| `PF.S_PreAssignBodyCoun_List` | **خیر** | BodyReports.GetPreAssignBodyCounts |
| `PF.S_PreAssignCriticalBody_SelectAll` | **خیر** | BodyReports.GetPreAssignCriticalBodies |
| `PF.S_VINBIWSite_CarModel_SelectAll` | **خیر** | BodyReports.GetProductsOfSites |
| `PF.S_Web_UpdateCriticalParts` | **خیر** | BodyCriticals.UpdateCriticalPart |
| `PF.SelectPartProductByPartNo` | **خیر** | BodyCriticals.GetPartProducts |
| `dbo.SEXT_BodyInfo_V2` | بله | BodyCriticals.GetBodyInfo |
| `dbo.SEXT_CriticalPartsStockLog` | بله | MechReports.GetStockLog |
| `dbo.SIMP_PartGetInfoByPartNo_Web` | بله | SyncSets.GetPartInfo |
| `dbo.SIMP_SelectParInShortagePartList` | بله | MechParts.GetPartInShortageList |
| `dbo.SIMP_SelectPartDescByPartNo` | بله | BodyCriticals.GetPartDescription, MechParts.GetPartDescription |
| `dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType` | بله | MechParts.GetProductsByClassAndPartType |
| `dbo.SIMP_SelectProductsUsePartNo` | بله | MechParts.GetPartProducts |
| `dbo.S_ASNReport` | بله | MechReports.GetAsnReport, SyncReports.GetAsnReport |
| `dbo.S_AddContrastOfSubClasses` | بله | MechParts.AddSubClass |
| `dbo.S_CarPropertiesSelectForCriticalParts` | بله | MechParts.GetBodyProperties |
| `dbo.S_CriticalPartDelete_Web` | بله | MechParts.Delete |
| `dbo.S_CriticalPartProductsSelectByCriticalPartId_V2` | بله | MechParts.GetProducts |
| `dbo.S_CriticalPartProductsSelectForEdit_V1` | بله | MechParts.GetProductsForEdit |
| `dbo.S_CriticalPartSearch` | بله | MechSearch.SearchByTrimShop |
| `dbo.S_CriticalPartSetColorSelectByCriticalPartSetId` | بله | SyncSets.GetColors |
| `dbo.S_CriticalPartSetPartSelectByCriticalPartsSetId` | بله | SyncSets.GetParts |
| `dbo.S_CriticalPartSetPartWithShortagePartListSelectByCriticalPartsSetId` | بله | SyncSets.GetPartsWithShortageList |
| `dbo.S_CriticalPartSetProductSelectByCriticalPartsSetId` | بله | SyncSets.GetProducts |
| `dbo.S_CriticalPartSetProductSelectDetailsByCriticalPartsSetId` | بله | SyncSets.GetProductDetails |
| `dbo.S_CriticalPartSetSaloonSelectByCriticalPartSetId` | بله | SyncSets.GetSaloons |
| `dbo.S_CriticalPartSetSelectById` | بله | SyncSets.GetById |
| `dbo.S_CriticalPart_RecievedParts` | بله | MechSearch.GetReceivedParts |
| `dbo.S_CriticalPartsExistPartNo` | بله | MechParts.ExistsPartNo |
| `dbo.S_CriticalPartsGeneralReport_Web` | بله | MechReports.GetGeneralReport |
| `dbo.S_CriticalPartsINSERT` | بله | MechParts.Create |
| `dbo.S_CriticalPartsProductExceptionsLicenseBodyNoList` | بله | MechExceptions.GetLicenseBodies |
| `dbo.S_CriticalPartsProductExceptionsLisenceList` | بله | MechExceptions.GetLicenses |
| `dbo.S_CriticalPartsProductExceptionsLisenceLog` | بله | MechExceptions.GetLicenseLog |
| `dbo.S_CriticalPartsProductionPossibilityAllWithUsedLicenseAndRemainingBySubClassId_V5` | بله | MechReports.GetProductionPossibilityWithLicense |
| `dbo.S_CriticalPartsPruductUpdate_Web` | بله | MechParts.SaveProduct |
| `dbo.S_CriticalPartsSearchByConditions` | بله | MechSearch.SearchCriticalParts |
| `dbo.S_CriticalPartsSelectById` | بله | MechParts.GetById |
| `dbo.S_CriticalPartsSelectFromBOP` | بله | MechParts.GetFromBop |
| `dbo.S_CriticalPartsSelectFromSAP` | بله | MechParts.GetFromSap |
| `dbo.S_CriticalPartsSetBasisSelectByCode` | بله | SyncBasis.GetByCode |
| `dbo.S_CriticalPartsSetBasisSelectById` | بله | SyncBasis.GetById |
| `dbo.S_CriticalPartsSetBasisSelectByStatus` | بله | SyncBasis.GetByStatus |
| `dbo.S_CriticalPartsSetCountSetCode` | بله | SyncBasis.CountBySetCode |
| `dbo.S_CriticalPartsSetDeleteLogically` | بله | SyncSets.Delete |
| `dbo.S_CriticalPartsSetInsert_Web` | بله | SyncSets.CreateHeader |
| `dbo.S_CriticalPartsSetProductInsert_Web` | بله | SyncSets.AddProduct |
| `dbo.S_CriticalPartsSetSaloonNotEnteredIndicatorCarsCount_Web` | بله | SyncSets.GetNotEnteredIndicatorCarsCount |
| `dbo.S_CriticalPartsSetSaloonUpdate_Web` | بله | SyncSets.UpdateSaloonIndicatorCar |
| `dbo.S_CriticalPartsSetUpdate_Web` | بله | SyncSets.UpdateHeader |
| `dbo.S_CriticalPartsUpdate` | بله | MechParts.Update |
| `dbo.S_CriticalProductExceptionList` | بله | MechExceptions.GetExceptionList |
| `dbo.S_DisableCriticalPartsProductExceptionsLicense` | بله | MechExceptions.DisableLicense |
| `dbo.S_DisableCriticalPartsProductsException` | بله | MechExceptions.DisableExceptions |
| `dbo.S_FailedPartTransferd_Sap_Log` | بله | MechReports.GetFailedSapTransfers |
| `dbo.S_GetCriticalPartsSet_Saloons` | بله | SyncSets.GetSetSaloons |
| `dbo.S_InsertCriticalPartSet` | بله | SyncBasis.Create |
| `dbo.S_InsertCriticalPartTypeProperty` | بله | SyncBasis.AddPartTypeProperty |
| `dbo.S_InsertCriticalPartsProductExceptionsLicense` | بله | MechExceptions.CreateLicense |
| `dbo.S_ProgNotPossibilityReasonSelectAll` | بله | MechReasons.GetList |
| `dbo.S_ProgNotPossibilityReason_Insert` | بله | MechReasons.Create |
| `dbo.S_ProgNotPossibilityReason_LogicalDelete` | بله | MechReasons.Delete |
| `dbo.S_ProgNotPossibilityReason_Update` | بله | MechReasons.Update |
| `dbo.S_RPT_DESC_ProdPossibilityByTrimShop` | بله | MechReports.GetProductionPossibilityByTrimShop |
| `dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException` | بله | MechExceptions.GetPartNumbersInExceptions |
| `dbo.S_RPT_IncommingBodiesByCriticalPartId` | بله | MechReports.GetIncomingBodiesByCriticalPart |
| `dbo.S_RPT_IncommingBodiesByCriticalParts` | بله | MechReports.GetIncomingBodiesSummary |
| `dbo.S_RPT_IncommingBodiesListByCriticalPartId` | بله | MechReports.GetIncomingBodyList |
| `dbo.S_RptCriticalPartsProduction_Mechanical` | بله | MechReports.GetProductionPossibility |
| `dbo.S_RptCriticalPartsProduction_Synchron` | بله | SyncReports.GetProductionPossibility |
| `dbo.S_RptGetCriticalPartsSet` | بله | SyncBasis.GetList |
| `dbo.S_SapCriticalLog` | بله | MechReports.GetSapCriticalLog |
| `dbo.S_SelectBodiesRelatedToUsedLicense` | بله | MechLicenses.GetBodiesRelatedToUsedLicense |
| `dbo.S_SelectColorsSimilarTo5DigitCode` | بله | SyncSets.GetSimilarColors |
| `dbo.S_SelectCriticalPartForEdit` | بله | MechParts.GetForEdit |
| `dbo.S_SelectSaloonsByCarModels` | بله | SyncSets.GetSaloonsByCarModels |
| `dbo.S_ShortageLicenseSelectAllTrackingInfo` | بله | MechLicenses.GetTrackingInfo |
| `dbo.S_ShortageLicenseTrackingSearch` | بله | MechLicenses.SearchTracking |
| `dbo.S_ShortageLicenseTrackingSelectAll` | بله | MechLicenses.GetTrackingList |
| `dbo.S_ShortagelicenseTrackingSearchCriticals` | بله | MechLicenses.SearchTrackingCriticals |
| `dbo.S_TrimInNotPossibleProductsSelectAll` | بله | MechReports.GetNotPossibleProductsToTrimStep |
| `dbo.S_TrimShopBodyCount_List` | بله | MechReports.GetTrimShopBodyCount |
| `dbo.S_TrimShopCriticalBody_SelectAll` | بله | MechReports.GetTrimShopCriticalBodies |
| `dbo.S_UpdateCriticalPartsSet` | بله | SyncBasis.Update |
| `dbo.S_Web_SAP_CriticalPartsMaster_SelectAll` | بله | SyncReports.GetSapCriticalPartsMaster |
| `dbo.S_Web_SAP_ReserveMaster_SelectAll` | بله | SyncReports.GetSapReserveMaster |

مجموع رویه‌ها: 104 — خارج از اسنپ‌شات: 24

## رویه‌های اختیاری (در صورت وجود استفاده می‌شوند)

- `dbo.S_InsertCriticalPartsSet_Parts_Web`، `dbo.S_InsertCriticalPartsSet_Colors_Web`، `dbo.S_InsertCriticalPartsSet_Saloons_Web`: اگر در پایگاه داده باشند، ثبت ترکیب ست با پارامتر جدولی انجام می‌شود؛ وگرنه درج مستقیم در جدول‌های `T_CriticalPartsSet_*`.
- `dbo.S_InsertCriticalPartsSet_Products` (پارامتر `@dt`).

## رویه‌های SQL مستقیم (Lookup)

فهرست‌های مرجع (`LookupRepository`) از یک کاتالوگ SQL ثابت و پارامتری می‌خوانند؛ ورودی کلاینت هرگز به SQL الحاق نمی‌شود.
