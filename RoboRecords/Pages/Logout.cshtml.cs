/*
 * Logout.cshtml: Backend for the logout page
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Pages;

public class Logout : RoboPageModel
{
    private RoboUserManager _roboUserManager;
    
    public Logout(RoboUserManager roboUserManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
    {
        _roboUserManager = roboUserManager;
    }
    
    public IActionResult OnGet()
    {
        string queryString = "https://" + HttpContext.Request.Host;

        if (Request.QueryString.HasValue && !string.IsNullOrWhiteSpace(Request.QueryString.Value))
        {
            queryString = Request.QueryString.Value;

            queryString = queryString.Substring(queryString.IndexOf('=') + 1);
        }

        _roboUserManager.SignOut().Wait();
        return Redirect(queryString);
    }
}