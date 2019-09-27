using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CalendrierAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PraticienAppServices;


namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
      [RoutePrefix("calendriers")]
    
    //Authoriser seulement les médecins à voir leurs comptes.
    public class CalendrierPraticienController : ApiController
    {
          #region Private Properties
          private readonly ICalendrierAppServices _calendrierApiApplicationServices;
          private readonly IPraticienApplicationServices _praticienApplicationServices;

          #endregion


          #region Constructor

          public CalendrierPraticienController(ICalendrierAppServices calendrierApiApplicationServices, IPraticienApplicationServices praticienApplicationServices)
          {
              if (calendrierApiApplicationServices == null)
                  throw new ArgumentNullException("calendrierApiApplicationServices");
              _calendrierApiApplicationServices = calendrierApiApplicationServices;
              if (praticienApplicationServices == null)
                  throw new ArgumentNullException("praticienApplicationServices");
              _praticienApplicationServices = praticienApplicationServices;
          }

          #endregion
          
          [Route("calendrier")]
          [HttpGet]
          //[ApiBasicAuthentication]
          //TODO ajouter un objet au lieu de la date statique !!!! TODO TODO TODO
          public HttpResponseMessage GetCalendarPatient(string praticien, string dateCourante)
          {
              var statusCode = HttpStatusCode.OK;
             
              var result = _calendrierApiApplicationServices.GetCalendrierParPraticienForPatient(praticien, dateCourante);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

              return Request.CreateResponse(statusCode, result);
          }

          [Route("calendriersem")]
          [HttpGet]
          //[ApiBasicAuthentication]
          //TODO ajouter un objet au lieu de la date statique !!!! TODO TODO TODO
          public HttpResponseMessage GetCalendarPatientParSem(string praticien, string dateCourante)
          {
              var statusCode = HttpStatusCode.OK;

              var result = _calendrierApiApplicationServices.GetCalendrierParPraticienForPatientParSem(praticien, dateCourante);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

              return Request.CreateResponse(statusCode, result);
          }
          [Route("calendriersemaine")]
          [HttpGet]
          //[ApiBasicAuthentication]
          //TODO ajouter un objet au lieu de la date statique !!!! TODO TODO TODO
          public HttpResponseMessage GetCalendarPatientParSemaine(string praticien, string dateCourante)
          {
              var statusCode = HttpStatusCode.OK;

              var result = _calendrierApiApplicationServices.GetCalendrierParPraticienForPatientParSemaine(praticien, dateCourante);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

              return Request.CreateResponse(statusCode, result);
          }

          [Route("calendrierpra")]
          [HttpGet]
          //[ApiBasicAuthenticationAttributeForPraticien]
          //TODO ajouter un objet au lieu de la date statique !!!! TODO TODO TODO
          public HttpResponseMessage GetCalendarPraticien(string praticien, string dateCourante)
          {
              var statusCode = HttpStatusCode.OK;

              var result = _calendrierApiApplicationServices.GetCalendrierParPraticienForPraticien(praticien, dateCourante);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

              return Request.CreateResponse(statusCode, result);
          }

          [Route("premieredisp")]
          [HttpGet]
          //[ApiBasicAuthenticationAttributeForPraticien]
          //TODO ajouter un objet au lieu de la date statique !!!! TODO TODO TODO
          public HttpResponseMessage PremiereDisponibilie(string praticien, string dateCourante)
          {
              var statusCode = HttpStatusCode.OK;

              var result = _calendrierApiApplicationServices.GetPremiereDisponibilite(praticien, dateCourante);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

              return Request.CreateResponse(statusCode, result);
          }
    }
}
