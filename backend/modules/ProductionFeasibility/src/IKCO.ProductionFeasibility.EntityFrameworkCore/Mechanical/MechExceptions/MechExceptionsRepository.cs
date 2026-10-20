using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Mechanical.MechExceptions.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Mechanical.MechExceptions;

public partial class MechExceptionsRepository : PfDapperRepository<PfDbContext>, IMechExceptionsRepository, ITransientDependency
{
    public MechExceptionsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<List<ProductExceptionRow>> GetExceptionListAsync(string? bodyNo, string? dateFrom, string? dateEnd, int state)
    {
        var parameters = new SpParameters()
            .Int("@State", state);
        if (bodyNo != null)
            parameters.Char("@BodyNo", bodyNo, 8);
        if (dateFrom != null)
            parameters.NVarChar("@DateFrom", dateFrom, 12);
        if (dateEnd != null)
            parameters.NVarChar("@DateEnd", dateEnd, 12);
        return await QueryRowsAsync<ProductExceptionRow>("dbo.S_CriticalProductExceptionList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task<List<ExceptionLicenseRow>> GetLicensesAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryRowsAsync<ExceptionLicenseRow>("dbo.S_CriticalPartsProductExceptionsLisenceList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<ExceptionLicenseBodyRow>> GetLicenseBodiesAsync(int id)
    {
        var parameters = new SpParameters()
            .Int("@Id", id);
        return await QueryRowsAsync<ExceptionLicenseBodyRow>("dbo.S_CriticalPartsProductExceptionsLicenseBodyNoList", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Lookup);
    }

    public async Task<List<ExceptionLicenseRow>> GetLicenseLogAsync(string partNo)
    {
        var parameters = new SpParameters()
            .VarChar("@PartNo", partNo, 50);
        return await QueryRowsAsync<ExceptionLicenseRow>("dbo.S_CriticalPartsProductExceptionsLisenceLog", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

    public async Task CreateLicenseAsync(int criticalPartsProductExceptionsId, int possibleLicenseCount, string letterNo, string? description, int userId)
    {
        var parameters = new SpParameters()
            .Int("@CriticalPartsProductExceptionsId", criticalPartsProductExceptionsId)
            .Int("@PossibleLicenseCount", possibleLicenseCount)
            .VarChar("@LetterNo", letterNo, 50)
            .NVarChar("@Description", description, 250)
            .Int("@UserId", userId);
        await ExecuteAsync("dbo.S_InsertCriticalPartsProductExceptionsLicense", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task DisableLicenseAsync(int userId, int id)
    {
        var parameters = new SpParameters()
            .Int("@UserId", userId)
            .Int("@Id", id);
        await ExecuteAsync("dbo.S_DisableCriticalPartsProductExceptionsLicense", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task DisableExceptionsAsync(string values, int userId)
    {
        var parameters = new SpParameters()
            .NVarChar("@Values", values, -1)
            .Int("@UserId", userId);
        await ExecuteAsync("dbo.S_DisableCriticalPartsProductsException", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }

    public async Task<List<ExceptionPartNumberRow>> GetPartNumbersInExceptionsAsync(string filterValues, byte trimId, byte filterType, string? partNumber)
    {
        var parameters = new SpParameters()
            .VarChar("@FilterValues", filterValues, -1)
            .TinyInt("@TrimId", trimId)
            .TinyInt("@FilterType", filterType)
            .VarChar("@PartNumber", partNumber ?? string.Empty, 50);
        return await QueryRowsAsync<ExceptionPartNumberRow>("dbo.S_RPT_GetPartNumbersInCriticalPartsProductsException", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
