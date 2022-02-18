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