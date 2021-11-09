using Microsoft.EntityFrameworkCore;
using RoboRecords.DatabaseContexts;
using RoboRecords.Models;
using System.Collections.Generic;
using System.Linq;

namespace RoboRecords
{
    // Seed class. All testing data should be put here so it is initialized.
    // TODO: Make InitialData() compare with database and update records if needed.
    public static class Seed
    {
        public static void InitialData()
        {
            using (RoboRecordsDbContext context = new RoboRecordsDbContext())
            {
                // Games / Level Packs
                var roboGames = new List<RoboGame>
                {
                    new RoboGame("Sonic Robo Blast 2 v2.2") { IconPath = "../assets/images/gfz2bg.png" },
                    new RoboGame("srb2 Cyberdime Realm") { IconPath = "../assets/images/cydmbg.png" },
                    new RoboGame("Sonic Robo Blast 3 LUL") { IconPath = "../assets/images/dreamhill1.png" }
                };

                // Levels for vanilla SRB2
                var roboLevels = new List<RoboLevel>
                {
                    new RoboLevel(1, "Green Flower Zone", 1),
                    new RoboLevel(2, "Green Flower Zone", 2),
                    new RoboLevel(3, "Green Flower Zone", 3),

                    new RoboLevel(4, "Techno Hill Zone", 1),
                    new RoboLevel(5, "Techno Hill Zone", 2),
                    new RoboLevel(6, "Techno Hill Zone", 3),

                    new RoboLevel(7, "Deep Sea Zone", 1),
                    new RoboLevel(8, "Deep Sea Zone", 2),
                    new RoboLevel(9, "Deep Sea Zone", 3),

                    new RoboLevel(10, "Castle Eggman Zone", 1),
                    new RoboLevel(11, "Castle Eggman Zone", 2),
                    new RoboLevel(12, "Castle Eggman Zone", 3),

                    new RoboLevel(13, "Arid Canyon Zone", 1),
                    new RoboLevel(14, "Arid Canyon Zone", 2),
                    new RoboLevel(15, "Arid Canyon Zone", 3),

                    new RoboLevel(16, "Red Volcano Zone", 0),

                    new RoboLevel(22, "Egg Rock Zone", 1),
                    new RoboLevel(23, "Egg Rock Zone", 2),

                    new RoboLevel(25, "Black Core Zone", 1),
                    new RoboLevel(26, "Black Core Zone", 2),
                    new RoboLevel(27, "Black Core Zone", 3),

                    new RoboLevel(30, "Frozen Hillside Zone", 0),
                    new RoboLevel(31, "Pipe Towers Zone", 0),
                    new RoboLevel(32, "Forest Fortress Zone", 0),
                    new RoboLevel(33, "Techno Legacy Zone", 0),

                    new RoboLevel(40, "Haunted Heights Zone", 0),
                    new RoboLevel(41, "Aerial Garden Zone", 0),
                    new RoboLevel(42, "Azure Temple Zone", 0)
                };

                var levelGroups = new List<LevelGroup>
                {
                    new LevelGroup("Green Flower Zone", new List<RoboLevel>(roboLevels.Take(3))),
                    new LevelGroup("Techno Hill Zone", new List<RoboLevel>(roboLevels.Skip(3).Take(3))),
                    new LevelGroup("Deep Sea Zone", new List<RoboLevel>(roboLevels.Skip(6).Take(3))),
                    new LevelGroup("Castle Eggman Zone", new List<RoboLevel>(roboLevels.Skip(9).Take(3))),
                    new LevelGroup("Arid Canyon Zone", new List<RoboLevel>(roboLevels.Skip(12).Take(3))),
                    new LevelGroup("Red Volcano Zone", new List<RoboLevel>(roboLevels.Skip(15).Take(1))),
                    new LevelGroup("Egg Rock Zone", new List<RoboLevel>(roboLevels.Skip(16).Take(2))),
                    new LevelGroup("Black Core Zone", new List<RoboLevel>(roboLevels.Skip(18).Take(3))),
                    new LevelGroup("Bonus Levels", new List<RoboLevel>(roboLevels.Skip(21).Take(4)), true),
                    new LevelGroup("Challenge Levels", new List<RoboLevel>(roboLevels.Skip(25).Take(3)), true)
                };

                // Sample users
                var roboUsers = new List<RoboUser>
                {
                    new RoboUser("Lemon", 1),
                    new RoboUser("Easy Mode SRB2", 1),
                    new RoboUser("ZeriTAS", 1),
                    new RoboUser("R3FR1G3R4T0R", 1),
                    new RoboUser("Emily", 1),
                    new RoboUser("DUCKERS", 1),
                    new RoboUser("Horse", 1),
                    new RoboUser("Red Sonic", 1),
                };

                // Sample records
                var roboRecords = new List<RoboRecord>
                {
                    new RoboRecord(roboUsers[0], null) {Tics = 420, Character = CharacterManager.GetCharacterById("sonic")},
                    new RoboRecord(roboUsers[3], null) {Tics = 421, Character = CharacterManager.GetCharacterById("sonic")},
                    new RoboRecord(roboUsers[1], null) {Tics = 419, Character = CharacterManager.GetCharacterById("tails")},
                    new RoboRecord(roboUsers[7], null) {Tics = 666, Character = CharacterManager.GetCharacterById("knuckles")},
                    new RoboRecord(roboUsers[7], null) {Tics = 6969, Character = CharacterManager.GetCharacterById("knuckles")},
                    new RoboRecord(roboUsers[2], null) {Tics = 6969, Character = CharacterManager.GetCharacterById("amy")},
                    new RoboRecord(roboUsers[4], null) {Tics = 54321, Character = CharacterManager.GetCharacterById("fang")},
                    new RoboRecord(roboUsers[5], null) {Tics = 12345, Character = CharacterManager.GetCharacterById("metalsonic")},
                    new RoboRecord(roboUsers[3], null) {Tics = 1337, Character = CharacterManager.GetCharacterById("sonic")},
                    new RoboRecord(roboUsers[3], null) {Tics = 1337, Character = CharacterManager.GetCharacterById("sonic")},
                    new RoboRecord(roboUsers[6], null) {Tics = 7331, Character = CharacterManager.GetCharacterById("sonic")}
                };

                // Add the records to their levels.
                for (int i = 0; i < roboRecords.Count; i++)
                {
                    if (i == 6)
                    {
                        roboLevels[1].Records.Add(roboRecords[i]); // Add the 7th record to GFZ2. #Fang Gang
                    }
                    else
                    {
                        roboLevels[0].Records.Add(roboRecords[i]); // Add every other record to GFZ1.
                    }
                }

                // Add everything to the context and save to database.
                foreach (var level in roboLevels)
                    context.RoboLevels.Add(level);

                foreach (var group in levelGroups)
                    roboGames[0].LevelGroups.Add(group);

                foreach (var game in roboGames)
                    context.RoboGames.Add(game);

                foreach (var group in levelGroups)
                    context.LevelGroups.Add(group);

                foreach (var user in roboUsers)
                    context.RoboUsers.Add(user);

                context.SaveChangesAsync();
            }
        }
    }
}
