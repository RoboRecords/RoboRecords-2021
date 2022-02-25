/*
 * Games.cshtml.cs: Backend for the games website's page
 * Copyright (C) 2022, Lemin <Leminn>, Ors <Riku-S>, Zenya <Zeritar> and Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using RoboRecords.DbInteraction; // Initiative to move all database interactions to one place. --- Zenya
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class Games : RoboPageModel
    {
        public static List<RoboGame> RoboGames;

        public Games(IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
        }
        
        public void OnGet()
        {
            RoboGames = new List<RoboGame>();

            DbSelector.TryGetGames(out RoboGames);
        }
    }
}