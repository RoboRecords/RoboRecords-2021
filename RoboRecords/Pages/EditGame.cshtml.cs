using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class EditGame : RoboPageModel
    {
        [BindProperty]
        public GameEditDb GameEdit { get; set; }
        public static RoboGame Game;

        public EditGame(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            DbSelector.TryGetGamesWithLevels(out var roboGames);

            var id = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("id");

            if (id != null)
            {
                DbSelector.TryGetGameWithLevelsFromID(id, out Game);
            }
            else
            {
                // SRB2 2.2 default for testing. Should be changed to throw an error.
                DbSelector.TryGetGameWithLevelsFromID("sonicroboblast2v22", out Game);
            }
        }
    }
}