using System.Collections.Generic;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RoboRecords.DatabaseContexts;
using Microsoft.AspNetCore.Identity;
using RoboRecords.Models;
using System.Linq;
using RoboRecords.Services;

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

        // Get the game matching the url and include level data.
        public static RoboGame GetGameWithLevelsFromID(string id)
        {
            RoboGame _roboGame;

            // SELECT * FROM RoboGames, JOIN levels
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGame = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .Where(e => e.UrlName == id)
                .FirstOrDefault();
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (_roboGame != null)
            {
                if (_roboGame.LevelGroups.Count > 0)
                    foreach (var levelGroup in _roboGame.LevelGroups)
                    {
                        List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                        levelGroup.Levels = sortedList;
                    }
                return _roboGame;
            }
            else
            {
                return new RoboGame("Invalid Game");
            }
        }

        // Get the game matching the url and include level and record data.
        public static RoboGame GetGameWithRecordsFromID(string id)
        {
            RoboGame _roboGame;

            // SELECT * FROM RoboGames, JOIN levels
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGame = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Character)
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ThenInclude(levels => levels.Records)
                .ThenInclude(records => records.Uploader)
                .Where(e => e.UrlName == id)
                .FirstOrDefault();
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (_roboGame != null)
            {
                if (_roboGame.LevelGroups.Count > 0)
                    foreach (var levelGroup in _roboGame.LevelGroups)
                    {
                        List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                        levelGroup.Levels = sortedList;
                    }
                return _roboGame;
            }
            else
            {
                return new RoboGame("Invalid Game");
            }
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

        public static RoboUser GetRoboUserFromUserName (string uname, short disc)
        {
            RoboUser _roboUser;
            // Return user with given username and discriminator. Return "invalid user" if not found.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboUser = context.RoboUsers.Where(e => e.UserNameNoDiscrim.ToLower() == uname.ToLower() && e.Discriminator == disc).FirstOrDefault();
            }

            if (_roboUser != null)
                return _roboUser;
            else
                return new RoboUser("Invalid User", -1);
        }

        public static RoboUser GetRoboUserFromUserName(string unameWithDiscriminator)
        {
            string[] splittedUsername = Validator.TrySplitUsername(unameWithDiscriminator);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            return GetRoboUserFromUserName(username, discriminator);
        }

        public static IdentityRoboUser GetIdentityUserFromUserName(string unameWithDiscriminator)
        {
            IdentityRoboUser iUser;
            // Return user with given username and discriminator. Return "invalid user" if not found.
            using (IdentityContext context = new IdentityContext())
            {
                iUser = context.Users.Where(e => e.NormalizedUserName == unameWithDiscriminator.ToUpper()).FirstOrDefault();
            }

            if (iUser != null)
                return iUser;
            else
                return new IdentityRoboUser("Invalid User");
        }

        public static IdentityRoboUser GetIdentityUserFromUserName(string uname, short disc)
        {
            return GetIdentityUserFromUserName($"{uname}#{disc}");
        }

        public static RoboUser GetRoboUserFromApiKey(string apiKey)
        {
            IdentityRoboUser identityRoboUser;
            
            using (IdentityContext context = new IdentityContext())
                identityRoboUser = context.Users.FirstOrDefault(e => e.ApiKey == apiKey);

            if (identityRoboUser == null)
                return new RoboUser("Invalid User", -1);

            return GetRoboUserFromUserName(identityRoboUser.UserName);
        }
        
        public static IdentityRoboUser GetIdentityUserFromApiKey(string apiKey)
        {
            IdentityRoboUser identityRoboUser;
            
            using (IdentityContext context = new IdentityContext())
                identityRoboUser = context.Users.FirstOrDefault(e => e.ApiKey == apiKey);

            if (identityRoboUser == null)
                return new IdentityRoboUser("Invalid User");

            return identityRoboUser;
        }

        public static RoboGame GetGameFromID(string id)
        {
            RoboGame _roboGame;
            // SELECT * FROM RoboGames WHERE UrlName = id. Return "invalid game" if not found.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                _roboGame = context.RoboGames.Where(e => e.UrlName == id).FirstOrDefault();
            }

            if (_roboGame != null)
                return _roboGame;
            else
                return new RoboGame("Invalid Game");
        }

        public static RoboLevel GetGameLevelFromMapId(string gameid, string _mapid)
        {
            // SELECT * FROM RoboLevels where gameid and mapid --- Took 6 hours to figure this one out; Zenya
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                int mapid = 0;
                int.TryParse(_mapid, out mapid);

                RoboLevel query = context.RoboGames
                    .Include(g => g.LevelGroups)
                    .ThenInclude(l => l.Levels)
                    .ThenInclude(l => l.Records)
                    .ThenInclude(r => r.Character)
                    .Include(g => g.LevelGroups)
                    .ThenInclude(l => l.Levels)
                    .ThenInclude(l => l.Records)
                    .ThenInclude(r => r.Uploader)
                    .Where(g => g.UrlName == gameid && g.LevelGroups.Any(l => l.Levels.Any(level => level.LevelNumber == mapid)))
                    .Select(g => g.LevelGroups.Where(e => e.Levels.Any(o => o.LevelNumber == mapid))
                    .Select(e => e.Levels.FirstOrDefault(l => l.LevelNumber == mapid))
                    .FirstOrDefault())
                    .FirstOrDefault();

                if (query != null)
                    return query;
                else
                    return new RoboLevel()
                    {
                        DbId = -1,
                        LevelName = "Invalid Level",
                        LevelNumber = 9001,
                        Act = 0,
                        IconUrl = "",
                        Records = new List<RoboRecord>()
                    };
            }
        }

        public static List<RoboGame> GetGamesWithLevels()
        {
            List<RoboGame> _roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, JOIN levels.
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
