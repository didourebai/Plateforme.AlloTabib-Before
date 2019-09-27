using Facebook;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices
{
    public interface IFacebookUserAppServices
    {
        ResultOfType<FacebookUserModel> GetFacebookUserProfile(FacebookClient facebookClient);
        ResultOfType<FacebookUserModel> ConnectToMyAccount(FacebookClient facebookClient);
        ResultOfType<FacebookUserModel> PostFacebookUser(FacebookClient facebookClient);
    }
}
