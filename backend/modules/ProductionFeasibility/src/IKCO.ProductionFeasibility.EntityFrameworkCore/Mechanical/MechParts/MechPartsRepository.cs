using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechParts.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechParts;

public partial class MechPartsRepository : PfDapperRepository<PfDbContext>, IMechPartsRepository, ITransientDependency
{
    public MechPartsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<CarPropertiesRow>> GetBodyPropertiesAsync(string bodyNo, byte plantId, byte? mode)
    {
        var parameters = new SpParameters()
            .NChar("@BodyNo", bodyNo, 8)
            .TinyInt("@PlantId", plantId);
        if (mode != null)
            parameters.TinyInt("@Mode", mode);
        return await QueryRowsAsync<CarPropertiesRow>("dbo.S_CarPropertiesSelectForCriticalParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<string?> GetPartDescriptionAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        var scalar = await ExecuteScalarAsync("dbo.SIMP_SelectPartDescByPartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
        return scalar is null or DBNull ? null : Convert.ToString(scalar);
    }

    public async Task<List<PartInShortageListRow>> GetPartInShortageListAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 20);
        return await QueryRowsAsync<PartInShortageListRow>("dbo.SIMP_SelectParInShortagePartList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BomProductRow>> GetPartProductsAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 20);
        return await QueryRowsAsync<BomProductRow>("dbo.SIMP_SelectProductsUsePartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<BomProductRow>> GetProductsByClassAndPartTypeAsync(string carClassCode, byte partTypeCode)
    {
        var parameters = new SpParameters()
            .Char("@CarClassCode", carClassCode, 5)
            .TinyInt("@PartTypeCode", partTypeCode);
        return await QueryRowsAsync<BomProductRow>("dbo.SIMP_SelectProductsUseAllPartNosByClassCodePartType", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalPartExistRow>> ExistsPartNoAsync(string partNo, int trimShopId)
    {
        var parameters = new SpParameters()
            .NVarChar("@PartNo", partNo, 200)
            .Int("@TrimShopId", trimShopId);
        return await QueryRowsAsync<CriticalPartExistRow>("dbo.S_CriticalPartsExistPartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalProductCandidateRow>> GetFromBopAsync(string partNo, int trimShopId)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 20)
            .Int("@TrimShopId", trimShopId);
        return await QueryRowsAsync<CriticalProductCandidateRow>("dbo.S_CriticalPartsSelectFromBOP", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalProductCandidateRow>> GetFromSapAsync(string subClassCode, string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@SubClassCode", subClassCode, 1000)
            .VarChar("@PartNo", partNo, 100);
        return await QueryRowsAsync<CriticalProductCandidateRow>("dbo.S_CriticalPartsSelectFromSAP", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalPartForEditRow>> GetForEditAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@ID", id);
        return await QueryRowsAsync<CriticalPartForEditRow>("dbo.S_SelectCriticalPartForEdit", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalPartRow>> GetByIdAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryRowsAsync<CriticalPartRow>("dbo.S_CriticalPartsSelectById", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalPartProductForEditRow>> GetProductsForEditAsync(int id, int? shopTrimId)
    {
        var parameters = new SpParameters()
            .Int("@ID", id);
        if (shopTrimId != null)
            parameters.Int("@ShopTrimId", shopTrimId);
        return await QueryRowsAsync<CriticalPartProductForEditRow>("dbo.S_CriticalPartProductsSelectForEdit_V1", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<CriticalPartProductRow>> GetProductsAsync(int criticalPartId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartId", criticalPartId);
        return await QueryRowsAsync<CriticalPartProductRow>("dbo.S_CriticalPartProductsSelectByCriticalPartId_V2", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<int?> CreateAsync(int carId, string partNo, string partDesc, byte plantId, string partGroup, string? alterNativePartNo, string? criticalPartNote, int stock, int used, int possibleProductionNo, int trimShopId, int usageFactor, int deptId, int userId, bool? isOutOfSap)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId)
            .VarChar("@PartNo", partNo, 30)
            .VarChar("@PartDesc", partDesc, 100)
            .TinyInt("@PlantId", plantId)
            .Char("@PartGroup", partGroup, 1)
            .NVarChar("@AlterNativePartNo", alterNativePartNo, 1000)
            .Char("@CriticalPartNote", criticalPartNote, 100)
            .Int("@Stock", stock)
            .Int("@Used", used)
            .Int("@PossibleProductionNo", possibleProductionNo)
            .Int("@TrimShopId", trimShopId)
            .Int("@UsageFactor", usageFactor)
            .Int("@DeptId", deptId)
            .Int("@UserId", userId);
        if (isOutOfSap != null)
            parameters.Bit("@IsOutOfSap", isOutOfSap);
        var scalar = await ExecuteScalarAsync("dbo.S_CriticalPartsINSERT", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
        return scalar is null or DBNull ? null : Convert.ToInt32(scalar);
    }

    public async Task UpdateAsync(int id, int stock, int used, string usageFactor, string? alterNativePartNo, string? criticalPartNote, string? productionStateLetterNo, string? productionStateNote, string partGroup, int productionPossibility, int userId, bool? isOutOfSap)
    {
        var parameters = new SpParameters()
            .Int("@Id", id)
            .Int("@Stock", stock)
            .Int("@Used", used)
            .NVarChar("@UsageFactor", usageFactor, 2)
            .NVarChar("@AlterNativePartNo", alterNativePartNo, 200)
            .NVarChar("@CriticalPartNote", criticalPartNote, 1000)
            .NVarChar("@ProductionStateLetterNo", productionStateLetterNo, 1000)
            .NVarChar("@ProductionStateNote", productionStateNote, 300)
            .NVarChar("@PartGroup", partGroup, 1)
            .Int("@ProductionPossibility", productionPossibility)
            .Int("@UserId", userId)
            .Bit("@IsOutOfSap", isOutOfSap);
        await ExecuteAsync("dbo.S_CriticalPartsUpdate", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> SaveProductAsync(int carId, int criticalPartsId, int carGroupId, int carModelId, int carClassId, int subClassId, int colorId, int? trimId, int optionId, int userId, int isNew, int trimShopId, short plantId, byte quantity)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId)
            .Int("@CriticalPartsId", criticalPartsId)
            .Int("@CarGroupId", carGroupId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId)
            .Int("@SubClassId", subClassId)
            .Int("@ColorId", colorId)
            .Int("@OptionId", optionId)
            .Int("@UserId", userId)
            .Int("@IsNew", isNew)
            .Int("@TrimShopId", trimShopId)
            .SmallInt("@PlantId", plantId)
            .TinyInt("@Quantity", quantity);
        if (trimId != null)
            parameters.Int("@TrimId", trimId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartsPruductUpdate_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task AddSubClassAsync(int criticalPartsId, string subClassCode, int userId, int trimShopId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsId", criticalPartsId)
            .Char("@SubClassCode", subClassCode, 7)
            .Int("@UserId", userId)
            .Int("@TrimShopId", trimShopId);
        await ExecuteAsync("dbo.S_AddContrastOfSubClasses", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> DeleteAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@id", id);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("dbo.S_CriticalPartDelete_Web", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

}
