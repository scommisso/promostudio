using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PromoStudio.Data
{
    public class ConnectionManager : IConnectionManager
    {
        public ConnectionManager()
        {
        }

        public IDbConnection GetConnection()
        {
            string connString = PromoStudio.Data.Properties.Settings.Default.ConnectionString;
            var conn = new MySql.Data.MySqlClient.MySqlConnection(connString);
            return conn;
        }
    }
}
