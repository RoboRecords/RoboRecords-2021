using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Pages
{
    public class Users : RoboPageModel
    {
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        
        private ApiKeyManager _apiKeyManager;
        
        public Users(ApiKeyManager apiKeyManager, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _apiKeyManager = apiKeyManager;
        }
        
        public void OnGet()
        {
            
        }

        public void OnPostAsync(IFormFile fileUpload)
        {
            if (fileUpload is null)
                return;

            if (!IsLoggedIn)
                return;

            RoboUser user = CurrentUser;

            string path = $"{FileManager.UserAssetsDirectoryName}/{user.DbId}/avatar.png";
            
            var stream = fileUpload.OpenReadStream();
            var bytes = new byte[fileUpload.Length];
            stream.Read(bytes, 0, (int)fileUpload.Length);

            if (!FileManager.Exists($"{FileManager.UserAssetsDirectoryName}/{user.DbId}"))
                FileManager.CreateDirectory($"{FileManager.UserAssetsDirectoryName}/{user.DbId}");
            
            FileManager.Write(path, bytes);
        }
        
        //TODO: Give the API key to the user
        public void OnPostApiKey()
        {
            if(!IsLoggedIn)
                return;

            Logger.Log(_apiKeyManager.GenerateApiKeyForUser(CurrentIdentityUser), Logger.LogLevel.Debug, true);
            
        }
    }
}