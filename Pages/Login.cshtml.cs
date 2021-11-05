using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Services;

namespace RoboRecords.Pages
{
    public class Login : PageModel
    {
        private RoboUserManager _roboUserManager;
        
        public Login(RoboUserManager roboUserManager)
        {
            _roboUserManager = roboUserManager;
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

            Console.WriteLine(string.Join(", ", email, usernamewithdiscrim, password, confirmedPassword));

            string[] splittedUsername = usernamewithdiscrim.Split('#');

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            Console.WriteLine(username);
            
            Console.WriteLine(_roboUserManager.Create(email, username, discriminator, password).Succeeded);
        }
        
        //FIXME: Don't require a page refresh for this action to happen?
        public void OnPostLogin()
        {
            string email = Request.Form["loginInputEmail"];
            string password = Request.Form["loginInputPassword"];
            
            Console.WriteLine(string.Join(", ", email, password));
        }
    }
}