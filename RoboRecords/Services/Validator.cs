/*
 * Validator.cs
 * Copyright (C) 2021, Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using RoboRecords.Models;
using RoboRecords.DbInteraction;
using System.Net.Mail;

namespace RoboRecords.Services
{
    [Flags]
    public enum UserRoles : int
    {
        None = 0,
        User = 1,
        Moderator = 2,
        Admin = 4,
    }
    public class Validator
    {

        public static string[] TrySplitUsername(string usernamewithdiscrim)
        {
            string expected = @"(.+)#(\d{4})";

            Regex rguser = new Regex(expected);

            if (usernamewithdiscrim == null || usernamewithdiscrim == "")
                return new string[] { "Invalid User", "0000" };

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

        public static bool UserHasRequiredRoles(IdentityRoboUser user, UserRoles roles)
        {
            if (user.Roles == 0)
                return false; // User is banned

            else if ((user.Roles & (int)roles) == (int)roles) // User has the required roles
                return true;

            return false; // User doesn't have the required roles
        }

        public static void GrantRolesToUser(IdentityRoboUser user, UserRoles roles)
        {
            user.Roles = user.Roles | (int)roles;
            DbUpdater.UpdateIdentityUser(user);
        }

        public static void RevokeRolesFromUser(IdentityRoboUser user, UserRoles roles)
        {
            user.Roles = user.Roles & (int.MaxValue ^ (int)roles);
            DbUpdater.UpdateIdentityUser(user);
        }

        public static bool ValidateEmail(string email)
        {
            try
            {
                MailAddress m = new MailAddress(email);

                return true;
            }
            catch(FormatException)
            {
                return false;
            }
        }
    }
}
