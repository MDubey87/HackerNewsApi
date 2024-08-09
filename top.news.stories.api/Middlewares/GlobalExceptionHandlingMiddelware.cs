using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using top.news.stories.api.Models.Responses;

namespace top.news.stories.api.Middlewares
{
    /// <summary>
    /// Middleware for Global Exception Handling
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class GlobalExceptionHandlingMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddelware> _logger;
        /// <summary>
        /// GlobalExceptionHandlingMiddelware Constructor
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public GlobalExceptionHandlingMiddelware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddelware> logger)
        {
            _next = next;
            _logger = logger;
        }
        /// <summary>
        /// Invoke method to pass the context to next middleware
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleException(httpContext, ex);
            }
        }

        /// <summary>
        /// Handle the exception occured in request pipelie
        /// </summary>
        /// <param name="context">HttpContext Object</param>
        /// <param name="exception">Excetion Object</param>
        /// <returns></returns>
        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new ProblemDetails();
            switch (exception)
            {
                case HttpRequestException ex:
                    response.StatusCode = ex.StatusCode != null ? (int)ex.StatusCode : (int)HttpStatusCode.InternalServerError;
                    errorResponse.Detail = GetErrorMessage(response.StatusCode);
                    errorResponse.Status = response.StatusCode;
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Detail = "Server Error";
                    errorResponse.Status = response.StatusCode;
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }
        /// <summary>
        /// Get error message for differenc status code
        /// </summary>
        /// <param name="statusCode">Http Status Code</param>
        /// <returns>Return error message</returns>
        private string GetErrorMessage(int statusCode)
        {
            string errorMessage = "";
            switch (statusCode)
            {
                case 400:
                    errorMessage = "Bad Resquest";
                    break;
                case 401:
                    errorMessage = "Unauthorized Access";
                    break;
                case 404:
                    errorMessage = "Not Found";
                    break;
                case 500:
                    errorMessage = "Server Error";
                    break;
                default:
                    errorMessage = "Error Occured";
                    break;

            }
            return errorMessage;
        }
    }
}
