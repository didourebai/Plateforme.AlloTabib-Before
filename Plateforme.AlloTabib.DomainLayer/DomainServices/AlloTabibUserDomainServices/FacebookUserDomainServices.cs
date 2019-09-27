using System;
using System.Collections.Generic;
using Facebook;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public class FacebookUserDomainServices : IFacebookUserDomainServices
    {
        private string AccessToken { get; set; }
       

        private dynamic _me;

        //private string GetAccessToken()
        //{
        //    var fb = new FacebookClient();
        //    var result = fb.Get("oauth/access_token", new
        //    {
        //        client_id = App.FaceBookId,
        //        client_secret = App.FacebookAppSecret,
        //        grant_type = "client_credentials"
        //    });
        //    var accessToken = Newtonsoft.Json.JsonConvert.DeserializeObject<string>(result.ToString());
        //}

        public ResultOfType<FacebookUserModel> GetFacebookUserProfile(FacebookClient facebookClient)
        {
            try
            {
                string firstName = _me.first_name;
                string lastName = _me.last_name;
               
            }
            catch (Exception ex)
            {
                
            }
            throw new NotImplementedException();
        }

        public ResultOfType<FacebookUserModel> ConnectToMyAccount(FacebookClient facebookClient)
        {
            throw new NotImplementedException();
        }

        public ResultOfType<FacebookUserModel> PostFacebookUser(FacebookClient facebookClient)
        {
            throw new NotImplementedException();
        }
    }
}
