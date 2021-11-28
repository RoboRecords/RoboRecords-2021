using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Services
{
    public class ApiKeyManager
    {
        private static readonly char[] chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_".ToCharArray();

        private const int ApiKeyStringSize = 32;
        
        //TODO: Cache some amount of API keys to avoid having to go through them all the time
        public bool TryAuthenticateFromApiKey(string apiKey, out RoboUser roboUser)
        {
            string hashedKey = HashApiKey(apiKey);

            roboUser = DbSelector.GetRoboUserFromApiKey(hashedKey);

            return roboUser.UserNameNoDiscrim != "Invalid User";
        }
        
        public string GenerateApiKeyForUser(IdentityRoboUser user)
        {
            StringBuilder apiKeyBuilder = new StringBuilder(ApiKeyStringSize);

            for (int i = 0; i < ApiKeyStringSize; i++)
            {
                int index = RandomNumberGenerator.GetInt32(chars.Length);
                apiKeyBuilder.Append(chars[index]);
            }

            string apiKey = apiKeyBuilder.ToString();

            user.ApiKey = HashApiKey(apiKey);

            DbUpdater.UpdateIdentityUser(user);
            
            return apiKey;
        }

        private string HashApiKey(string apiKey)
        {
            string hash = "";
            
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.Default.GetBytes(apiKey));
                foreach (byte hashByte in hashBytes)
                {
                    hash += hashByte.ToString("x2");
                }
            }
            
            return hash;
        }
    }
}