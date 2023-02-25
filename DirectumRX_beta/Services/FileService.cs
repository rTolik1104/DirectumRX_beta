using Npgsql;
using System.Data;


namespace DirectumRX_beta.Services
{
    public class FileService
    {
        private readonly DbServices _dbServices;
        private readonly IWebHostEnvironment _webHost;
        private IConfiguration _configuration;

        string storage_path = "";

        public FileService(DbServices dbServices, IWebHostEnvironment webHost, IConfiguration configuration)
        {
            _dbServices = dbServices;
            _webHost = webHost;
            _configuration = configuration;
            storage_path = _configuration.GetSection("StoragePath").Value.ToString();
            storage_path=storage_path.Replace("/", @"\");
        }

        public string GetDocumentBodyId(int id, string password)
        {
            try
            {
                string sql = $"SELECT publicbody_id FROM sungero_content_edocversion WHERE edoc={id} AND public_password='{password}'";
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
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string GetDocumentPath(int id, string password)
        {
            var docBodyId = GetDocumentBodyId(id, password);
            var uploadFolder = Path.Combine(_webHost.WebRootPath, @"files\");
            string? checkFile = Directory.GetFiles(uploadFolder, $"{docBodyId}.pdf", SearchOption.AllDirectories).FirstOrDefault();

            if (checkFile != null)
            {
                return $"{docBodyId}.pdf";
            }

            if (!String.IsNullOrEmpty(docBodyId))
            {
                var path = $@"{storage_path}\";
                var fileName = docBodyId + ".blob";


                string? file = Directory.GetFiles(path, fileName, SearchOption.AllDirectories).FirstOrDefault();
                var fileNewPath = uploadFolder + fileName;
                var pdfFile = uploadFolder + docBodyId + ".pdf";

                if (!File.Exists(pdfFile))
                {
                    File.Copy(file, fileNewPath);
                }

                string? newFile = Directory.GetFiles(uploadFolder, fileName, SearchOption.AllDirectories).FirstOrDefault();
                if (newFile != null)
                {
                    var currentFile = newFile.Substring(0, newFile.Length - 4);
                    currentFile += "pdf";

                    if (!File.Exists(currentFile))
                    {
                        File.Move(newFile, Path.ChangeExtension(newFile, ".pdf"));
                    }
                }
                string? resultPath = $"{docBodyId}.pdf";
                return resultPath;
            }
            return string.Empty;
        }

        public string GetDocumentName(int id)
        {
            try
            {
                string sql = $"SELECT name  FROM sungero_content_edoc WHERE id = {id}";
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
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string GetSignatoryFullName(int id)
        {
            try
            {
                string sql = $"SELECT signer_name  FROM eimzo_data WHERE edoc_id = {id} ORDER BY id";
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
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string GetRegistrationNumber(int id)
        {
            try
            {
                string sql = $"SELECT regnumber_docflow_sungero FROM sungero_content_edoc WHERE id = {id}";
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql))
                {
                    DataTable dataTabel = _dbServices.GetData(cmd);
                    string? regNumber = "";
                    if (dataTabel != null)
                    {
                        foreach (DataRow row in dataTabel.Rows)
                        {
                            regNumber = row["regnumber_docflow_sungero"].ToString();
                        }
                    }
                    return regNumber;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string GetDocumentDate(int id)
        {
            try
            {
                string sql = $"SELECT created FROM sungero_content_edoc WHERE id = {id}";
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
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }

        public string GetSignDate(int id)
        {
            try
            {
                string sql = $"SELECT sign_date  FROM eimzo_data WHERE edoc_id = {id} ORDER BY id";
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
                Console.WriteLine(ex.Message);
            }
            return string.Empty;
        }
    }
}
