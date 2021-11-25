/*
 * RoboLevel.cs
 * Copyright (C) 2021, Ors <Riku-S>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class RoboLevel
    {
        public int DbId;

        public string IconUrl;
        // Map id, Techno Hill Zone 1 = 4
        private int _levelNumber;
        // Map string, eg. "MAP01".
        private string _mapString;

        public int LevelNumber
        {
            get => _levelNumber;
            set
            {
                _levelNumber = value;
                _mapString = MakeMapString(value);
            }
        }
        public virtual IList<RoboRecord> Records { get; set; }
        // eg. Green Flower Zone
        public string LevelName;
        // 1 in Green Flower Zone Act 1
        public int Act;
        // Read-only, changed by changing the map number
        public string MapString => _mapString;

        private const int MaxLevelNumber = 1035;
        public static string MakeMapString(int levelNumber)
        {
            var numberForm = "";

            if (levelNumber < 0 || levelNumber > MaxLevelNumber)
            {
                return "Invalid-map-number";
            }
            if (levelNumber < 100)
            {
                // Map number is expressed with two digits
                numberForm = levelNumber.ToString("D2");
            }
            else
            {
                // Map number is expressed with a leading letter and a digit/letter
                var firstChar = (char)((levelNumber - 100) / 36 + 'A');
                var remainder = (levelNumber - 100) % 36;
                var secondChar = remainder < 10 ? (char)(remainder + '0') : (char)(remainder - 10 + 'A');
                char[] chars = { firstChar, secondChar };
                numberForm = new string(chars);
            }
            return "MAP" + numberForm;
        }

        public static int MakeLevelNum(string _mapString)
        {
            string mapString;
            int levelNum;
            int[] digits = new int[] { 0, 0 };
            Regex rgnum = new Regex(@"M?A?P?([A-Z0-9]{1,2})");

            // Works regardless of the string having leading "MAP" for ease of use
            // If a non-mapstring was somehow given to this method, return -1
            Match match = rgnum.Match(_mapString.ToUpper());
            if (!match.Success)
            {
                return -1;
            }
            else
            {
                mapString = match.Groups[1].Value;

                // MAP00-MAP99
                if (int.TryParse(mapString[0].ToString(), out digits[0]) && int.TryParse(mapString[1].ToString(), out digits[1]))
                {
                    levelNum = digits[0] * 10 + digits[1];
                    return levelNum;
                }
                // MAPA0-MAPZZ
                else
                {
                    digits[0] = 100 + (mapString[0] - 'A') * 36;
                    digits[1] = int.TryParse(mapString[1].ToString(), out digits[1]) ? digits[1] = digits[1] : ((mapString[1] - 'A') + 10);
                    levelNum = digits[0] + digits[1];
                }
                return levelNum;
            }
        }

        public RoboRecord GetBestRecord(RoboCharacter character)
        {
            RoboRecord bestRecord = null;
            long bestTime = UInt32.MaxValue;
            if (Records != null && Records.Count() > 0)
                foreach (var record in Records.Where(rec => rec.Character.NameId == character.NameId))
                {
                    if (bestTime > record.Tics)
                    {
                        bestRecord = record;
                        bestTime = record.Tics;
                    }
                }

            return bestRecord;
        }

        public List<RoboRecord> GetBestRecords(bool allowNonStandard = false)
        {
            var records = new List<RoboRecord>();
            foreach (var record in Records)
            {
                if (allowNonStandard ||
                    CharacterManager.StandardCharacters.FindIndex(c => c.NameId == record.Character.NameId) != -1)
                {
                    var bestRecord = GetBestRecord(record.Character);
                    if (bestRecord != null)
                    {
                        records.Add(bestRecord);
                    }
                }
            }

            return records;
        }

        int SortByTime(RoboRecord a, RoboRecord b)
        {
            return (int)a.Tics - (int)b.Tics;
        }

        public List<RoboRecord> GetCharacterRecords(RoboCharacter character)
        {
            // Add all the records done with a character to a list
            var allRecordsWithCharacter = new List<RoboRecord>();
            foreach (var record in Records.Where(rec => rec.Character.NameId == character.NameId))
            {
                allRecordsWithCharacter.Add(record);
            }
            // Sort the list of all records for the next part
            allRecordsWithCharacter.Sort(SortByTime);

            // Add only the first, meaning best times by each player to the record list.
            var records = new List<RoboRecord>();
            foreach (var record in allRecordsWithCharacter)
            {
                if (records.FindIndex(rec => rec.Uploader.UserNameNoDiscrim == record.Uploader.UserNameNoDiscrim && rec.Uploader.NumberDiscriminator == record.Uploader.NumberDiscriminator) == -1)
                {
                    records.Add(record);
                }
            }

            return records;
        }

        public RoboLevel(int levelNumber, string levelName, int act)
        {
            LevelNumber = levelNumber;
            LevelName = levelName;
            Act = act;
            Records = new List<RoboRecord>();

            // REPLACE WITH SOME DATA BASE STUFF LATER!!!
            IconUrl = "../assets/images/mappics/" + MapString + "P.png";
        }

        // Needed for the database context
        public RoboLevel()
        {
        }

        public override string ToString()
        {
            if (Act > 0)
                return $"{LevelName} Act {Act}";
            else
                return $"{LevelName}";
        }
    }
}