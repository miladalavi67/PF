using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.BodyShop.BodyCriticals.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.BodyShop.BodyCriticals;

public partial class BodyCriticalsRepository : PfDapperRepository<PfDbContext>, IBodyCriticalsRepository, ITransientDependency
{
    public BodyCriticalsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<BodyInfoRow?> GetBodyInfoAsync(string bodyNumber, int userId)
    {
        var parameters = new SpParameters()
            .Char("@BodyNumber", bodyNumber, 8)
            .Int("@UserId", userId);
        return await QueryFirstOrDefaultAsync<BodyInfoRow>("dbo.SEXT_BodyInfo_V2", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<string?> GetPartDescriptionAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        var scalar = await ExecuteScalarAsync("dbo.SIMP_SelectPartDescByPartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
        return scalar is null or DBNull ? null : Convert.ToString(scalar);
    }

    public async Task<List<PartProductRow>> GetPartProductsAsync(string partNo, int biwSiteId)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50)
            .Int("@BIWSiteId", biwSiteId);
        return await QueryAsync<PartProductRow>("PF.SelectPartProductByPartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetCodeClassByPartNoAsync(string partNo, int biwSiteId)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50)
            .Int("@BIWSiteId", biwSiteId);
        return await QueryTableAsync("PF.S_GetCodeClassByPartNo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<GlobalResultDto?> CreateCriticalPartAsync(int carId, string partNo, string partDesc, string partGroup, string? alterNativePartNo, string? criticalPartNote, int biwSiteId, int totalStock, int rawStock, int userId, string? productionStateNote, string? letterNo, bool isOutOfSAP)
    {
        var parameters = new SpParameters()
            .Int("@CarId", carId)
            .VarChar("@PartNo", partNo, 30)
            .VarChar("@PartDesc", partDesc, 100)
            .Char("@PartGroup", partGroup, 1)
            .NVarChar("@AlterNativePartNo", alterNativePartNo, 1000)
            .Char("@CriticalPartNote", criticalPartNote, 100)
            .Int("@BIWSiteId", biwSiteId)
            .Int("@TotalStock", totalStock)
            .Int("@RawStock", rawStock)
            .Int("@UserId", userId)
            .NVarChar("@ProductionStateNote", productionStateNote, 1000)
            .NVarChar("@LetterNo", letterNo, 1000)
            .Bit("@IsOutOfSAP", isOutOfSAP);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("PF.S_CreateCriticalParts_V3", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> UpdateCriticalPartAsync(int criticalPartsId, string partGroup, string? alterNativePartNo, string? criticalPartNote, int rawStock, int userId, string? productionStateNote, string? letterNo)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsId", criticalPartsId)
            .Char("@PartGroup", partGroup, 1)
            .NVarChar("@AlterNativePartNo", alterNativePartNo, 1000)
            .Char("@CriticalPartNote", criticalPartNote, 100)
            .Int("@RawStock", rawStock)
            .Int("@UserId", userId)
            .NVarChar("@ProductionStateNote", productionStateNote, 1000)
            .NVarChar("@LetterNo", letterNo, 1000);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("PF.S_Web_UpdateCriticalParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<GlobalResultDto?> DisableCriticalPartsAsync(string idString, int userId)
    {
        var parameters = new SpParameters()
            .VarChar("@IdString", idString, -1)
            .Int("@UserId", userId);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>("PF.S_DisableCriticalParts", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task InsertDisableReasonAsync(int eventTypeId, int criticalPartId, string note, int userId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartTypeId", eventTypeId)
            .Int("@AttributeId", 4)
            .Int("@ChangeRecId", criticalPartId)
            .NVarChar("@OldValue", null, 500)
            .NVarChar("@NewValue", null, 500)
            .VarChar("@RequestNo", null, 50)
            .NVarChar("@Note", note, 500)
            .Int("@UserId", userId);
        await ExecuteAsync("PF.InsertEventLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<TabularResult> GetCriticalPartInfoAsync(int criticalPartsId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsId", criticalPartsId);
        return await QueryTableAsync("PF.S_CriticalPartsInfo", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<TabularResult> GetEventLogAsync(int eventTypeId, int changedRecordId)
    {
        var parameters = new SpParameters()
            .Int("@EventTypeId", eventTypeId)
            .Int("@ChangedRecordId", changedRecordId);
        return await QueryTableAsync("PF.S_EventLogList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
