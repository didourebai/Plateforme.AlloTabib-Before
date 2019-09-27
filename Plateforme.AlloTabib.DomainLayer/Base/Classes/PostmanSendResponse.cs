using System.Net;

namespace Plateforme.AlloTabib.DomainLayer.Base.Classes
{
    public class PostmanSendResponse
    {
        public string ResponseData { get; set; }

        public HttpStatusCode Status { get; set; }
    }
}
