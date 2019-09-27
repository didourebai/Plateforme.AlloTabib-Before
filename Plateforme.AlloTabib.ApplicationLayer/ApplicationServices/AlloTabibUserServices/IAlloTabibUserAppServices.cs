using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices
{
    public interface IAlloTabibUserAppServices
    {
        bool IsUserAuthenticated(string userName, string password);
        ResultOfType<Null> AddAlloTabibUser(string userName, string password, string type);
        ResultOfType<UserAccountResultModel> GetUsers();

        // Partie réservé au patient
        bool IsPatientAuthenticated(string userName, string password);
        bool IsPraticienAuthenticated(string userName, string password);
        ResultOfType<string> SendPasswordByEmail(string username);
        ResultOfType<bool> GetUserByEmail(string email);
        ResultOfType<IList<string>> GetAllEmailsSubscriber();

    }
}
