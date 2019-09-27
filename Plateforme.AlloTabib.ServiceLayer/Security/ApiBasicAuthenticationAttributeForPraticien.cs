using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices;

namespace Plateforme.AlloTabib.ServiceLayer.Security
{
    public class ApiBasicAuthenticationAttributeForPraticien : AuthorizeAttribute
    {
        #region Override Methods

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Unauthorized : Access denied!");
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var req = actionContext.Request;

            var auth = req.Headers.Authorization;

            if (auth != null)
                if (auth.Scheme == "Basic" && !string.IsNullOrEmpty(auth.Parameter))
                {
                    string userName;
                    string password;

                    var gotIt = GetUserNameAndPassword(actionContext, out userName, out password);

                    if (gotIt)
                    {
                        var alloTabibUserServices = (IAlloTabibUserAppServices)
                                                GlobalConfiguration.Configuration.DependencyResolver.GetService(
                                                    typeof(IAlloTabibUserAppServices));

                        if (alloTabibUserServices != null)
                        {
                            UnityConfig.OnExecuting();

                            var isAuthenticated = alloTabibUserServices.IsPraticienAuthenticated(userName, password);

                            UnityConfig.OnExecuted();

                            return isAuthenticated;
                        }
                    }

                    return false;
                }

            return false;
        }

        #endregion

        #region Private Methods

        private static bool GetUserNameAndPassword(HttpActionContext actionContext, out string username,
                                                   out string password)
        {
            var gotIt = false;
            username = string.Empty;
            password = string.Empty;
            IEnumerable<string> headerVals;
            if (actionContext.Request.Headers.TryGetValues("Authorization", out headerVals))
            {
                try
                {
                    var authHeader = headerVals.FirstOrDefault();
                    char[] delims = { ' ' };
                    if (authHeader != null)
                    {
                        var authHeaderTokens = authHeader.Split(delims);
                        if (authHeaderTokens[0].Contains("Basic"))
                        {
                            var decodedStr = DecodeFrom64(authHeaderTokens[1]);
                            var unpw = decodedStr.Split(new[] { ':' });
                            username = unpw[0];
                            password = unpw[1];
                        }
                        else
                        {
                            if (authHeaderTokens.Length > 1)
                                username = DecodeFrom64(authHeaderTokens[1]);
                        }
                    }

                    gotIt = true;
                }
                catch
                {
                    gotIt = false;
                }
            }

            return gotIt;
        }

        private static string DecodeFrom64(string encodedData)
        {
            var encodedDataAsBytes = Convert.FromBase64String(encodedData);
            var returnValue = Encoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }

        #endregion
    }
}