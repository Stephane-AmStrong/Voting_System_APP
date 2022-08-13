using Application.Exceptions;
using Application.Wrappers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebApi.Middlewares
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError($"\n\n{ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var response = context.Response;
            response.ContentType = "application/json";
            var errorDetails = new ErrorDetails { Message = exception?.Message};



            switch (exception)
            {
                case ApiException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;

                    errorDetails.StatusCode = response.StatusCode;
                    break;
                case KeyNotFoundException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                case SecurityTokenException e:
                    // not found error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    errorDetails.StatusCode = response.StatusCode;
                    break;
                case ValidationException e:
                    // custom application error
                    response.StatusCode = (int)HttpStatusCode.BadRequest;

                    errorDetails.StatusCode = response.StatusCode;
                    errorDetails.Errors = e.Errors;
                    break;
                default:
                    // unhandled error
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;

                    errorDetails.StatusCode = response.StatusCode;
                    errorDetails.Message = "Internal Server Error.";
                    errorDetails.Errors = null ;
                    break;
            }

            var result = JsonSerializer.Serialize(errorDetails);

            await response.WriteAsync(result);
        }
    }
}
