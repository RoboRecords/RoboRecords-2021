using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class WadReader
    {
        // TODO: Also get level pics from PK3 and convert them using Ors' project
        public static List<LevelGroup> GetMainCFGFromPK3(string filename)
        {
            var zip = new ZipInputStream(File.OpenRead(filename));
            var filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            ZipFile zipFile = new ZipFile(filestream);
            ZipEntry item;
            while ((item = zip.GetNextEntry()) != null)
            {
                if (item.Name.ToLower().Contains("maincfg"))
                {
                    Debug.WriteLine("Found MAINCFG!");
                    using (StreamReader s = new StreamReader(zipFile.GetInputStream(item)))
                    {
                        return ParseMainCFG(s.ReadToEnd());
                    }
                }
            }
            return new List<LevelGroup>();
        }

        static List<LevelGroup> ParseMainCFG(string maincfg)
        {
            string[] lines = maincfg.Split('\n');
            var levelEntries = new List<LevelEntry>();
            var levels = new List<RoboLevel>();

            string lvlnum = @"[Ll][Ee][Vv][Ee][Ll]\s([a-zA-Z0-9]{1,2})";
            string lvlname = @"[Ll][Ee][Vv][Ee][Ll][Nn][Aa][Mm][Ee]\s?\=\s?([A-Za-z ]+)";
            string lvlact = @"[Aa][Cc][Tt]\s?\=\s?(\d+)";
            string lvlrecatk = @"recordattack\s?\=\s?";

            Regex rgnum = new Regex(lvlnum);
            Regex rgname = new Regex(lvlname);
            Regex rgact = new Regex(lvlact);
            Regex rgrecatk = new Regex(lvlrecatk);

            if (lines.Length > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    if (lines[i].ToLower().Contains("level ")
                        && !lines[i].ToLower().Contains("typeoflevel")
                        && !lines[i].ToLower().Contains("nextlevel"))
                    {
                        levelEntries.Add(new LevelEntry
                        {
                            startline = i
                        });

                        if (levelEntries.Count > 1)
                        {
                            levelEntries[levelEntries.Count - 2].endline = i;
                        }
                    }
                }

                if (levelEntries.Count > 0)
                {
                    foreach (var levelEntry in levelEntries)
                    {
                        for (int i = levelEntry.startline; i < levelEntry.endline; i++)
                        {
                            levelEntry.raw += lines[i];
                        }

                        // Get only levels which can be record attacked
                        Match match = rgrecatk.Match(levelEntry.raw.ToLower());
                        if (!match.Success)
                        {
                            continue;
                        }

                        match = rgnum.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.levelNumber = RoboLevel.MakeLevelNum(match.Groups[1].Value);
                        }

                        match = rgname.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.levelName = $"{match.Groups[1].Value} Zone";
                        }

                        match = rgact.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.act = Convert.ToInt32(match.Groups[1].Value);
                        }

                        levels.Add(new RoboLevel(levelEntry.levelNumber, levelEntry.levelName, levelEntry.act));

                        Debug.WriteLine(levelEntry.levelNumber);
                    }
                    return SortLevelsToGroups(levels);
                }
            }
            return new List<LevelGroup>();
        }

        //TODO: This might be useful elsewhere, so find a better place for it
        public static List<LevelGroup> SortLevelsToGroups (List<RoboLevel> levels)
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
                else if (levels[i].LevelName == levels[i-1].LevelName)
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

        class LevelEntry
        {
            public int startline;
            public int endline;
            public string raw = "";
            public string entry = "";
            public int levelNumber = 0;
            public int act = 0;
            public string levelName = "";
        }
    }
}
