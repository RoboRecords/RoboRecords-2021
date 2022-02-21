using System.Collections.Generic;
using System;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Linq;

namespace RoboRecords.DbInteraction
{
    public class DbUpdater
    {
        public static void UpdateGame(RoboGame game)
        {
            // UPDATE the submitted RoboGame
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Update(game);
                context.SaveChangesAsync().Wait();
            }
        }

        public static void UpdateIdentityUser(IdentityRoboUser user)
        {
            using (IdentityContext context = new IdentityContext())
            {
                context.Users.Update(user);
                context.SaveChangesAsync().Wait();
            }
        }
    }
}
