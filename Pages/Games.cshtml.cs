using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using System.Collections.Generic;

namespace RoboRecords.Pages
{
    public class Games : PageModel
    {
        private List<RoboGame> _games;
        
        private void OrganizeGames()
        {
            
            foreach (var game in _games)
            {
                
            }
        }
        public void OnGet()
        {
            
            _games = new List<RoboGame>();
            _games.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){Icon = "../assets/images/gfz2bg.png", Levels = new List<RoboLevel>()});
            _games.Add(new RoboGame("srb2 Cyberdime Realm"){Icon = "../assets/images/cydmbg.png", Levels = new List<RoboLevel>()});
            _games.Add(new RoboGame("Sonic Robo Blast 3 LUL"){Icon = "../assets/images/dreamhill1.png", Levels = new List<RoboLevel>()});
            
            OrganizeGames();
        }
    }
}