using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ContactAppServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.ServiceLayer.Models;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
  [RoutePrefix("contact")]
    public class ContactApiController : ApiController
    {
        #region Private Properties
        private readonly IContactAppServices _contactAppServices;

        #endregion

          #region Constructor
        public ContactApiController(IContactAppServices contactAppServices)
        {
            if (contactAppServices == null)
                throw new ArgumentNullException("contactAppServices");
            _contactAppServices = contactAppServices;
        }

        #endregion
      
    
        [Route("sendmail")]
        [HttpGet]
        public HttpResponseMessage SendMail(string from, string to, string sujet,string body)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _contactAppServices.SendMail(from, to, sujet, body);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("sendpassword")]
        [HttpPost]
        public HttpResponseMessage SendMailMotDePasseOublie(ContactDto contact)
        {
            var statusCode = HttpStatusCode.OK;

            var result = _contactAppServices.SendMail(contact.From,contact.To,contact.Sujet,contact.Body);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Post(ContactDto contactDto)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _contactAppServices.SendMail(contactDto.From, contactDto.To, contactDto.Sujet, contactDto.Body);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("exportoutlook")]
        [HttpGet]
        public HttpResponseMessage SendRendezVousVersOutlook(string adressePraticien,string body, string subject, string from, string to, string hdfrom, string hdt)
        {
            var statusCode = HttpStatusCode.OK;

            var mail = new MailTemplateModel
            {
                AdressePraticien = adressePraticien,
                Body = body,
                Subject = subject,
                To = to,
                From = from
            };

            var result = _contactAppServices.SendRendezVousVersOutlook(mail, hdfrom, hdt);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

    }
}
