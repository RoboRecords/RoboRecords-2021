/*
 * LevelGroup.cs
 * Copyright (C) 2022, Ors <Riku-S>, Refrag <Refragg> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoboRecords.Models
{
    public class LevelGroup
    {
        public int DbId;
        
        public string Name;
        public virtual IList<RoboLevel> Levels { get; set; }
        public virtual RoboGame RoboGame { get; set; }
        public bool WriteLevelNames;

        public LevelGroup(string name, List<RoboLevel> levels = null, bool writeLevelNames = false)
        {
            Name = name;
            WriteLevelNames = writeLevelNames;
            levels ??= new List<RoboLevel>();

            Levels = levels;
        }

        // Needed for the database context
        public LevelGroup()
        {
        }

        public override string ToString()
        {
            string levelstrings = "";
            if (Levels.Count > 0)
            foreach (RoboLevel level in Levels)
                {
                    levelstrings += $"  {level}\n";
                }

            return levelstrings;
        }
        
        public static bool operator ==(LevelGroup levelGroup1, LevelGroup levelGroup2)
        {
            if (levelGroup1 is null && levelGroup2 is not null || levelGroup1 is not null && levelGroup2 is null)
                return false;
            
            return levelGroup1 is null || (levelGroup1.Name == levelGroup2.Name && levelGroup1.RoboGame == levelGroup2.RoboGame);
        }

        public static bool operator !=(LevelGroup levelGroup1, LevelGroup levelGroup2) => !(levelGroup1 == levelGroup2);
    }
}