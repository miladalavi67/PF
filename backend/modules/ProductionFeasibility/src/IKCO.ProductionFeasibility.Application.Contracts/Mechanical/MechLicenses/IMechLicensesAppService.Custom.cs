using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

public partial interface IMechLicensesAppService
{
    /// <summary>تغییر شرایط تولید و ثبت مجوز (بحران مکانیکی یا سنکرون)</summary>
    Task<GlobalResultDto> ChangeAsync(MechLicensesChangeInput input);

    /// <summary>مجوز مصرف‌شده و باقی‌مانده‌ها</summary>
    Task<LicenseSummaryDto> GetSummaryAsync(MechLicensesGetSummaryInput input);
}
