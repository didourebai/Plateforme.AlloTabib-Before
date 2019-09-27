using System.Web;
using System.Web.SessionState;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public class FacebookLogin : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            var accessToken = context.Request["accessToken"];
            context.Session["AccessToken"] = accessToken;

            context.Response.Redirect("./index.html");
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
