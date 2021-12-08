using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DbInteraction;
using RoboRecords.Models;
using System.Web;

namespace RoboRecords.Pages
{
    public class EditGame : PageModel
    {
        [BindProperty]
        public GameEditDb GameEdit { get; set; }
        public static RoboGame Game;
        
        public void OnGet()
        {
            DbSelector.TryGetGamesWithLevels(out var roboGames);
            // Game = roboGames[1]; // DONE: CHANGE THIS HARDCODED CRAP TO SUPPORT THE CURRENT SELECTED GAME --- Zenya

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