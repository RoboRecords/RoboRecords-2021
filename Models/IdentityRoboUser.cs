using Microsoft.AspNetCore.Identity;

namespace RoboRecords.Models
{
    public class IdentityRoboUser : IdentityUser
    {
        public string ApiKey = string.Empty;

        public IdentityRoboUser(string userName) : base(userName)
        {
        }

        public IdentityRoboUser()
        {
        }
    }
}