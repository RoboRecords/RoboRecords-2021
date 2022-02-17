using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoboRecords.DbInteraction;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class IndexModel : RoboPageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger, IHttpContextAccessor httpContextAccessor) : base(httpContextAccessor)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // Test of record selection with accompanying data. Check console or log on run.
            DbSelector.TryGetRoboRecordWithDataFromDbId(3, out RoboRecord record);

            string levelInfo = $"{record.Level.LevelGroup.RoboGame.Name} - {record.Level} - "
                + $"{RoboRecord.GetTimeFromTics(record.Tics)} as {record.Character.Name} "
                + $"by {record.Uploader.UserNameNoDiscrim}. "
                + $"It is {((record.Level.Nights != true) ? "not" : "" )} a NiGHTS record.";

            Logger.Log(levelInfo, true);
        }
    }
}
