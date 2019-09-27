using Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.TwitterAppServices
{
    public class TwitterAppServices : ITwitterAppServices
    {
        #region Private Properties

        private readonly ITwitterUserDomainServices _twitterDomainServices;

        #endregion

        public TwitterAppServices(ITwitterUserDomainServices twitterDomainServices)
        {
            if (twitterDomainServices == null)
                throw new ArgumentNullException("twitterDomainServices");
            _twitterDomainServices = twitterDomainServices;
        }

        public ResultOfType<TwitterUserModel> VerifyCredentials()
        {
            return _twitterDomainServices.VerifyCredentials();
        }
    }
}
