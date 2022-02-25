/*
 * IdentityRoboUser.cs: the identity user model as stored in the identity database
 * Copyright (C) 2022, Refrag <Refragg> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

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