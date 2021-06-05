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
using System.IO;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class RoboLevel
    {
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
        
        public RoboLevel(int levelNumber, string levelName, int act)
        {
            LevelNumber = levelNumber;
            LevelName = levelName;
            Act = act;
            Records = new List<RoboRecord>();
        }
    }
}