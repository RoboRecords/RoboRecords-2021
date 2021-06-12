using System.Collections.Generic;

namespace RoboRecords.Models
{
    public struct LevelGroup
    {
        public string Name;
        public List<RoboLevel> Levels;
        public bool WriteLevelNames;

        public LevelGroup(string name, List<RoboLevel> levels = null, bool writeLevelNames = false)
        {
            Name = name;
            WriteLevelNames = writeLevelNames;
            levels ??= new List<RoboLevel>();

            Levels = levels;
        }
    }
}