/*
 * DbSelector.cs
 * Copyright (C) 2022, Zenya <Zeritar>, Refrag <Refragg> and Ors <Riku-S>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

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
        public static bool TryGetCharacterFromDbId(int dbId, out RoboCharacter character)
        {
            character = null;
            
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                character = context.RoboCharacters.FirstOrDefault(x => x.DbId == dbId);
            }

            return character is not null;
        }
        
        public static bool TryGetCharacterFromNameId(string nameId, out RoboCharacter character)
        {
            character = null;
            
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                character = context.RoboCharacters.FirstOrDefault(x => x.NameId == nameId);
            }

            return character is not null;
        }
        
        public static bool TryGetRoboRecordFromDbId(int dbId, out RoboRecord record)
        {
            record = null;
            
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                record = context.RoboRecords.FirstOrDefault(x => x.DbId == dbId);
            }

            return record is not null;
        }

        public static bool TryGetRoboRecordWithDataFromDbId(int dbId, out RoboRecord record)
        {
            record = null;

            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                record = context.RoboRecords
                    .Include(e => e.Level)
                    .ThenInclude(l => l.LevelGroup)
                    .ThenInclude(g => g.RoboGame)
                    .Include(e => e.Uploader)
                    .Include(e => e.Character)
                    .FirstOrDefault(x => x.DbId == dbId);
            }

            return record is not null;
        }

        public static bool TryGetAllGameData(out List<RoboGame> roboGames)
        {
            roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, JOIN all foreign keys
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGames = context.RoboGames
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
            if (roboGames.Count > 0)
                foreach (var roboGame in roboGames)
                {
                    if (roboGame.LevelGroups.Count > 0)
                        foreach (var levelGroup in roboGame.LevelGroups)
                        {
                            List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                            levelGroup.Levels = sortedList;
                        }
                }

            return roboGames.Count != 0;
        }

        // Get the game matching the url and include level data.
        public static bool TryGetGameWithLevelsFromID(string id, out RoboGame roboGame)
        {
            roboGame = null;
            if (string.IsNullOrEmpty(id))
                return false;
            
            // SELECT * FROM RoboGames, JOIN levels
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGame = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .Where(e => e.UrlName == id)
                .FirstOrDefault();
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (roboGame is not null)
            {
                if (roboGame.LevelGroups.Count > 0)
                    foreach (var levelGroup in roboGame.LevelGroups)
                    {
                        List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                        levelGroup.Levels = sortedList;
                    }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public static bool TryGetGameWithLevelsFromDbID(int id, out RoboGame roboGame)
        {
            roboGame = null;

            // SELECT * FROM RoboGames, JOIN levels
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGame = context.RoboGames
                    .Include(e => e.LevelGroups)
                    .ThenInclude(levelGroups => levelGroups.Levels)
                    .Where(e => e.DbId == id)
                    .FirstOrDefault();
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (roboGame is not null)
            {
                if (roboGame.LevelGroups.Count > 0)
                    foreach (var levelGroup in roboGame.LevelGroups)
                    {
                        List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                        levelGroup.Levels = sortedList;
                    }
                return true;
            }
            else
            {
                return false;
            }
        }

        // Get the game matching the url and include level and record data.
        public static bool TryGetGameWithRecordsFromID(string id, out RoboGame roboGame)
        {
            roboGame = null;
            if (string.IsNullOrEmpty(id))
                return false;
            
            // SELECT * FROM RoboGames, JOIN levels
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGame = context.RoboGames
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
            if (roboGame is not null)
            {
                if (roboGame.LevelGroups.Count > 0)
                    foreach (var levelGroup in roboGame.LevelGroups)
                    {
                        List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                        levelGroup.Levels = sortedList;
                    }
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool TryGetGames(out List<RoboGame> roboGames)
        {
            roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, no JOINs. Used for Games page.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGames = context.RoboGames
                .ToListAsync().Result;
            }

            return roboGames.Count != 0; // FIXME: This might be bad if for some reason all of the games were to be removed, figure out if this can be a problem or not
        }

        public static bool TryGetRoboUserFromUserName(string uname, short disc, out RoboUser roboUser)
        {
            roboUser = null;
            if (string.IsNullOrEmpty(uname))
                return false;
            
            // Return user with given username and discriminator. Return "invalid user" if not found.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboUser = context.RoboUsers.Where(e => e.UserNameNoDiscrim.ToLower() == uname.ToLower() && e.Discriminator == disc).FirstOrDefault();
            }

            if (roboUser is not null)
                return true;
            else
                return false;
        }

        public static bool TryGetRoboUserFromUserName(string unameWithDiscriminator, out RoboUser roboUser)
        {
            roboUser = null;
            if (string.IsNullOrEmpty(unameWithDiscriminator))
                return false;
        
            string[] splittedUsername = Validator.TrySplitUsername(unameWithDiscriminator);

            string username = splittedUsername[0];
            short discriminator = short.Parse(splittedUsername[1]);

            return TryGetRoboUserFromUserName(username, discriminator, out roboUser);
        }

        public static bool TryGetIdentityUserFromUserName(string unameWithDiscriminator, out IdentityRoboUser iUser)
        {
            iUser = null;
            if (string.IsNullOrEmpty(unameWithDiscriminator))
                return false;
            
            // Return user with given username and discriminator. Return "invalid user" if not found.
            using (IdentityContext context = new IdentityContext())
            {
                iUser = context.Users.Where(e => e.NormalizedUserName == unameWithDiscriminator.ToUpper()).FirstOrDefault();
            }

            if (iUser is not null)
                return true;
            else
                return false;
        }

        public static bool TryGetIdentityUserFromUserName(string uname, short disc, out IdentityRoboUser iUser)
        {
            return TryGetIdentityUserFromUserName($"{uname}#{disc}", out iUser);
        }

        public static bool TryGetRoboUserFromApiKey(string apiKey, out RoboUser roboUser)
        {
            roboUser = null;
            if (string.IsNullOrEmpty(apiKey))
                return false;
            
            IdentityRoboUser identityRoboUser;
            
            using (IdentityContext context = new IdentityContext())
                identityRoboUser = context.Users.FirstOrDefault(e => e.ApiKey == apiKey);

            if (identityRoboUser is null)
            {
                roboUser = null;
                return false;
            }

            return TryGetRoboUserFromUserName(identityRoboUser.UserName, out roboUser);
        }
        
        public static bool TryGetIdentityUserFromApiKey(string apiKey, out IdentityRoboUser identityRoboUser)
        {
            identityRoboUser = null;
            if (string.IsNullOrEmpty(apiKey))
                return false;
            
            using (IdentityContext context = new IdentityContext())
                identityRoboUser = context.Users.FirstOrDefault(e => e.ApiKey == apiKey);

            if (identityRoboUser is null)
                return false;

            return true;
        }

        public static bool TryGetGameFromID(string id, out RoboGame roboGame)
        {
            roboGame = null;
            if (string.IsNullOrEmpty(id))
                return false;
            
            // SELECT * FROM RoboGames WHERE UrlName = id. Return "invalid game" if not found.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGame = context.RoboGames.Where(e => e.UrlName == id).FirstOrDefault();
            }

            if (roboGame is not null)
                return true;
            else
                return false;
        }
        
        public static bool TryGetGameFromDbID(int id, out RoboGame roboGame)
        {
            roboGame = null;

            // SELECT * FROM RoboGames WHERE UrlName = id. Return "invalid game" if not found.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGame = context.RoboGames.Where(e => e.DbId == id).FirstOrDefault();
            }

            if (roboGame is not null)
                return true;
            else
                return false;
        }

        public static bool TryGetGameLevelFromMapId(string gameid, string _mapid, out RoboLevel roboLevel)
        {
            roboLevel = null;
            if (string.IsNullOrEmpty(gameid) || string.IsNullOrEmpty(_mapid))
                return false;
            
            // SELECT * FROM RoboLevels where gameid and mapid --- Took 6 hours to figure this one out; Zenya
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                int mapid = 0;
                int.TryParse(_mapid, out mapid);

                roboLevel = context.RoboGames
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

                if (roboLevel is not null)
                    return true;
                else
                    return false;
            }
        }

        public static bool TryGetGamesWithLevels(out List<RoboGame> roboGames)
        {
            roboGames = new List<RoboGame>();

            // SELECT * FROM RoboGames, JOIN levels.
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                roboGames = context.RoboGames
                .Include(e => e.LevelGroups)
                .ThenInclude(levelGroups => levelGroups.Levels)
                .ToListAsync().Result;
            }


            // Sort the levels by level number, as they may not be in order in the database
            if (roboGames.Count > 0)
            {
                foreach (var roboGame in roboGames)
                {
                    if (roboGame.LevelGroups.Count > 0)
                        foreach (var levelGroup in roboGame.LevelGroups)
                        {
                            List<RoboLevel> sortedList = levelGroup.Levels.OrderBy(l => l.LevelNumber).ToList();
                            levelGroup.Levels = sortedList;
                        }
                }

                return true;
            }

            return false;
        }
    }
}
