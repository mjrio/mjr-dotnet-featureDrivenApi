using System;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using NLog;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Tracing
{
    /// <summary>
    /// Tracing class
    /// Methods should be in correct order with corresponding eventid.
    /// eventid's: should be in ascending order.
    /// 
    /// GUID: MjrApinet4Trace= {1D839E71-1CF6-565E-121B-DDFAD6AD0489}
    /// obtained by running GenerateProxyManifest -name "MjrApinet4Trace" 
    /// 
    /// if for some reason IsEnabled is false, check this class for inconsistencies.
    /// - order of methods, eventids
    /// - no helper methods (methods without event attribute)
    /// 
    /// Other:
    /// http://blogs.msmvps.com/kathleen/2014/01/24/how-are-event-parameters-best-used-to-create-an-intuitive-custom-evnetsourcetrace/
    /// Almost never use an Opcode parameter other than Start/Stop.
    /// When using Start/Stop Opcodes, also supply a Task and ensure the name of the method is the Task concatenated with the Opcode for the sake of the humans.
    /// Start and stop method signature should be identical to get durationMS to work.
    /// 
    /// to create on dev:
    ///     logman create trace MjrApinet4Trace -p {1D839E71-1CF6-565E-121B-DDFAD6AD0489} -bs 130 -o c:\temp\MjrApinet4Trace\MjrApinet4Trace -v mmddhhmm  -s servername
    /// </summary>
    [EventSource(Name = "MjrApinet4Trace")]
    public partial class ApiEventSource : EventSource
    {
        private static readonly Lazy<ApiEventSource> _log = new Lazy<ApiEventSource>();
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        public static ApiEventSource Log => _log.Value;

        #region WebApi log methods (starting from 100)
        //Do not change the order of these, only append new. these id's are 'registered' in windows, so changes them would brake the logging
        private const int WebApiVerboseEventId = 100;
        private const int ModelStateEventId = WebApiVerboseEventId + 1;
        private const int WebApiRequestStartEventId = ModelStateEventId + 1;
        private const int HeadersEventId = WebApiRequestStartEventId + 1;
        private const int WebApiRequestStopEventId = HeadersEventId + 1;
        private const int WebApiErrorEventId = WebApiRequestStopEventId + 1;
        private const int WebApiWarningEventId = WebApiErrorEventId + 1;
        private const int FunctionalWarningEventId = WebApiWarningEventId + 1;
        private const int PrincipalEventId = FunctionalWarningEventId + 1;
        private const int TraceMessageEventId = PrincipalEventId + 1;
        private const int SynchronisationErrorEventId = TraceMessageEventId + 1;
        private const int BodyEventId = SynchronisationErrorEventId + 1;
        private const int PermissiondeniedEventId = BodyEventId + 1;
        public static TraceSource TraceSource = new TraceSource("MjrApinet4Trace");
        //message= formatted message
        [Event(WebApiVerboseEventId, Message = "{0}.{1} - {2}", Level = EventLevel.Verbose)]
        public void WebApiVerbose(string Operator, string operation, string message)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(WebApiVerboseEventId, Operator, operation, message);
                }
        }
        [Event(ModelStateEventId, Message = "{0}", Level = EventLevel.Informational)]
        public void ModelState(string message)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(ModelStateEventId, message);
                }
        }

        [Event(WebApiRequestStartEventId, Message = "{0}", Level = EventLevel.LogAlways, Opcode = EventOpcode.Start)]
        public void WebApiRequestStart(string url)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
                {
                    WriteEvent(WebApiRequestStartEventId, url);
                }
        }
       
        [Event(WebApiRequestStopEventId, Message = "{0}", Level = EventLevel.LogAlways, Opcode = EventOpcode.Stop)]
        public void WebApiRequestStop(string url)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(url) && !string.IsNullOrWhiteSpace(url))
                {
                    WriteEvent(WebApiRequestStopEventId, url);
                }
        }
        //

        [Event(WebApiErrorEventId, Message = "{0} - {1} (see stacktrace for details)", Level = EventLevel.Error)]
        public void WebApiError(string url, string message, string stacktrace)
        {
            //TraceSource.Error(stacktrace);
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(WebApiErrorEventId, url, message, stacktrace);
                }
        }

        [Event(WebApiWarningEventId, Message = "{0}.{1} - {2}", Level = EventLevel.Warning)]
        public void WebApiWarning(string Operator, string operation, string message)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(WebApiWarningEventId, Operator, operation, message);
                }
        }

        [Event(FunctionalWarningEventId, Message = "{0} - {1} (see stacktrace for details)", Level = EventLevel.Warning)]
        public void FunctionalWarning(string url, string message, string stacktrace)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(FunctionalWarningEventId, url, message, stacktrace);
                }
        }
        [Event(PrincipalEventId, Message = "{0} - {1} (see roles for details)", Level = EventLevel.Informational)]
        public void Principal(string ivuser, string principal, string roles)
        {
            if (IsEnabled())
                if (!string.IsNullOrWhiteSpace(principal))
                {
                    WriteEvent(PrincipalEventId, ivuser, principal, roles);
                }
        }
        [Event(TraceMessageEventId, Message = "{0} - {1} (see messagedetails for details)", Level = EventLevel.Informational)]
        public void TraceMessage(string category, string message, string messagedetails)
        {
            if (IsEnabled())
                if (!string.IsNullOrWhiteSpace(category))
                {
                    WriteEvent(TraceMessageEventId, category, message, messagedetails);
                }
        }
        [Event(SynchronisationErrorEventId, Message = "{0} - {1} (see stacktrace for details)", Level = EventLevel.Error)]
        public void SynchronisationError(string panNumber, string message, string stacktrace)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(SynchronisationErrorEventId, panNumber, message, stacktrace);
                }
        }
        [Event(BodyEventId, Message = "{0}", Level = EventLevel.Informational)]
        public void Body(string message)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(message) && !string.IsNullOrWhiteSpace(message))
                {
                    WriteEvent(BodyEventId, message);
                }
        }

        //
        [Event(PermissiondeniedEventId, Message = "No {1}: {2}", Level = EventLevel.LogAlways)]
        public void Permissiondenied(string permission, string message )
        {
            if (IsEnabled())
                if ( !string.IsNullOrWhiteSpace(permission) && !string.IsNullOrWhiteSpace(message) )
                {
                    WriteEvent(PermissiondeniedEventId, permission, message);
                }
        }

        #endregion

        #region EF log methods (starting from 200)

        private const int EntityFrameworkVerboseEventId = 200;
        private const int RawSqlId = 201;
        private const int EntityFrameworkErrorEventId = 202;


        [Event(EntityFrameworkVerboseEventId, Message = "{0} (see EFMessage for more details)", Level = EventLevel.Verbose)]
        public void EntityFrameworkVerbose(string message, string EFMessage)
        {
             _logger.Info(EFMessage);
            if (IsEnabled())
                if (!string.IsNullOrEmpty(EFMessage) && !string.IsNullOrWhiteSpace(EFMessage))
                {
                    WriteEvent(EntityFrameworkVerboseEventId, message, EFMessage);
                }
        }

        [Event(RawSqlId, Message = "{0} (see rawSql for more details)", Level = EventLevel.Verbose)]
        public void RawSql(string message, string rawSql)
        {
            if (IsEnabled())
                if (!string.IsNullOrEmpty(rawSql) && !string.IsNullOrWhiteSpace(rawSql))
                {
                    WriteEvent(RawSqlId, message, rawSql);
                }
        }
        [Event(EntityFrameworkVerboseEventId, Message = "{0} (see EFMessage for more details)", Level = EventLevel.Verbose)]
        
        public void EntityFrameworkError(string message, string EFMessage)
        {
            _logger.Error(EFMessage);
            if (IsEnabled())
                if (!string.IsNullOrEmpty(EFMessage) && !string.IsNullOrWhiteSpace(EFMessage))
                {
                    WriteEvent(EntityFrameworkErrorEventId, message, EFMessage);
                }
        }
        #endregion
    }
}
