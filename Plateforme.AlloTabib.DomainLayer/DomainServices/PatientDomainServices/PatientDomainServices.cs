using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Plateforme.AlloTabib.CrossCuttingLayer.Logging;
using Plateforme.AlloTabib.DomainLayer.Base.Interfaces;
using Plateforme.AlloTabib.DomainLayer.Entities;
using Plateforme.AlloTabib.DomainLayer.Helpers;
using Plateforme.AlloTabib.DomainLayer.Models;
using PlateformeAlloTabib.Standards.Domain;
using PlateformeAlloTabib.Standards.Helpers;

namespace Plateforme.AlloTabib.DomainLayer.DomainServices.PatientDomainServices
{
    public class PatientDomainServices : IPatientDomainServices
    {
        #region Private Properties

        private readonly IRepository<Patient> _patientRepository;
        private readonly IRepository<UserAccount> _userAccountRepository;
        private readonly IRepository<GoogleMapsCoordinations> _googleMapsCoordinations;
       

        #endregion

        #region Constructors

        public PatientDomainServices(IRepository<Patient> patientRepository, IRepository<UserAccount> userAccountRepository, IRepository<GoogleMapsCoordinations> googleMapsCoordinations)
        {
             if (patientRepository == null)
                throw new ArgumentNullException("patientRepository");
            if (userAccountRepository == null)
                throw new ArgumentNullException("userAccountRepository");

            if (googleMapsCoordinations == null)
                throw new ArgumentNullException("googleMapsCoordinations");
            _patientRepository = patientRepository;
            _userAccountRepository = userAccountRepository;
            _googleMapsCoordinations = googleMapsCoordinations;
         
        }
           
           

        #endregion

        public ResultOfType<PatientResultModel> GetPatients(int take = 0, int skip = 0)
        {
            try
            {
               
                Logger.LogInfo("Get Patients With Take And Skip Parameters : Start.");

                var totalCount = _patientRepository.GetCount();
                var totalPages = (take != 0) ? (int)Math.Ceiling((double)totalCount / take) : 0;

                var paginationHeader = new PaginationHeader
                {
                    TotalCount = totalCount,
                    TotalPages = totalPages
                };

                var patients = (take == 0 && skip == 0)
                                   ? _patientRepository
                                        .GetAll()
                                        .OrderBy(a => a.CreationDate)
                                        .ToList()
                                   : _patientRepository
                                        .GetAll()
                                        .OrderBy(a => a.CreationDate)
                                        .Skip(skip)
                                        .Take(take)
                                        .ToList();

                var data = new PatientResultModel();

                patients.ForEach(patient => 
                {
                    var dataModel = PatientWrapper.ConvertPatientEntityToDataModel(patient);
                    data.Items.Add(dataModel);
                });

                data.PaginationHeader = paginationHeader;

                Logger.LogInfo("Get Patients With Take And Skip Parameters : End --- Status : OK");
                return new Return<PatientResultModel>().OK().WithResult(data);
            }
            catch (Exception exception)
            {
                Logger.LogError("Get Patients Exception", exception);
                throw;
            }
        }

        public IEnumerable<Patient> GetAll()
        {
            return _patientRepository.GetAll();
        }

        public void AddNewPatient(Patient patient)
        {
            _patientRepository.Add(patient);
        }

        public void ModifyPatient(Patient patient)
        {
            _patientRepository.Update(patient);
        }

        public void DeletePatient(object id)
        {
            var item = _patientRepository.Get(id);

            if (item != null)
                _patientRepository.Delete(id);
        }

        public ResultOfType<PatientResultDataModel> PostNewPatient(PatientDataModel patientToAdd)
        {
           
            Logger.LogInfo("PostNewPatient : Start." );

            if (patientToAdd == null)
                return new Return<PatientResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Les données sont vides.").WithDefaultResult();
            Logger.LogInfo(string.Format("Post New Patient : Start --- CIN = {0}, Email = {1}",
                                           patientToAdd.Cin, patientToAdd.Email));

            // Id validation
            var validationResult = ValidateNewPatientProperties(patientToAdd);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Patient : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }
            
            validationResult = ValidatePasword(patientToAdd.Password, patientToAdd.NomPrenom);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Patient : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }
          
            patientToAdd.CreationDate =  DateTime.UtcNow;
            
            
            
            var patient = new Patient
            {
                Adresse = patientToAdd.Adresse,
                Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Encrypt(patientToAdd.Password),
                Cin = patientToAdd.Cin,
                Email = patientToAdd.Email,
                NomPrenom = patientToAdd.NomPrenom,
                Telephone = patientToAdd.Telephone,
                CreationDate = patientToAdd.CreationDate.Value,
                LastModificationDate = DateTime.UtcNow,
                DateNaissance = patientToAdd.DateNaissance,
                Sexe =  patientToAdd.Sexe
            };
            var userAccount = new UserAccount
            {
                CreationDate = patientToAdd.CreationDate.Value,
                Email = patientToAdd.Email,
                LastModificationDate = patient.LastModificationDate,
                Password = patient.Password,
                EstActive = true,
                Type = "patient"
            };


            #region validation of the email
            //send email before adding in DB
            try
            {
               //EmailTest emailTest = new EmailTest();
               //bool isCorrect=  emailTest.chkEmailExist(patient.Email);
               //if (isCorrect)
               //{
                   Logger.LogInfo(string.Format("Post New Patient : End --- OK - Email : {0}  Nom prenom : {1} - mot de passe : {2} - adresse : {3} -- Telephone {4} - Date naissance : {5} ", patient.Email, patient.NomPrenom, patient.Password, patient.Adresse, patient.Telephone, patient.DateNaissance));
                   //ajout d'un compte
                   _patientRepository.Add(patient);
                   _userAccountRepository.Add(userAccount);
                   return
                   new Return<PatientResultDataModel>().OK().WithResult(new PatientResultDataModel { Cin = patient.Cin, Adresse = patient.Adresse, Email = patient.Email, NomPrenom = patient.NomPrenom, Telephone = patient.Telephone, DateNaissance = patient.DateNaissance });

               //}
               //else
               //{
               //    //tracer par log ces informations
               //    Logger.LogInfo(string.Format("Post New Patient : End --- KO -- erreur dans la saisie de l'email - Email : {0} - Nom prenom : {1} - mot de passe : {2} - adresse : {3} -- Telephone {4} - Date naissance : {5} ", patient.Email, patient.NomPrenom,patient.Password, patient.Adresse, patient.Telephone, patient.DateNaissance));
               //    return
               //    new Return<PatientResultDataModel>()
               //    .Error()
               //    .AsValidationFailure(null,
               //        "Votre adresse Email n'existe pas, veuillez vérifier la syntaxe !",
               //    "Email")
               //    .WithDefaultResult();

               //}
                   

            }
            catch (Exception ex)
            {
                var exception = ex.Message;
                return
                    new Return<PatientResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Votre adresse Email n'existe pas, veuillez vérifier la syntaxe !",
                    "Email")
                    .WithDefaultResult();
            }

          
            #endregion

              }

        public ResultOfType<PatientResultDataModel> DeleteAPatient(string cin)
        {
            Logger.LogInfo("PostNewPatient : Start.");

            if(string.IsNullOrEmpty(cin))
                return new Return<PatientResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Veuillez introduire un cin ou patient possédant un cin vide !!!!").WithDefaultResult();
            Logger.LogInfo(string.Format("Delete patient : Start --- CIN = {0}", cin));

           try
            {
                _patientRepository.Delete(cin);
                return new Return<PatientResultDataModel>().OK().WithResult(new PatientResultDataModel { Cin = cin});
            }
            catch (Exception ex)
            {
                Logger.LogInfo(string.Format("Delete patient : end error --- Exception = {0}", ex.Message));
                return new Return<PatientResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Erreur suite à une exception avec notre serveur.").WithDefaultResult();
              
            }

        }

        private ResultOfType<PatientResultDataModel> ValidateUpdatedPatientProperties(PatientDataModel patient)
        {
            if (string.IsNullOrEmpty(patient.Cin))
                return new Return<PatientResultDataModel>()
                   .Error().AsValidationFailure(null, "Veuillez introduire votre CIN.", "CIN")
                   .WithDefaultResult();
            var myRegex = new Regex(@"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");
            var isNumber = myRegex.IsMatch(patient.Cin);
            if (isNumber == false)
                return new Return<PatientResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte, elle est composé de 8 chiffres.", "CIN")
                         .WithDefaultResult();
            if (string.IsNullOrEmpty(patient.Email))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre Email.", "Email")
                  .WithDefaultResult();
            if (ValidateEmail(patient.Email))
                return new Return<PatientResultDataModel>()
                .Error().AsValidationFailure(null, "Le format de l'email introduit est incorrecte : exemple : xxx@domain.xxx.", "Email")
                .WithDefaultResult();



            #region Telephone

            if (string.IsNullOrEmpty(patient.Telephone))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            if (patient.Telephone.Length > 30)
                return new Return<PatientResultDataModel>()
                    .Error().AsValidationFailure(null, "Fax number length must be between 0 and 30 characters", "Téléphone")
                    .WithDefaultResult();

            if (!Regex.IsMatch(patient.Telephone, @"^[0-9+() ][0-9+() -]*"))
                return new Return<PatientResultDataModel>()
                    .Error().AsValidationFailure(null, "Le numéro de téléphone introduit est incorrecte.", "Téléphone")
                    .WithDefaultResult();
            #endregion

            #region Adresse

            if (string.IsNullOrEmpty(patient.Adresse))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            #endregion

            #region Date naissance
            if (string.IsNullOrEmpty(patient.DateNaissance))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre date de naissance.", "Date de naissance")
                  .WithDefaultResult();


            #endregion

            return null;
        }
        /// <summary>
        /// Validation patient
        /// </summary>
        /// <param name="patient">patient.</param>
        /// <returns>Null if the id is valid, a specific error message otherwise.</returns>
        private ResultOfType<PatientResultDataModel> ValidateNewPatientProperties(PatientDataModel patient)
        {
            #region validation CIN

            if(string.IsNullOrEmpty(patient.Cin))
                return new Return<PatientResultDataModel>()
                   .Error().AsValidationFailure(null, "Veuillez introduire votre CIN.", "CIN")
                   .WithDefaultResult();

            if (CinExists(patient.Cin))
                return new Return<PatientResultDataModel>()
                    .Error().AsValidationFailure(null, "Le CIN introduit est utilisé déjà par un autre utilisateur.", "CIN")
                    .WithDefaultResult();

            if (patient.Cin.Length != 8)
                return new Return<PatientResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte.", "CIN")
                         .WithDefaultResult();

            var myRegex = new Regex(@"[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]");
            var isNumber = myRegex.IsMatch(patient.Cin);
            if (isNumber == false)
                return new Return<PatientResultDataModel>()
                         .Error().AsValidationFailure(null, "Le CIN introduit est incorrecte, elle est composé de 8 chiffres.", "CIN")
                         .WithDefaultResult();

            #endregion

            #region Email

            if(string.IsNullOrEmpty(patient.Email))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre Email.", "Email")
                  .WithDefaultResult();
            if (EmailExists(patient.Email))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "L'Email introduit est utilisé déjà par un autre utilisateur.", "Email")
                  .WithDefaultResult();
            if (ValidateEmail(patient.Email))
                return new Return<PatientResultDataModel>()
                .Error().AsValidationFailure(null, "Le format de l'email introduit est incorrecte : exemple : xxx@domain.xxx.", "Email")
                .WithDefaultResult();

            #endregion

            #region Telephone

            if (string.IsNullOrEmpty(patient.Telephone))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            if (patient.Telephone.Length > 30)
                return new Return<PatientResultDataModel>()
                    .Error().AsValidationFailure(null, "Fax number length must be between 0 and 30 characters", "Téléphone")
                    .WithDefaultResult();

            if (!Regex.IsMatch(patient.Telephone, @"^[0-9+() ][0-9+() -]*"))
                return new Return<PatientResultDataModel>()
                    .Error().AsValidationFailure(null, "Le numéro de téléphone introduit est incorrecte.", "Téléphone")
                    .WithDefaultResult();
            #endregion  

            #region Adresse

            if (string.IsNullOrEmpty(patient.Adresse))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre téléphone.", "Téléphone")
                  .WithDefaultResult();

            #endregion

            #region Date naissance
            if (string.IsNullOrEmpty(patient.DateNaissance))
                return new Return<PatientResultDataModel>()
                  .Error().AsValidationFailure(null, "Veuillez introduire votre date de naissance.", "Date de naissance")
                  .WithDefaultResult();
            #endregion
            return null;
        }


      /// <summary>
      /// vérifier si le cin existe ou pas
      /// </summary>
      /// <param name="cin"></param>
      /// <returns></returns>
        private bool CinExists(string cin)
        {
            return _patientRepository.GetAll().Any(a => a.Cin == cin);
        }

        private bool EmailExists(string email)
        {
            return _userAccountRepository.GetAll().Any(c => c.Email == email);
        }

        private bool ValidateEmail(string emailAddress)
        {
            return Regex.IsMatch(emailAddress,
                              @"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}$");
        }

        private ResultOfType<PatientResultDataModel> ValidatePasword(string password, string nomprenom)
        {
            if (string.IsNullOrWhiteSpace(password))
                return
                    new Return<PatientResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Veuillez introduire un mot de passe.",
                    "Mot de passe")
                    .WithDefaultResult();

            if (password.Length > 20 || password.Length < 5)
                return
                    new Return<PatientResultDataModel>()
                    .Error()
                    .AsValidationFailure(null,
                        "Le mot de passe est composé de  5 et 20 caractères.",
                    "Mot de passe")
                    .WithDefaultResult();

            var split = nomprenom.Split(Convert.ToChar(" "));

            if (split.Length == 2 && split[0] != null)
            {
                if (password.Contains(split[0]))
                    return
                        new Return<PatientResultDataModel>()
                        .Error()
                        .AsValidationFailure(null,
                            "Le mot de passe ne doit pas contenir votre nom ou votre prenom",
                        "Mot de passe")
                        .WithDefaultResult();
            }
            if (split.Length == 2 && split[1] != null)
            {
                if (password.Contains(split[1]))
                    return
                        new Return<PatientResultDataModel>()
                            .Error()
                            .AsValidationFailure(null,
                                "Le mot de passe ne doit pas contenir votre nom ou votre prenom",
                                "Mot de passe")
                            .WithDefaultResult();
            }
            //int pwStrength = 0;

            //if (Regex.IsMatch(password, @"[\!@#\$%\^&*?_~]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[a-z]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[A-Z]"))
            //    pwStrength++;

            //if (Regex.IsMatch(password, @"[0-9]"))
            //    pwStrength++;

            //if (pwStrength >= 3)
                return null;

            //return
            //    new Return<PatientResultDataModel>()
            //    .Error()
            //    .AsValidationFailure(null,
            //        "Pour améliorer la résistance du mot de passe, vous devez utiliser des chiffres, " +
            //        "caractères en majuscule et miniscule, " +
            //        "et des caractères spéciaux comme : !,@,#,$,%,^,&,*,?,_,~",
            //    "Mot de passe")
            //    .WithDefaultResult();
        }


        public ResultOfType<PatientResultDataModel> GetPatientByEmail(string email)
        {
          
            Logger.LogInfo("GetPatientByEmail : Start." );
            try
            {
                var list = _patientRepository.GetAll();
                if (list != null)
                {
                    var patients = list as Patient[] ?? list.ToArray();
                    var patient = patients.FirstOrDefault(p => p.Email.Equals(email));


                    if (patient != null)
                    {
                        var dataModel = PatientWrapper.ConvertPatientEntityToDataModel(patient);
                        return new Return<PatientResultDataModel>().OK().WithResult(dataModel);
                    }
                    else
                    {
                        return new Return<PatientResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                null, "Pas de patient enregistré actuellement !!").WithDefaultResult();
                    }


                }
                else
                {

                    return new Return<PatientResultDataModel>().Error().As(EStatusDetail.NotFound).AddingGenericError(
                   null, "Pas de patient enregistré actuellement !!").WithDefaultResult();
                }


            }
            catch (Exception ex)
            {
                return new Return<PatientResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                   null, "Erreur indéterminé :" + ex.Message).WithDefaultResult();
            }
        }


        public ResultOfType<PatientResultDataModel> PatchPatient(PatientDataModel patientDataModel)
        {
            Logger.LogInfo("PostNewPatient : Start.");

            if (patientDataModel == null)
                return new Return<PatientResultDataModel>().Error().As(EStatusDetail.BadRequest).AddingGenericError(
                    null, "Les données sont vides.").WithDefaultResult();
            Logger.LogInfo(string.Format("Post New Patient : Start --- CIN = {0}, Email = {1}",
                                           patientDataModel.Cin, patientDataModel.Email));

            // Id validation
            var validationResult = ValidateUpdatedPatientProperties(patientDataModel);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Patient : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            validationResult = ValidatePasword(patientDataModel.Password, patientDataModel.NomPrenom);
            if (validationResult != null)
            {
                Logger.LogInfo(string.Format("Post New Patient : End --- Status = {0}, Message= {1}",
                                             validationResult.Status, validationResult.Errors[0].Message));
                return validationResult;
            }

            patientDataModel.CreationDate = DateTime.UtcNow;


            //Get patient to update it
            var patientToUpdate = _patientRepository.GetAll().FirstOrDefault(p => p.Cin.Equals(patientDataModel.Cin));

            if(patientToUpdate == null)
                return new Return<PatientResultDataModel>()
                .Error().AsValidationFailure(null, "Problème de récupération d'un patient. Veuillez nous contacter pour signaler ce problème.", "Patient")
                .WithDefaultResult();


            patientToUpdate.Adresse =patientDataModel.Adresse;
            patientToUpdate.Password = CrossCuttingLayer.Encryption.RijndaelEncryption.Encrypt(patientDataModel.Password);
            patientToUpdate.NomPrenom = patientDataModel.NomPrenom;
            patientToUpdate.Telephone = patientDataModel.Telephone;
            patientToUpdate.Sexe = patientDataModel.Sexe;
            patientToUpdate.DateNaissance = patientDataModel.DateNaissance;
            patientToUpdate.LastModificationDate = DateTime.UtcNow;

            _patientRepository.Update(patientToUpdate);
        
            Logger.LogInfo("Post New patient : End --- Status = OK, Message= {1}");
            return
                new Return<PatientResultDataModel>().OK().WithResult(new PatientResultDataModel { Cin = patientToUpdate.Cin, Adresse = patientToUpdate.Adresse, Email = patientToUpdate.Email, NomPrenom = patientToUpdate.NomPrenom, Telephone = patientToUpdate.Telephone, DateNaissance = patientToUpdate.DateNaissance });
        }
    }
}
