using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RoboRecords.Models
{
    public class LevelGroup
    {
        public int DbId;
        
        public string Name;
        public virtual IList<RoboLevel> Levels { get; set; }
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
    }
}