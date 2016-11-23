using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions
{

    [Serializable]
    public class MissingDetailsException : Exception
    {
        public MissingDetailsException(string message, params object[] args)
            : base(string.Format(message, args))
        {

        }
    }
}