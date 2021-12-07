using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.DbInteraction;

namespace RoboRecords.Models
{
    public class RoboPageModel : PageModel
    {
        private IHttpContextAccessor _httpContextAccessor;
        
        public RoboPageModel(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        public bool IsLoggedIn => CurrentUser.UserNameNoDiscrim != "Invalid User";

        public bool isModerator = false;

        public RoboUser CurrentUser => DbSelector.GetRoboUserFromUserName(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
        
        public IdentityRoboUser CurrentIdentityUser => DbSelector.GetIdentityUserFromUserName(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name));
    }
}