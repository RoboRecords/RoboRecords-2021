using System;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Pages
{
    public class Users : RoboPageModel
    {
        private ApiKeyManager _apiKeyManager;

        private IAntiforgery _antiforgery;

        public string Token;
        
        public Users(IAntiforgery antiforgery, ApiKeyManager apiKeyManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _antiforgery = antiforgery;
            _apiKeyManager = apiKeyManager;
        }
        
        public void OnGet()
        {
            Token = _antiforgery.GetTokens(HttpContext).RequestToken;
        }

        public IActionResult OnPostAvatar()
        {
            if (Request.Form?.Files?.Count == 0)
                return BadRequest("No file attached");

            if (!IsLoggedIn)
                return BadRequest("Not logged in");

            IFormFile fileUpload = Request.Form.Files[0];

            RoboUser user = CurrentUser;

            string path = $"{FileManager.UserAssetsDirectoryName}/{user.DbId}/avatar.png";
            
            var stream = fileUpload.OpenReadStream();
            var bytes = new byte[fileUpload.Length];
            stream.Read(bytes, 0, (int)fileUpload.Length);

            if (!FileManager.Exists($"{FileManager.UserAssetsDirectoryName}/{user.DbId}"))
                FileManager.CreateDirectory($"{FileManager.UserAssetsDirectoryName}/{user.DbId}");
            
            FileManager.Write(path, bytes);
            return Content("Success!");
        }
        
        public IActionResult OnPostApiKey()
        {
            if(!IsLoggedIn)
                return BadRequest("There is no user logged in");

            string apiKey = _apiKeyManager.GenerateApiKeyForUser(CurrentIdentityUser);
            
            Logger.Log(apiKey, Logger.LogLevel.Debug, true);


            return Content(apiKey);
        }
    }
}