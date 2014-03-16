using System.Data;
using MySql.Data.MySqlClient;
using PromoStudio.Data.Properties;

namespace PromoStudio.Data
{
    public class ConnectionManager : IConnectionManager
    {
        public IDbConnection GetConnection()
        {
            string connString = Settings.Default.ConnectionString;
            var conn = new MySqlConnection(connString);
            return conn;
        }
    }
}