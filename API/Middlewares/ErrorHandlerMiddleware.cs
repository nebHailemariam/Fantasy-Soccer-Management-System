using System.Diagnostics;
using System.Net;
using System.Text.Json;
using API.Helpers;

namespace API.MiddelWare
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                HttpResponse response = context.Response;
                string errorMessage;
                response.ContentType = "application/json";

                switch (exception)
                {
                    case AppException e:
                        // custom application error
                        response.StatusCode = (int)e.StatusCode;
                        errorMessage = e.Message;
                        Console.WriteLine(e.Detail);
                        break;
                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        errorMessage = "Internal Server Error";
                        break;
                }

                var result = JsonSerializer.Serialize(new { message = errorMessage });
                await response.WriteAsync(result);

                Console.WriteLine(exception);
            }
        }
    }
}