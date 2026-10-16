using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.EntityFrameworkCore;
using IKCO.ProductionFeasibility.StoredProcedures;
using IKCO.ProductionFeasibility.Users.Repositories;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;

namespace IKCO.ProductionFeasibility.Users;

/// <summary>خواندن کاربر از <c>PF.B_User</c> و <c>dbo.B_Person</c> با پرس‌وجوی پارامتری.</summary>
public class PfUserRepository : PfDapperRepository<PfDbContext>, IPfUserRepository, ITransientDependency
{
    public PfUserRepository(IDbContextProvider<PfDbContext> dbContextProvider)
        : base(dbContextProvider)
    {
    }

    public async Task<PfUserRecord?> FindByUserNameAsync(string userName)
    {
        var rows = await QuerySqlAsync<UserRow>(
            "SELECT TOP (1) Id, UserID AS IkcoUserId, UserName, FirstName, LastName, IsAdmin, VirtualPlantId FROM PF.B_User WITH (NOLOCK) WHERE UserName = @UserName ORDER BY Id",
            new { UserName = userName });
        var row = rows.FirstOrDefault();
        return row == null ? null : new PfUserRecord(row.Id, row.IkcoUserId, row.UserName, row.FirstName, row.LastName, row.IsAdmin, row.VirtualPlantId);
    }

    public async Task<int?> FindPersonIdAsync(string userName)
    {
        var ids = await QuerySqlAsync<int>(
            "SELECT TOP (1) Id FROM dbo.B_Person WITH (NOLOCK) WHERE (PersonelCode = @UserName OR UserName = @UserName) ORDER BY Status, Id",
            new { UserName = userName });
        return ids.Count == 0 ? null : ids[0];
    }

    private sealed class UserRow
    {
        public int Id { get; set; }
        public int IkcoUserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public bool? IsAdmin { get; set; }
        public int? VirtualPlantId { get; set; }
    }
}
