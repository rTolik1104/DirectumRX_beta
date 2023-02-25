using Npgsql;
using System.Data;

namespace DirectumRX_beta.Services
{
    public class LocalDbServices
    {
        public static string? NpgSqlConnectionString { get; private set; }
        public IConfiguration _configuration;

        public LocalDbServices(IConfiguration configuration)
        {
            _configuration = configuration;
            NpgSqlConnectionString = _configuration.GetConnectionString("DirectumConnectionStringLocal");
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

        public Int64 CRUD(NpgsqlCommand cmd)
        {
            try
            {
                using (NpgsqlConnection connection = new NpgsqlConnection(NpgSqlConnectionString))
                {
                    if (connection.State == ConnectionState.Closed) connection.Open();

                    cmd.Connection = connection;
                    object result = cmd.ExecuteScalar();

                    if (int.TryParse("" + result, out int res))
                    {
                        if (res > 0)
                        {
                            return res;
                        }
                    }
                }
            }
            catch (Exception ex) { Console.WriteLine(ex); }
            return 0;
        }
    }
}
