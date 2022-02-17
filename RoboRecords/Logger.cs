using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RoboRecords
{
    public static class Logger
    {
        public enum LogLevel
        {
            Info,
            Warning,
            Error,
            Debug
        }

        public static void Log(string message)
        {
            WriteToLogFile(message, LogLevel.Info, true);
        }

        public static void Log(string message, LogLevel logLevel)
        {
            WriteToLogFile(message, logLevel, true);
        }

        public static void Log(string message, bool toConsole)
        {
            WriteToLogFile(message, LogLevel.Info, toConsole);
        }

        public static void Log(string message, LogLevel logLevel, bool toConsole)
        {
            WriteToLogFile(message, logLevel, toConsole);
        }

        static void WriteToLogFile(string message, LogLevel _logLevel, bool toConsole)
        {
            // Don't log Debug messages if not in debug env
            if (_logLevel == LogLevel.Debug && !EnvVars.IsDevelopment)
                return;

            string logPath = EnvVars.LogPath;

            string logLevel;

            switch (_logLevel)
            {
                case LogLevel.Info:
                    logLevel = "INFO";
                    break;
                case LogLevel.Warning:
                    logLevel = "WARN";
                    break;
                case LogLevel.Error:
                    logLevel = "ERROR";
                    break;
                case LogLevel.Debug:
                    logLevel = "DEBUG";
                    break;
                default:
                    logLevel = "";
                    break;
            }
            DateTime now = DateTime.Now;

            string log = $"{now.ToString("yyyy-MM-dd HH:mm:ss")} | {logLevel} | {message}";

            using (StreamWriter sw = new StreamWriter(Path.Combine(logPath, $"roborecordslog-{now.ToString("yyyy-MM-dd")}.log"), true))
            {
                sw.WriteLineAsync(log);
            }

            if (toConsole)
            {
                // Hax
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    Debug.WriteLine($"{logLevel} | {message}");
                else
                    Console.WriteLine($"{logLevel} | {message}");

                // If that doesn't work, just do this instead:
                // Debug.WriteLine($"{logLevel} | {message}");
                // Console.WriteLine($"{logLevel} | {message}");
                // Catch-all solution, everyone is happy, except the CPU.
            }

        }
    }
}
