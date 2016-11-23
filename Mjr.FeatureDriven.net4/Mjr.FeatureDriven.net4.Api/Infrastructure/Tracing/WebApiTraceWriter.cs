using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web.Http.Tracing;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing
{
    public class WebApiTraceWriter : ITraceWriter
    {
        public void Trace(System.Net.Http.HttpRequestMessage request, string category, TraceLevel level, Action<TraceRecord> traceAction)
        {
            if (level == TraceLevel.Off)
            {
                return;
            }
            var record = new TraceRecord(request, category, level);
            traceAction(record);
            LogTraceRecord(record);
        }

        private void LogTraceRecord(TraceRecord record)
        {
            if (record.Category == "System.Web.Http.Request")
            {
                if (record.Kind == TraceKind.Begin)
                {
                    ApiEventSource.Log.WebApiRequestStart(record.Message);
                }
                else if (record.Kind == TraceKind.End)
                    ApiEventSource.Log.WebApiRequestStop(record.Request.RequestUri.AbsoluteUri);
            }
            else
            {
                switch (record.Level)
                {
                    case TraceLevel.Error:
                    case TraceLevel.Fatal:
                        //we do not handle the webapi errors at this level
                        //we handle them using the UnhandledExceptionfilter so we don't get duplicate error messages
                        break;
                    case TraceLevel.Warn:
                        ApiEventSource.Log.WebApiWarning(record.Operator, record.Operation, record.Message);
                        break;
                    default:
                        if (record.Message != null && record.Message.StartsWith("Model state"))
                            ApiEventSource.Log.ModelState(record.Message);
                        //too much verbose Tracing, unless sth goes wrong, enable this again.
                        
                        break;
                }
            }
        }
    }
}