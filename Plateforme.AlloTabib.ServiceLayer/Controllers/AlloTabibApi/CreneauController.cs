using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.CreneauAppServices;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
      [RoutePrefix("creneau")]
    public class CreneauController : ApiController
    {
        #region Private Properties
          private readonly ICreneauAppServices _creneauApiApplicationServices;

        #endregion

          public CreneauController(ICreneauAppServices creneauApiApplicationServices)
          {
              if (creneauApiApplicationServices == null)
                  throw new ArgumentNullException("creneauApiApplicationServices");

              _creneauApiApplicationServices = creneauApiApplicationServices;
          }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Post(IList<CreneauDTO> creneauDto)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<IList<CreneauResultDataModel>> result = _creneauApiApplicationServices.PostNewCreneau(creneauDto);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage Delete(string praticien, string dateCurrent, string heureDebut)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<Null> result = _creneauApiApplicationServices.DeleteCreneau(praticien, dateCurrent, heureDebut);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("ajoutercreneaux")]
        [HttpGet]
        public HttpResponseMessage PostCreneaux(string from, string to, string cinPraticien, string dateSelected)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<Null> result = _creneauApiApplicationServices.PostCreneaux(from,to,cinPraticien,dateSelected);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("postcreneaux")]
        [HttpGet]
        public HttpResponseMessage PostCreneauxJour(string from, string to, string cinPraticien, string jour)
        {
            var statusCode = HttpStatusCode.OK;

            ResultOfType<Null> result = _creneauApiApplicationServices.PostCreneauxJour(from, to, cinPraticien, jour);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }
    }
}
