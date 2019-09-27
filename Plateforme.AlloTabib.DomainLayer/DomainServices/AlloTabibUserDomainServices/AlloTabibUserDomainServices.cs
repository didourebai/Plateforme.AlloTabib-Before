using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using Plateforme.AlloTabib.DomainLayer.Entities;
using PlateformeAlloTabib.Standards.Helpers;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.AlloTabibUserDomainServices
{
    public class AlloTabibUserDomainServices : IAlloTabibUserDomainServices
    {
        #region properties
        private readonly IRepository<UserAccount> _alloTabibUserRepository;
        #endregion

        #region constructor

        public AlloTabibUserDomainServices(IRepository<UserAccount> alloTabibUserRepository)
        {
            if (alloTabibUserRepository == null)
                throw new ArgumentNullException("alloTabibUserRepository");

            _alloTabibUserRepository = alloTabibUserRepository;
          
        }

        #endregion

        #region public methods
        public bool IsUserAuthenticated(string userName, string password)
        {
          
            Logger.LogInfo("IsUserAuthenticated : Start ");
            return
               _alloTabibUserRepository.GetAll().Any(
                   u =>
                   u.Email.ToLower().Equals(userName.ToLower()) &&
                   CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(u.Password) == password);
        }

        public bool IsPraticienAuthenticated(string userName, string password)
        {

            Logger.LogInfo("IsPraticienAuthenticated : Start avec IP : ");
            return
               _alloTabibUserRepository.GetAll().Any(
                   u =>
                   u.Email == userName && u.Type == "Praticien" && u.EstActive &&
                   CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(u.Password) == password) ;
        }

        public ResultOfType<Null> AddAlloTabibUser(string userName, string password,string type)
        {
           
            Logger.LogInfo("AddAlloTabibUser : Start avec IP : ");

            if (_alloTabibUserRepository.GetAll().Any(u => u.Email == userName))
            {
                return
                    new Return<Null>().Error().As(EStatusDetail.Forbidden).AddingGenericError(null,
                                                                                           "L'email existe déjà.").WithDefaultResult();
            }

            var user = new UserAccount
            {
                Email = userName,
                Password =
                    CrossCuttingLayer.Encryption.RijndaelEncryption.Encrypt(password),
                Type = type
            };

            _alloTabibUserRepository.Add(user);

            return new Return<Null>().OK().WithDefaultResult();
        }

        public ResultOfType<UserAccountResultModel> GetUsers()
        {
            try
            {
                
                Logger.LogInfo("GetUsers : Start avec IP : " );
               

                var users = _alloTabibUserRepository.GetAll();

                var data = new UserAccountResultModel();

                users.ForEach(user =>
                {
                    var dataModel = UserAccountWrapper.ConvertBoardEntityToDataModel(user);
                    data.Items.Add(dataModel);
                });

             

                Logger.LogInfo("Get Users : End --- Status : OK");
                return new Return<UserAccountResultModel>().OK().WithResult(data);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get Patients Exception", exception);
                throw;
            }
        }

        public bool IsPatientAuthenticated(string userName, string password)
        {
           
            Logger.LogInfo("IsPatientAuthenticated : Start");
            //Donner accés même au médecin poru accéder à l'espace patient pour réserver
           return IsUserAuthenticated(userName, password);
            
            
        }

        public ResultOfType<bool> GetUserByEmail(string email)
        {
            try
            {

                Logger.LogInfo("GetUsers : Start ");
                var user = _alloTabibUserRepository.GetAll().FirstOrDefault(x => x.Email.Equals(email));

                if (user == null)
                {
                    return new Return<bool>().OK().WithResult(false);
                }
                else
                {
                    return new Return<bool>().OK().WithResult(true);
                }
            }
            catch (Exception exception)
            {
                Logger.LogError("GetUserByEmail Exception", exception);
                throw;
            }
        }
        public ResultOfType<string> SendPasswordByEmail(string username)
        {
            try
            {

                Logger.LogInfo("GetUsers : Start avec IP : ");
                var user = _alloTabibUserRepository.GetAll().FirstOrDefault(x => x.Email.Equals(username));

               if(user == null)
               {
                   return new Return<string>()
                    .Error()
                    .AsValidationFailure(null,
                        "Veuillez vérifier votre adresse mail, elle n'existe pas dans nos comptes.",
                    "Email")
                    .WithDefaultResult();
               }
               else
               {
                   //send email with the new password

                   SendMailHelper.SendEmail("contact@allotabib.net", username, "[AlloTabib] Mot de passe oublié", "Votre mot de passe est : " + CrossCuttingLayer.Encryption.RijndaelEncryption.Decrypt(user.Password));
                   return new Return<string>().OK().WithResult(username);
               }


            }
            catch (Exception exception)
            {
                Logger.LogError("SendPasswordByEmail Exception", exception);
                throw;
            }
        }

        /// <summary>
        /// Get All Emails Subscriber
        /// </summary>
        /// <returns></returns>
        public ResultOfType<IList<string>> GetAllEmailsSubscriber()
        {
            
          
            Logger.LogInfo(string.Format("{0} ", "GetAllEmailsSubscriber : Start "));

            try
            {
                var emails = _alloTabibUserRepository.GetAll().Select(x => x.Email);

                IList<string> listEmails = new List<string>();
                emails.ForEach(email =>
                {
                    var dataModel = email.ToString();
                    listEmails.Add(dataModel);
                });

                return new Return<IList<string>>().OK().WithResult(listEmails);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get All Emails Subscriber Exception", exception);
                throw;
            }
        }

        #endregion
    }
}
