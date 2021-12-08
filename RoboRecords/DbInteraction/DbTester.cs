using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RoboRecords.Models;
using System.Diagnostics;
using RoboRecords.Services;

namespace RoboRecords.DbInteraction
{
    public class DbTester
    {
        static string cyberdimeFile = @"C:\SRB2\SL_CyberDimeRealm-v1.5.1.pk3";
        // Try adding Red Volcano Act 2 to Red Volcano Zone in SRB2 2.2
        public static void TestUpdate()
        {
            bool found = DbSelector.TryGetGameWithLevelsFromID("sonicroboblast2v22", out RoboGame Game);

            if (!found)
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

        // Try adding a record to Red Volcano Zone Act 1 of 4:20.00 by ZeriTAS
        public static void TestRecord()
        {
            bool found = DbSelector.TryGetGameLevelFromMapId("sonicroboblast2v22", "16", out RoboLevel level);
            DbSelector.TryGetRoboUserFromUserName("ZeriTAS", 1, out RoboUser roboUser);
            RoboRecord record = new RoboRecord(roboUser, null)
            { Tics = 9100, Character = CharacterManager.GetCharacterById("amy") };

            if (!found)
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
            if (File.Exists(cyberdimeFile))
                WadReader.GetMainCFGFromPK3(cyberdimeFile);
            else
                Console.WriteLine("Cyberdime PK3 not found at specified location");
        }

        public static void TryAddCyberdime()
        {
            if (File.Exists(cyberdimeFile))
            {
                RoboGame cyberGame = new RoboGame("Cyberdime Realm")
                {
                    UrlName = "cyber",
                    IconPath = @"../assets/images/cydmbg.png",
                    LevelGroups = WadReader.GetMainCFGFromPK3(cyberdimeFile)
                };
                DbInserter.AddGame(cyberGame);
            }
            else
                Console.WriteLine("Cyberdime PK3 not found at specified location");
        }

        // Try adding Red Volcano Act 2 with automatic sorting
        public static void TryAddRedVolcano2()
        {
            bool foundGame = DbSelector.TryGetGameWithLevelsFromID("sonicroboblast2v22", out RoboGame Game);
            bool foundLevel = DbSelector.TryGetGameLevelFromMapId("sonicroboblast2v22", "17", out RoboLevel level);

            if (!foundGame || foundLevel)
                return;

            level = new RoboLevel(17, "Red Volcano Zone", 2)
                        { IconUrl = "../assets/images/mappics/" + RoboLevel.MakeMapString(16) + "P.png" };

            DbInserter.AddLevelToGame(level, Game);
        }

        // Doesn't work, needs instance. Figure it out or I'll never be a moderator :[  --- Zenya
        public static void TryAddZenyaTheModerator()
        {
            Console.WriteLine("Test failed successfully.");
            //RoboUserManager.Create("zenya@zenya.zenya", "zenya", 2468, "secure-password1234", UserRoles.User | UserRoles.Moderator);
        }
    }

}
