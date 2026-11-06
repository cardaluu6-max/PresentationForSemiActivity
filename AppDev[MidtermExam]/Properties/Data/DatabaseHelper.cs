using Microsoft.Data.SqlClient;
using System.Data;

namespace AppDev_MidtermExam_.Properties.Data
{
    public class DatabaseHelper
    {
        private readonly IConfiguration _configuration;

        public DatabaseHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IDbConnection CreateConnection()
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection")!;
            return new SqlConnection(connectionString);
        }
    }
}
