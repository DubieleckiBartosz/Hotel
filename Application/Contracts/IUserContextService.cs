using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUserContextService
    {
        ClaimsPrincipal User { get; }
        string UserId { get; }
        List<string> UserRoles { get; }
        bool IsAdmin();
    }
}
