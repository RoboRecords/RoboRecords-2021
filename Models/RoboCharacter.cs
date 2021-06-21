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

using System.Text.RegularExpressions;

namespace RoboRecords.Models
{
    public class RoboCharacter
    {
        public string Name;
        public string NameId;
        public string IconUrl;

        public RoboCharacter(string name, string nameId = null)
        {
            Name = name;
            
            var regex = new Regex("[^a-zA-Z0-9_-]");
            nameId ??= regex.Replace(name, "").ToLower();
            NameId = nameId;

            IconUrl = "../assets/images/characters/" + nameId + ".png";
        }
    }
}