
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.ContactDomainServices
{
    public interface IContactDomainServices
    {
        ResultOfType<MailTemplateModel> SendMail(string from,string to,string subject,string body);
        ResultOfType<MessageModel> SendMotDePasseOublie(string destination);

        ResultOfType<MailTemplateModel> SendRendezVousVersOutlook(MailTemplateModel mail, string HDFrom, string HDTo);

    }
}
