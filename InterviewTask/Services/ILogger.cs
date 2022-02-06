using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterviewTask.Services
{
    public interface ILogger
    {
        void Log(LogLevel priority, string message);
    }
}
