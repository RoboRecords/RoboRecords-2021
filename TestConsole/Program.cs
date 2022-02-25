/*
 * TestConsole.Program.cs
 * Copyright (C) 2022, Zenya <Zeritar> and Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using RoboRecords;
using RoboRecords.DatabaseContexts;
using System;
using TestConsole.Models;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestConsole
{
    // Zenya sucks at Unit Tests
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        static void Main(string[] args)
        {
            EnvVars.IsDevelopment = true;
            Configure();

            Methods methods = new Methods();

            Menu menu = new Menu("Main Menu");
            MenuItem testItem = new MenuItem("Select all RoboGames", MenuAction.SelectRoboGames);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TestUpdate", MenuAction.TestUpdate);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TestRecord", MenuAction.TestRecord);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TryReadPK3", MenuAction.TryReadPK3);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TryAddCyberdime", MenuAction.TryAddCyberdime);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TryAddRedVolcano2", MenuAction.TryAddRedVolcano2);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("TryTestFileManager", MenuAction.TryTestFileManager);
            testItem.parameter = $"{testItem.name} menu action called.";
            menu.menuItems.Add(testItem);

            testItem = new MenuItem("Exit", MenuAction.Quit);
            testItem.parameter = $"";
            menu.menuItems.Add(testItem);

            while (true)
                methods.DrawMenu(menu);
        }



        static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<Program>();

            Configuration = builder.Build();

            EnvVars.ParseEnvironmentVariables(Configuration);

            RoboRecordsDbContext.SetConnectionString(EnvVars.SqlDbConnection);
            IdentityContext.SetConnectionString(EnvVars.SqlUserDbConnection);
        }
    }
}
