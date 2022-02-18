using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RoboRecords.Models;

namespace RoboRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticationController : Controller
    {
        private SignInManager<IdentityRoboUser> _signInManager;

        public AuthenticationController(SignInManager<IdentityRoboUser> signInManager)
        {
            _signInManager = signInManager;
        }
    }
}