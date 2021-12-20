using System;
using System.IO;
using System.Threading.Tasks;
using Renci.SshNet;

namespace RoboRecords
{
    public static class FileManager
    {
        // The / directory is not editable from the SFTP user so we start doing things in the RoboRecords folder
        private const string SftpRootDirectory = "RoboRecords";
        
        private static SftpClient _sftpClient;
        

        public static void Initialize()
        {
            // Initialize the SftpClient and connect to it (it might be preferable to stay disconnected as much as possible but this is fine for now)
            if (EnvVars.IsDevelopment)
            {
                _sftpClient = new SftpClient(EnvVars.SftpHost, EnvVars.SftpUser, new PrivateKeyFile(EnvVars.SftpKeyPath));
                
                _sftpClient.Connect();
            }
        }

        // The wrapper that should be used for any SFTP action as it takes care or retrying and connecting 
        private static bool SftpTryAction(Action<SftpClient> sftpAction)
        {
            int retryCount = 5;
            
            while (retryCount != 0)
            {
                if(!_sftpClient.IsConnected)
                    _sftpClient.Connect();

                try
                {
                    sftpAction(_sftpClient);
                    return true;
                }
                catch (Exception e)
                {
                    if(retryCount != 1)
                        Console.WriteLine("Action failed, retrying {0} more time in a second ({1})", retryCount - 1, e.Message);
                    else
                        Console.WriteLine("Action failed ({0})", e.Message);
                    retryCount--;
                    Task.Delay(1000).Wait();
                }
            }

            return false;
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
                Console.WriteLine("Action failed ({0})", e.Message);
                return false;
            }
        }
        
        public static bool CreateFile(string relativePath)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.Create($"{SftpRootDirectory}/{relativePath}"));

            return LocalTryAction(() => File.Create(Path.Combine(EnvVars.DataPath, relativePath)));
        }

        public static bool DeleteFile(string relativePath)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.DeleteFile($"{SftpRootDirectory}/{relativePath}"));
            
            string localPath = Path.Combine(EnvVars.DataPath, relativePath);
            
            // Deleting a non existing file can return true so to keep it consistent with SFTP we return false 
            if (!File.Exists(localPath))
                return false;
                
            return LocalTryAction(() => File.Delete(localPath));
        }

        public static bool Read(string relativePath, out byte[] fileBytes)
        {
            byte[] bytes = null;

            bool worked;
            
            if (EnvVars.IsDevelopment)
                worked = SftpTryAction(client => bytes = client.ReadAllBytes($"{SftpRootDirectory}/{relativePath}"));
            else
                worked = LocalTryAction(() => bytes = File.ReadAllBytes(Path.Combine(EnvVars.DataPath, relativePath)));

            fileBytes = bytes;
            return worked;
        }
        
        public static bool Read(string relativePath, out string fileContents)
        {
            string contents = string.Empty;

            bool worked;
            
            if (EnvVars.IsDevelopment)
                worked = SftpTryAction(client => contents = client.ReadAllText($"{SftpRootDirectory}/{relativePath}"));
            else
                worked = LocalTryAction(() => contents = File.ReadAllText(Path.Combine(EnvVars.DataPath, relativePath)));

            fileContents = contents;
            return worked;
        }
        
        public static bool Read(string relativePath, out string[] fileLines)
        {
            string[] lines = null;

            bool worked;
            
            if (EnvVars.IsDevelopment)
                worked = SftpTryAction(client => lines = client.ReadAllLines($"{SftpRootDirectory}/{relativePath}"));
            else
                worked = LocalTryAction(() => lines = File.ReadAllLines(Path.Combine(EnvVars.DataPath, relativePath)));

            fileLines = lines;
            return worked;
        }

        public static bool Write(string relativePath, byte[] bytes)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.WriteAllBytes($"{SftpRootDirectory}/{relativePath}", bytes));

            return LocalTryAction(() => File.WriteAllBytes(Path.Combine(EnvVars.DataPath, relativePath), bytes));
        }
        
        public static bool Write(string relativePath, string contents)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.WriteAllText($"{SftpRootDirectory}/{relativePath}", contents));

            return LocalTryAction(() => File.WriteAllText(Path.Combine(EnvVars.DataPath, relativePath), contents));
        }
        
        public static bool Write(string relativePath, string[] lines)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.WriteAllLines($"{SftpRootDirectory}/{relativePath}", lines));

            return LocalTryAction(() => File.WriteAllLines(Path.Combine(EnvVars.DataPath, relativePath), lines));
        }
    }
}