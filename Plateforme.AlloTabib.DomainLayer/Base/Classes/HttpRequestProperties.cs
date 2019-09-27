using System.Collections.Generic;
using System.Net;

namespace Plateforme.AlloTabib.DomainLayer.Base.Classes
{
    public class HttpRequestProperties
    {
        public string Uri { get; set; }

        public string Method { get; set; }

        public string ContentType { get; set; }

        public int MaximumAutomaticRedirections { get; set; }

        public bool AllowAutoRedirect { get; set; }

        public Dictionary<HttpRequestHeader, string> Headers { get; set; }

        public string JsonDataToSend { get; set; }

        public Dictionary<string, string> UriAttributesToSend { get; set; }
    }
}
