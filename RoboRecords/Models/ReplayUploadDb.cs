/*
 * ReplayUploadDb.cs
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
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;

namespace RoboRecords.Models
{
    public class ReplayUploadDb
    {
        [Required]
        [Display(Name="File")]
        public List<IFormFile> FormFiles { get; set; }
    }
}