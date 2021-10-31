/*
 * Submit.cshtml.cs: the website main configuration
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
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Submit : PageModel
    {
        public static List<RoboRecord> RecordList;
        [BindProperty]
        public ReplayUploadDb FileUpload { get; set; }
        private RoboRecordsDbContext _dbContext;
        public static RoboGame Game;

        public Submit(RoboRecordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void OnGet()
        {
            RecordList = new List<RoboRecord>();
            var roboGames = _dbContext.RoboGames.Include(e => e.LevelGroups).Include("LevelGroups.Levels").ToListAsync().Result;
            Game = roboGames[1]; // TODO: CHANGE THIS HARDCODED CRAP TO SUPPORT THE CURRENT SELECTED GAME
        }

        public IActionResult OnPostAsync(ReplayUploadDb fileUpload)
        {
            if (fileUpload == null || fileUpload.FormFiles == null || fileUpload.FormFiles.Count == 0)
            {
                // TODO: Error message thing or something
                return null;
            }
            List<IFormFile> files = fileUpload.FormFiles;
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var stream = formFile.OpenReadStream();
                    var bytes = new byte[formFile.Length];
                    stream.Read(bytes, 0, (int)formFile.Length);
                    var rec = new RoboRecord(new RoboUser("Example", 420), bytes.ToArray());

                    // If something went wrong while reading the replay, don't upload it.
                    if (rec.FileBytes != null && rec.FileBytes.Length > 0)
                    {
                        RecordList.Add(rec);
                    }
                    else
                    {
                        // TODO: Error message or something
                    }
                    
                }
            }
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.

            // return new OkObjectResult(new {count = files.Count, size});
            return null;
        }

        public IActionResult OnPostUploadAsync()
        {
            // TODO: Upload things to the server
            // Should NEVER happen
            if (RecordList.Count == 0)
            {
                return null;
            }
            return RedirectToPage();
        }
        
        public IActionResult OnPostCancelAsync()
        {
            // Refresh the page to cancel the uploads
            return RedirectToPage();
        }

        public IActionResult OnPostDeleteAsync(int rec)
        {
            // Should NEVER happen
            if (RecordList.Count <= rec)
            {
                return null;
            }

            RecordList.RemoveAt(rec);
            return null;
        }
    }
}