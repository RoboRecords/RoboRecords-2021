/*
 * RunsController.cs: Runs Web API Endpoints implementation 
 * Copyright (C) 2022, Refrag <Refragg>
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * See the 'LICENSE' file for more details.
 */

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RoboRecords.DbInteraction;
using RoboRecords.Filters;
using RoboRecords.Models;

namespace RoboRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        [RequireApiKey]
        [HttpPost]
        [Route("submit")]
        public IActionResult Submit([FromQuery] int gameId, [FromQuery(Name = "File")] IFormFile file)
        {
            if (gameId == 0)
                return BadRequest("Game ID was not provided");

            if (file is null)
                return BadRequest("Replay file was not provided");
            
            if (!DbSelector.TryGetGameFromDbID(gameId, out RoboGame roboGame))
                return BadRequest($"The game with the id {gameId} was not found");

            byte[] fileBytes = new byte[file.Length];
            
            using (Stream fileStream = file.OpenReadStream())
                fileStream.Read(fileBytes, 0, (int)file.Length);

            RoboRecord record = new RoboRecord((RoboUser)RouteData.Values["apiKeyRoboUser"], fileBytes);
            switch (record.readStatus)
            {
                case RoboRecord.ReadStatus.Success:
                    // The condition SHOULD always be true, but let's be careful
                    if (record.FileBytes != null && record.FileBytes.Length > 0)
                        break;
                    else
                        return Problem($"Unknown error when processing {file.Name}: {RoboRecord.MessageSuccess}");
                default:
                    return UnprocessableEntity(
                        $"Error when processing {file.Name}: {RoboRecord.GetReadStatusMessage(record.readStatus)}");
            }
            
            DbSelector.TryGetGameLevelFromMapId(roboGame.UrlName, record.LevelNumber.ToString(), out RoboLevel level);
            if (!DbInserter.AddRecordIfNeeded(record, level))
                return Conflict("The same record or a better one was already submitted");
            
            return Ok(record);
        }
    }
}