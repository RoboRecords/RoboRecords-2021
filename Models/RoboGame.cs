/*
 * RoboLevel.cs
 * Copyright (C) 2021, Ors <Riku-S>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class RoboGame
    {
        public int DbId;

        public string Name;
        public string UrlName;

        public string GetAspLink()
        {
            return "/GamePage?id=" + UrlName;
        }
        public string IconPath;
        public virtual IList<LevelGroup> LevelGroups { get; set; }

        public RoboLevel GetLevelByNumber(int number)
        {
            foreach (var levelGroup in LevelGroups)
            {
                foreach (var level in levelGroup.Levels)
                {
                    if (level.LevelNumber == number)
                    {
                        return level;
                    }
                }
            }
            return null;
        }

        public RoboGame(string name)
        {
            Name = name;
            var regex = new Regex("[^a-zA-Z0-9_-]");
            UrlName = regex.Replace(name, "").ToLower();
            LevelGroups = new List<LevelGroup>();
        }

        public override string ToString()
        {
            string roboString = $"RoboGame\nID: {DbId}\nName: {Name}\nURLName: {UrlName}\n" +
                $"Level Groups:\n";

            if (LevelGroups.Count > 0)
                foreach (LevelGroup group in LevelGroups)
                {
                    roboString += $" {group.Name}\n{group}";
                }
            return roboString;
        }
    }
}