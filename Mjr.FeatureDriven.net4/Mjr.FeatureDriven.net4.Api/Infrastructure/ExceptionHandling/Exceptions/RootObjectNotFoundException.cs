
using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions
{
    /// <summary>
    ///     Exception thrown when the primary, or "aggregate root", object is not found.
    /// </summary>
    [Serializable]
    public class RootObjectNotFoundException : Exception
    {
        public RootObjectNotFoundException(string message, params object[] args) : base(string.Format(message, args))
        {

        }
    }

}