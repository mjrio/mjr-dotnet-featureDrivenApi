using NLog;
using NLog.Config;
using NLog.Targets;

namespace Mjr.FeatureDriven.net4.Api.Infrastructure.Logging
{
    public class LoggingConfig
    {
        public static void Configure()
        {
            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 

            // Step 3. Set target properties 
            var fileTarget = CreateFileTarget(LogFileName);
            config.AddTarget("file", fileTarget);

            // Step 4. Define rules
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, fileTarget));

            // Step 5. Activate the configuration
            LogManager.Configuration = config;

        }
        private static FileTarget CreateFileTarget(string fileName)
        {
            var fileTarget = new FileTarget();

            fileTarget.FileName = fileName;
            fileTarget.Layout = "${longdate} ${callsite} ${level} ${message}";
            fileTarget.ArchiveNumbering = ArchiveNumberingMode.Rolling;
            return fileTarget;
        }

        public static string LogFileName
        {
            get
            {
                var fileName = "${date:format=yyyy-MM-dd}.log";
                var root = @"c:\temp\MjrApinet4Trace\";
                return root + fileName;
            }
        }

    }
}