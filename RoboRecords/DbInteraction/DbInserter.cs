using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Linq;
using System.Collections.Generic;

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
    }
}
