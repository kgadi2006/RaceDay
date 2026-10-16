using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace RaceDayAPI.Data
{//start of namespace
    public class DatabaseConnection
    {//start of class
        private readonly string _connectionString;

        public DatabaseConnection(IConfiguration configuration)
        {
            _connectionString = configuration
                .GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "Database connection string was not found.");
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }//end of class
}//end of namespace
