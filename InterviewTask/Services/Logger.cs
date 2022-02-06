using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;

namespace InterviewTask.Services
{
    public class Logger : ILogger
    {
        public void Log(LogLevel priority, string message)
        {
            string filePath = ConfigurationManager.AppSettings.Get("LoggerDirectoryPath");

            //Create if doesn't exist
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            using (StreamWriter writer = new StreamWriter(filePath + "log.txt",append: true))
            {
                writer.WriteLine(string.Concat(Enum.GetName(typeof(LogLevel),priority)," ",message));
                writer.Close();
            }
        }
    }
}