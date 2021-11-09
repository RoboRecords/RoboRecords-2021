using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RoboRecords.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        // TODO: Move these to a better location
        public void OnGet()
        {
            // Zenya's methods to test if database interactions work properly. These are not supposed to be called.
            //DbInteraction.DbTester.TestUpdate();
            //DbInteraction.DbTester.TestRecord(); // This will do nothing if TestUpdate isn't run first. This shouldn't cause a crash.
            //DbInteraction.DbTester.TryReadPK3(); // Reads the Cyberdime PK3 without adding it to the database.
            //DbInteraction.DbTester.TryAddCyberdime(); // Reads Cyberdime PK3 and adds Cyberdime Realm RoboGame to database.
        }
    }
}
