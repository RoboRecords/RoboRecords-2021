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

namespace RoboRecords.Models
{
    public class RoboLevel
    {
        public string IconUrl;
        // Map id, Techno Hill Zone 1 = 4
        private int _levelNumber;
        // Map string, eg. "MAP01".
        private string _mapString;

        public int LevelNumber
        {
            get =>_levelNumber;
            set
            {
                _levelNumber = value;
                _mapString = MakeMapString(value);
            }
        }
        public List<RoboRecord> Records;
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
                var firstChar = (char) ((levelNumber - 100) / 36 + 'A');
                var remainder = (levelNumber - 100) % 36;
                var secondChar = remainder < 10 ? (char)(remainder + '0') : (char)(remainder - 10 + 'A');
                char[] chars = {firstChar, secondChar};
                numberForm = new string(chars);
            }
            return "MAP" + numberForm;
        }

        public RoboRecord GetBestRecord(RoboCharacter character)
        {
            RoboRecord bestRecord = null;
            uint bestTime = UInt32.MaxValue;
            foreach (var record in Records.FindAll(rec => rec.Character.NameId == character.NameId))
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
            foreach (var record in Records.FindAll(rec => rec.Character.NameId == character.NameId))
            {
                allRecordsWithCharacter.Add(record);
            }
            // Sort the list of all records for the next part
            allRecordsWithCharacter.Sort(SortByTime);
            
            // Add only the first, meaning best times by each player to the record list.
            var records = new List<RoboRecord>();
            foreach (var record in  allRecordsWithCharacter)
            {
                if (records.FindIndex(rec => rec.Uploader.UserNameNoDiscrim == record.Uploader.UserNameNoDiscrim && rec.Uploader.Discriminator == record.Uploader.Discriminator) == -1)
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
    }
}