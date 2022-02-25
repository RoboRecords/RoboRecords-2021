/*
 * FileManager.cs: Various file, directory, paths abstractions meant to be used throughout most of the codebase
 * Copyright (C) 2022, Refrag <Refragg> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.IO;
using System.Threading.Tasks;
using Renci.SshNet;

namespace RoboRecords
{
    public static class FileManager
    {
        public const string AssetsDirectoryName = "assets";
        public const string UserAssetsDirectoryName = "UserAssets";
        public const string ReplaysDirectoryName = "Replays";
        
        public static void Initialize()
        {
            if (!Exists(UserAssetsDirectoryName))
                CreateDirectory(UserAssetsDirectoryName);
            if (!Exists(ReplaysDirectoryName))
                CreateDirectory(ReplaysDirectoryName);
        }

        private static bool LocalTryAction(Action localAction)
        {
            try
            {
                localAction();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Action failed ({e.Message})", Logger.LogLevel.Error, true);
                return false;
            }
        }
        
        private static bool LocalTryAction<TResult>(Func<TResult> localAction, out TResult result)
        {
            result = default;
            try
            {
                result = localAction();
                return true;
            }
            catch (Exception e)
            {
                Logger.Log($"Action failed ({e.Message})", Logger.LogLevel.Error, true);
                return false;
            }
        }
        
        public static bool CreateDirectory(string relativePath)
        {
            string localPath = Path.Combine(EnvVars.DataPath, relativePath);
            
            // Creating an already existing directory can return true so to keep it consistent with SFTP we return false 
            if (Directory.Exists(localPath))
                return false;
            
            return LocalTryAction(() => Directory.CreateDirectory(localPath));
        }
        
        public static bool Exists(string relativePath, out string absolutePath)
        {
            absolutePath = Path.Combine(EnvVars.DataPath, relativePath);

            string localPath = absolutePath;
            
            LocalTryAction(() => File.Exists(localPath), out bool fileResult);
            if (fileResult)
                return true;
            
            LocalTryAction(() => Directory.Exists(localPath), out bool directoryResult);
            if (directoryResult)
                return true;
            
            return false;
        }

        public static bool Exists(string relativePath)
        {
            return Exists(relativePath, out _);
        }
        
        public static bool CreateFile(string relativePath)
        {
            return LocalTryAction(() => File.Create(Path.Combine(EnvVars.DataPath, relativePath)));
        }

        public static bool DeleteFile(string relativePath)
        {
            string localPath = Path.Combine(EnvVars.DataPath, relativePath);
            
            // Deleting a non existing file can return true so to keep it consistent with SFTP we return false 
            if (!File.Exists(localPath))
                return false;
                
            return LocalTryAction(() => File.Delete(localPath));
        }

        public static bool Read(string relativePath, out byte[] fileBytes)
        {
            byte[] bytes = null;

            bool worked = LocalTryAction(() => bytes = File.ReadAllBytes(Path.Combine(EnvVars.DataPath, relativePath)));

            fileBytes = bytes;
            return worked;
        }
        
        public static bool Read(string relativePath, out string fileContents)
        {
            string contents = string.Empty;
            
            bool worked = LocalTryAction(() => contents = File.ReadAllText(Path.Combine(EnvVars.DataPath, relativePath)));

            fileContents = contents;
            return worked;
        }
        
        public static bool Read(string relativePath, out string[] fileLines)
        {
            string[] lines = null;

            bool worked = LocalTryAction(() => lines = File.ReadAllLines(Path.Combine(EnvVars.DataPath, relativePath)));

            fileLines = lines;
            return worked;
        }

        public static bool Write(string relativePath, byte[] bytes)
        {
            return LocalTryAction(() => File.WriteAllBytes(Path.Combine(EnvVars.DataPath, relativePath), bytes));
        }
        
        public static bool Write(string relativePath, string contents)
        {
            return LocalTryAction(() => File.WriteAllText(Path.Combine(EnvVars.DataPath, relativePath), contents));
        }
        
        public static bool Write(string relativePath, string[] lines)
        {
            return LocalTryAction(() => File.WriteAllLines(Path.Combine(EnvVars.DataPath, relativePath), lines));
        }
    }
}