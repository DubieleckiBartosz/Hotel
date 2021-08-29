using Application.AccountModels.Enums;
using Application.Contracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Service
{
    public class UserContextService : IUserContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private string _userId;
        public UserContextService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public ClaimsPrincipal User =>
              _httpContextAccessor.HttpContext?.User;

        //public string UserId => User is null ? null :
        //       User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public string UserId => !User.HasClaim(c=>c.Type==ClaimTypes.NameIdentifier) 
            || User is null ? null :
             User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;

        public List<string> UserRoles => User is null ? null :
            User.Claims.Where(c => c.Type == ClaimTypes.Role)
            .Select(c=>c.Value).ToList();
        public bool IsAdmin() => User is null ? false : 
            User.IsInRole(role: Roles.Admin.ToString());
           
    }
}
