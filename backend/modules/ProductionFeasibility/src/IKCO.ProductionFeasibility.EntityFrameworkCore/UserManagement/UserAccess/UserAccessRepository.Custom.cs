using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;
using IKCO.ProductionFeasibility.StoredProcedures;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess;

public partial class UserAccessRepository
{
    public async Task<GlobalResultDto?> InsertAsync(IReadOnlyList<(int UserId, int BiwSiteId)> items, int createUserId, DateTime now)
    {
        const string procedure = "PF.S_InsertUserAccess";
        var rows = items.Select(i => (IReadOnlyDictionary<string, object?>)new Dictionary<string, object?>
        {
            ["Status"] = true, ["UserId"] = i.UserId, ["CreateDateTime"] = now, ["BIWSiteId"] = i.BiwSiteId, ["createUserId"] = createUserId,
        }).ToList();
        var table = await CreateTableParameterAsync(procedure, "@Table", rows);
        return await QueryFirstOrDefaultAsync<GlobalResultDto>(procedure, new SpParameters().Table("@Table", table), LegacyCallStyle.AdoNet, PfCommandTimeouts.Command);
    }
}
