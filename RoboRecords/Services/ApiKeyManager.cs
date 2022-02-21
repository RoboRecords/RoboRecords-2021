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
        
        public bool TryAuthenticateFromApiKey(string apiKey, out RoboUser roboUser, out IdentityRoboUser identityUser)
        {
            string hashedKey = HashApiKey(apiKey);

            (roboUser, identityUser, bool success) = GetRoboUser(hashedKey);

            return success;
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
            {
                DbSelector.TryGetRoboUserFromApiKey(newApiKey, out RoboUser roboUser);
                _apiKeyCache.AddKey(newApiKey, roboUser, user);
            }

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

        private (RoboUser user, IdentityRoboUser identityUser, bool success) GetRoboUser(string apiKey)
        {
            if (_apiKeyCache.ContainsKey(apiKey))
            {
                (RoboUser cachedUser, IdentityRoboUser cachedIdentityUser) = _apiKeyCache.GetRoboUser(apiKey);
                return (cachedUser, cachedIdentityUser, true);
            }

            IdentityRoboUser identityUser = null;
            bool foundUser = DbSelector.TryGetRoboUserFromApiKey(apiKey, out RoboUser user);
            bool foundIdentityUser = false;

            
            if (foundUser)
            {
                foundIdentityUser = DbSelector.TryGetIdentityUserFromUserName(user.UserNameNoDiscrim, user.Discriminator, out identityUser);
                
                if (foundIdentityUser)
                    _apiKeyCache.AddKey(apiKey, user, identityUser);
            }
            
            return (user, identityUser, foundIdentityUser);
        }
    }

    public class ApiKeyCache
    {
        private const int ApiKeyCacheCapacity = 20;

        private OrderedDictionary _cache = new OrderedDictionary(ApiKeyCacheCapacity);
        
        public bool ContainsKey(string apiKey) => _cache.Contains(apiKey);

        public (RoboUser user, IdentityRoboUser identityUser) GetRoboUser(string apiKey)
        {
            (RoboUser user, IdentityRoboUser identityUser) = ((RoboUser, IdentityRoboUser))_cache[apiKey];
            
            _cache.Remove(apiKey);
            _cache.Add(apiKey, (user, identityUser));
            return (user, identityUser);
        }

        public void AddKey(string apiKey, RoboUser user, IdentityRoboUser identityUser)
        {
            if (_cache.Keys.Count == ApiKeyCacheCapacity)
                _cache.RemoveAt(0);

            _cache.Add(apiKey, (user, identityUser));
        }

        public void UpdateKey(string oldApiKey, string newApiKey)
        {
            (RoboUser user, IdentityRoboUser identityUser) = ((RoboUser, IdentityRoboUser))_cache[oldApiKey];
            
            _cache.Remove(oldApiKey);
            _cache.Add(newApiKey, (user, identityUser));
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