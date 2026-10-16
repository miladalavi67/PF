using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.Common.ValueObjects;
using IKCO.ProductionFeasibility.Exceptions;
using IKCO.ProductionFeasibility.Mechanical.MechLicenses.Dtos;
using Volo.Abp.Uow;

namespace IKCO.ProductionFeasibility.Mechanical.MechLicenses;

public partial class MechLicensesAppService
{
    /// <summary>
    /// جریان فرم frmCriticalPartsProdPosChange سامانه SFC: ۱) به‌روزرسانی شرایط تولید خود بحران
    /// (<c>S_CriticalPartsProdPosUpdate</c> برای مکانیکی یا <c>S_CriticalPartsSetUpdateProductionPossibility</c> برای سنکرون) و
    /// ۲) ثبت/ویرایش ردیابی مجوز (<c>S_InfluenceShortageLicenseTracking</c>) — هر دو در یک تراکنش.
    /// </summary>
    [UnitOfWork(isTransactional: true)]
    public async Task<GlobalResultDto> ChangeAsync(MechLicensesChangeInput input)
    {
        var personId = await _currentUser.GetPersonIdAsync();
        var toDate = ShamsiDate.OptionalValue(input.ToDate);
        if (input.ProductionPossibility != 1 && (input.PossibleProductionNo.HasValue || toDate != null))
            throw new BaseException(PfErrorCodes.InputInvalid, "تعداد/تاریخ مجاز فقط برای شرایط «تولید با کسری» معتبر است.");

        var fall = input.ProductionFallId ?? 0;
        if (input.CriticalPartTypeId == 0)
        {
            await _repository.UpdateMechanicalProductionPossibilityAsync(input.CriticalPartId, input.ProductionPossibility, input.PossibleProductionNo,
                toDate, input.ToTime, input.LetterNo, input.Note, fall);
        }
        else
        {
            await _repository.UpdateSetProductionPossibilityAsync(input.CriticalPartId, input.ProductionPossibility, input.PossibleProductionNo, fall,
                toDate, input.ToTime, input.LetterNo, input.Note);
        }

        await _repository.InfluenceTrackingAsync(input.CriticalPartTypeId, input.CriticalPartId, input.ProductionPossibility, input.PossibleProductionNo,
            toDate, input.ToTime, personId, input.EditLast ? 3 : 0);
        return new GlobalResultDto { Result = 1, Message = "شرایط تولید با موفقیت ثبت شد." };
    }

    public async Task<LicenseSummaryDto> GetSummaryAsync(MechLicensesGetSummaryInput input)
    {
        var s = await _repository.GetSummaryAsync(input.CriticalPartTypeId, input.CriticalPartId);
        return new LicenseSummaryDto
        {
            UsedLicense = s.UsedLicense,
            ValidRemainingOfAllShouldPreviousShortageLicenses = s.ValidRemainingOfAll,
            ValidRemainingOfLastLimitedShortageLicense = s.ValidRemainingOfLastLimited,
        };
    }
}
