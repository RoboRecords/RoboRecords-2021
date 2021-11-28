using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using RoboRecords.Filters;
using RoboRecords.Models;

namespace RoboRecords.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        //TODO: 
        //   - Get the user from the request somehow
        //   - Save the replay
        //   - Give more information about what failed if it failed
        [RequireApiKeyFilterFactory]
        [HttpPost]
        [Route("submit")]
        public IActionResult Submit([FromQuery(Name = "File")] IFormFile file)
        {
            if (file is null)
                return NoContent();

            byte[] fileBytes = new byte[file.Length];
            
            using (Stream fileStream = file.OpenReadStream())
                fileStream.Read(fileBytes, 0, (int)file.Length);

            RoboRecord record = new RoboRecord((RoboUser)RouteData.Values["apiKeyRoboUser"], fileBytes);
            if (record.FileBytes == null)
                return UnprocessableEntity();
            
            return Ok(record);
        }
    }
}