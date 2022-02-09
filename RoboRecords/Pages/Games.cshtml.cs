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

            // Replace the next bit by actually getting the game from a database
            /*RoboGames.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){IconPath = "../assets/images/gfz2bg.png"});
            RoboGames.Add(new RoboGame("srb2 Cyberdime Realm"){IconPath = "../assets/images/cydmbg.png"});
            RoboGames.Add(new RoboGame("Sonic Robo Blast 3 LUL"){IconPath = "../assets/images/dreamhill1.png"});*/

            // Returns all games, including the levels and records.
            // DONE: Return only the games without joined data. We don't need it here. --- Zenya
            DbSelector.TryGetGames(out RoboGames);
        }
    }
}