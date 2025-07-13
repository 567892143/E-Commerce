using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Shared.Exceptions;

namespace Shared.Exceptions;
public class GlobalException : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        

        int statusCode = StatusCodes.Status500InternalServerError;
        string title = "An unexpected error occurred";
        string message = exception.Message;
        string? details = null;

        if (exception is BaseCustomException customEx)
        {
            statusCode = customEx.StatusCode;
            title = "A custom error occurred";
            message = customEx.ErrorMessage;
            details = customEx.Details;
        }

        var response = new
        {
            Title = title,
            Message = message,
            Details = details,
            Code =statusCode
        };

       
        await httpContext.Response.WriteAsync(response.ToString(), cancellationToken);

        return true;
    }
}
