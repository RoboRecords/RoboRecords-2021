/*
 * GamePage.cshtml.cs: Backend for a single game page
 * Copyright (C) 2022, Lemin <Leminn>, Refrag <Refragg>, Ors <Riku-S> and Zenya <Zeritar>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Web;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class GamePage : RoboPageModel
    {
        // private List<RoboGame> _roboGames;
        public static RoboGame CurrentGame;

        public GamePage(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }

        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");

            var id = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("id");

            if (id != null)
            {
                DbSelector.TryGetGameWithRecordsFromID(id, out CurrentGame);
            }
            else
            {
                // SRB2 2.2 default for testing. Should be changed to throw an error if not found.
                DbSelector.TryGetGameWithRecordsFromID("sonicroboblast2v22", out CurrentGame);
            }
        }
    }
}