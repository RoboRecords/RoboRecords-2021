using System.IO;
using System;
using Microsoft.Extensions.Configuration;

namespace RoboRecords
{
    public static class EnvVars
    {
        private const string EnvPrefix = "RoboRecords_";

        private const string EnvDataPath = EnvPrefix + "DataPath";
        private const string EnvLogPath = EnvPrefix + "LogPath";
        private const string EnvSftpKeyPath = EnvPrefix + "SftpKeyPath";
        private const string EnvSftpHost = EnvPrefix + "SftpHostAddress";
        private const string EnvSftpUser = EnvPrefix + "SftpUserName";
        
        public static string DataPath;
        public static string LogPath;
        public static string SftpKeyPath;
        public static string SftpHost;
        public static string SftpUser;

        public static bool IsDevelopment = false;

        public static void ParseEnvironmentVariables(IConfiguration configuration)
        {
            LogPath = ParseEnvironmentVariable(EnvLogPath, false, true, false, configuration, "Logs");
            DataPath = ParseEnvironmentVariable(EnvDataPath, false, true, true, configuration);
        }

        private static string ParseEnvironmentVariable(string varName, bool isFilePath, bool isDirPath, bool exitIfEmpty, IConfiguration configuration, string defaultValue = "")
        {
            string value = configuration[varName];

            bool isEmpty = string.IsNullOrEmpty(value);

            if (exitIfEmpty && isEmpty)
                LogAndExit(varName, false);

            if (isEmpty)
                value = defaultValue;

            if (isDirPath)
            {
                try
                {
                    value = Path.GetFullPath(value);

                    if (!Directory.Exists(value))
                        Directory.CreateDirectory(value);
                }
                catch { LogAndExit(varName, true); }
            }

            if (isFilePath)
                value = Path.GetFullPath(value);

            return value;
        }

        private static void LogAndExit(string varName, bool isPathError)
        {
            if(!isPathError)
                Logger.Log($"Environment variable {varName} is required but empty", Logger.LogLevel.Error, true);
            else
                Logger.Log($"Environment variable {varName} has an invalid path", Logger.LogLevel.Error, true);
            Environment.Exit(1);
        }
    }
}