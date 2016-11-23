using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions
{
    [Serializable]
    public class ValidationException : Exception
    {
        public ValidationException(string message, params object[] args)
            : base(string.Format(message, args))
        {

        }
    }
}