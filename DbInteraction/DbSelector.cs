using System.Collections.Generic;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Linq;

namespace RoboRecords.DbInteraction
{
    public class DbSelector
    {
        public static List<RoboGame> GetAllGameData()
        {
            List<RoboGame> _roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, JOIN all foreign keys
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGames = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Character)
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Uploader)
                .ToListAsync().Result;
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (_roboGames.Count > 0)
                foreach (var roboGame in _roboGames)
                {
                    if (roboGame.LevelGroups.Count > 0)
                        foreach (var levelGroup in roboGame.LevelGroups)
                        {
                            List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                            levelGroup.Levels = sortedList;
                        }
                }

            return _roboGames;
        }

        public static List<RoboGame> GetGames()
        {
            List<RoboGame> _roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, no JOINs. Used for Games page.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGames = context.RoboGames
                .ToListAsync().Result;
            }

            return _roboGames;
        }

        public static List<RoboGame> GetGamesWithLevels()
        {
            List<RoboGame> _roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, JOIN all foreign keys
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGames = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ToListAsync().Result;
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (_roboGames.Count > 0)
                foreach (var roboGame in _roboGames)
                {
                    if (roboGame.LevelGroups.Count > 0)
                        foreach (var levelGroup in roboGame.LevelGroups)
                        {
                            List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                            levelGroup.Levels = sortedList;
                        }
                }

            return _roboGames;
        }
    }
}
