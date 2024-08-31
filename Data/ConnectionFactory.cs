using MySql.Data.MySqlClient;
using System.Data;

namespace FoodFestAPI.Data
{
    public interface IConnectionFactory
    {
        public Task<IDbConnection> CreateConnection(); 
    }

    public class ConnectionFactory : IConnectionFactory
    {
        private readonly string _connString;
        public ConnectionFactory(string connString)
        {
            _connString = connString;
        }

        public async Task<IDbConnection> CreateConnection()
        {
            try
            {
                var connection = new MySqlConnection(_connString);
                return connection;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
