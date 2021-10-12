using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PelitietokantaAnalytic
{
    class UserData
    {
        private List<Player> players;

        private UserData() { } //prevent parameterless contsruction

        public UserData(List<Player> players)
        {
            players = new List<Player>();
        }

        public ActivityReport GetActivityReport()
        {
            ActivityReport a = new ActivityReport();
            //To be implemented
            return a;
        }
        public ActivityReport GetActivityReport(DateTime start, DateTime end)
        {
            ActivityReport a = new ActivityReport();
            //To be implemented
            return a;
        }
    }
}
