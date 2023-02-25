using Npgsql;
using System.Data;

namespace DirectumRX_beta.Services
{
    public class LocalFileServices
    {
        private readonly LocalDbServices _dbServices;
        private readonly ILogger _logger;


        public LocalFileServices(LocalDbServices dbServices, ILogger<LocalFileServices> logger)
        {
            _dbServices = dbServices;
            _logger = logger;
        }

        public string GetDocumentBodyId(int id, string password)
        {
            try
            {
                string sql = $"SELECT publicbody_id FROM local_directum_data WHERE edoc_id={id} AND password='{password}'";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? bodyId = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            bodyId = row["publicbody_id"].ToString();
                        }
                    }
                    return bodyId;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get document body id from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public string GetDocumentPath(int id, string password)
        {
            var docBodyId = GetDocumentBodyId(id, password);
            return $"{docBodyId}.pdf";
        }

        public string GetDocumentName(int id)
        {
            try
            {
                string sql = $"SELECT name  FROM local_directum_data WHERE edoc_id = {id}";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? name = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            name = row["name"].ToString();
                        }
                    }
                    return name;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get document name from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public string GetSignatoryFullName(int id)
        {
            try
            {
                string sql = $"SELECT signer_name  FROM local_directum_data WHERE edoc_id = {id}";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? signatoryName = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            signatoryName = row["signer_name"].ToString();
                        }
                    }
                    return signatoryName;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get signer name from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public string GetRegistrationNumber(int id)
        {
            try
            {
                string sql = $"SELECT register_number FROM local_directum_data WHERE id = {id}";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? regNumber = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            regNumber = row["register_number"].ToString();
                        }
                    }
                    return regNumber;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get reg number from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public string GetDocumentDate(int id)
        {
            try
            {
                string sql = $"SELECT created FROM local_directum_data WHERE edoc_id = {id}";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? createdDate = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            createdDate = row["created"].ToString();
                        }
                    }
                    return createdDate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get document date from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public string GetSignDate(int id)
        {
            try
            {
                string sql = $"SELECT sign_date  FROM local_directum_data WHERE edoc_id = {id} ORDER BY id";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? signingDate = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            signingDate = row["sign_date"].ToString();
                        }
                    }
                    return signingDate;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("The get sign date from table (eimzo_data) failed: " + ex.Message);
            }
            return string.Empty;
        }

        public void InsertFileData(int edoc_id, string publicbodyId, string register, string created_date, string signed_date, string name, string signer_name, string password)
        {
            try
            {
                string sql = $"INSERT INTO local_directum_data(edoc_id, publicbody_id, register_number, created, sign_date, name, signer_name, password) VALUES({edoc_id}, '{publicbodyId}', '{register}', '{created_date}', '{signed_date}', '{name}', '{signer_name}', '{password}')";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    _dbServices.CRUD(cmd);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("The insert data to table (eimzoa_data) failed: " + ex);
            }
        }
    }
}
