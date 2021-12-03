using System.IO;
using System;
using Microsoft.Extensions.Configuration;

namespace RoboRecords
{
    public static class EnvVars
    {
        private const string EnvPrefix = "RoboRecords_";

        private const string EnvDataPath = EnvPrefix + "DataPath";

        public static string DataPath;

        public static bool isDevelopment = false;

        public static void ParseEnvironmentVariables(IConfiguration configuration)
        {
            DataPath = ParseEnvironmentVariable(EnvDataPath, true, true, configuration);
        }

        private static string ParseEnvironmentVariable(string varName, bool isPath, bool exitIfEmpty, IConfiguration configuration, string defaultValue = "")
        {
            string value = configuration[varName];

            bool isEmpty = string.IsNullOrEmpty(value);

            if (exitIfEmpty && isEmpty)
                LogAndExit(varName, false);

            if (isEmpty)
                value = defaultValue;

            if (isPath)
            {
                try
                {
                    value = Path.GetFullPath(value);

                    if (!Directory.Exists(value))
                        Directory.CreateDirectory(value);
                }
                catch { LogAndExit(varName, true); }
            }

            return value;
        }

        private static void LogAndExit(string varName, bool isPathError)
        {
            if(!isPathError)
                Console.WriteLine($"Environment variable {varName} is required but empty");
            else
                Console.WriteLine($"Environment variable {varName} has an invalid path");
            Environment.Exit(1);
        }
    }
}