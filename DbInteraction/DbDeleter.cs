using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;

namespace RoboRecords.DbInteraction
{
    public class DbDeleter
    {
        // For use with test program to make it clean up after itself.
        // I don't think we want to just straight up delete entries.
        public static void DeleteRoboLevel(RoboLevel level)
        {
            // DELETE the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboLevels.Remove(level);
                context.SaveChangesAsync();
            }
        }

        public static void DeleteRoboRecord(RoboRecord record)
        {
            // DELETE the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboRecords.Remove(record);
                context.SaveChangesAsync();
            }
        }

        public static void DeleteRoboGame(RoboGame game)
        {
            // DELETE the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Remove(game);
                context.SaveChangesAsync();
            }
        }
    }
}
