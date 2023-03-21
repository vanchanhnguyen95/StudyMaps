using log4net.Config;
using log4net;
using System.Reflection;
using log4net.Repository.Hierarchy;
using log4net.Core;
using Microsoft.Extensions.Hosting.Internal;

namespace Elastic02.Services.Test
{
    public class LogService : ILogService
    {
        private ILog myLog4net;

        public LogService()
        {
            log4net.Config.XmlConfigurator.Configure();
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo("log4net.config"));
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            //log4net.GlobalContext.Properties["logFolder"] = @".\wwwroot\logs\";
            myLog4net = LogManager.GetLogger(typeof(LoggerManager));
        }

        public bool WriteLog(string message, LogLevel level = LogLevel.Debug)
        {
            if (myLog4net != null)
            {
                switch (level)
                {
                    case LogLevel.Error:
                        if (myLog4net.IsErrorEnabled)
                            myLog4net.Error(message);
                        break;

                    case LogLevel.Debug:
                        if (myLog4net.IsDebugEnabled)
                            myLog4net.Debug(message);
                        break;

                    case LogLevel.Fatal:
                        if (myLog4net.IsFatalEnabled)
                            myLog4net.Fatal(message);
                        break;

                    case LogLevel.Info:
                        if (myLog4net.IsInfoEnabled)
                            myLog4net.Info(message);
                        break;

                    case LogLevel.Warn:
                        if (myLog4net.IsWarnEnabled)
                            myLog4net.Warn(message);
                        break;
                }
                return true;

            }    

             return false;
        }
    }
}
