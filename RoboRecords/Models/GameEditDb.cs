/*
 * GameEditDb.cs
 * Copyright (C) 2021, Ors <Riku-S>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RoboRecords.Models
{
    public class GameEditDb
    {
        [Required] [Display(Name = "Name")] public string Name;
        [Required] [Display(Name = "Url name")] public string UrlName;

        [Required] [Display(Name = "Level Groups")]
        public IList<LevelGroup> LevelGroups;
    }
}