using Facebook;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public interface IFacebookUserDomainServices
    {
        ResultOfType<FacebookUserModel> GetFacebookUserProfile(FacebookClient facebookClient);
        ResultOfType<FacebookUserModel> ConnectToMyAccount(FacebookClient facebookClient);
        ResultOfType<FacebookUserModel> PostFacebookUser(FacebookClient facebookClient);
    }
}
