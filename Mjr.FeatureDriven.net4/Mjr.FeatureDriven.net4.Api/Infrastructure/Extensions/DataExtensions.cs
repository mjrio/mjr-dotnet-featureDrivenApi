using System.Data.Common;
using Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Extensions
{
    public static class DataExtensions
    {
        /// <summary>
        /// Replaces the commandText with the values of the sqlcommand parameters.
        /// </summary>
        /// <param name="sqlCommand"></param>
        /// <param name="message"></param>
        public static void LogRawSql(this DbCommand sqlCommand, string message)
        {
            var query = sqlCommand.CommandText;
            foreach (DbParameter p in sqlCommand.Parameters)
            {
                query = query.Replace(p.ParameterName, string.Format("'{0}'", p.Value));
            }
            ApiEventSource.Log.RawSql(message, query);
        }
    }
}