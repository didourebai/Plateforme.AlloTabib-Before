using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CalendrierAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PatientAppServices;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.RendezVousAppServices;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
    [RoutePrefix("patient")]
    public class PatientApiController : ApiController
    {
        #region Private Properties
        private readonly IPatientApplicationServices _patientApiApplicationServices;
        private readonly ICalendrierAppServices _calendrierApiApplicationServices;
        private readonly IRendezVousAppServices _rendezVousApiApplicationServices;

        #endregion

        #region Constructor
        public PatientApiController(IPatientApplicationServices patientApiApplicationServices, ICalendrierAppServices calendrierApiApplicationServices, IRendezVousAppServices rendezVousApiApplicationServices)
        {
            if (patientApiApplicationServices == null)
                throw new ArgumentNullException("patientApiApplicationServices");
            _patientApiApplicationServices = patientApiApplicationServices;
            if (calendrierApiApplicationServices == null)
                throw new ArgumentNullException("calendrierApiApplicationServices");
            _calendrierApiApplicationServices = calendrierApiApplicationServices;
            if (rendezVousApiApplicationServices == null)
                throw new ArgumentNullException("rendezVousApiApplicationServices");
            _rendezVousApiApplicationServices = rendezVousApiApplicationServices;
        }

        #endregion

        #region Public Methods


        [Route("patients")]
        public HttpResponseMessage GetPatients()
        {
            var statusCode = HttpStatusCode.OK;
            var result = _patientApiApplicationServices.GetPatients();
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Post(PatientDTO patientDto)
        {
            var statusCode = HttpStatusCode.OK;
      
            ResultOfType<PatientResultDataModel> result = _patientApiApplicationServices.PostNewPatient(patientDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("update")]
        [HttpPatch]
        public HttpResponseMessage Patch(PatientDTO patientDto)
        {
            var statusCode = HttpStatusCode.OK;
           
            ResultOfType<PatientResultDataModel> result = _patientApiApplicationServices.PatchPatient(patientDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeletePatient(string  cin)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _patientApiApplicationServices.DeletePatient(cin);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("getpatient")]
        [HttpGet]
        public HttpResponseMessage GetPatientByEmail(string email)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _patientApiApplicationServices.GetPatientByEmail(email);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("getrendezvous")]
        [HttpGet]
        public HttpResponseMessage GetAllRendezVousParPatientEnCours(string email)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _rendezVousApiApplicationServices.GetAllRendezVousParPatientEnCours(email);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }
        #endregion

      
    }
}
