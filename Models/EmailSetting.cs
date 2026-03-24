namespace AuthDemo.Models
{
    public class EmailSetting
    {
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public bool SendErrorEmail { get; set; }
        public bool EnableSsl { get; set; }
    }
}
