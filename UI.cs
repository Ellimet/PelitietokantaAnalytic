using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace PelitietokantaAnalytic
{
    public class UI
    {
        UserData userData;
        DatabaseManager databaseManager;
        string game;

        public UI() {

            databaseManager = new DatabaseManager();

            while (InputGame())
                InputCommand();

            if (databaseManager.isConnected)
                databaseManager.Disconnect();
            Environment.Exit(0);
        }

        private bool InputGame()
        {
            while (true)
            {
                Console.WriteLine("Insert game name or..\n0 to exit\n1 to list available games\n");
                string line = Console.ReadLine();
                if (line == "0") return false;
                if (line == "1")
                {
                    databaseManager.ListGames();
                }
                else
                {
                    if (databaseManager.GameExist(line))
                    {
                        game = line;
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Game doesn't exist. Try again.");
                    }
                }
            }
        }

        private void InputCommand()
        {

            while(true)
            { 
                Console.WriteLine("Selected game: " + game +
                    "\n\n0: Return to main menu" +
                    "\n1: View userdata\n");
                ConsoleKey cmd = Console.ReadKey().Key;
                Console.WriteLine();

                if (cmd == ConsoleKey.D0)
                    break;
                if (cmd == ConsoleKey.D1)
                {
                    if (userData == null)
                        userData = databaseManager.GenerateUserData(game);
                    Console.WriteLine("Analytics to be implemented..");
                    Console.WriteLine("\nPress any key to continue..");
                }

                Console.ReadKey();
            } 
        }
    }
    

}
