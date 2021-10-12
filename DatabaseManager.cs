using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelitietokantaAnalytic
{
    class DatabaseManager
    {
        private MySqlConnection connection;
        public bool isConnected;

        public void Connect()
        {
            // Tietokantayhteyden luominen
            string connetionString = null;
            connetionString = "server=localhost;database=pelianalytiikka;uid=root;pwd=turvaisa;SSLMode=None";
            connection = new MySqlConnection(connetionString);
            try
            {
                connection.Open();
                isConnected = true;
            }
            catch
            {
                Console.WriteLine("Cannot open connection ! ");
                throw;
            }
        }

        public void Disconnect()
        {
            connection.Close();
        }

        public MySqlDataReader Command(string command) //like "select * from pelaaja"
        {
            if (!isConnected)
                Connect();
            try
            {
                MySqlCommand cmd = new MySqlCommand(command, connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            catch (Exception e)
            {
                Console.WriteLine("Sql query catch: " + e);
                Console.ReadKey();
                throw;
            }
        }

        public bool GameExist(string name)
        {
            if (!isConnected)
                Connect();
            MySqlDataReader reader = Command($"select * from peli where nimi=\"{name}\"");
            bool found = reader.HasRows;
            reader.Close();
            return found;
        }

        public void ListGames()
        {
            if (!isConnected)
                Connect();
            MySqlDataReader reader = Command("select * from peli");
            int i = 1;
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Console.WriteLine($"{i++}. {reader.GetString(reader.GetOrdinal("nimi"))}");
                }
            }
            reader.Close();
            Console.WriteLine();
        }

        public UserData GenerateUserData(string game)
        {
            MySqlDataReader reader;

            List<Player> players = new List<Player>();
            double sessions_count = 0, purchases_count = 0;
            //Select players associated with the game
            reader = Command($" SELECT * FROM pelaaja p INNER JOIN pelisessio s ON p.id = s.pelaaja " +
                $"INNER JOIN peli ON peli.id = s.peli WHERE peli.nimi = \"{game}\"");

            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    Player p = new Player();
                    p.sessions = new List<Session>();
                    p.id = reader.GetInt32(reader.GetOrdinal("id"));
                    p.name = reader.GetString(reader.GetOrdinal("etunimi"));
                    p.surname = reader.GetString(reader.GetOrdinal("sukunimi"));
                    p.mail = reader.GetString(reader.GetOrdinal("sahkoposti"));
                    players.Add(p);
                    //Console.WriteLine("added player " + p.name);
                }
                reader.Close();

                //Select sessions in the game
                reader = Command($" SELECT * FROM pelisessio s INNER JOIN peli ON peli.id = s.peli WHERE peli.nimi = \"{game}\"");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(reader.GetOrdinal("peli")) == game)
                        {
                            Session s = new Session();
                            s.purchases = new List<Purchase>();
                            s.id = reader.GetInt32(reader.GetOrdinal("id"));
                            s.start = reader.GetDateTime(reader.GetOrdinal("alkuaika"));
                            s.end = reader.GetDateTime(reader.GetOrdinal("loppuaika"));
                            int ownerId = reader.GetInt32(reader.GetOrdinal("pelaaja"));
                            Player owner = players.Find(p => p.id == ownerId);
                            owner.sessions.Add(s);
                            sessions_count++;
                            //Console.WriteLine("added session " + s.id + " for " + owner.name);
                        }

                    }
                }
                reader.Close();

                //Select purchases in the game
                reader = Command($" SELECT * FROM rahasiirto r INNER JOIN pelisessio s ON r.sessio = s.id " +
                    $"INNER JOIN peli ON peli.id = s.peli WHERE peli.nimi = \"{game}\"");
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (reader.GetString(reader.GetOrdinal("peli")) == game)
                        {
                            Purchase p = new Purchase();
                            p.id = reader.GetInt32(reader.GetOrdinal("id"));
                            p.time = reader.GetDateTime(reader.GetOrdinal("aikaleima"));
                            p.amountUsd = reader.GetInt32(reader.GetOrdinal("summa"));
                            int sessionId = reader.GetInt32(reader.GetOrdinal("session"));
                            Session ownerSession = null;
                            Player owner = players.Find(x => (ownerSession = x.sessions.Find(s => s.id == sessionId)) != null);
                            if (ownerSession != null)
                            {
                                ownerSession.purchases.Add(p);
                                purchases_count++;
                                Console.WriteLine("added purchase of " + p.amountUsd + " for " + owner.name + " for session " + ownerSession.id);
                            }
                        }
                    }
                }
                reader.Close();
            }
            Console.WriteLine($"User data constructed. Players: {players.Count}, Sessions: {sessions_count}, Purchases: {purchases_count}");
            return new UserData(players); ;
        }
    }   
    
}
