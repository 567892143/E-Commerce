using System;

namespace Shared.Exceptions
{
    public abstract class BaseCustomException : Exception
    {
        public int StatusCode { get; }
        public string ErrorMessage { get; }
        public string? Details { get; }

        protected BaseCustomException(int statusCode, string errorMessage, string? details = null)
            : base(errorMessage)
        {
            StatusCode = statusCode;
            ErrorMessage = errorMessage;
            Details = details;
        }
    }
}
