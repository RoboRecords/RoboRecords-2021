using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RoboRecords.Services
{
    public class Validator
    {
        public static string[] TrySplitUsername(string usernamewithdiscrim)
        {
            string expected = @"(.+)#(\d{4})";

            Regex rguser = new Regex(expected);

            Match match = rguser.Match(usernamewithdiscrim);
            if (match.Success)
            {
                return new string[] { match.Groups[1].Value, match.Groups[2].Value };
            }
            else
            {
                // TODO: Randomly generate a discriminator if not specified.
                return new string[] { usernamewithdiscrim, "0000" };
            }

        }
    }
}
