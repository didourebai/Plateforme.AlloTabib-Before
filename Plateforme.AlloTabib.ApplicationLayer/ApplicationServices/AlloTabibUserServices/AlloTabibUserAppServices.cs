using System;
using System.Collections.Generic;
using Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;

namespace Plateforme.AlloTabib.ApplicationLayer.ApplicationServices.AlloTabibUserServices
{
    public class AlloTabibUserAppServices : IAlloTabibUserAppServices
    {
        private readonly IAlloTabibUserDomainServices _userServices;

        public AlloTabibUserAppServices(IAlloTabibUserDomainServices userServices)
        {
            if(userServices == null)
                throw new ArgumentNullException("userServices");
            _userServices = userServices;
        }

        public bool IsUserAuthenticated(string userName, string password)
        {
            return _userServices.IsUserAuthenticated(userName, password);
        }

        public ResultOfType<Null> AddAlloTabibUser(string userName, string password, string type)
        {
            return _userServices.AddAlloTabibUser(userName, password, type);
        }

        public ResultOfType<UserAccountResultModel> GetUsers()
        {
            return _userServices.GetUsers();
        }

        public bool IsPatientAuthenticated(string userName, string password)
        {
            return _userServices.IsPatientAuthenticated(userName, password);
        }

        public bool IsPraticienAuthenticated(string userName, string password)
        {
            return _userServices.IsPraticienAuthenticated(userName, password);
        }

        public ResultOfType<string> SendPasswordByEmail(string username)
        {
            return _userServices.SendPasswordByEmail(username);
        }

        public ResultOfType<bool> GetUserByEmail(string email)
        {
            return _userServices.GetUserByEmail(email);
        }

        public ResultOfType<IList<string>> GetAllEmailsSubscriber()
        {
            return _userServices.GetAllEmailsSubscriber();
        }
    }
}
