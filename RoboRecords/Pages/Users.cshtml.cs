using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Users : RoboPageModel
    {
        [BindProperty]
        public IFormFile FileUpload { get; set; }
        
        public Users(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            
        }

        public void OnPostAsync(IFormFile fileUpload)
        {
            if (!IsLoggedIn)
                return;

            RoboUser user = CurrentUser;

            string path = $"UserAssets/{user.DbId}/avatar.png";
            
            var stream = fileUpload.OpenReadStream();
            var bytes = new byte[fileUpload.Length];
            stream.Read(bytes, 0, (int)fileUpload.Length);

            if (!FileManager.Exists($"UserAssets/{user.DbId}"))
                FileManager.CreateDirectory($"UserAssets/{user.DbId}");
            
            FileManager.Write(path, bytes);
        }
    }
}