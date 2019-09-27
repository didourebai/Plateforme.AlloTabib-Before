
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ContactAppServices
{
    public interface IContactAppServices
    {
        ResultOfType<MailTemplateModel> SendMail(string from, string To,string subject, string body);
        ResultOfType<MessageModel> SendMotDePasseOublie(string destination);
        ResultOfType<MailTemplateModel> SendRendezVousVersOutlook(MailTemplateModel mail, string HDFrom, string HDTo);
    }
}
