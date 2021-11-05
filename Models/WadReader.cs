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
        public static void GetMainCFGFromPK3(string filename)
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
                        ParseMainCFG(s.ReadToEnd());
                    }
                }
            }
        }

        static void ParseMainCFG(string maincfg)
        {
            string[] lines = maincfg.Split('\n');
            var levelEntries = new List<LevelEntry>();

            string lvlnum = @"[Ll][Ee][Vv][Ee][Ll]\s([a-fA-f0-9]{2})";
            string lvlname = @"[Ll][Ee][Vv][Ee][Ll][Nn][Aa][Mm][Ee]\s?\=\s?([A-Za-z ]+)";
            string lvlact = @"[Aa][Cc][Tt]\s?\=\s?(\d+)";

            Regex rgnum = new Regex(lvlnum);
            Regex rgname = new Regex(lvlname);
            Regex rgact = new Regex(lvlact);

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

                        Match match = rgnum.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.entry += $"MAP{match.Groups[1].Value}";
                        }

                        match = rgname.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.entry += $", {match.Groups[1].Value} Zone";
                        }

                        match = rgact.Match(levelEntry.raw);
                        if (match.Success)
                        {
                            levelEntry.entry += $" Act {match.Groups[1].Value}";
                        }

                        Debug.WriteLine(levelEntry.entry);
                    }
                }
            }
        }

        class LevelEntry
        {
            public int startline;
            public int endline;
            public string raw = "";
            public string entry = "";
        }
    }
}
