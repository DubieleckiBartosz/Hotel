using Application.AccountModels;
using Application.AccountModels.Enums;
using Application.Contracts;
using Application.Wrappers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status418ImATeapot)]
        [SwaggerOperation(Summary ="Register user")]
        [HttpPost(Name = "RegisterUser")]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterRequest registerModel)
        {
            var result = await _userService.RegisterUserAsync(registerModel,Request.Headers["origin"]);
            return Ok(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Generate token")]
        [HttpPost("GetToken")]
        public async Task<IActionResult> GetToken([FromBody] AuthenticationRequest request)
        {
            var result = await _userService.GetTokenAsync(request, ipAddress());
            if(result.Succeeded is true)
            {
                if (!string.IsNullOrEmpty(result.Data.RefreshToken))
                {
                    setTokenCookie(result.Data.RefreshToken);
                    return Ok(result);
                }
                else
                {
                    return StatusCode(206, new { Token = $"{result.Data.Token}" });
                }
            }
            return BadRequest(result);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Refresh token")]
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var response =await _userService.RefreshTokenAsync(refreshToken, ipAddress());

           if(response.Succeeded is true)
            {
                if (!string.IsNullOrEmpty(response.Data.RefreshToken))
                {
                    setTokenCookie(response.Data.RefreshToken);
                    return Ok(response);
                }
            }
            return BadRequest(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "E-mail confirmation")]
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery]string userId,[FromQuery]string code)
        {
            var result = await _userService.ConfirmEmailAsync(userId, code);
            return Ok(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Forgot password")]
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordRequest model)
        {
            await _userService.ForgotPasswordAsync(model);
            return Ok(new { message = "Please check your email for password reset instructions" });
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Reset password")]
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromQuery]ResetPasswordRequest model)
        {
            var result=await _userService.ResetPasswordAsync(model);
            return result is true?
                Ok(new { message = "Password reset successful, you can now login" })
                : BadRequest(new { message = "Error occured while reseting the password" });
        }

        [Authorize(Roles ="Admin")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(200, Type = typeof(Response<string>))]
        [SwaggerOperation(Summary = "Add new role to user")]
        [HttpPost("[action]")]
        public async Task<IActionResult> AddNewRoleToUser([FromBody]AddNewRoleRequest addNewRole)
        {
            var result = await _userService.CreateNewRoleAsync(addNewRole);
            return result.Succeeded is true ? Ok(result)
                : BadRequest(result);
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [SwaggerOperation(Summary = "Revoke token")]
        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody]RevokeTokenRequest model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(token))
                return BadRequest(new { message = "Token is required" });
            var response =await _userService.RevokeToken(token);

             return response is false? 
                NotFound(new { message = "Token not found" }):
                Ok(new { message = "Token revoked" });
        }
        private void setTokenCookie(string token)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(2)
            };
            Response.Cookies.Append("refreshToken", token, cookieOptions);
        }
        private string ipAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
