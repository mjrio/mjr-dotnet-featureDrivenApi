
using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions
{
    /// <summary>
    ///     Exception thrown when a required child of the primary object is not found.
    /// </summary>
    [Serializable]
    public class ChildObjectNotFoundException : Exception
    {
        public ChildObjectNotFoundException(string message, params object[] args) : base(string.Format(message, args))
        {
        }
    }
}