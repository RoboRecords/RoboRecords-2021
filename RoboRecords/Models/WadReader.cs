using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class WadReader
    {
        // TODO: Also get level pics from PK3 and convert them using Ors' project
        // Done, almost. Needs reworking --- Zenya
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
                    Logger.Log("Found MAINCFG!", Logger.LogLevel.Debug, true);
                    using (StreamReader s = new StreamReader(zipFile.GetInputStream(item)))
                    {
                        return ParseMainCFG(s.ReadToEnd(), filename);
                    }
                }
            }
            return new List<LevelGroup>();
        }

        public static byte[] GetLevelPicFromPK3(string filename, string mapString)
        {
            var zip = new ZipInputStream(File.OpenRead(filename));
            var filestream = new FileStream(filename, FileMode.Open, FileAccess.Read);
            ZipFile zipFile = new ZipFile(filestream);
            ZipEntry item;
            byte[] buffer = new byte[16 * 1024];
            while ((item = zip.GetNextEntry()) != null)
            {
                if (item.Name.ToUpper().Contains($"{mapString}P"))
                {
                    Logger.Log($"Found {item.Name}!", Logger.LogLevel.Debug, true);
                    Stream s = zipFile.GetInputStream(item);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        int read;
                        while ((read = s.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ms.Write(buffer, 0, read);
                        }
                        return ms.ToArray();
                    }
                }
            }
            return new byte[0];
        }

        static List<LevelGroup> ParseMainCFG(string maincfg, string filename)
        {
            string[] lines = maincfg.Split('\n');
            var levelEntries = new List<LevelEntry>();
            var levels = new List<RoboLevel>();
            var levelPics = new List<LevelPic>();

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
                        RoboLevel level = new RoboLevel(levelEntry.levelNumber, levelEntry.levelName, levelEntry.act);

                        levels.Add(level);

                        levelPics.Add(new LevelPic(GetLevelPicFromPK3(filename, level.MapString), $"{level.MapString}P"));
                    }

                    // Don't do this automatically yet, needs fixing first. --- Zenya
                    //foreach (var levelPic in levelPics)
                    //{
                    //    LevelPic.SaveToFile(levelPic.picture, levelPic.filename);
                    //}

                    return SortLevelsToGroups(levels);
                }
            }
            return new List<LevelGroup>();
        }

        //TODO: Include NiGHTS, Bonus and Challenge flags in sort since those stages rarely have the same name
        //TODO: This might be useful elsewhere, so find a better place for it
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
                    groups[groups.Count-1].Levels.Add(levels[i]);
                }
            }
            return groups;
        }

        //TODO: This might be useful elsewhere, so find a better place for it
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
