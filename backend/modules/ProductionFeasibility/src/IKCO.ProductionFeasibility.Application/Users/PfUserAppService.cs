using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Users.Dtos;

namespace IKCO.ProductionFeasibility.Users;

public class PfUserAppService : PfAppService, IPfUserAppService
{
    private readonly ICurrentPfUser _currentUser;

    public PfUserAppService(ICurrentPfUser currentUser)
    {
        _currentUser = currentUser;
    }

    public async Task<PfUserInfoDto> GetCurrentAsync()
    {
        var session = await _currentUser.GetAsync();
        return new PfUserInfoDto
        {
            Id = session.Id,
            IkcoUserId = session.IkcoUserId,
            UserName = session.UserName,
            FullName = session.FullName,
            IsAdmin = session.IsAdmin,
            VirtualPlantId = session.VirtualPlantId,
        };
    }
}
