using Application.AccountModels;
using Application.AccountModels.Enums;
using Application.Contracts;
using Application.Exceptions;
using Application.Models;
using Application.Wrappers;
using Domain.Settings;
using Infrastructure.Identity.DbContext;
using Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Identity.Services
{
    public class UserService: IUserService
    {
        private const string errorMsg = "Entered data is incorrect";
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<UserService> _logger;
        private readonly IdentityContext _context;
        public UserService(IOptions<JwtSettings> settings,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,IdentityContext context,
            IEmailService emailService,ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
            _jwtSettings = settings.Value;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<Response<string>> RegisterUserAsync(RegisterRequest registerModel,string origin)
        {
            var useremail = await _userManager.FindByEmailAsync(registerModel.Email);
            var username = await _userManager.FindByNameAsync(registerModel.UserName);
            if(useremail is not null || username is not null)
            {
                _logger.LogInformation("Attempt to create an existing user account");
                return ResponseMessage($"You can't create an account");
            }
            var user = new ApplicationUser
            {
                UserName=registerModel.UserName,
                Email=registerModel.Email,
                FirstName=registerModel.FirstName,
                LastName=registerModel.LastName
            };
            var result=await _userManager.CreateAsync(user, registerModel.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(c => c.Description).ToList();
                return ErrorResponse(null, errors);
            }
            await _userManager.AddToRoleAsync(user, Roles.BasicUser.ToString());
            var emailResult = await SendVerificationEmail(user, origin);

            if(emailResult is true)
            {
                return ResponseMessage("Confirmation email sent");
            }
            return ErrorResponse("Something went wrong", null);
        }

        public async Task<Response<string>> ConfirmEmailAsync(string userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId);

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(c => c.Description).ToList();
                return ErrorResponse(null, errors);
            }
            return ResponseMessage($"Account Confirmed for {user.Email}");
        }

        public async Task ForgotPasswordAsync(ForgotPasswordRequest forgotPassword)
        {
            var user = await _userManager.FindByEmailAsync(forgotPassword.Email);
            if(user is not null)
            {
                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                await sendPasswordResetEmail(forgotPassword.Email, code);
            }
           
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequest model)
        {
            var account = await _userManager.FindByEmailAsync(model.EmailAddress);
            if (account == null) throw new BadRequestException($"No Accounts Registered with {model.EmailAddress}.");
            var result = await _userManager.ResetPasswordAsync(account, model.Token, model.Password);
            return result.Succeeded ? true : false;
        }


        public async Task<Response<string>> CreateNewRoleAsync(AddNewRoleRequest model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(model is null || user is null)
            {
               return ErrorResponse(errorMsg,null);
            }
            var existRole = Enum.GetNames(typeof(Roles)).Any(c => c.ToLower() == model.Role.ToLower());
            if (existRole)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var userAlreadyuContainsRole = userRoles.Where(c => c.ToLower() == model.Role.ToLower());
                if (userAlreadyuContainsRole.Any())
                {
                    return ResponseMessage($"User already has role {model.Role}");
                }

                var roleValue = Enum.GetValues(typeof(Roles)).Cast<Roles>()
                    .Where(c => c.ToString().ToLower() == model.Role.ToLower());
                var addRole = await _userManager.AddToRoleAsync(user, model.Role);
                if (addRole.Succeeded)
                {
                   return ResponseMessage($"Added new role {model.Role} to user {user.UserName}");
                }
                var errors = addRole.Errors.Select(c => c.Description);
                return ErrorResponse("The role could not be added", errors.ToList());
            }
            return ErrorResponse(errorMsg, null);
        }



        public async Task<Response<AuthenticationResponse>> GetTokenAsync(AuthenticationRequest request,string ipAddress)
        {
            Response<AuthenticationResponse> response = new();
            var user = await _userManager.FindByEmailAsync(request.Email);
            if(user is null)
            {
                response.Message = "Sorry but there is no token for this data";
                response.Succeeded = false;
                return response;
            }

            var checkPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!checkPassword)
            {
                throw new BadRequestException(errorMsg);
            }
            if (!user.EmailConfirmed && (checkPassword is true))
            {
                throw new BadRequestException($"Account Not Confirmed for '{request.Email}'");
            }
            var jwtSecurityToken = await GenerateToken(user,ipAddress);

            AuthenticationResponse modelResponse = new AuthenticationResponse();
            modelResponse.UserId = user.Id;
            modelResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            modelResponse.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            modelResponse.UserRoles = rolesList.ToList();


            if (user.RefreshTokens.Any(a => a.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.Where(a => a.IsActive == true).FirstOrDefault();
                modelResponse.RefreshToken = activeRefreshToken.Token;
                modelResponse.RefreshTokenExpiration = activeRefreshToken.Expires;
            }
            else
            {
                var refreshToken = GenerateRefreshToken(ipAddress);
                modelResponse.RefreshToken = refreshToken.Token;
                modelResponse.RefreshTokenExpiration = refreshToken.Expires;
                user.RefreshTokens.Add(refreshToken);
                _context.Update(user);
                _context.SaveChanges();
                user.RefreshTokens.Add(refreshToken);
                removeOldRefreshTokens(user);

                _context.Update(user);
                await _context.SaveChangesAsync();
            }


            return new Response<AuthenticationResponse>(modelResponse, $"Authenticated {user.UserName}");
        }

        public async Task<Response<AuthenticationResponse>> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = await _context.Users.SingleOrDefaultAsync(c => 
            c.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
               throw new BadRequestException("Token not found on user");

            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
            if (!refreshToken.IsActive)
              throw new BadRequestException("Token not active");

            var newRefreshToken = GenerateRefreshToken(ipAddress);
            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReplacedByToken = newRefreshToken.Token;
      
            
            user.RefreshTokens.Add(newRefreshToken);
            removeOldRefreshTokens(user);

            _context.Update(user);
            await _context.SaveChangesAsync();

            var jwtSecurityToken = await GenerateToken(user, ipAddress);


            AuthenticationResponse modelResponse = new ();
            modelResponse.UserId = user.Id;
            modelResponse.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            modelResponse.RefreshToken = newRefreshToken.Token;
            modelResponse.UserName = user.UserName;
            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            modelResponse.UserRoles = rolesList.ToList();
            modelResponse.RefreshTokenExpiration = newRefreshToken.Expires;
            return new Response<AuthenticationResponse>(modelResponse);
        }

        private void removeOldRefreshTokens(ApplicationUser account)
        {
             account.RefreshTokens.RemoveAll(x =>
                !x.IsActive &&
                x.Created.AddMinutes(_jwtSettings.DurationInMinutes) <= DateTime.UtcNow);
        }
        private RefreshToken GenerateRefreshToken(string ip)
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);
            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomBytes),
                Expires = DateTime.UtcNow.AddDays(2),
                Created = DateTime.UtcNow,
                CreatedByIp = ip
            };
        }

        public async Task<bool> RevokeToken(string token)
        {
            var user = await _context.Users.SingleOrDefaultAsync(c => 
            c.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return false;
            var refreshToken = user.RefreshTokens.Single(x => x.Token == token);
   
            if (!refreshToken.IsActive) return false;

            refreshToken.Revoked = DateTime.UtcNow;
            _context.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task<JwtSecurityToken> GenerateToken(ApplicationUser applicationUser,string ipAddress)
        {
            var userClaims = await _userManager.GetClaimsAsync(applicationUser);
            var userRoles = await _userManager.GetRolesAsync(applicationUser);
            var roleClaims = new List<Claim>();
            foreach (var item in userRoles)
            {
                roleClaims.Add(new Claim(ClaimTypes.Role,item));
            }
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,applicationUser.Id),
                new Claim(ClaimTypes.Email,applicationUser.Email),
                new Claim(ClaimTypes.Name,applicationUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("ip",ipAddress)
            }.Union(userClaims).Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }

        private async Task<bool> SendVerificationEmail(ApplicationUser user,string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "api/account/confirm-email/";
            var _enpointUri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(_enpointUri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "code", code);

            var email = new Email
            {
                To = user.Email,
                Subject = "Register New User",
                Body=$"Confirm account: {verificationUri}"
            };
            var result =await _emailService.SendEmail(email);
            return result;
        }


        private async Task sendPasswordResetEmail(string mail,string resetToken)
        {
            string message;

                message = $@"<p>Please use the below token to reset your password with the <code>/accounts/reset-password</code> api route:</p>
                             <p><code>{resetToken}</code></p>";

            var email = new Email
            {
                To = mail,
                Subject = "Reset Password",
                Body = $@"<h4>Reset Password Email</h4>
                         {message}"
            };
            await _emailService.SendEmail(email);
        }

        private Response<string> ResponseMessage(string message) =>
            new Response<string>(null, message);
        private Response<string> ErrorResponse(string message,List<string> errors) =>
            new Response<string>(message) { Succeeded = false,Errors=errors };
    }
}
