using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.PraticienAppServices;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.ServiceLayer.Security;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
     [RoutePrefix("praticien")]
    public class PraticienApiController : ApiController
    {
        #region Private Properties
        private readonly IPraticienApplicationServices _praticinApiApplicationServices;
        #endregion

        #region Constructor

        public PraticienApiController(IPraticienApplicationServices praticinApiApplicationServices)
        {
            if (praticinApiApplicationServices == null)
                throw new ArgumentNullException("praticinApiApplicationServices");
            _praticinApiApplicationServices = praticinApiApplicationServices;
        }

         #endregion

        #region Public Methods

        [Route("praticiens")]
        public HttpResponseMessage GetPraticiens(int take = 0, int skip = 0)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _praticinApiApplicationServices.GetPraticiens(take,skip);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("add")]
        [HttpPost]
        public HttpResponseMessage Post(PraticienDTO praticienDto)
        {
          
            var statusCode = HttpStatusCode.OK;
            var result = _praticinApiApplicationServices.PostNewPraticien(praticienDto);

            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("update")]
        [HttpPatch]
        //[ApiBasicAuthenticationAttributeForPraticien]
        public HttpResponseMessage UpdatePraticien(PraticienDTO praticienDto)
        {

            var statusCode = HttpStatusCode.OK;
            var result = _praticinApiApplicationServices.PatchPraticien(praticienDto);

            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

        [Route("delete")]
        [HttpDelete]
        public HttpResponseMessage DeletePraticien(string cin)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _praticinApiApplicationServices.DeleteAPraticien(cin);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

         [Route("getpraticien")]
         [HttpGet]
        public HttpResponseMessage GetPraticienByEmail(string email)
        {
            var statusCode = HttpStatusCode.OK;
            var result = _praticinApiApplicationServices.GetPraticienByEmail(email);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }

         [Route("getpraticienparnom")]
         [HttpGet]
         public HttpResponseMessage GetPraticienByNomPrenom(string nomPrenom)
         {
             var statusCode = HttpStatusCode.OK;
             var result = _praticinApiApplicationServices.GetPraticienByNomPrenom(nomPrenom.Replace("%20"," "));
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

             return Request.CreateResponse(statusCode, result);
         }

         [Route("")]
         [HttpGet]
         public HttpResponseMessage GetPraticiensOpenSearch(string gouvernerat, string specialite, string nomPraticien, int takeForSearch = 0, int skipForSearch = 0)
         {
             var statusCode = HttpStatusCode.OK;
             var result = _praticinApiApplicationServices.SearchForPraticien(gouvernerat,specialite,nomPraticien, takeForSearch, skipForSearch);
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

             return Request.CreateResponse(statusCode, result);
         }

         [Route("searchfilter")]
         [HttpGet]
         public HttpResponseMessage GetListSpecialiteGouvernerat()
         {
             var statusCode = HttpStatusCode.OK;
             var result = _praticinApiApplicationServices.GetListSpecialiteGouvernerat();
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

             return Request.CreateResponse(statusCode, result);
         }

         [Route("getrendezvous")]
         [HttpGet]
         public HttpResponseMessage GetListOfRendezVousTousEnCours(string email)
         {
             var statusCode = HttpStatusCode.OK;
             var result = _praticinApiApplicationServices.GetListOfRendezVousTousEnCours(email);
             if (result != null)
                 statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

             return Request.CreateResponse(statusCode, result);
         }
        #endregion

    }
}
