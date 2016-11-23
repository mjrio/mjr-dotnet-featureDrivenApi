using System;
using System.Text;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions
{
    public static class ExceptionExtensions
    {
        #region GetContentOf
        /// <summary>
        /// Gets a delegate method that extracts information from the specified exception.
        /// </summary>
        /// <value>
        /// A <see cref="Func{Exception, String}"/> delegate method that extracts information 
        /// from the specified exception.
        /// </value>
        public static string GetContentOf(this Exception exception)
        {
            if (exception == null)
            {
                return String.Empty;
            }

            var result = new StringBuilder();

            result.AppendLine(exception.Message);
            result.AppendLine();

            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                result.AppendLine(innerException.Message);
                result.AppendLine();
                innerException = innerException.InnerException;
            }

#if DEBUG
            result.AppendLine(exception.StackTrace);
#endif

            return result.ToString();
        }
        #endregion
    }
}