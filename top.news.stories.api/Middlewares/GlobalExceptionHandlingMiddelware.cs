using Newtonsoft.Json;
using System.Net;
using top.news.stories.api.Models.Responses;

namespace top.news.stories.api.Middlewares
{
    public class GlobalExceptionHandlingMiddelware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlingMiddelware> _logger;

        public GlobalExceptionHandlingMiddelware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddelware> logger)
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
                await HandleException(httpContext, ex);
            }
        }

        private async Task HandleException(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new Error
            {
                Success = false
            };
            switch (exception)
            {
                case HttpRequestException ex:
                    response.StatusCode = ex.StatusCode != null ? (int)ex.StatusCode : (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = GetErrorMessage(response.StatusCode);
                    break;
                default:
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    errorResponse.Message = "Server Error";
                    break;
            }
            _logger.LogError(exception.Message);
            var result = JsonConvert.SerializeObject(errorResponse);
            await context.Response.WriteAsync(result);
        }
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
