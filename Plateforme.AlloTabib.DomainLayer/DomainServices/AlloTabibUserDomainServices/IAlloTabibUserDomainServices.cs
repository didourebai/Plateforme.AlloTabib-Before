using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public interface IAlloTabibUserDomainServices
    {
        bool IsUserAuthenticated(string userName, string password);
        ResultOfType<Null> AddAlloTabibUser(string userName, string password, string type);
        ResultOfType<UserAccountResultModel> GetUsers();

        //partie réservé au patient
        bool IsPatientAuthenticated(string userName, string password);
        bool IsPraticienAuthenticated(string userName, string password);
        ResultOfType<string> SendPasswordByEmail(string username);
        //Get user by Email
        ResultOfType<bool> GetUserByEmail(string email);
        ResultOfType<IList<string>> GetAllEmailsSubscriber();
    }
}
