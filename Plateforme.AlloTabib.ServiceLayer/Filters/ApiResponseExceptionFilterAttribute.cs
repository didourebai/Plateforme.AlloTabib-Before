using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;


namespace Plateforme.AlloTabib.ServiceLayer.Filters
{
    public class ApiResponseExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            Logger.LogError("** Logging Error ** " + context.Exception.Message);


            var result = new HttpRequestException(context.Exception.Message);


            context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError, result);
        }
    }
}