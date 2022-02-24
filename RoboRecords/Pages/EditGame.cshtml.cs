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

        class InterpretedLevel
        {
            public string LevelName{ get; set; }
            public string Act{ get; set; }
            public string LevelNumber{ get; set; }
            public bool Nights{ get; set; }
            public string IconUrl{ get; set; }
        }
        class InterpretedGroup
        {
            public string Name{ get; set; }
            public bool WriteLevelNames{ get; set; }
            public List<InterpretedLevel> Levels{ get; set; }
        }
        public class GameData
        {
            public string Name { get; set; }
            public string UrlName { get; set; }
            public string IconPath { get; set; }
            public string GroupsJson { get; set; }
        }
        
        public IActionResult OnPostSaveAsync([FromBody] GameData data)
        {
            // TODO: Compare the data to existing data, and replace what needs to be replaced in order to edit the game
            
            try
            {
                RoboGame game = new RoboGame(data.Name);
                game.Name = data.Name;
                game.UrlName = data.UrlName;
                game.IconPath = data.IconPath;

                List<InterpretedGroup> jsonGroups = System.Text.Json.JsonSerializer.Deserialize<List<InterpretedGroup>>(data.GroupsJson);
                
                Logger.Log(game.Name + ", " + game.UrlName);
                List<LevelGroup> groups = new List<LevelGroup>();
                foreach (InterpretedGroup levelGroup in jsonGroups)
                {
                    LevelGroup newGroup = new LevelGroup();
                    newGroup.Name = levelGroup.Name;
                    newGroup.WriteLevelNames = levelGroup.WriteLevelNames;
                    Logger.Log("Write names: " + levelGroup.WriteLevelNames + ", Name: " + levelGroup.Name + ", " + levelGroup.Levels.Count);
                    newGroup.Levels = new List<RoboLevel>();
                    foreach (InterpretedLevel level in levelGroup.Levels)
                    {
                        Logger.Log("Url: " + level.IconUrl + ", Act: " + level.Act + ", Nights: " + level.Nights + ", Name: " + level.LevelName);
                        RoboLevel gameLevel = game.GetLevelByNumber(Int32.Parse(level.LevelNumber) );
                        RoboLevel newLevel;
                        if (gameLevel is not null)
                        {
                            gameLevel.LevelName = level.LevelName;
                            gameLevel.Act = Int32.Parse(level.Act);
                            gameLevel.Nights = level.Nights;
                            gameLevel.IconUrl = level.IconUrl;
                            gameLevel.LevelGroup = newGroup;
                            newLevel = gameLevel;
                        }
                        else
                        {
                            newLevel = new RoboLevel(Int32.Parse(level.LevelNumber), level.LevelName, Int32.Parse(level.Act), level.Nights);
                            newLevel.LevelGroup = newGroup;
                        }
                        newGroup.Levels.Add(newLevel);
                    }

                    newGroup.RoboGame = game;
                    groups.Add(newGroup);
                }
                game.LevelGroups.Clear();
                game.LevelGroups = groups;
                using (RoboRecordsDbContext context = new RoboRecordsDbContext())
                {
                    context.RoboGames.Remove(Game);
                    
                    Logger.Log("Updating game...");
                    context.RoboGames.Add(game);

                    Logger.Log("Saving...");
                    context.SaveChangesAsync().Wait();
                }
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