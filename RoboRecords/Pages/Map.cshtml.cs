using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Web;
// using RoboRecords.DatabaseContexts;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya
using RoboRecords.Models;


namespace RoboRecords.Pages
{
    public class Map : RoboPageModel
    {
        public static RoboGame CurrentGame;
        public static RoboLevel CurrentLevel;
        // private List<RoboGame> _roboGames;

        // Initiative to move all database interactions to one place. --- Zenya
        
        //private RoboRecordsDbContext _dbContext;

        public Map(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");
            CurrentLevel = new RoboLevel(-1,"Invalid level", 0);

            //Get the data from the database
            //FIXME: This doesn't feel like the right way to do it, figure out how this works better
            /*_dbContext.RoboRecords.Include(e => e.Uploader).ToListAsync().Wait();
            _dbContext.RoboRecords.Include(e => e.Character).ToListAsync().Wait();
            _dbContext.RoboRecords.Include(e => e.Uploader).ToListAsync().Wait();
            _dbContext.LevelGroups.Include(e => e.Levels).ToListAsync().Wait();*/
            /*_roboGames = _dbContext.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Character)
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Uploader)
                .ToListAsync().Result;*/

            // Returns all games, including the levels and records.
            // DONE: Return only the current level so sorting is done by SQL server. Less data transfer required. --- Zenya
            // _roboGames = DbSelector.GetAllGameData();

            var gameId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("game");
            var mapId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("map");

            DbSelector.TryGetGameFromID(gameId, out CurrentGame);
            DbSelector.TryGetGameLevelFromMapId(gameId, mapId, out CurrentLevel);

            /*if (gameId != null)
            {
                var roboGame = _roboGames.Find(game => game.UrlName == gameId);
                // var roboGame = DbSelector.GetGameFromID(gameId);
                if (roboGame != null)
                {
                    CurrentGame = roboGame;
                    if (mapId != null)
                    {
                        int parsedLevel = -1;
                        if (int.TryParse(mapId, out parsedLevel))
                        {
                            var roboLevel = roboGame.GetLevelByNumber(parsedLevel);
                            if (roboLevel != null)
                            {
                                CurrentLevel = roboLevel;
                            }
                        }
                    }
                }
            }*/
        }
    }
}