using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTask.Services
{
    public interface ILoggerService
    {
        void LogTrace(string message);
        void LogError(string message, string area,Exception e);
    }
}
