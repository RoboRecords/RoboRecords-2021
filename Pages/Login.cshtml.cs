using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.DbInteraction;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Pages
{
    public class Login : PageModel
    {
        private RoboUserManager _roboUserManager;
        private SignInManager<IdentityUser> _signInManager;
        private IHttpContextAccessor _httpContextAccessor;

        // Fields to be accessed by the frontend part
        public bool IsLogged;
        public string UserName = string.Empty;
        // ==========================================

        public Login(RoboUserManager roboUserManager, SignInManager<IdentityUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _roboUserManager = roboUserManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;

            string? tempName = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
            if (tempName != null)
            {
                IsLogged = true;
                UserName = tempName;
            }
        }

        public void OnGet()
        {
            
        }

        //FIXME: Don't require a page refresh for this action to happen?
        public void OnPostRegister()
        {
            string email = Request.Form["registerInputEmail"];
            string usernamewithdiscrim = Request.Form["registerInputUsername"];
            string password = Request.Form["registerInputPassword"];
            string confirmedPassword = Request.Form["registerInputConfirmPassword"];

            if (password != confirmedPassword)
            {
                Console.WriteLine("not the same passwords");
                return;
            }

            string[] splittedUsername = usernamewithdiscrim.Split('#');

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            Console.WriteLine(username);

            // Try to create new IdentityUser and if it succeeds, create a RoboUser with the same username.
            if (_roboUserManager.Create(email, username, discriminator, password).Succeeded)
            {
                DbInserter.AddRoboUser(new RoboUser(username, discriminator));
            }
            // Console.WriteLine(_roboUserManager.Create(email, username, discriminator, password).Succeeded);
        }
        
        //FIXME: Don't require a page refresh for this action to happen?
        public void OnPostLogin()
        {
            string usernamewithdiscrim = Request.Form["loginInputUsername"];
            string password = Request.Form["loginInputPassword"];
            
            string[] splittedUsername = usernamewithdiscrim.Split('#');

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            // RoboUser userToLogin = DbSelector.GetRoboUserFromUserName(username, discriminator);

            // IdentityUser has discrim included in username
            IdentityUser userToLogin = DbSelector.GetIdentityUserFromUserName(usernamewithdiscrim);

            SignInResult result = _signInManager.PasswordSignInAsync(userToLogin, password, true, false).Result;
            
            if (result.Succeeded)
                Console.WriteLine("Success");
            Response.Redirect("Login");
        }
    }
}