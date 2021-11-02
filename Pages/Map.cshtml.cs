using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
// using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya


namespace RoboRecords.Pages
{
    public class Map : PageModel
    {
        public static RoboGame CurrentGame;
        public static RoboLevel CurrentLevel;
        private List<RoboGame> _roboGames;

        // Initiative to move all database interactions to one place. --- Zenya
        /*
        private RoboRecordsDbContext _dbContext;

        public Map(RoboRecordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }*/
        
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
            // TODO: Return only the current level so sorting is done by SQL server. Less data transfer required.
            _roboGames = DbSelector.GetAllGameData();

            var gameId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("game");
            var mapId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("map");

            if (gameId != null)
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
            }
        }
    }
}