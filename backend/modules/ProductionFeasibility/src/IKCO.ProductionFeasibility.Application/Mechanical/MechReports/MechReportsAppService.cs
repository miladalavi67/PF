using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.BodyShop.Results;
using IKCO.ProductionFeasibility.Mechanical.Results;
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

    public async Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityAsync(MechReportsGetProductionPossibilityInput input)
    {
        return new ListResultDto<ProductionPossibilityRow>(await _repository.GetProductionPossibilityAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, PartNumber.OptionalValue(input.PartNo), input.CarModelId, input.CarClassId));
    }

    public async Task<ListResultDto<ProductionPossibilityRow>> GetProductionPossibilityWithLicenseAsync(MechReportsGetProductionPossibilityWithLicenseInput input)
    {
        return new ListResultDto<ProductionPossibilityRow>(await _repository.GetProductionPossibilityWithLicenseAsync(input.SaloonId, input.CrisisTypeId, input.CriticalPartTypeId, input.SubClassId, input.CarModelId, input.CarClassId));
    }

    public async Task<ListResultDto<ProdPossibilityByTrimShopRow>> GetProductionPossibilityByTrimShopAsync(MechReportsGetProductionPossibilityByTrimShopInput input)
    {
        return new ListResultDto<ProdPossibilityByTrimShopRow>(await _repository.GetProductionPossibilityByTrimShopAsync(input.TrimShopId, input.ProdState));
    }

    public async Task<ListResultDto<GeneralReportRow>> GetGeneralReportAsync(MechReportsGetGeneralReportInput input)
    {
        return new ListResultDto<GeneralReportRow>(await _repository.GetGeneralReportAsync(input.CriticalPartTypeId, input.SaloonId, input.CrisisTypeId, input.PlantId));
    }

    public async Task<ListResultDto<IncomingBodiesCountRow>> GetIncomingBodiesByCriticalPartAsync(MechReportsGetIncomingBodiesByCriticalPartInput input)
    {
        return new ListResultDto<IncomingBodiesCountRow>(await _repository.GetIncomingBodiesByCriticalPartAsync(input.Id, input.CriticalPartTypeId, input.SaloonId, input.PlantId));
    }

    public async Task<ListResultDto<IncomingBodyRow>> GetIncomingBodyListAsync(MechReportsGetIncomingBodyListInput input)
    {
        return new ListResultDto<IncomingBodyRow>(await _repository.GetIncomingBodyListAsync(input.Id, input.CriticalPartTypeId, input.SaloonId, input.ProductionStepGroupId, input.PlantId));
    }

    public async Task<ListResultDto<IncomingBodiesSummaryRow>> GetIncomingBodiesSummaryAsync(MechReportsGetIncomingBodiesSummaryInput input)
    {
        return new ListResultDto<IncomingBodiesSummaryRow>(await _repository.GetIncomingBodiesSummaryAsync(input.SaloonId, input.CrisisTypeId, input.ProductionStepGroupId, input.PlantId));
    }

    public async Task<ListResultDto<StockLogRow>> GetStockLogAsync(MechReportsGetStockLogInput input)
    {
        return new ListResultDto<StockLogRow>(await _repository.GetStockLogAsync(new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<SapCriticalLogRow>> GetSapCriticalLogAsync(MechReportsGetSapCriticalLogInput input)
    {
        return new ListResultDto<SapCriticalLogRow>(await _repository.GetSapCriticalLogAsync(new PartNumber(input.PartNo).Value));
    }

    public async Task<ListResultDto<FailedSapTransferRow>> GetFailedSapTransfersAsync(MechReportsGetFailedSapTransfersInput input)
    {
        return new ListResultDto<FailedSapTransferRow>(await _repository.GetFailedSapTransfersAsync(new ShamsiDate(input.FromDate).Value, new ShamsiDate(input.ToDate).Value));
    }

    public async Task<ListResultDto<AsnRow>> GetAsnReportAsync(MechReportsGetAsnReportInput input)
    {
        return new ListResultDto<AsnRow>(await _repository.GetAsnReportAsync(PartNumber.OptionalValue(input.PartNumber), ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), input.Type));
    }

    public async Task<ListResultDto<TrimInNotPossibleRow>> GetNotPossibleProductsToTrimStepAsync(MechReportsGetNotPossibleProductsToTrimStepInput input)
    {
        return new ListResultDto<TrimInNotPossibleRow>(await _repository.GetNotPossibleProductsToTrimStepAsync(input.CarId));
    }

    public async Task<ListResultDto<TrimShopBodyCountRow>> GetTrimShopBodyCountAsync(MechReportsGetTrimShopBodyCountInput input)
    {
        return new ListResultDto<TrimShopBodyCountRow>(await _repository.GetTrimShopBodyCountAsync(input.CarId));
    }

    public async Task<ListResultDto<TrimShopCriticalBodyRow>> GetTrimShopCriticalBodiesAsync(MechReportsGetTrimShopCriticalBodiesInput input)
    {
        return new ListResultDto<TrimShopCriticalBodyRow>(await _repository.GetTrimShopCriticalBodiesAsync(ShamsiDate.OptionalValue(input.FromDate), ShamsiDate.OptionalValue(input.ToDate), PartNumber.OptionalValue(input.PartNo), input.CriticalPartTypeId, input.SetCode));
    }

}
