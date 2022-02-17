using Microsoft.AspNetCore.Identity;
using System;

namespace RoboRecords.Models
{
    public class IdentityRoboUser : IdentityUser
    {
        public string ApiKey = string.Empty;
        public int Roles;

        public IdentityRoboUser(string userName) : base(userName)
        {
        }

        public IdentityRoboUser()
        {
        }

        public static bool operator ==(IdentityRoboUser user1, IdentityRoboUser user2)
        {
            if (user1 is null && user2 is not null || user1 is not null && user2 is null)
                return false;
            
            return user1 is null || user1.NormalizedUserName == user2.NormalizedUserName;
        }

        public static bool operator !=(IdentityRoboUser user1, IdentityRoboUser user2) => !(user1 == user2);
    }
}