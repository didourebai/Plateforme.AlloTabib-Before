using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.TwitterAppServices;
using System.Web.Http;



namespace Plateforme.AlloTabib.ServiceLayer.Controllers.AlloTabibApi
{
    [RoutePrefix("twitter")]
    public class TwitterController : ApiController
    {
        #region Private Properties
        private readonly ITwitterAppServices _twitterApiApplicationServices;
        #endregion

        #region Constructor

        public TwitterController(ITwitterAppServices twitterApiApplicationServices)
          {
              if (twitterApiApplicationServices == null)
                  throw new ArgumentNullException("twitterApiApplicationServices");
              _twitterApiApplicationServices = twitterApiApplicationServices;
            
          }

          #endregion

        [Route("credentials")]
        [HttpGet]
        public HttpResponseMessage VerifyCredentials()
        {
            var statusCode = HttpStatusCode.OK;

            var result = _twitterApiApplicationServices.VerifyCredentials();
            if (result != null)
                statusCode = (HttpStatusCode)Enum.Parse(typeof(HttpStatusCode), result.StatusDetail.ToString());

            return Request.CreateResponse(statusCode, result);
        }
    }
}
