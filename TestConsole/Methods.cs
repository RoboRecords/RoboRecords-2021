using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestConsole.Models;
using RoboRecords.Models;
using RoboRecords.DbInteraction;

namespace TestConsole
{
    public enum MenuAction
    {
        WriteLine,
        CallMenu,
        SelectRoboGames
    }
    class Methods
    {

        public void DrawMenu(Menu menu)
        {
            Console.Clear();

            Console.WriteLine(menu.name + "\n");
            for (int i = 0; i < menu.menuItems.Count; i++)
            {
                Console.WriteLine($"({i + 1}) {menu.menuItems[i].name}");
            }
            GetMenuAction(menu);
        }

        public void GetMenuAction(Menu menu)
        {
            bool validChoice = false;
            int yPos = 0;

            while (!validChoice)
            {
                Console.WriteLine("\nEnter your choice:");
                yPos = Console.GetCursorPosition().Top;
                int choice;
                validChoice = int.TryParse(Console.ReadLine(), out choice);
                if (!validChoice || choice < 1 || choice > menu.menuItems.Count)
                    validChoice = false;
                else
                {
                    DoMenuAction(menu.menuItems[choice - 1]);
                }
            }
        }

        // Zenya's methods to test if database interactions work properly. These are not supposed to be called.
        //DbInteraction.DbTester.TestUpdate();
        //DbInteraction.DbTester.TestRecord(); // This will do nothing if TestUpdate isn't run first. This shouldn't cause a crash.
        //DbInteraction.DbTester.TryReadPK3(); // Reads the Cyberdime PK3 without adding it to the database.
        //DbInteraction.DbTester.TryAddCyberdime(); // Reads Cyberdime PK3 and adds Cyberdime Realm RoboGame to database.
        //DbInteraction.DbTester.TryAddRedVolcano2(); // Add Red Volcano Act 2 with automatic Level Group sorting so it appears next to Act 1.
        //DbInteraction.DbTester.TryAddZenyaTheModerator(); // Doesn't work currently

        public void DoMenuAction(MenuItem item)
        {
            if (item.parameter == null)
                item.parameter = "";

            switch (item.action)
            {
                case MenuAction.WriteLine:
                    Console.WriteLine(item.parameter);
                    Console.ReadKey();
                    break;
                case MenuAction.CallMenu:
                    Console.WriteLine("New menu goes here");
                    Console.ReadKey();
                    break;
                case MenuAction.SelectRoboGames:
                    SelectRoboGames();
                    Console.ReadKey();
                    break;
            }
        }

        public void DoTestUpdate()
        {
            DbTester.TestUpdate();
        }

        public void DoTestRecord()
        {
            DbTester.TestRecord();
        }

        public void DoTryReadPK3()
        {
            DbTester.TryReadPK3();
        }

        public void DoTryAddCyberdime()
        {
            DbTester.TryAddCyberdime();
        }

        public void DoTryAddRedVolcano2()
        {
            DbTester.TryAddRedVolcano2();
        }

        public void DoTryAddZenyaTheModerator()
        {
            DbTester.TryAddZenyaTheModerator();
        }

        public void SelectRoboGames()
        {
            List<RoboGame> games = DbSelector.GetGames();

            Console.Clear();
            Console.WriteLine("Query Result:\n");
            for (int i = 0; i < games.Count; i++)
            {
                Console.WriteLine($"  {i + 1}  {games[i].Name}");
            }
        }
    }
}
