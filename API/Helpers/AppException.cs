using System.Net;

namespace API.Helpers
{
    // Custom exception class for throwing application specific exceptions 
    // that can be caught and handled within the application
    public class AppException : Exception
    {
        public string Detail { get; private set; }
        public HttpStatusCode StatusCode { get; private set; }
        public AppException(string message, string detail = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError) : base(message)
        {
            Detail = detail;
            StatusCode = statusCode;
        }
    }
}