using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Users.Dtos;
using Volo.Abp.Application.Services;

namespace IKCO.ProductionFeasibility.Users;

public interface IPfUserAppService : IApplicationService
{
    /// <summary>کاربر جاری سامانه امکان تولید</summary>
    Task<PfUserInfoDto> GetCurrentAsync();
}
