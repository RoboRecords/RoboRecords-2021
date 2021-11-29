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
    public class Login : RoboPageModel
    {
        private RoboUserManager _roboUserManager;
        private SignInManager<IdentityRoboUser> _signInManager;
        private ApiKeyManager _apiKeyManager;

        public Login(RoboUserManager roboUserManager, SignInManager<IdentityRoboUser> signInManager, ApiKeyManager apiKeyManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _roboUserManager = roboUserManager;
            _signInManager = signInManager;
            _apiKeyManager = apiKeyManager;
        }

        public void OnGet()
        {
            // Improper example of checking if current user is a moderator. To test, manually change Roles column of your IdentityRoboUser entry to 3 and log in.
            if (IsLoggedIn)
            {
                isModerator = Validator.UserHasRequiredRoles(DbSelector.GetIdentityUserFromUserName($"{CurrentUser.UserNameNoDiscrim}#{CurrentUser.Discriminator}"), UserRoles.Moderator);
            }
        }

        //TODO: Give the API key to the user
        public void OnPostApiKey()
        {
            if(!IsLoggedIn)
                return;

            Console.WriteLine(_apiKeyManager.GenerateApiKeyForUser(CurrentIdentityUser));
            
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

            string[] splittedUsername = Validator.TrySplitUsername(usernamewithdiscrim);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            // TODO: Make a more elegant way to interupt if the discriminator is invalid
            if (discriminator == 0)
            {
                return;
            }

            Console.WriteLine(username);


            // TODO: Inform the user of the outcome.
            // TODO: Move this to RoboUserManager
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
            
            string[] splittedUsername = Validator.TrySplitUsername(usernamewithdiscrim);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            // TODO: Make a more elegant way to interupt if the discriminator is invalid
            // TODO: Actually allow email login
            if (discriminator == 0)
            {
                return;
            }

            // RoboUser userToLogin = DbSelector.GetRoboUserFromUserName(username, discriminator);

            // IdentityUser has discrim included in username
            IdentityRoboUser userToLogin = DbSelector.GetIdentityUserFromUserName(usernamewithdiscrim);

            SignInResult result = _signInManager.PasswordSignInAsync(userToLogin, password, true, false).Result;
            
            if (result.Succeeded)
            {
                Console.WriteLine("Success");
                isModerator = Validator.UserHasRequiredRoles(userToLogin, UserRoles.Moderator);
            }
                
            Response.Redirect("Login");
        }
    }
}