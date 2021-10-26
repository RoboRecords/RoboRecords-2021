using System;
using System.Collections.Generic;
using System.Web;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using RoboRecords.Services;

namespace RoboRecords.Pages
{
    public class Map : PageModel
    {
        public static RoboGame CurrentGame;
        public static RoboLevel CurrentLevel;
        private List<RoboGame> _roboGames;

        private RoboRecordsDbContext _dbContext;

        public Map(RoboRecordsDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public void OnGet()
        {
            CurrentGame = new RoboGame("Invalid Game");
            CurrentLevel = new RoboLevel(-1,"Invalid level", 0);
            
                        var record1 = new RoboRecord(new RoboUser("Lemon", 1), null);
            record1.Tics = 420;
            record1.Character = CharacterManager.GetCharacterById("sonic");
            
            var record2 = new RoboRecord(new RoboUser("R3FR1G3R4T0R", 1), null);
            record2.Tics = 421;
            record2.Character = CharacterManager.GetCharacterById("sonic");
            
            var record3 = new RoboRecord(new RoboUser("Easy mode SRB2", 1), null);
            record3.Tics = 419;
            record3.Character = CharacterManager.GetCharacterById("tails");
            
            var record4 = new RoboRecord(new RoboUser("Red Sonic", 1), null);
            record4.Tics = 666;
            record4.Character = CharacterManager.GetCharacterById("knuckles");
            
            var record5 = new RoboRecord(new RoboUser("Red Sonic", 1), null);
            record5.Tics = 6969;
            record5.Character = CharacterManager.GetCharacterById("knuckles");
            
            var record6 = new RoboRecord(new RoboUser("ZeriTAS", 1), null);
            record6.Tics = 6969;
            record6.Character = CharacterManager.GetCharacterById("amy");
            
            var record7 = new RoboRecord(new RoboUser("Emily", 1), null);
            record7.Tics = 54321;
            record7.Character = CharacterManager.GetCharacterById("fang");
            
            var record8 = new RoboRecord(new RoboUser("DUCKERS", 1), null);
            record8.Tics = 12345;
            record8.Character = CharacterManager.GetCharacterById("metalsonic");
            
            var record9 = new RoboRecord(new RoboUser("R3FR1G3R4T0R", 1), null);
            record9.Tics = 1337;
            record9.Character = CharacterManager.GetCharacterById("sonic");
            
            var recordA = new RoboRecord(new RoboUser("R3FR1G3R4T0R", 1), null);
            recordA.Tics = 1337;
            recordA.Character = CharacterManager.GetCharacterById("sonic");
            
            var recordB = new RoboRecord(new RoboUser("Horse", 1), null);
            recordB.Tics = 7331;
            recordB.Character = CharacterManager.GetCharacterById("sonic");
            
            var gfz1 = new RoboLevel(1, "Green Flower Zone", 1);
            gfz1.Records.Add(record1);
            gfz1.Records.Add(record2);
            gfz1.Records.Add(record3);
            gfz1.Records.Add(record4);
            gfz1.Records.Add(record5);
            gfz1.Records.Add(record6);
            gfz1.Records.Add(record7);
            gfz1.Records.Add(record8);
            gfz1.Records.Add(recordA);
            gfz1.Records.Add(recordB);
            var gfz2 = new RoboLevel(2, "Green Flower Zone", 2);
            gfz2.Records.Add(record9);
            var gfz3 = new RoboLevel(3, "Green Flower Zone", 3);
            
            var thz1 = new RoboLevel(4, "Techno Hill Zone", 1);
            var thz2 = new RoboLevel(5, "Techno Hill Zone", 2);
            var thz3 = new RoboLevel(6, "Techno Hill Zone", 3);
            
            var dsz1 = new RoboLevel(7, "Techno Hill Zone", 1);
            var dsz2 = new RoboLevel(8, "Techno Hill Zone", 2);
            var dsz3 = new RoboLevel(9, "Techno Hill Zone", 3);
            
            var cez1 = new RoboLevel(10, "Castle Eggman Zone", 1);
            var cez2 = new RoboLevel(11, "Castle Eggman Zone", 2);
            var cez3 = new RoboLevel(12, "Castle Eggman Zone", 3);
            
            var acz1 = new RoboLevel(13, "Arid Canyon Zone", 1);
            var acz2 = new RoboLevel(14, "Arid Canyon Zone", 2);
            var acz3 = new RoboLevel(15, "Arid Canyon Zone", 3);
            
            var rvz = new RoboLevel(16, "Red Volcano Zone", 0);
            
            var erz1 = new RoboLevel(22, "Egg Rock Zone", 1);
            var erz2 = new RoboLevel(23, "Egg Rock Zone", 2);
            
            var bcz1 = new RoboLevel(25, "Black Core Zone", 1);
            var bcz2 = new RoboLevel(26, "Black Core Zone", 2);
            var bcz3 = new RoboLevel(27, "Black Core Zone", 3);
            
            var fhz = new RoboLevel(30, "Frozen Hillside Zone", 0);
            var ptz = new RoboLevel(31, "Pipe Towers Zone", 0);
            var ffz = new RoboLevel(32, "Forest Fortress Zone", 0);
            var tlz = new RoboLevel(33, "Techno Legacy Zone", 0);
            
            var hhz = new RoboLevel(40, "Haunted Heights Zone", 0);
            var agz = new RoboLevel(41, "Aerial Garden Zone", 0);
            var atz = new RoboLevel(42, "Azure Temple Zone", 0);
            
            var levelGroupGfz = new LevelGroup(name: "Green Flower Zone", new List<RoboLevel>{gfz1, gfz2, gfz3});
            var levelGroupThz = new LevelGroup(name: "Techno Hill Zone", new List<RoboLevel>{thz1, thz2, thz3});
            var levelGroupDsz = new LevelGroup(name: "Deep Sea Zone", new List<RoboLevel>{dsz1, dsz2, dsz3});
            var levelGroupCez = new LevelGroup(name: "Castle Eggman Zone", new List<RoboLevel>{cez1, cez2, cez3});
            var levelGroupAcz = new LevelGroup(name: "Arid Canyon Zone", new List<RoboLevel>{acz1, acz2, acz3});
            var levelGroupRvz = new LevelGroup(name: "Red Volcano Zone", new List<RoboLevel>{rvz});
            var levelGroupErz = new LevelGroup(name: "Egg Rock Zone", new List<RoboLevel>{erz1, erz2});
            var levelGroupBcz = new LevelGroup(name: "Black Core Zone", new List<RoboLevel>{bcz1, bcz2, bcz3});
            var levelGroupBonus = new LevelGroup(name: "Bonus Levels", new List<RoboLevel>{fhz, ptz, ffz, tlz}, true);
            var levelGroupChallenge = new LevelGroup(name: "Challenge Levels", new List<RoboLevel>{hhz, agz, atz}, true);

            var groups = new List<LevelGroup> {levelGroupGfz, levelGroupThz, levelGroupDsz, levelGroupCez, levelGroupAcz, levelGroupRvz, levelGroupErz, levelGroupBcz, levelGroupBonus, levelGroupChallenge};

            //Save hardcoded game data to the database
            /*_roboGames = new List<RoboGame>();
            _roboGames.Add(new RoboGame("Sonic Robo Blast 2 v2.2"){LevelGroups = groups, IconPath = "../assets/images/gfz2bg.png"});
            _roboGames.Add(new RoboGame("srb2 Cyberdime Realm"){IconPath = "../assets/images/cydmbg.png"});
            _roboGames.Add(new RoboGame("Sonic Robo Blast 3 LUL"){IconPath = "../assets/images/dreamhill1.png"});
            foreach (RoboGame game in _roboGames)
            {
                _dbContext.RoboGames.Add(game);
            }

            _dbContext.SaveChanges();*/

            //Get the data from the database
            //FIXME: This doesn't feel like the right way to do it, figure out how this works better
            _dbContext.RoboRecords.Include(e => e.Uploader).ToListAsync().Wait();
            _dbContext.RoboRecords.Include(e => e.Character).ToListAsync().Wait();
            _dbContext.RoboRecords.Include(e => e.Uploader).ToListAsync().Wait();
            _dbContext.LevelGroups.Include(e => e.Levels).ToListAsync().Wait();
            _roboGames = _dbContext.RoboGames.Include(e => e.LevelGroups).ToListAsync().Result;

            var gameId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("game");
            var mapId = HttpUtility.ParseQueryString(Request.QueryString.ToString()).Get("map");

            if (gameId != null)
            {
                var roboGame = _roboGames.Find(game => game.UrlName == gameId);
                if (roboGame != null)
                {
                    CurrentGame = roboGame;
                    if (mapId != null)
                    {
                        int parsedLevel = -1;
                        if (int.TryParse(mapId, out parsedLevel))
                        {
                            var roboLevel = roboGame.GetLevelByNumber(parsedLevel);
                            if (roboLevel != null)
                            {
                                CurrentLevel = roboLevel;
                            }
                        }
                    }
                }
            }
        }
    }
}