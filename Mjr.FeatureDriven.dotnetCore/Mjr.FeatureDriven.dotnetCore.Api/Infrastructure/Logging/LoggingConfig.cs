using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog.Config;
using NLog.Targets;
using System.IO;
using System.Reflection;

namespace Mjr.FeatureDriven.dotnetCore.Api.Infrastructure.Logging
{
    public static class LoggingConfig
    {
        public static ILoggerFactory ConfigureLogging(this ILoggerFactory loggerFactory, IHostingEnvironment env, IConfigurationSection configuration)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
            }
            NLog.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging")));
            NLog.LogManager.AddHiddenAssembly(Assembly.Load(new AssemblyName("Microsoft.Extensions.Logging.Abstractions")));
            NLog.LogManager.AddHiddenAssembly(typeof(LoggingConfig).GetTypeInfo().Assembly);
            using (var provider = new NLogLoggerProvider())
            {
                loggerFactory.AddProvider(provider);
            }

            var fileName = Path.Combine(env.ContentRootPath, "nlog.config");
            NLog.LogManager.Configuration = new XmlLoggingConfiguration(fileName, true);

            return loggerFactory;
        }

        private static void ConfigureNLog()
        {
            string _logFilename = @"c:\temp\Mjr.FeatureDriven.dotnetCore.log";

            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();
            var fileTarget = new FileTarget
            {
                Name = "defaultFile",
                FileName = _logFilename,
                Layout = "${longdate} ${callsite} ${level} ${message}",
                ArchiveNumbering = ArchiveNumberingMode.Rolling
            };

            config.AddTarget(fileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, fileTarget);
            NLog.LogManager.Configuration = config;
        }
    }
}
