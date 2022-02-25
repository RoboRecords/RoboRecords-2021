/*
 * DbUpdater.cs
 * Copyright (C) 2022, Zenya <Zeritar> and Refrag <Refragg>
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
