using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InterviewTask.Services
{
    public class LoggerService : ILoggerService
    {
        private ILogger _logger = new Logger();

        public LoggerService()
        {
            _logger = new Logger();
        }

        public void LogTrace(string message)
        {
            _logger.Log(LogLevel.Trace, message);
        }

        public void LogError(string message,string area, Exception e)
        {
            _logger.Log(LogLevel.Error, string.Concat("Area: ",area," Message: ", message," Exception: " ,e.Message, " StackTrace:" + e.StackTrace));
        }

        public void LogError(string message, string area)
        {
            _logger.Log(LogLevel.Error, string.Concat("Area: ", area, " Message: ", message));
        }

    }
}