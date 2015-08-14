using log4net;
using log4net.Appender;
using log4net.Core;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace AD.Workbench.Logging
{
    sealed class LogMessageRecorder : AppenderSkeleton
    {
        public const int DefaultBufferSize = 20;

        LoggingEvent[] buffer = new LoggingEvent[DefaultBufferSize];
        int bufferSize = DefaultBufferSize;
        int nextIndex;

        public LogMessageRecorder()
            : base()
        {
        }
        public int BufferSize
        {
            get { return bufferSize; }
            set
            {
                lock (this)
                {
                    bufferSize = value;
                    buffer = new LoggingEvent[bufferSize];
                    nextIndex = 0;
                }
            }
        }

        ICollection<LoggingEvent> RecordedEvents
        {
            get
            {
                lock (this)
                {
                    List<LoggingEvent> events = new List<LoggingEvent>(bufferSize);
                    int i = nextIndex;
                    LoggingEvent e;
                    do
                    {
                        e = buffer[i++];
                        if (e != null)
                        {
                            events.Add(e);
                        }
                        if (i >= bufferSize)
                        {
                            i = 0;
                        }
                    } while (i != nextIndex);
                    return events.AsReadOnly();
                }
            }
        }
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            loggingEvent.Fix = FixFlags.Exception | FixFlags.Message | FixFlags.ThreadName;
            buffer[nextIndex] = loggingEvent;
            if (++nextIndex >= bufferSize)
            {
                nextIndex = 0;
            }
        }


        public static void AppendRecentLogMessages(StringBuilder sb, ILog log)
        {
            LogMessageRecorder recorder = log.Logger.Repository.GetAppenders().OfType<LogMessageRecorder>().Single();
            foreach (LoggingEvent e in recorder.RecordedEvents)
            {
                sb.Append(e.TimeStamp.ToString(@"HH\:mm\:ss\.fff", CultureInfo.InvariantCulture));
                sb.Append(" [");
                sb.Append(e.ThreadName);
                sb.Append("] ");
                sb.Append(e.Level.Name);
                sb.Append(" - ");
                sb.Append(e.RenderedMessage);
                sb.AppendLine();

                if (e.ExceptionObject != null)
                {
                    sb.AppendLine("--> Exception:");
                    sb.AppendLine(e.GetExceptionString());
                }
            }
        }
    }
}
