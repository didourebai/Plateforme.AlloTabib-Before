using System.Net;

namespace Plateforme.AlloTabib.DomainLayer.Models.Base
{
    public class PostmanSendResponse
    {
        public string ResponseData { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
