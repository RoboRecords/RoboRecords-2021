using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoboRecords.Models;

namespace RoboRecords.DbInteraction
{
    public class DbTester
    {
        // Try adding Red Volcano Act 2 to Red Volcano Zone in SRB2 2.2
        public static void TestUpdate()
        {
            RoboGame Game = DbSelector.GetGameWithLevelsFromID("sonicroboblast2v22");

            if (Game == null)
                return;

            foreach (LevelGroup group in Game.LevelGroups)
            {
                if (group.Name == "Red Volcano Zone")
                {
                    // Dirty "check if exists" without knowing the ID
                    bool exists = false;
                    foreach (RoboLevel level in group.Levels)
                    {
                        if (level.Act == 2)
                            exists = true;
                    }

                    if (!exists)
                        group.Levels.Add(new RoboLevel(17, "Red Volcano Zone", 2)
                        { IconUrl = "../assets/images/mappics/" + RoboLevel.MakeMapString(16) + "P.png" } // Use Act 1's icon
                        );
                }
            }

            DbUpdater.UpdateGame(Game);
        }

        // Try adding a record to Red Volcano Zone Act 2 of 4:20.00 by ZeriTAS
        public static void TestRecord()
        {
            RoboLevel level = DbSelector.GetGameLevelFromMapId("sonicroboblast2v22", "17"); // Only exists if above method has been called.
            RoboRecord record = new RoboRecord(DbSelector.GetRoboUserFromUserName("ZeriTAS", 1), null)
            { Tics = 9100, Character = CharacterManager.GetCharacterById("amy") };

            if (level == null)
                return;

            // Dirty "check if exists" without knowing the ID
            bool exists = false;
            foreach (RoboRecord _record in level.Records)
            {
                if (_record.Uploader.UserNameNoDiscrim == "ZeriTAS" && _record.Tics == 9100)
                    exists = true;
            }

            if (!exists)
                DbInserter.AddRecordToLevel(record, level);
        }

        public static void TryReadPK3()
        {
            WadReader.GetMainCFGFromPK3(@"C:\SRB2\SL_CyberDimeRealm-v1.5.1.pk3");
        }

        public static void TryAddCyberdime()
        {
            RoboGame cyberGame = new RoboGame("Cyberdime Realm")
            {
                UrlName = "cyber",
                IconPath = @"../assets/images/cydmbg.png",
                LevelGroups = WadReader.GetMainCFGFromPK3(@"C:\SRB2\SL_CyberDimeRealm-v1.5.1.pk3")
            };
            DbInserter.AddGame(cyberGame);
        }

        // Try adding Red Volcano Act 2 with automatic sorting
        public static void TryAddRedVolcano2()
        {
            RoboGame Game = DbSelector.GetGameWithLevelsFromID("sonicroboblast2v22");
            RoboLevel level = DbSelector.GetGameLevelFromMapId("sonicroboblast2v22", "17");

            if (Game == null || level.LevelName != "Invalid Level")
                return;

            level = new RoboLevel(17, "Red Volcano Zone", 2)
                        { IconUrl = "../assets/images/mappics/" + RoboLevel.MakeMapString(16) + "P.png" };

            DbInserter.AddLevelToGame(level, Game);
        }
    }
}
