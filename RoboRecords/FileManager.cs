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
        
        // The first of many others to be implemented :p
        public static bool Create(string relativePath)
        {
            if (EnvVars.IsDevelopment)
                return SftpTryAction(client => client.Create($"{SftpRootDirectory}/{relativePath}"));

            File.Create(Path.Combine(EnvVars.DataPath, relativePath));
            return true;
        }
    }
}