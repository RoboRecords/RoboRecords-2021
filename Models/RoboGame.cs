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
using System.Text.RegularExpressions;
using AspNetCore.Identity.Mongo.Model;
using MongoDB.Bson.Serialization.Attributes;

namespace RoboRecords.Models
{
    public class RoboGame
    {
        public string Name;
        public string IdName;
        public string IconPath;
        public List<RoboLevel> Levels;

        public RoboGame(string name)
        {
            Name = name;
            var regex = new Regex("[^a-zA-Z0-9_-]");
            IdName = regex.Replace(name, "").ToLower();
            Levels = new List<RoboLevel>();
        }
    }
}