using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.JourFerieAppServices;
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
     [RoutePrefix("jourferie")]
    public class JourFerieController : ApiController
    {
        #region Private Properties
         private readonly IJourFerieAppServices _jourFerieAppServices;
        #endregion

         public JourFerieController(IJourFerieAppServices jourFerieAppServices)
         {
             if (jourFerieAppServices == null)
                 throw new ArgumentNullException("jourFerieAppServices");
             _jourFerieAppServices = jourFerieAppServices;
         }


         [Route("new")]
         [HttpPost]
         public HttpResponseMessage Post(JourFerieDTO jourFerieDTO)
         {
             var statusCode = HttpStatusCode.OK;

             ResultOfType<JourFerieResultDataModel> result = _jourFerieAppServices.PostNewJourFerie(jourFerieDTO);
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
             return Request.CreateResponse(statusCode, result);
         }

         [Route("delete")]
         [HttpGet]
         public HttpResponseMessage Delete(string jourName,string email)
         {
             var statusCode = HttpStatusCode.OK;

             ResultOfType<Null> result = _jourFerieAppServices.DeleteJourFerie(jourName,email);
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
             return Request.CreateResponse(statusCode, result);
         }

         [Route("estferie")]
         [HttpGet]
         public HttpResponseMessage EstUnJourFerie(string jourferie, string email)
         {
             var statusCode = HttpStatusCode.OK;

             ResultOfType<JourFerieResultDataModel> result = _jourFerieAppServices.EstUnJourFerie(jourferie, email);
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
             return Request.CreateResponse(statusCode, result);
         }
    }
}
