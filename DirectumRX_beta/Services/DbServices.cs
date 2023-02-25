using Npgsql;
using System.Data;

namespace DirectumRX_beta.Services
{
    public class DbServices
    {
        public static string? NpgSqlConnectionString { get; private set; }
        public IConfiguration _configuration;

        public DbServices(IConfiguration configuration)
        {
            _configuration = configuration;
            NpgSqlConnectionString = _configuration.GetConnectionString("DirectumConnectionString");
        }

        public DataTable GetData(NpgsqlCommand cmd)
        {
            DataTable dataTable = new DataTable();
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(NpgSqlConnectionString))
                {
                    cmd.Connection = connection;
                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return dataTable;
        }
    }
}
