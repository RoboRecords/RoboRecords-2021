using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Html;

namespace RoboRecords.Pages
{
    public class GamePage : PageModel
    {
        private List<RoboGame> _roboGames;
        public static RoboGame CurrentGame;

        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");

            var gfz1 = new RoboLevel(1, "Green Flower Zone", 1);
            var gfz2 = new RoboLevel(2, "Green Flower Zone", 2);
            var gfz3 = new RoboLevel(3, "Green Flower Zone", 3);
            
            var thz1 = new RoboLevel(4, "Techno Hill Zone", 1);
            var thz2 = new RoboLevel(5, "Techno Hill Zone", 2);
            var thz3 = new RoboLevel(6, "Techno Hill Zone", 3);
            
            var dsz1 = new RoboLevel(7, "Techno Hill Zone", 1);
            var dsz2 = new RoboLevel(8, "Techno Hill Zone", 2);
            var dsz3 = new RoboLevel(9, "Techno Hill Zone", 3);
            
            var cez1 = new RoboLevel(10, "Techno Hill Zone", 1);
            var cez2 = new RoboLevel(11, "Techno Hill Zone", 2);
            var cez3 = new RoboLevel(12, "Techno Hill Zone", 3);
            
            var acz1 = new RoboLevel(13, "Techno Hill Zone", 1);
            var acz2 = new RoboLevel(14, "Techno Hill Zone", 2);
            var acz3 = new RoboLevel(15, "Techno Hill Zone", 3);
            
            var rvz = new RoboLevel(16, "Techno Hill Zone", 0);
            
            var erz1 = new RoboLevel(22, "Techno Hill Zone", 1);
            var erz2 = new RoboLevel(23, "Techno Hill Zone", 2);
            
            var bcz1 = new RoboLevel(25, "Techno Hill Zone", 1);
            var bcz2 = new RoboLevel(26, "Techno Hill Zone", 2);
            var bcz3 = new RoboLevel(27, "Techno Hill Zone", 3);
            
            var fhz = new RoboLevel(30, "Frozen Hillside Zone", 0);
            var ptz = new RoboLevel(31, "Pipe Towers Zone", 0);
            var ffz = new RoboLevel(32, "Forest Fortress Zone", 0);
            var tlz = new RoboLevel(33, "Techno Legacy Zone", 0);
            
            var hhz = new RoboLevel(40, "Haunted Heights Zone", 0);
            var agz = new RoboLevel(41, "Aerial Garden Zone", 0);
            var atz = new RoboLevel(42, "Azure Temple Zone", 0);
            
            var levelGroupGfz = new LevelGroup(name: "Green Flower Zone", new List<RoboLevel>{gfz1, gfz2, gfz3});
            var levelGroupThz = new LevelGroup(name: "Techno Hill Zone", new List<RoboLevel>{thz1, thz2, thz3});
            var levelGroupDsz = new LevelGroup(name: "Deep Sea Zone", new List<RoboLevel>{dsz1, dsz2, dsz3});
            var levelGroupCez = new LevelGroup(name: "Castle Eggman Zone", new List<RoboLevel>{cez1, cez2, cez3});
            var levelGroupAcz = new LevelGroup(name: "Arid Canyon Zone", new List<RoboLevel>{acz1, acz2, acz3});
            var levelGroupRvz = new LevelGroup(name: "Red Volcano Zone", new List<RoboLevel>{rvz});
            var levelGroupErz = new LevelGroup(name: "Egg Rock Zone", new List<RoboLevel>{erz1, erz2});
            var levelGroupBcz = new LevelGroup(name: "Black Core Zone", new List<RoboLevel>{bcz1, bcz2, bcz3});
            var levelGroupBonus = new LevelGroup(name: "Bonus Levels", new List<RoboLevel>{fhz, ptz, ffz, tlz}, true);
            var levelGroupChallenge = new LevelGroup(name: "Challenge Levels", new List<RoboLevel>{hhz, agz, atz}, true);

            var groups = new List<LevelGroup> {levelGroupGfz, levelGroupThz, levelGroupDsz, levelGroupCez, levelGroupAcz, levelGroupRvz, levelGroupErz, levelGroupBcz, levelGroupBonus, levelGroupChallenge};
            
            _roboGames = new List<RoboGame>();
            // Replace the next bit by actually getting the game from a database
            _roboGames.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){LevelGroups = groups, IconPath = "../assets/images/gfz2bg.png"});
            _roboGames.Add(new RoboGame("srb2 Cyberdime Realm"){IconPath = "../assets/images/cydmbg.png"});
            _roboGames.Add(new RoboGame("Sonic Robo Blast 3 LUL"){IconPath = "../assets/images/dreamhill1.png"});
            
            var id = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("id");

            if (id != null)
            {
                var roboGame = _roboGames.Find(game => game.IdName == id);
                if (roboGame != null)
                {
                    CurrentGame = roboGame;
                }
            }
        }
    }
}