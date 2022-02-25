/*
 * AuthenticationController.cs: Authentication Web API Endpoints implementation 
 * Copyright (C) 2022, Refrag <Refragg> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboRecords.Models;

namespace RoboRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private SignInManager<IdentityRoboUser> _signInManager;

        public AuthenticationController(SignInManager<IdentityRoboUser> signInManager)
        {
            _signInManager = signInManager;
        }
    }
}