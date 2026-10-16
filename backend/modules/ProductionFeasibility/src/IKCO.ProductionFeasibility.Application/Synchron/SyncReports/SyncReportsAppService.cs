using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Dtos;
using IKCO.ProductionFeasibility.Synchron.SyncReports.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Synchron.SyncReports;

/// <summary>گزارش‌های امکان تولید سنکرون. مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class SyncReportsAppService : PfAppService, ISyncReportsAppService
{
    private readonly ISyncReportsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public SyncReportsAppService(ISyncReportsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> GetProductionPossibilityAsync(SyncReportsGetProductionPossibilityInput input)
    {
        return await _repository.GetProductionPossibilityAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, PartNumber.OptionalValue(input.PartNo), input.CarModelId, input.CarClassId);
    }

}
