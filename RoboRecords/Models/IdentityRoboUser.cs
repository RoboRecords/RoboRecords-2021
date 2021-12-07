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
    }
}