using ICSharpCode.Core;
using log4net;
using log4net.Config;
using System;
using System.IO;

namespace AD.Workbench.Logging
{
    sealed class Log4netLoggingService : ILoggingService
    {
        ILog log;
        public Log4netLoggingService()
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "log4net.xml"));
            log = LogManager.GetLogger(typeof(Log4netLoggingService));
        }
        public void Debug(object message)
        {
            log.Debug(message);
        }

        public void DebugFormatted(string format, params object[] args)
        {
            log.DebugFormat(format, args);
        }

        public void Info(object message)
        {
            log.Info(message);
        }

        public void InfoFormatted(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Warn(object message)
        {
            log.Warn(message);
        }

        public void Warn(object message, Exception exception)
        {
            log.Warn(message, exception);
        }

        public void WarnFormatted(string format, params object[] args)
        {
            log.WarnFormat(format, args);
        }

        public void Error(object message)
        {
            log.Error(message);
        }

        public void Error(object message, Exception exception)
        {
            log.Error(message, exception);
        }

        public void ErrorFormatted(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message)
        {
            throw new NotImplementedException();
        }

        public void Fatal(object message, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void FatalFormatted(string format, params object[] args)
        {
            throw new NotImplementedException();
        }

        public bool IsDebugEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsInfoEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsWarnEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsErrorEnabled
        {
            get { throw new NotImplementedException(); }
        }

        public bool IsFatalEnabled
        {
            get { throw new NotImplementedException(); }
        }
    }
}
