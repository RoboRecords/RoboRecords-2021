using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Web;
using Microsoft.AspNetCore.Antiforgery;
using RoboRecords.DatabaseContexts;
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
        public class GameData
        {
            public string Name { get; set; }
            public string UrlName { get; set; }
            public List<LevelGroup> Groups { get; set; }
        }
        
        public IActionResult OnPostSaveAsync([FromBody] GameData data)
        {
            // TODO: Compare the data to existing data, and replace what needs to be replaced in order to edit the game
            
            try
            {
                string oldUrl = Game.UrlName;
                Game.Name = data.Name;
                Game.UrlName = data.UrlName;
                List<LevelGroup> groups = new List<LevelGroup>();
                foreach (LevelGroup levelGroup in data.Groups)
                {
                    LevelGroup newGroup = new LevelGroup();
                    newGroup.Name = levelGroup.Name;
                    newGroup.WriteLevelNames = levelGroup.WriteLevelNames;
                    newGroup.Levels = new List<RoboLevel>();
                    foreach (RoboLevel level in levelGroup.Levels)
                    {
                        RoboLevel gameLevel = Game.GetLevelByNumber(level.LevelNumber);
                        RoboLevel newLevel;
                        if (gameLevel is not null)
                        {
                            gameLevel.LevelName = level.LevelName;
                            gameLevel.Act = level.Act;
                            gameLevel.Nights = level.Nights;
                            gameLevel.IconUrl = level.IconUrl;
                            newLevel = gameLevel;
                        }
                        else
                        {
                            newLevel = level;
                        }
                        newGroup.Levels.Add(newLevel);
                    }
                    groups.Add(levelGroup);
                }

                Game.LevelGroups = groups;

            }
            catch (Exception e)
            {
                Logger.Log("Error while trying to save a game: " + e.Message, Logger.LogLevel.Error);
                throw;
            }
            
            return Page();
        }
    }
}