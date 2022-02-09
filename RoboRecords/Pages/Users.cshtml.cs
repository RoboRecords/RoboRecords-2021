using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Users : RoboPageModel
    {
        public Users(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            
        }
    }
}