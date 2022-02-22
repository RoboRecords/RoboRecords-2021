using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Games : RoboPageModel
    {
        public static List<RoboGame> RoboGames;

        public Games(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            RoboGames = new List<RoboGame>();

            DbSelector.TryGetGames(out RoboGames);
        }
    }
}