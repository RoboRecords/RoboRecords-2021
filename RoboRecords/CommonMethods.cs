using System.Collections.Generic;
using RoboRecords.Models;

namespace RoboRecords
{
    public class CommonMethods
    {
        //TODO: Include NiGHTS, Bonus and Challenge flags in sort since those stages rarely have the same name
        public static List<LevelGroup> SortLevelsToGroups(List<RoboLevel> levels, List<LevelGroup> groups)
        {
            for (int i = 0; i < levels.Count; i++)
            {
                bool added = false;
                for (int j = 0; j < groups.Count; j++)
                    if (levels[i].LevelName == groups[j].Name)
                    {
                        groups[j].Levels.Add(levels[i]);
                        added = true;
                    }

                // Just add new group if no group with matching name is found.
                if (!added)
                {
                    groups.Add(new LevelGroup(levels[i].LevelName));
                    groups[groups.Count - 1].Levels.Add(levels[i]);
                }
            }
            return groups;
        }

        public static List<LevelGroup> SortLevelsToGroups(List<RoboLevel> levels)
        {
            List<LevelGroup> groups = new List<LevelGroup>();
            int currentGroup = 0;

            for (int i = 0; i < levels.Count; i++)
            {
                if (i == 0)
                {
                    groups.Add(new LevelGroup(levels[i].LevelName));
                    groups[currentGroup].Levels.Add(levels[i]);
                }
                else if (levels[i].LevelName == levels[i - 1].LevelName)
                {
                    groups[currentGroup].Levels.Add(levels[i]);
                }
                else
                {
                    currentGroup++;
                    groups.Add(new LevelGroup(levels[i].LevelName));
                    groups[currentGroup].Levels.Add(levels[i]);
                }
            }
            return groups;
        }
    }
}
