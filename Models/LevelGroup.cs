using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class LevelGroup
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