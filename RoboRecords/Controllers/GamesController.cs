/*
 * GamesController.cs: Games Web API Endpoints implementation 
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.Collections.Generic;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetGameFromId(int id)
        {
            if (!DbSelector.TryGetGameWithLevelsFromDbID(id, out RoboGame roboGame))
                return NotFound("No game found at that address");
            return Ok(roboGame);
        }

        [HttpGet]
        [Route("")]
        public IActionResult GetGames()
        {
            if (!DbSelector.TryGetGamesWithLevels(out List<RoboGame> roboGames))
                return Problem();
            return Ok(roboGames);
        }
    }
}