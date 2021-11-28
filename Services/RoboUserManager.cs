/*
 * RoboUserManager.cs: APIs to handle database user management and retrieval
 * Copyright (C) 2021, Refrag <R3FR4G>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RoboRecords.Models;

namespace RoboRecords.Services
{
    public class RoboUserManager
    {
        private SignInManager<IdentityRoboUser> _signInManager;
        private UserManager<IdentityRoboUser> _userManager;
        
        public RoboUserManager(UserManager<IdentityRoboUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityResult Create(string email, string userName, short discriminator, string password)
        {
            // return _userManager.CreateAsync(new RoboUser(email, userName, discriminator), password).Result;
            return _userManager.CreateAsync(new IdentityRoboUser()
            {
                UserName = $"{userName}#{discriminator}",
                Email = email,
            }
            , password).Result;
        }
    }
}