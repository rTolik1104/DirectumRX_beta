namespace DirectumRX_beta.Models
{
    public class FileModel
    {
        public string? FileName { get; set; }
        public string? RegistrationNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public DateTime? SignDate { get; set; }
        public string? FilePath { get; set; }
        public string? SignatoryName { get; set; }
        public string? SignId { get; set; }
    }
}
