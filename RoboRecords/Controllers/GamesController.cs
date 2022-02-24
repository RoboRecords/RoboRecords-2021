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