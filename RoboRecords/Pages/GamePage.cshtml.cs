using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RoboRecords.Models;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Web;
using Microsoft.AspNetCore.Html;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya

namespace RoboRecords.Pages
{
    public class GamePage : PageModel
    {
        // private List<RoboGame> _roboGames;
        public static RoboGame CurrentGame;

        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");

            // _roboGames = new List<RoboGame>();
            // Replace the next bit by actually getting the game from a database --- DONE, Zenya
            //_roboGames.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){LevelGroups = groups, IconPath = "../assets/images/gfz2bg.png"});
            //_roboGames.Add(new RoboGame("srb2 Cyberdime Realm"){IconPath = "../assets/images/cydmbg.png"});
            //_roboGames.Add(new RoboGame("Sonic Robo Blast 3 LUL"){IconPath = "../assets/images/dreamhill1.png"});

            // Returns all games, including the levels and records.
            // DONE: Return only the current game so sorting is done by SQL server. Less data transfer required. --- Zenya

            // _roboGames = DbSelector.GetAllGameData();

            var id = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("id");

            if (id != null)
            {
                CurrentGame = DbSelector.GetGameWithRecordsFromID(id);
            }
            else
            {
                // SRB2 2.2 default for testing. Should be changed to throw an error if not found.
                CurrentGame = DbSelector.GetGameWithRecordsFromID("sonicroboblast2v22");
            }
        }
    }
}