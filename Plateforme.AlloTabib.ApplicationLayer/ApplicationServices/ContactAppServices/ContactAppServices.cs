using System;
using Plateforme.AlloTabib.DomainLayer.DomainServices.ContactDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ContactAppServices
{
    public class ContactAppServices : IContactAppServices
    {
        #region Private Properties

        private readonly IContactDomainServices _contactDomainServices;

        #endregion

        #region Constructor

        public ContactAppServices(IContactDomainServices contactDomainServices)
        {
            if (contactDomainServices == null)
                throw new ArgumentNullException("contactDomainServices");

            _contactDomainServices = contactDomainServices;
        }

        #endregion

        public ResultOfType<MailTemplateModel> SendMail(string from, string to,string subject, string body)
        {
            return _contactDomainServices.SendMail(from, to,subject, body);
        }


        public ResultOfType<MessageModel> SendMotDePasseOublie(string destination)
        {
            return _contactDomainServices.SendMotDePasseOublie(destination);
        }

        public ResultOfType<MailTemplateModel> SendRendezVousVersOutlook(MailTemplateModel mail, string HDFrom, string HDTo)
        {
            return _contactDomainServices.SendRendezVousVersOutlook(mail, HDFrom, HDTo);
        }
    }
}
