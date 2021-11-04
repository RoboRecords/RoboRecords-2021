using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
        [HttpPost]
        [Route("submit")]
        public IActionResult Submit([FromQuery(Name = "File")] IFormFile file)
        {
            if (file is null)
                return NoContent();

            byte[] fileBytes;
            
            using (Stream fileStream = file.OpenReadStream())
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fileStream.CopyTo(memoryStream);
                fileBytes = memoryStream.ToArray();
            }

            RoboRecord record = new RoboRecord(new RoboUser("anon", 1234), fileBytes);
            if (record.FileBytes == null)
                return UnprocessableEntity();
            
            return Ok(record);
        }
    }
}