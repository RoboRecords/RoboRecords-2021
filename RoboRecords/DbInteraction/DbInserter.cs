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
                context.SaveChangesAsync();
            }
        }

        public static void AddGame(RoboGame game)
        {
            // INSERT the submitted RoboGame
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Add(game);
                context.SaveChangesAsync();
            }
        }

        public static void AddRoboUser(RoboUser user)
        {
            // INSERT the submitted RoboGame
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboUsers.Add(user);
                context.SaveChangesAsync();
            }
        }

        public static void AddLevelToGame(RoboLevel level, RoboGame game)
        {
            // INSERT the given RoboLevel into the given RoboGame
            game.LevelGroups =  WadReader.SortLevelsToGroups(new List<RoboLevel>() {level}, game.LevelGroups.ToList());
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Update(game);
                context.SaveChangesAsync();
            }
        }

        public static void AddRecordIfNeeded(RoboRecord record, RoboLevel level)
        {
            RoboUser user = record.Uploader;
            
            if (level is null)
            {
                Logger.Log("Map not found, WTF!?!?", Logger.LogLevel.Error, true);
                return;
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
                DbInserter.AddRecordToLevel(record, level);
                FileManager.Write(Path.Combine("Replays", $"{record.DbId}.lmp"), record.FileBytes);
            }
        }
    }
}
