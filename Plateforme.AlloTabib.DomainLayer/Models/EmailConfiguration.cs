namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class EmailConfiguration
    {
        public string SmtpServerAddress { get; set; }

        public string SmtpPort { get; set; }

        public string EmailSubject { get; set; }

        public string EmailSender { get; set; }

        public string SmtpUsername { get; set; }

        public string SmtpPassword { get; set; }

        public bool WithCredentialsFlag { get; set; }

        public string Destination { get; set; }
    }
}
