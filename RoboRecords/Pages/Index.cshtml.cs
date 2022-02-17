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
            // Test of record selection. Check console or log on run.
            DbSelector.TryGetRoboRecordFromDbId(12, out RoboRecord record);

            Logger.Log(record.ToString(), true);
            Logger.Log(record.ToStringDetailed(), true);

            DbSelector.TryGetRoboRecordWithDataFromDbId(12, out RoboRecord recordData);

            Logger.Log(recordData.ToString(), true);
            Logger.Log(recordData.ToStringDetailed(), true);
        }
    }
}
