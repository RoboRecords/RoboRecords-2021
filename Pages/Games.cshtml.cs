using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using System.Collections.Generic;

namespace RoboRecords.Pages
{
    public class Games : PageModel
    {
        public static List<RoboGame> RoboGames;
        
        public void OnGet()
        {
            RoboGames = new List<RoboGame>();
            
            // Replace the next bit by actually getting the game from a database
            RoboGames.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){IconPath = "../assets/images/gfz2bg.png", Levels = new List<RoboLevel>()});
            RoboGames.Add(new RoboGame("srb2 Cyberdime Realm"){IconPath = "../assets/images/cydmbg.png", Levels = new List<RoboLevel>()});
            RoboGames.Add(new RoboGame("Sonic Robo Blast 3 LUL"){IconPath = "../assets/images/dreamhill1.png", Levels = new List<RoboLevel>()});
        }
    }
}