using System;
using System.Collections;
using System.Collections.Specialized;
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
        
        private ApiKeyCache _apiKeyCache = new ApiKeyCache();
        
        public bool TryAuthenticateFromApiKey(string apiKey, out RoboUser roboUser)
        {
            string hashedKey = HashApiKey(apiKey);

            roboUser = GetRoboUser(hashedKey);

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

            string oldApiKey = user.ApiKey;
            string newApiKey = HashApiKey(apiKey);

            user.ApiKey = newApiKey;

            DbUpdater.UpdateIdentityUser(user);
            
            if (!string.IsNullOrEmpty(oldApiKey) && _apiKeyCache.ContainsKey(oldApiKey))
                _apiKeyCache.UpdateKey(oldApiKey, newApiKey);
            else
                _apiKeyCache.AddKey(newApiKey, DbSelector.GetRoboUserFromApiKey(newApiKey));

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

        private RoboUser GetRoboUser(string apiKey)
        {
            if (_apiKeyCache.ContainsKey(apiKey))
                return _apiKeyCache.GetRoboUser(apiKey);

            RoboUser user = DbSelector.GetRoboUserFromApiKey(apiKey);
            
            if (user.UserNameNoDiscrim != "Invalid User")
                _apiKeyCache.AddKey(apiKey, user);
            
            return user;
        }
    }

    public class ApiKeyCache
    {
        private const int ApiKeyCacheCapacity = 20;

        private OrderedDictionary _cache = new OrderedDictionary(ApiKeyCacheCapacity);
        
        public bool ContainsKey(string apiKey) => _cache.Contains(apiKey);

        public RoboUser GetRoboUser(string apiKey)
        {
            RoboUser user = (RoboUser)_cache[apiKey];
            
            _cache.Remove(apiKey);
            _cache.Add(apiKey, user);
            return user;
        }

        public void AddKey(string apiKey, RoboUser user)
        {
            if (_cache.Keys.Count == ApiKeyCacheCapacity)
                _cache.RemoveAt(0);

            _cache.Add(apiKey, user);
        }

        public void UpdateKey(string oldApiKey, string newApiKey)
        {
            RoboUser user = (RoboUser)_cache[oldApiKey];
            
            _cache.Remove(oldApiKey);
            _cache.Add(newApiKey, user);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (DictionaryEntry o in _cache)
            {
                RoboUser user = (RoboUser)o.Value;
                
                sb.AppendLine($"\t-{o.Key} : {user.UserNameNoDiscrim}#{user.Discriminator}");
            }

            return sb.ToString();
        }
    }
}