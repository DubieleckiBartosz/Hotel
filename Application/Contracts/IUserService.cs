using Application.AccountModels;
using Application.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Contracts
{
    public interface IUserService
    {
        Task<Response<string>> RegisterUserAsync(RegisterRequest registerModel,string origin);
        Task<Response<AuthenticationResponse>> GetTokenAsync(AuthenticationRequest request,string ipAddress);
        Task<Response<string>> ConfirmEmailAsync(string userId, string code);
        Task ForgotPasswordAsync(ForgotPasswordRequest forgotPassword);
        Task<bool> ResetPasswordAsync(ResetPasswordRequest model);
        Task<Response<string>> CreateNewRoleAsync(AddNewRoleRequest model);
        Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress);
        Task<bool> RevokeToken(string token);
    }
}
