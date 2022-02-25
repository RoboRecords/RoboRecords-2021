/*
 * DbDeleter.cs
 * Copyright (C) 2022, Zenya <Zeritar> and Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

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
                context.SaveChangesAsync().Wait();
            }
        }

        public static void DeleteRoboRecord(RoboRecord record)
        {
            // DELETE the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboRecords.Remove(record);
                context.SaveChangesAsync().Wait();
            }
        }

        public static void DeleteRoboGame(RoboGame game)
        {
            // DELETE the submitted RoboLevel
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                context.RoboGames.Remove(game);
                context.SaveChangesAsync().Wait();
            }
        }
    }
}
