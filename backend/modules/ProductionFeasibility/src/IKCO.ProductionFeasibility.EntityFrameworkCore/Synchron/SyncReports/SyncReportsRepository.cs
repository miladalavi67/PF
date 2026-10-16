using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

public partial class SyncReportsRepository : PfDapperRepository<PfDbContext>, ISyncReportsRepository, ITransientDependency
{
    public SyncReportsRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<TabularResult> GetProductionPossibilityAsync(string saloonId, int crisisTypeId, int criticalPartTypeId, int subClassId, string? partNo, int carModelId, int carClassId)
    {
        var parameters = new SpParameters()
            .NVarChar("@SaloonId", saloonId, -1)
            .Int("@CrisisTypeId", crisisTypeId)
            .Int("@CriticalPartTypeId", criticalPartTypeId)
            .Int("@SubClassId", subClassId)
            .Int("@CarModelId", carModelId)
            .Int("@CarClassId", carClassId);
        if (partNo != null)
            parameters.VarChar("@PartNo", partNo, 50);
        return await QueryTableAsync("dbo.S_RptCriticalPartsProduction_Synchron", parameters, LegacyCallStyle.AdoNet, PfCommandTimeouts.Report);
    }

}
