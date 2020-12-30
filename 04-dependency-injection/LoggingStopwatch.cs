using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace MonolithToMicroservices
{
    public interface ILoggingStopwatch
    {
        T Run<T>(Func<T> action);
    }

    public class LoggingStopwatch : ILoggingStopwatch
    {
        private readonly ILogger<LoggingStopwatch> _logger;

        public LoggingStopwatch(ILogger<LoggingStopwatch> logger)
        {
            _logger = logger;
        }

        public T Run<T>(Func<T> action)
        {
            
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                return action.Invoke();
            }
            finally
            {
                stopwatch.Stop();
                
                TimeSpan ts = stopwatch.Elapsed;
                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                    ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);
                _logger.LogInformation($"Method took {elapsedTime}");
            }
        }
    }
}