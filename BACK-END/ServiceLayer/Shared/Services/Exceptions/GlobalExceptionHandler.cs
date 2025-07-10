// using Microsoft.AspNetCore.Diagnostics;
// using Microsoft.AspNetCore.Http;
// using Shared.Exceptions;


// public class GlobalExceptionHandler : IExceptionHandler
// {
//     public async ValueTask<bool> TryHandleAsync(HttpContent httpContext, Exception exception, CancellationToken cancellationToken)
//     {
        

//         int statusCode = StatusCodes.Status500InternalServerError;
//         string title = "An unexpected error occurred";
//         string message = exception.Message;
//         string? details = null;

//         if (exception is BaseCustomException customEx)
//         {
//             statusCode = customEx.StatusCode;
//             title = "A custom error occurred";
//             message = customEx.ErrorMessage;
//             details = customEx.Details;
//         }

//         var response = new
//         {
//             Title = title,
//             Message = message,
//             Details = details
//         };

       
//         await httpContext.R.WriteAsync(response, cancellationToken);

//         return true;
//     }
// }
