/*
 * DbInserter.cs
 * Copyright (C) 2022, Zenya <Zeritar>, Ors <Riku-S> and Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace RoboRecords.DbInteraction
{
    public class DbInserter
    {
        public static void AddRecordToLevel(RoboRecord record, RoboLevel level)
        {
            // UPDATE the submitted RoboGame with the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.Entry(level).State = EntityState.Modified;
                record.LevelNumber = level.LevelNumber;
                level.Records.Add(record);
                //context.Entry(record).State = EntityState.Detached;
                context.RoboLevels.Update(level);
                context.SaveChangesAsync().Wait();
            }
        }

        public static void AddGame(RoboGame game)
        {
            // INSERT the submitted RoboGame
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Add(game);
                context.SaveChangesAsync().Wait();
            }
        }

        public static void AddRoboUser(RoboUser user)
        {
            // INSERT the submitted RoboGame
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboUsers.Add(user);
                context.SaveChangesAsync().Wait();
            }
        }

        public static void AddLevelToGame(RoboLevel level, RoboGame game)
        {
            // INSERT the given RoboLevel into the given RoboGame
            game.LevelGroups = CommonMethods.SortLevelsToGroups(new List<RoboLevel>() {level}, game.LevelGroups.ToList());
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Update(game);
                context.SaveChangesAsync().Wait();
            }
        }

        public static bool AddRecordIfNeeded(RoboRecord record, RoboLevel level)
        {
            RoboUser user = record.Uploader;
            
            if (level is null)
            {
                Logger.Log("Map not found, WTF!?!?", Logger.LogLevel.Error, true);
                return false;
            }

            bool isBest = true;
            bool isBestNightsScore = level.Nights;
            // Check if the user is uploading their best time. If yes, upload it to the DB
            foreach (var levelRecord in level.Records)
            {

                if (levelRecord.Uploader.DbId == user.DbId && levelRecord.Character.NameId == record.Character.NameId)
                {
                    if (levelRecord.Tics <= record.Tics)
                    {
                        isBest = false;
                    }
                    else if (level.Nights && levelRecord.Score >= record.Score)
                    {
                        isBestNightsScore = false;
                    }

                    if (!isBest && !isBestNightsScore)
                    {
                        break;
                    }
                }
            }

            if (isBest || isBestNightsScore)
            {
                AddRecordToLevel(record, level);
                FileManager.Write(Path.Combine(FileManager.ReplaysDirectoryName, $"{record.DbId}.lmp"), record.FileBytes);
                return true;
            }

            return false;
        }
        
        public static void AddSiteAsset(SiteAsset siteAsset)
        {
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.SiteAssets.Add(siteAsset);
                context.SaveChangesAsync().Wait();
            }
        }
        
        public static void AddGameAsset(GameAsset gameAsset)
        {
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.Entry(gameAsset).State = EntityState.Modified;
                context.GameAssets.Add(gameAsset);
                context.SaveChangesAsync().Wait();
            }
        }
        
        public static void AddCharacterAsset(CharacterAsset characterAsset)
        {
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.Entry(characterAsset).State = EntityState.Modified;
                context.CharacterAssets.Add(characterAsset);
                context.SaveChangesAsync().Wait();
            }
        }
    }
}
