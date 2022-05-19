/*
 * WadReader.cs
 * Copyright (C) 2022, Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class WadReader
    {
        // TODO: Rework using cross-platform framework.
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

                    return CommonMethods.SortLevelsToGroups(levels);
                }
            }
            return new List<LevelGroup>();
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
