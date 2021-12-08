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
using System.Web;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Submit : PageModel
    {
        public static List<RoboRecord> RecordList;
        [BindProperty]
        public ReplayUploadDb FileUpload { get; set; }
        public static RoboGame Game;

        public void OnGet()
        {
            RecordList = new List<RoboRecord>();
            // TODO: Change to only select current level, as this won't make Entity Framework
            // generate an UPDATE for the whole RoboGame object.
            // Not gonna start changing stuff here, as it might break the Submit page.
            // Same method as in Map page can be used to get just the level.
            // DbInserter.AddRecord just
            var roboGames = DbSelector.GetGamesWithLevels();

            var gameId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("game");
            if (gameId != null)
            {
                Game = roboGames.Find(game => game.UrlName == gameId);
            }
            else
            {
                // SRB2 2.2 default for testing. Should be changed to throw an error.
                Game = roboGames.Find(game => game.UrlName == "sonicroboblast2v22");
            }
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