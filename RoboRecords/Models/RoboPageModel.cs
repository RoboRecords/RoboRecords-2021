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

            // Trigger the getter to set _foundUserLastResult
            _ = CurrentUser;
            _ = CurrentIdentityUser;
        }
        
        public bool IsLoggedIn => _foundUserLastResult;

        public bool isModerator = false;

        public RoboUser CurrentUser
        {
            get
            {
                RoboUser roboUser;
                _foundUserLastResult = DbSelector.TryGetRoboUserFromUserName(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name), out roboUser);
                return roboUser;
            }
        }
        
        private bool _foundUserLastResult;
        
        public IdentityRoboUser CurrentIdentityUser
        {
            get
            {
                IdentityRoboUser identityRoboUser;
                _foundIdentityUserLastResult = DbSelector.TryGetIdentityUserFromUserName(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name), out identityRoboUser);
                return identityRoboUser;
            }
        }
        
        private bool _foundIdentityUserLastResult;
    }
}