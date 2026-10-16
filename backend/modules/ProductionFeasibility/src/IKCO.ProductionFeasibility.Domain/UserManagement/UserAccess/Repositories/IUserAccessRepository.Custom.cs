using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IKCO.ProductionFeasibility.Common;

namespace IKCO.ProductionFeasibility.UserManagement.UserAccess.Repositories;

public partial interface IUserAccessRepository
{
    /// <summary><c>PF.S_InsertUserAccess</c> با پارامتر جدولی <c>@Table</c> (Status, UserId, CreateDateTime, BIWSiteId, createUserId)</summary>
    Task<GlobalResultDto?> InsertAsync(IReadOnlyList<(int UserId, int BiwSiteId)> items, int createUserId, DateTime now);
}
