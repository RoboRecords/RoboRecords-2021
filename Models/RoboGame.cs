using System.Collections.Generic;;

namespace RoboRecords.Models
{
    public class RoboGame
    {
        public string Name;
        public string Icon;
        public List<RoboLevel> Levels;

        public RoboGame()
        {
            Levels = new List<RoboLevel>();
        }
        public RoboGame(string name)
        {
            Name = name;
            Levels = new List<RoboLevel>();
        }
    }
}