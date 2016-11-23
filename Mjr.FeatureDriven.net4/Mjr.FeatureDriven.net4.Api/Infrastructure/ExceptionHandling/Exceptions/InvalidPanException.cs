using System;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.ExceptionHandling.Exceptions
{
    public class InvalidPanException: FormatException
    {
        public InvalidPanException(string panNumber, string reason):base($"Format of pan: '{panNumber}' is invalid => {reason}.")
        {
            
        }
    }
}