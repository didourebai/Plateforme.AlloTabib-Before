using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.ConfigurationAppServices;
using Plateforme.AlloTabib.ApplicationLayer.DTO;
using Plateforme.AlloTabib.DomainLayer.Models;
using Plateforme.AlloTabib.ServiceLayer.Security;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
    
    [RoutePrefix("configuration")]
    //[ApiBasicAuthenticationAdminAttribute]
    public class ConfigurationController : ApiController
    {
        #region Private Properties

        private readonly IConfigurationAppServices _configurationApiApplicationServices;

        #endregion

        public ConfigurationController(IConfigurationAppServices configurationApiApplicationServices)
        {
            if (configurationApiApplicationServices == null)
                throw new ArgumentNullException("configurationApiApplicationServices");
            _configurationApiApplicationServices = configurationApiApplicationServices;
        }

        [Route("new")]
        [HttpPost]
        public HttpResponseMessage Post(ConfigurationPraticienDto configurationDTO)
        {
         
            var statusCode = HttpStatusCode.OK;

            ResultOfType<ConfigurationResultDataModel> result = _configurationApiApplicationServices.PostNewConfiguration(configurationDTO);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("dimanche")]
        [HttpGet]
        public HttpResponseMessage AjouterDimancheFerie(string praticienCin)
        {

            var statusCode = HttpStatusCode.OK;

            ResultOfType<IList<string>> result = _configurationApiApplicationServices.AjouterDimancheFerie(praticienCin);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("samedi")]
        [HttpGet]
        public HttpResponseMessage AjouterSamediFerie(string praticienCin)
        {

            var statusCode = HttpStatusCode.OK;

            ResultOfType<IList<string>> result = _configurationApiApplicationServices.AjouterSamediFerie(praticienCin);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }

        [Route("ferie")]
        [HttpGet]
        public HttpResponseMessage AjouterFerie(string praticienCin, string jour)
        {

            var statusCode = HttpStatusCode.OK;

            ResultOfType<IList<string>> result = _configurationApiApplicationServices.AjouterFerie(praticienCin, jour);
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());
            return Request.CreateResponse(statusCode, result);
        }
    }
}
