using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace MATH_CALC_COM.Services.Helpers
{
    public static class LoggerExtensions
    {
        public static void LogCustomInformation(this ILogger logger, IWebHostEnvironment env, string message)
        {
            if (logger.IsEnabled(LogLevel.Information))
            {
                string logDirectory = Path.Combine(env.ContentRootPath, "Logs");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                    string fileName = "log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
               
                var logFilePath = Path.Combine(logDirectory, fileName);
                
                File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}{Environment.NewLine}");
                
                logger.LogInformation(message);
            }
        }

        public static void LogCustomError(this ILogger logger, IWebHostEnvironment env, string message = null, Exception ex = null)
        {
            if (logger.IsEnabled(LogLevel.Error))
            {

                string logDirectory = Path.Combine(env.ContentRootPath, "Logs");

                if (!Directory.Exists(logDirectory))
                {
                    Directory.CreateDirectory(logDirectory);
                }

                string fileName = "log_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
                
                var logFilePath = Path.Combine(logDirectory, fileName);

                string errorString;

                if (ex != null && string.IsNullOrEmpty(message) == false)
                {
                    errorString = ex.ToString() + "; " + ex.Message + "; " + message;
                }
                else if (ex != null && string.IsNullOrEmpty(message) == true)
                {
                    errorString = ex.ToString() + "; " + ex.Message;
                }
                else
                {
                    errorString = message;
                }
                
                File.AppendAllText(logFilePath, $"{DateTime.Now}: ERROR: { errorString }{Environment.NewLine}");
                
                logger.LogError(ex, ex.Message);
            }
        }
    }
}
