
using Shared.Exceptions;
namespace Shared.Services.Exceptions
{
    public class NotFoundCustomException : BaseCustomException
    {
        public NotFoundCustomException(string message, string? details = null)
            : base(404, message, details)
        {
        }
    }
}
