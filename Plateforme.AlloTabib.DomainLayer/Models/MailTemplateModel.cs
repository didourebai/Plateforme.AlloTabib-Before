
namespace Plateforme.AlloTabib.DomainLayer.Models
{
    public class MailTemplateModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public string AdressePraticien { get; set; }
    }
}
