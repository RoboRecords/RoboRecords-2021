using System.Collections.Generic;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Antiforgery;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class EditGame : RoboPageModel
    {
        [BindProperty]
        public GameEditDb GameEdit { get; set; }
        public static RoboGame Game;
        public string Token;
        
        private IAntiforgery _antiforgery;
        public EditGame(IAntiforgery antiforgery, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _antiforgery = antiforgery;
        }
        
        public void OnGet()
        {
            Token = _antiforgery.GetTokens(HttpContext).RequestToken;
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
        public class GameData
        {
            public string Name { get; set; }
            public string UrlName { get; set; }
            public string Groups { get; set; }
        }

        class InterpretedLevel
        {
            public string levelName;
            public int act;
            public int number;
            public bool nights;
            public string iconPath;
        }
        
        class InterpretedGroup
        {
            public string groupName{ get; set; }
            public bool writeNames{ get; set; }
            public InterpretedLevel[] levels{ get; set; }
        }
        
        public IActionResult OnPostSaveAsync([FromBody] GameData data)
        {
            Logger.Log(data.Groups, true);
            List<InterpretedGroup> groups =
                System.Text.Json.JsonSerializer.Deserialize<List<InterpretedGroup>>(data.Groups);
            
            Logger.Log("Length" + groups.Count, true);
            
            // TODO: Compare the data to existing data, and replace what needs to be replaced in order to edit the game
            
            return Page();
        }
    }
}