using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Dtos;
using IKCO.ProductionFeasibility.Mechanical.MechReports.Repositories;
using IKCO.ProductionFeasibility.Users;
using Volo.Abp.Application.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechReports;

/// <summary>گزارش‌های امکان تولید مکانیکی. مجوز در کنترلر (سیاست‌های IKCO.Auth) بررسی می‌شود.</summary>
public partial class MechReportsAppService : PfAppService, IMechReportsAppService
{
    private readonly IMechReportsRepository _repository;
    private readonly ICurrentPfUser _currentUser;

    public MechReportsAppService(IMechReportsRepository repository, ICurrentPfUser currentUser)
    {
        _repository = repository;
        _currentUser = currentUser;
    }

    public async Task<TabularResult> GetProductionPossibilityAsync(MechReportsGetProductionPossibilityInput input)
    {
        return await _repository.GetProductionPossibilityAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, PartNumber.OptionalValue(input.PartNo), input.CarModelId, input.CarClassId);
    }

    public async Task<TabularResult> GetProductionPossibilityWithLicenseAsync(MechReportsGetProductionPossibilityWithLicenseInput input)
    {
        return await _repository.GetProductionPossibilityWithLicenseAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, input.CarModelId, input.CarClassId);
    }

    public async Task<TabularResult> GetProductionPossibilityByTrimShopAsync(MechReportsGetProductionPossibilityByTrimShopInput input)
    {
        return await _repository.GetProductionPossibilityByTrimShopAsync(input.TrimShopId, input.ProdState);
    }

    public async Task<TabularResult> GetGeneralReportAsync(MechReportsGetGeneralReportInput input)
    {
        return await _repository.GetGeneralReportAsync(input.CriticalPartTypeId, input.SaloonId, input.CrisisTypeId, input.PlantId);
    }

    public async Task<TabularResult> GetIncomingBodiesByCriticalPartAsync(MechReportsGetIncomingBodiesByCriticalPartInput input)
    {
        return await _repository.GetIncomingBodiesByCriticalPartAsync(input.Id, input.CriticalPartTypeId, input.SaloonId, input.PlantId);
    }

    public async Task<TabularResult> GetIncomingBodyListAsync(MechReportsGetIncomingBodyListInput input)
    {
        return await _repository.GetIncomingBodyListAsync(input.Id, input.CriticalPartTypeId, input.SaloonId, input.ProductionStepGroupId, input.PlantId);
    }

    public async Task<TabularResult> GetIncomingBodiesSummaryAsync(MechReportsGetIncomingBodiesSummaryInput input)
    {
        return await _repository.GetIncomingBodiesSummaryAsync(input.SaloonId, input.CrisisTypeId, input.ProductionStepGroupId, input.PlantId);
    }

    public async Task<TabularResult> GetStockLogAsync(MechReportsGetStockLogInput input)
    {
        return await _repository.GetStockLogAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetSapCriticalLogAsync(MechReportsGetSapCriticalLogInput input)
    {
        return await _repository.GetSapCriticalLogAsync(new PartNumber(input.PartNo).Value);
    }

    public async Task<TabularResult> GetFailedSapTransfersAsync(MechReportsGetFailedSapTransfersInput input)
    {
        return await _repository.GetFailedSapTransfersAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value);
    }

    public async Task<TabularResult> GetAsnReportAsync(MechReportsGetAsnReportInput input)
    {
        return await _repository.GetAsnReportAsync(PartNumber.OptionalValue(input.PartNumber), ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.Type);
    }

    public async Task<TabularResult> GetNotPossibleProductsToTrimStepAsync(MechReportsGetNotPossibleProductsToTrimStepInput input)
    {
        return await _repository.GetNotPossibleProductsToTrimStepAsync(input.CarId);
    }

    public async Task<TabularResult> GetTrimShopBodyCountAsync(MechReportsGetTrimShopBodyCountInput input)
    {
        return await _repository.GetTrimShopBodyCountAsync(input.CarId);
    }

    public async Task<TabularResult> GetTrimShopCriticalBodiesAsync(MechReportsGetTrimShopCriticalBodiesInput input)
    {
        return await _repository.GetTrimShopCriticalBodiesAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo), input.CriticalPartTypeId, input.SetCode);
    }

}
