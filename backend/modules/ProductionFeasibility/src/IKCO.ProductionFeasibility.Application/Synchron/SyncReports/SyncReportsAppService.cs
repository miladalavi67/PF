using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.Results;
using IKCO.ProductionFeasibility.Synchron.Results;
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

    public async Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityAsync(SyncReportsGetProductionPossibilityInput input)
    {
        return new ListResultDto<ProductionPossibilityRow>(await _repository.GetProductionPossibilityAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, PartNumber.OptionalValue(input.PartNo), input.CarModelId, input.CarClassId));
    }

    public async Task<ListResultDto<SapCriticalPartsMasterRow>> GetSapCriticalPartsMasterAsync(SyncReportsGetSapCriticalPartsMasterInput input)
    {
        return new ListResultDto<SapCriticalPartsMasterRow>(await _repository.GetSapCriticalPartsMasterAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value, input.SaloonId, BodyNumber.OptionalValue(input.BodyNO), PartNumber.OptionalValue(input.PartNO)));
    }

    public async Task<ListResultDto<SapReserveMasterRow>> GetSapReserveMasterAsync(SyncReportsGetSapReserveMasterInput input)
    {
        return new ListResultDto<SapReserveMasterRow>(await _repository.GetSapReserveMasterAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value, input.SaloonId, PartNumber.OptionalValue(input.PartNO)));
    }

    public async Task<ListResultDto<AsnRow>> GetAsnReportAsync(SyncReportsGetAsnReportInput input)
    {
        return new ListResultDto<AsnRow>(await _repository.GetAsnReportAsync(PartNumber.OptionalValue(input.PartNumber), ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.Type));
    }

}
