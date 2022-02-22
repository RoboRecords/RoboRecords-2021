using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.DbInteraction;
using RoboRecords.Models;
using RoboRecords.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace RoboRecords.Pages
{
    public class Login : RoboPageModel
    {
        private IAntiforgery _antiforgery;
        private RoboUserManager _roboUserManager;
        private SignInManager<IdentityRoboUser> _signInManager;
        
        public string Token;

        public Login(IAntiforgery antiforgery, RoboUserManager roboUserManager, SignInManager<IdentityRoboUser> signInManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _antiforgery = antiforgery;
            _roboUserManager = roboUserManager;
            _signInManager = signInManager;
        }

        public void OnGet()
        {
            Token = _antiforgery.GetTokens(HttpContext).RequestToken;
            
            // Improper example of checking if current user is a moderator. To test, manually change Roles column of your IdentityRoboUser entry to 3 and log in.
            if (IsLoggedIn)
            {
                isModerator = Validator.UserHasRequiredRoles(CurrentIdentityUser, UserRoles.Moderator);
            }
        }

        public class RegisterData
        {
            public string Email { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string PasswordConfirmation { get; set; }
        }

        public IActionResult OnPostRegister([FromBody] RegisterData data)
        {
            string email = data.Email;
            string usernamewithdiscrim = data.Username;
            string password = data.Password;
            string confirmedPassword = data.PasswordConfirmation;

            if (password != confirmedPassword)
            {
                return BadRequest("Password Confirmation Error");
            }

            string[] splittedUsername = Validator.TrySplitUsername(usernamewithdiscrim);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);
            
            if (discriminator == 0)
            {
                return BadRequest("No Discriminator");
            }

            Logger.Log(username, Logger.LogLevel.Debug, true);
            
            // TODO: Move this to RoboUserManager
            // Try to create new IdentityUser and if it succeeds, create a RoboUser with the same username.
            IdentityResult userCreationResult = _roboUserManager.Create(email, username, discriminator, password);
            if (userCreationResult.Succeeded)
            {
                DbInserter.AddRoboUser(new RoboUser(username, discriminator));
                return Content("Success");
            }

            return BadRequest(userCreationResult.Errors);
        }
        
        public class LoginData
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }
        
        public IActionResult OnPostLogin([FromBody] LoginData data)
        {
            string usernamewithdiscrim = data.Email;
            string password = data.Password;
            
            string[] splittedUsername = Validator.TrySplitUsername(usernamewithdiscrim);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            // TODO: Actually allow email login
            if (discriminator == 0)
            {
                return BadRequest("No Discriminator");
            }

            // RoboUser userToLogin = DbSelector.GetRoboUserFromUserName(username, discriminator);

            // IdentityUser has discrim included in username
            DbSelector.TryGetIdentityUserFromUserName(usernamewithdiscrim, out IdentityRoboUser userToLogin);

            if (userToLogin is null)
                return BadRequest("No user with this username / discriminator combination was found");

            SignInResult result = _signInManager.PasswordSignInAsync(userToLogin, password, true, false).Result;
            
            if (result.Succeeded)
            {
                Logger.Log("Success", true);
                isModerator = Validator.UserHasRequiredRoles(userToLogin, UserRoles.Moderator);
                return Content("Success");
            }
            
            return BadRequest("The password for this user is invalid");
        }
    }
}