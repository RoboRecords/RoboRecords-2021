using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Linq;

namespace RoboRecords.DbInteraction
{
    public class DbInserter
    {
        public static void AddRecordToLevel(RoboRecord record, RoboLevel level)
        {
            // UPDATE the submitted RoboGame with the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                level.Records.Add(record);
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
    }
}
