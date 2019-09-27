using System;
using Facebook;
using Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices;
using Plateforme.AlloTabib.DomainLayer.DomainServices.PatientDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices
{
    public class FacebookUserAppServices : IFacebookUserAppServices
    { 
        #region Private Properties

        private readonly IFacebookUserDomainServices _facebookUserDomainServices;

        public FacebookUserAppServices(IFacebookUserDomainServices facebookUserDomainServices)
        {
            if(facebookUserDomainServices == null)
                throw new ArgumentNullException("facebookUserDomainServices");
            _facebookUserDomainServices = facebookUserDomainServices;
        }

        #endregion

        public ResultOfType<FacebookUserModel> GetFacebookUserProfile(FacebookClient facebookClient)
        {
            return _facebookUserDomainServices.GetFacebookUserProfile(facebookClient);
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
