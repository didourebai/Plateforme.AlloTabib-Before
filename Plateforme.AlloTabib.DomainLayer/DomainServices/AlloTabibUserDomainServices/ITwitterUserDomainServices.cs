using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public interface ITwitterUserDomainServices
    {
        ResultOfType<TwitterUserModel> VerifyCredentials();
       
    }
}
