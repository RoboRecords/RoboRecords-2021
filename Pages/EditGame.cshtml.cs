using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;

namespace RoboRecords.Pages
{
    public class EditGame : PageModel
    {
        [BindProperty]
        public GameEditDb GameEdit { get; set; }
        public static RoboGame Game;
        private RoboRecordsDbContext _dbContext;

        public EditGame(RoboRecordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void OnGet()
        {
            var roboGames = _dbContext.RoboGames.Include(e => e.LevelGroups).Include("LevelGroups.Levels").ToListAsync().Result;
            Game = roboGames[1]; // TODO: CHANGE THIS HARDCODED CRAP TO SUPPORT THE CURRENT SELECTED GAME
        }
    }
}