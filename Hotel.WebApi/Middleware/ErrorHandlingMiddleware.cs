using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Hotel.WebApi.Middleware
{
    public class ErrorHandlingMiddleware:IMiddleware
    {
        private readonly ILogger<ErrorHandlingMiddleware> _logger;
        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next.Invoke(context);
            }
            catch(ValidationException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new Response<string>() 
                { Message = ex?.Message,Errors=ex.Errors, Succeeded = false });
            }
            catch(NotFoundException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                await context.Response.WriteAsJsonAsync(new Response<string>(){Message=ex?.Message,Succeeded=false });
            }
            catch (BadRequestException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsJsonAsync(new Response<string>() { Message = ex?.Message, Succeeded = false });
            }
            catch(ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = 418;
                await context.Response.WriteAsJsonAsync(new Response<string>() { Message = ex?.Message, Succeeded = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsJsonAsync(new Response<bool>(false, $"Internal server error") { Succeeded=false});
            }
        }
    }
}
