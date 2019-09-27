using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.RendezVousAppServices;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.ServiceLayer.Security;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
    [RoutePrefix("rendezvous")]
    //[ApiBasicAuthentication]
    public class RendezVousController : ApiController
    {
        #region Private Properties
        private readonly IRendezVousAppServices _rendezVousApiApplicationServices;

        #endregion

        public RendezVousController(IRendezVousAppServices rendezVousApiApplicationServices)
        {
            if (rendezVousApiApplicationServices == null)
                throw new ArgumentNullException("rendezVousApiApplicationServices");

            _rendezVousApiApplicationServices = rendezVousApiApplicationServices;
        }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Post( RendezVousDTO rendezVousDto)
        {
            var statusCode = HttpStatusCode.OK;
           
            ResultOfType<RendezVousResultDataModel> result = _rendezVousApiApplicationServices.PostNewRendezVous(rendezVousDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(string rendezVousDto)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<Null> result = _rendezVousApiApplicationServices.DeleteRendezVous(rendezVousDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("update")]
        [HttpPatch]
        public HttpResponseMessage Patch(RendezVousDTO rendezVousDto)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<RendezVousResultDataModel> result = _rendezVousApiApplicationServices.PatchNewRendezVous(rendezVousDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("getpatients")]
        [HttpGet]
        public HttpResponseMessage GetPatients(string praticien)
        {
              var statusCode = HttpStatusCode.OK;

              ResultOfType<PatientResultModel> result = _rendezVousApiApplicationServices.GetPatientsParPraticien(praticien);
              if (result != null)
                  statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
              return Request.CreateResponse(statusCode, result);

        }


        [Route("getrendezvous")]
        [HttpGet]
        public HttpResponseMessage GetRendezVous(string praticien, string dateCurrent)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<RendezVousResultModel> result = _rendezVousApiApplicationServices.GetRendezVousByDateAndPraticien(praticien, dateCurrent);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);

        }

        [Route("creneauhasrdv")]
        [HttpGet]
        public HttpResponseMessage CreneauAyantRendezVous(string praticien, string dateCurrent, string heureDebut)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<RendezVousResultDataModel> result = _rendezVousApiApplicationServices.CreneauAyantRendezVous(praticien, dateCurrent,heureDebut);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);

        }

        [Route("rendezvousnonconfirme")]
        [HttpGet]
        public HttpResponseMessage GetAllRendezVousNonConfirmeOuRejete(string praticien)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<RendezVousResultModel> result = _rendezVousApiApplicationServices.GetAllRendezVousNonConfirmeOuRejete(praticien);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);

        }


    }
}
